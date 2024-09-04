using System.ComponentModel.DataAnnotations;
using FoolProof.Core;

namespace FoolProof.Core.Tests.E2eTests.WebApp.Models
{
    public enum EducationalLevel
    {
        Elementary = 0,
        High,
        College,
        University,
        Ms,
        Phd
    }

    public class PersonalInfo
    {
        public string? FirstName { get; set; }

        [RequiredIfEmpty(nameof(FirstName))]
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public string? Country { get; set; }

        [RequiredIf(nameof(Country), "USA")]
        public string? Address { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        public bool ContactByPhone { get; set; }

        [RequiredIfTrue(nameof(ContactByPhone))]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [RequiredIfEmpty(nameof(BirthDate))]
        [Range(1, 200)]
        public int? Age { get; set; }

        public string? Occupation { get; set; }

        public EducationalLevel? Education { get; set; }

        [DataType(DataType.Date)]
        [RequiredIfNotEmpty(nameof(BirthDate))]
        [RequiredIf(nameof(Education), Operator.EqualTo, EducationalLevel.Elementary)]
        [GreaterThan(nameof(BirthDate))]
        [LessThan(nameof(HighScoolGraduationDate))]
        public DateTime? ElementaryScoolGraduationDate { get; set; }

        [DataType(DataType.Date)]
        [RequiredIfEmpty(nameof(ElementaryScoolGraduationDate))]
        [RequiredIf(nameof(Education), Operator.EqualTo, EducationalLevel.High)]
        [GreaterThan(nameof(ElementaryScoolGraduationDate))]
        public DateTime? HighScoolGraduationDate { get; set; }

        [DataType(DataType.Date)]
        [RequiredIfEmpty(nameof(HighScoolGraduationDate))]
        [RequiredIf(nameof(Age), Operator.GreaterThan, 18)]
        [GreaterThan(nameof(HighScoolGraduationDate))]
        public DateTime? CollegeGraduationDate { get; set; }
    }
}
