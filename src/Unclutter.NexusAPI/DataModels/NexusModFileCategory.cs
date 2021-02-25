using System.Runtime.Serialization;

namespace Unclutter.NexusAPI.DataModels
{
	public enum NexusModFileCategory
	{
		[EnumMember(Value = "MAIN")]
		Main = 1,

		[EnumMember(Value = "UPDATE")]
		Update = 2,

		[EnumMember(Value = "OPTIONAL")]
		Optional = 3,

		[EnumMember(Value = "OLD_VERSION")]
		Old = 4,

		[EnumMember(Value = "MISCELLANEOUS")]
		Miscellaneous = 5,

		[EnumMember(Value = "DELETED")]
		Deleted = 6
	}
}
