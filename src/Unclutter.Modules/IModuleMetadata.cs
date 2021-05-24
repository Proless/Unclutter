using System;

namespace Unclutter.Modules
{
    public interface IModuleMetadata
    {
        string Name { get; }
        string Version { get; }
        string Author { get; }
        string Description { get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModuleMetadataAttribute : Attribute, IModuleMetadata
    {
        public string Name { get; }
        public string Version { get; }
        public string Author { get; }
        public string Description { get; }

        public ModuleMetadataAttribute(string name, string version, string author, string description = "")
        {
            Name = name;
            Version = version;
            Author = author;
            Description = description;
        }
    }
}
