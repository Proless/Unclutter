using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Unclutter.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class FileNameAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var valid = false;
			
			if (value is string stringValue)
			{
				if (!string.IsNullOrEmpty(stringValue))
				{
					valid = stringValue.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
				}
			}

			return valid;
		}
	}
}
