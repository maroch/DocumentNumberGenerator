using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace DocumentNumberGenerator.Validators
{
    public class OnlyNumbersValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var result = new ValidationResult(true, null);

            string NumberPattern = @"^[1-9][0-9]*$";
            Regex rgx = new Regex(NumberPattern);

            if (rgx.IsMatch(value.ToString()) == false)
            {
                result = new ValidationResult(false, "Must be only numbers");
            }

            return result;
        }
    }
}
