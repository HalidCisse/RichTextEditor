using System.Globalization;
using System.Windows.Controls;

namespace RichTextEditor.Extensions
{
    internal class NameValidator : ValidationRule
    {
        public bool IsNullable { get; set; } = false;

        public int Min { get; set; } = 1;
        public int Max { get; set; } = 240;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var name = ((string)value)?.Trim();

            if (IsNullable && string.IsNullOrEmpty(name))
                return new ValidationResult(true, null);

            if (string.IsNullOrEmpty(name)) return new ValidationResult(false, "");
            if (name.Length < Min) return new ValidationResult(false, "Too short");
            return name.Length > Max ? new ValidationResult(false, "Text too long") : new ValidationResult(true, null);
        }
    }
}
