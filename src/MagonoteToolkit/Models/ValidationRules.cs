using System;
using System.Globalization;
using System.Windows.Controls;

namespace MagonoteToolkit.Models
{
    /// <summary>
    /// バリデーションルール集
    /// </summary>
    
    /// <summary>
    /// TimeSpan型のバリデーションルール
    /// </summary>
    public class TimeSpanValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value?.ToString();

            if (!TimeSpan.TryParse(input, out _))
            {
                return new ValidationResult(false, Resources.Strings.MessageValidationErrorInvalidTimeFormat);
            }

            return ValidationResult.ValidResult;
        }
    }
}
