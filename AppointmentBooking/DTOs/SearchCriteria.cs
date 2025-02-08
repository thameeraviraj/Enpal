using System.ComponentModel.DataAnnotations;

namespace AppointmentBooking.DTOs
{
    public class SearchCriteria
    {
        [Required(ErrorMessage = "Date is required.")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "At least one product must be specified.")]
        [MinLength(1, ErrorMessage = "At least one product must be provided.")]
        [ProductValidation(ErrorMessage = "Invalid product. Allowed values: SolarPanels, Heatpumps.")]
        public string[] Products { get; set; }

        [Required(ErrorMessage = "Language is required.")]
        [RegularExpression("^(German|English)$", ErrorMessage = "Invalid language. Allowed values: German, English.")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [RegularExpression("^(Gold|Silver|Bronze)$", ErrorMessage = "Invalid rating. Allowed values: Gold, Silver, Bronze.")]
        public string Rating { get; set; }
    }

    /// <summary>
    /// Custom validation attribute for checking product names.
    /// </summary>
    public class ProductValidation : ValidationAttribute
    {
        private static readonly HashSet<string> AllowedProducts = new() { "SolarPanels", "Heatpumps" };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string[] products)
            {
                if (products.All(p => AllowedProducts.Contains(p)))
                {
                    return ValidationResult.Success!;
                }
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}
