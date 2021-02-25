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
    public class Blog : ISlugifiedEntity
    {
        public Blog()
        {
            CoverPhotoPath = "/img/default_blog_cover.png";
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(80)]
        public string Slug { get; set; }

        [StringLength(64)]
        public string CoverPhotoPath { get; set; }

        public virtual AppUser Author { get; set; }

        [Required(ErrorMessage = "Please enter the article topic name")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter the article topic name")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        public string TitleRu { get; set; }

        [Required(ErrorMessage = "Please enter the article topic name")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        public string TitleUz { get; set; }

        [Required(ErrorMessage = "Please enter the summary of article")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Please enter the summary of article")]
        public string SummaryRu { get; set; }

        [Required(ErrorMessage = "Please enter the summary of article")]
        public string SummaryUz { get; set; }

        public int ViewCount { get; set; }

        public ICollection<BlogTag> BlogTags { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public static string GetShortContent(string articleContent, int length)
        {
            var content = HttpUtility.HtmlDecode(articleContent);
            content = Regex.Replace(content, @"<(.|\n)*?>", "");
            content = content.Substring(0, length).Trim() + "...";
            return content;
        }
    }
}
