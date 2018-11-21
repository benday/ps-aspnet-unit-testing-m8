using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Benday.Presidents.Api.Models
{
    public class President : IValidatableObject
    {
        public President()
        {
            Terms = new List<Term>();

            FirstName = String.Empty;
            LastName = String.Empty;
            BirthCity = String.Empty;
            BirthState = String.Empty;
            DeathCity = String.Empty;
            DeathState = String.Empty;
        }

        public int Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ImageFilename { get; set; }

        public List<Term> Terms { get; private set; }

        [Display(Name = "Date of Birth")]
        [DateTimePropertyCompareValidatorAttribute(
            DateTimeCompareTypeEnum.LessThan, nameof(DeathDate))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Date of Death")]
        [DateTimePropertyCompareValidatorAttribute(
            DateTimeCompareTypeEnum.GreaterThan, nameof(BirthDate))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DeathDate { get; set; }

        [Display(Name = "Birth City")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BirthCity { get; set; }

        [Display(Name = "Birth State")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BirthState { get; set; }

        [Display(Name = "Death City")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DeathCity { get; set; }

        [Display(Name = "Death State")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string DeathState { get; set; }

        [Display(Name = "Days In Office")]
        public int DaysInOffice { get; set; }

        public void AddTerm(string role, DateTime startDate, DateTime endDate, int number)
        {
            Terms.Add(new Term()
            {
                Role = role,
                Start = startDate,
                End = endDate,
                Number = number
            });
        }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (Terms.Count == 0)
            {
                yield return
                    new ValidationResult("President has no terms.");
            }

            if (Terms.Count > 2)
            {
                yield return
                    new ValidationResult("President cannot have more than 2 terms.");
            }
        }
    }
}
