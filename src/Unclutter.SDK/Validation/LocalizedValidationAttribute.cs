using System;
using System.ComponentModel.DataAnnotations;
using Unclutter.SDK.Services;

namespace Unclutter.SDK.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class LocalizedValidationAttribute : ValidationAttribute
    {
        public string ErrorMessageKey { get; set; }
        public string KeyAssemblyName { get; set; } = ResourceKeys.DefaultAssemblyName;
        public string KeyDictionaryName { get; set; } = ResourceKeys.DefaultDictionaryName;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (IsValid(value))
            {
                return ValidationResult.Success;
            }

            var localizationService = (ILocalizationProvider)validationContext.GetService(typeof(ILocalizationProvider));
            var message = localizationService?.GetLocalizedString(ErrorMessageKey, KeyAssemblyName, KeyDictionaryName);
            return new ValidationResult(message);
        }
    }
}
