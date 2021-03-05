using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.BlogModel
{
    public class Blog : ISlugifiedEntity, IEntity<string>
    {
        public Blog()
        {
            CoverPhotoPath = "/img/blog_image.png";
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(80)]
        public string Slug { get; set; }

        [StringLength(64)]
        public string CoverPhotoPath { get; set; }

        public virtual AppUser Author { get; set; }

        public DateTime PostedDate { get; set; }

        [Required(ErrorMessage = "Please enter the article title")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите заголовок статьи")]
        [StringLength(80, ErrorMessage = "Символов должно быть меньше 80")]
        [Display(Name = "Заголовок")]
        public string TitleRu { get; set; }

        [Required(ErrorMessage = "Iltimos, maqola sarlavhasini kiriting")]
        [StringLength(80, ErrorMessage = "Belgilar 80 dan kam bo'lishi kerak")]
        [Display(Name = "Sarlavha")]
        public string TitleUz { get; set; }

        [Required(ErrorMessage = "Please enter a summary of the article")]
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите содержание статьи")]
        [Display(Name = "Содержание")]
        public string SummaryRu { get; set; }

        [Required(ErrorMessage = "Iltimos, maqolaning mazmunini kiriting")]
        [Display(Name = "Mazmunin")]
        public string SummaryUz { get; set; }

        public int ViewCount { get; set; }

        public virtual ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public static string GetShortContent(string articleContent, int length)
        {
            var content = HttpUtility.HtmlDecode(articleContent);
            content = Regex.Replace(content, @"<(.|\n)*?>", "");
            content = content.Substring(0, length).Trim() + "...";
            return content;
        }
    }
}
