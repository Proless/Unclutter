using System.Linq;

namespace Unclutter.SDK.Validation
{
    public class OnlyAsciiAttribute : LocalizedValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var valid = false;
            if (value is string stringValue)
            {
                valid = string.IsNullOrWhiteSpace(stringValue) || stringValue.All(c => c <= sbyte.MaxValue);
            }
            return valid;
        }
    }
}
