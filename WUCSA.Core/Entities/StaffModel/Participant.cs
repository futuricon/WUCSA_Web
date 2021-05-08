using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WUCSA.Core.Entities.Base;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Interfaces;

namespace WUCSA.Core.Entities.StaffModel
{
    public class Participant : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateComplex();

        [Required(ErrorMessage = "Please enter Name")]
        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [StringLength(40, ErrorMessage = "Characters must be less than 40")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; }

        [StringLength(400, ErrorMessage = "Characters must be less than 400")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(400, ErrorMessage = "Символов должно быть меньше 400")]
        [Display(Name = "Описание")]
        public string DescriptionRu { get; set; }

        [StringLength(400, ErrorMessage = "Belgilar 400 dan kam bo'lishi kerak")] 
        [Display(Name = "Tavsif")]
        public string DescriptionUz { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "This is not valid number")]
        [Display(Name = "Weight in kilograms")]
        public int Weight { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "This is not valid number")]
        [Display(Name = "Height in centimeters")]
        public int Height { get; set; }

        [Required(ErrorMessage = "Please enter the position number in the rank")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "This is not valid number")]
        [Display(Name = "Position number")]
        public int PositionNumber { get; set; }

        [Required(ErrorMessage = "Please enter the country, state or city of the representative")]
        [StringLength(100, ErrorMessage = "Characters must be less than 100")]
        [Display(Name = "Representative from")]
        public string Location { get; set; }

        public virtual EventParticipant EventParticipant { get; set; }

        public virtual RankParticipant RankParticipant { get; set; }
    }
}