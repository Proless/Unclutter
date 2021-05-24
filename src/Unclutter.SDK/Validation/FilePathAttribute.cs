using System;
using System.IO;
using System.Security;

namespace Unclutter.SDK.Validation
{
    public class FilePathAttribute : LocalizedValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string stringValue)
            {
                return IsValidPath(stringValue);
            }

            return false;
        }

        private bool IsValidPath(string path)
        {
            var isValid = false;

            if (string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                if (Path.IsPathRooted(path) && !string.IsNullOrWhiteSpace(Path.GetFullPath(path)))
                {
                    isValid = true;
                }
            }
            catch (ArgumentException) { }
            catch (SecurityException) { }
            catch (NotSupportedException) { }
            catch (PathTooLongException) { }

            return isValid;
        }
    }
}
