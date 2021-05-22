using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Devcorner.NIdenticon;
using Devcorner.NIdenticon.BrushGenerators;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WUCSA.Web.Utils
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _env;
        public const string DefaultUserAvatarPath = "/img/profile_image.png";
        public const string DefaultBlogCoverPhotoPath = "/img/blog_image.png";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Upload image file to server
        /// </summary>
        /// <param name="image">Image file from form</param>
        /// <param name="imageFileName">Image file name without extension</param>
        /// <param name="resizeToQuadratic">Flag that indicates resize image to quadratic proportion</param>
        /// <param name="resizeToRectangle">Flag that indicates resize image to rectangle proportion</param>
        /// <returns>Output file path</returns>var g = new IdenticonGenerator();
        
        private UInt16 GetRandRgbNum()
        {
            return Convert.ToUInt16(new Random().Next(new Random().Next(0, 14), new Random().Next(16, 200)));
        }

        public void DeleteFile(string filePath)
        {
            var absolutePath = Path.Combine(_env.WebRootPath, filePath);
            if (File.Exists(absolutePath))
            {
               File.Delete(absolutePath);
            }
        }

        public string GenerateImage(string fileName,string subFolder, string size = "small")
        {
            int height = 228, width = 228, horizontal = 6, vertical = 6;
            if (size == "big"){ height = 648; width = 864; horizontal = 8; vertical = 6; }
            var randColor = Color.FromArgb(GetRandRgbNum(), GetRandRgbNum(), GetRandRgbNum());
            var statColor = new StaticColorBrushGenerator(randColor);
            var g = new IdenticonGenerator()
                .WithSize(width, height)
                .WithBlocks(horizontal, vertical)
                .WithAlgorithm("MD5")
                .WithBackgroundColor(Color.White)
                .WithBrushGenerator(statColor)
                .WithBlockGenerators(IdenticonGenerator.DefaultBlockGeneratorsConfig);
            var mybitmap = g.Create(fileName);
            var imagePath = $"{fileName}{".png"}";
            var absolutePath = Path.Combine(_env.WebRootPath, "img", subFolder, imagePath);
            mybitmap.Save(absolutePath, ImageFormat.Png);
            return $"/img/{subFolder}/{imagePath}";
        }

        public string UploadPostCoverImage(string base64img, string fileName)
        {
            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            base64img = regex.Replace(base64img, string.Empty);

            var imagePath = $"{fileName}{".png"}";
            var absolutePath = Path.Combine(_env.WebRootPath, "img", "profile_imgs", imagePath);

            byte[] imageBytes = Convert.FromBase64String(base64img);
            File.WriteAllBytes(absolutePath, imageBytes);

            return $"/img/profile_imgs/{imagePath}";
        }

        public string UploadCoverImage(IFormFile image, string imageFileName, string subFolder)
        {
            try
            {
                var fileExtension = Path.GetExtension(image.FileName);
                var isAnimatedImage = fileExtension != null && fileExtension.ToLower() == ".gif";
                var imagePath = $"{imageFileName}{fileExtension}";
                var absolutePath = Path.Combine(_env.WebRootPath, "img", subFolder, imagePath);
                if (isAnimatedImage)
                {
                    using var magickAnimatedImage = new MagickImageCollection(image.OpenReadStream());
                    magickAnimatedImage.Write(absolutePath, MagickFormat.Gif);
                }
                else
                {
                    using var magickImage = new MagickImage(image.OpenReadStream());
                    magickImage.Write(absolutePath);
                }

                return $"/img/{subFolder}/{imagePath}";
            }
            catch (MagickException)
            {
                return DefaultUserAvatarPath;
            }
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public async Task<string> UploadSatffImage(IFormFile image, string imageFileName, string subFolder)
        {
            try
            {
                var fileExtension = Path.GetExtension(image.FileName);
                var imagePath = $"{imageFileName}{fileExtension}";
                var absolutePath = Path.Combine(_env.WebRootPath, "img", subFolder, imagePath);

                using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return $"/img/{subFolder}/{imagePath}";
            }
            catch (Exception)
            {
                return $"/img/profile_image.png";
            }
        }

        public void RemoveImage(string imgPath, string subFolder)
        {
            if (imgPath == DefaultUserAvatarPath || imgPath == DefaultBlogCoverPhotoPath)
            {
                return;
            }

            var imgFileName = Path.GetFileName(imgPath);
            var imgFullPath = Path.Combine(_env.WebRootPath, "img", subFolder,  imgFileName);

            if (File.Exists(imgFullPath))
            {
                File.Delete(imgFullPath);
            }
        }
    }
}
