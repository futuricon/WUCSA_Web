using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Entities.UserModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.EventModel
{
    public enum EventLocation
    {
        World = 1,
        National = 2
    }

    public class Event : IEntity<string>
    {
        public Event()
        {
            IsDeleted = false;
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        public DateTime PostedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter the event date")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Please enter the event end date")]
        public DateTime EventEndDate { get; set; }

        [StringLength(80)]
        public string Slug { get; set; }

        [StringLength(64)]
        public string CoverPhotoPath { get; set; }

        public virtual AppUser Author { get; set; }

        [Display(Name = "Results of the passed event")]
        public virtual Rank? Rank { get; set; }

        [Required(ErrorMessage = "Please select location")]
        [Display(Name = "Location")]
        public EventLocation EventLocation { get; set; }

        [Required(ErrorMessage = "Please enter title")]
        [StringLength(100, ErrorMessage = "Characters must be less than 100")]
        [Display(Name = "Title of the event")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите название")]
        [StringLength(100, ErrorMessage = "Символов должно быть меньше 100")]
        [Display(Name = "Название события")]
        public string TitleRu { get; set; }

        [Required(ErrorMessage = "Iltimos, y'onalishni nomini kiriting")]
        [StringLength(100, ErrorMessage = "Belgilar 100 dan kam bo'lishi kerak")]
        [Display(Name = "Name of ")]
        public string TitleUz { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Пожалуйста, введите описание")]
        [Display(Name = "Описание")]
        public string DescriptionRu { get; set; }

        [Required(ErrorMessage = "Iltimos, Tavsif kiriting")]
        [Display(Name = "Tavsif")]
        public string DescriptionUz { get; set; }

        [Required(ErrorMessage = "Please enter the country, state or city of the rank")]
        [StringLength(100, ErrorMessage = "Characters must be less than 100")]
        [Display(Name = "Event Location")]
        public string Location { get; set; }

        [Display(Name = "URL of the event video")]
        public string? EventPromoVideo { get; set; }

        public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();

        public virtual ICollection<EventSportType> EventSportTypes { get; set; } = new List<EventSportType>();

        public virtual ICollection<EventRelatedFile> EventRelatedFiles { get; set; } = new List<EventRelatedFile>();
        
        public bool IsDeleted { get; set; }

        public static string GetShortContent(string articleContent, int length)
        {
            var content = HttpUtility.HtmlDecode(articleContent);
            content = Regex.Replace(content, @"<(.|\n)*?>", "");
            if (content.Length > 200)
            {
                content = content.Substring(0, length).Trim() + "...";
            }

            return content;
        }
    }
}
