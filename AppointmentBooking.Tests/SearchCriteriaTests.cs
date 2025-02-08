using AppointmentBooking.DTOs;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;

namespace AppointmentBooking.Tests
{
    public class SearchCriteriaTests
    {
        private static ValidationResult ValidateModel(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, context, results, true);
            return isValid ? ValidationResult.Success! : results.First();
        }

        [Fact]
        public void SearchCriteria_ShouldBeValid_WithValidData()
        {
            var model = new SearchCriteria
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Products = new[] { "SolarPanels" },
                Language = "English",
                Rating = "Gold"
            };

            var result = ValidateModel(model);
            result.Should().Be(ValidationResult.Success);
        }

        [Fact]
        public void SearchCriteria_ShouldBeInvalid_WithInvalidLanguage()
        {
            var model = new SearchCriteria
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Products = new[] { "SolarPanels" },
                Language = "French", // Invalid
                Rating = "Gold"
            };

            var result = ValidateModel(model);
            result.ErrorMessage.Should().Be("Invalid language. Allowed values: German, English.");
        }

        [Fact]
        public void SearchCriteria_ShouldBeInvalid_WithInvalidProduct()
        {
            var model = new SearchCriteria
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Products = new[] { "InvalidProduct" }, // Invalid
                Language = "English",
                Rating = "Gold"
            };

            var result = ValidateModel(model);
            result.ErrorMessage.Should().Be("Invalid product. Allowed values: SolarPanels, Heatpumps.");
        }

        [Fact]
        public void SearchCriteria_ShouldBeInvalid_WithInvalidRating()
        {
            var model = new SearchCriteria
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                Products = new[] { "SolarPanels" },
                Language = "English",
                Rating = "Diamond" // Invalid
            };

            var result = ValidateModel(model);
            result.ErrorMessage.Should().Be("Invalid rating. Allowed values: Gold, Silver, Bronze.");
        }
    }
}
