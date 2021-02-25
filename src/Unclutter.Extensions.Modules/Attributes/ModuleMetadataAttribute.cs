using System;

namespace Unclutter.Extensions.Modules.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleMetadataAttribute : Attribute, IModuleMetadata
    {
        public ModuleMetadataAttribute(string name, string version, string author, string description = "")
        {
            Name = name;
            Version = version;
            Author = author;
            Description = description;
        }
        public string Name { get; }
        public string Version { get; }
        public string Author { get; }
        public string Description { get; }
    }
}
