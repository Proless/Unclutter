using System.IO;

namespace Unclutter.SDK.Validation
{
    public class FileNameAttribute : LocalizedValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var valid = false;

            if (value is string stringValue)
            {
                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    valid = stringValue.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
                }
            }

            return valid;
        }
    }
}
