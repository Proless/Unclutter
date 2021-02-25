using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Unclutter.Validation
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
	public class OnlyAsciiAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var valid = false;
			if (value is string stringValue)
			{
				if (!string.IsNullOrEmpty(stringValue))
				{
					valid = stringValue.All(c => c <= sbyte.MaxValue);
				}
			}
			return valid;
		}
	}
}
