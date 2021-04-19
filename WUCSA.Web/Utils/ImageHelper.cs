using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
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

        public void DeleteFile(string fname)
        {
            fname = fname.Remove(0, 18);
            var absolutePath = Path.Combine(_env.WebRootPath, "img", "profile_imgs", fname);
            if (File.Exists(absolutePath))
            {
               File.Delete(absolutePath);
            }
        }

        public string GenerateImage(string fileName, string size = "small")
        {
            int height = 228, width = 228;
            if (size == "big"){ height = 648; width = 864; }
            var randColor = Color.FromArgb(GetRandRgbNum(), GetRandRgbNum(), GetRandRgbNum());
            var statColor = new StaticColorBrushGenerator(randColor);
            var g = new IdenticonGenerator()
                .WithSize(height, width)
                .WithBlocks(6, 6)
                .WithAlgorithm("MD5")
                .WithBackgroundColor(Color.White)
                .WithBrushGenerator(statColor)
                .WithBlockGenerators(IdenticonGenerator.DefaultBlockGeneratorsConfig);
            var mybitmap = g.Create(fileName);
            var imagePath = $"{fileName}{".png"}";
            var absolutePath = Path.Combine(_env.WebRootPath, "img", "profile_imgs", imagePath);
            mybitmap.Save(absolutePath, ImageFormat.Png);
            return $"/img/profile_imgs/{imagePath}";
        }

        public string UploadPostCoverImage(string base64img, string fileName)
        {
            Regex regex = new Regex(@"^[\w/\:.-]+;base64,");
            base64img = regex.Replace(base64img, string.Empty);

            var imagePath = $"{fileName}{".png"}";
            var absolutePath = Path.Combine(_env.WebRootPath, "img", "post_imgs", imagePath);

            byte[] imageBytes = Convert.FromBase64String(base64img);
            File.WriteAllBytes(absolutePath, imageBytes);

            return $"/img/post_imgs/{imagePath}";
        }
        
        public string UploadNextImage(IFormFile image, string imageFileName, string previousFilePath,
            bool resizeToQuadratic = false, bool resizeToRectangle = false)
        {
            DeleteFile(previousFilePath);
            return UploadImage(image, imageFileName, resizeToQuadratic, resizeToRectangle);
        }

        public string UploadImage(IFormFile image, string imageFileName,
            bool resizeToQuadratic = false, bool resizeToRectangle = false)
        {
            try
            {
                var fileExtension = Path.GetExtension(image.FileName);
                var isAnimatedImage = fileExtension != null && fileExtension.ToLower() == ".gif";
                var imagePath = $"{imageFileName}{fileExtension}";
                var absolutePath = Path.Combine(_env.WebRootPath, "img", "profile_imgs", imagePath);
                if (isAnimatedImage)
                {
                    using var magickAnimatedImage = new MagickImageCollection(image.OpenReadStream());
                    foreach (var imageFrame in magickAnimatedImage)
                    {
                        if (resizeToQuadratic)
                        {
                            ResizeToQuadratic(imageFrame);
                        }

                        if (resizeToRectangle)
                        {
                            ResizeToRectangle(imageFrame);
                        }
                    }

                    magickAnimatedImage.Write(absolutePath, MagickFormat.Gif);
                }
                else
                {
                    using var magickImage = new MagickImage(image.OpenReadStream());

                    if (resizeToQuadratic)
                    {
                        ResizeToQuadratic(magickImage);
                    }

                    if (resizeToRectangle)
                    {
                        ResizeToRectangle(magickImage);
                    }

                    magickImage.Write(absolutePath);
                }

                return $"/img/profile_imgs/{imagePath}";
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

        public void RemoveImage(string imgPath)
        {
            if (imgPath == DefaultUserAvatarPath || imgPath == DefaultBlogCoverPhotoPath)
            {
                return;
            }

            var imgFileName = Path.GetFileName(imgPath);
            var imgFullPath = Path.Combine(_env.WebRootPath, "db_files", "img", imgFileName);

            if (File.Exists(imgFullPath))
            {
                File.Delete(imgFullPath);
            }
        }

        public static void ResizeToQuadratic(IMagickImage<ushort> image, int xySize = 225)
        {
            if (image.Height > xySize || image.Width > xySize)
            {
                image.Resize(xySize, xySize);
                image.Strip();
            }
        }

        public static void ResizeToRectangle(IMagickImage<ushort> image, int width = 850)
        {
            if (image.Width > width)
            {
                var proportion = 1 - ((image.Width - width) / image.Width);
                var resizingHeight = image.Height * proportion;
                image.Resize(width, resizingHeight);
                image.Strip();
            }
        }
    }
}
