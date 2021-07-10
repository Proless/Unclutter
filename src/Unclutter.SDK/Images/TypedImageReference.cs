#nullable enable
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Unclutter.SDK.Images
{
    /// <summary>
    /// Create a new <see cref="ImageReference"/> from a type.
    /// </summary>
    public class TypedImageReference : ImageReference
    {
        /* Properties */
        public Type ReferencedType { get; }
        public object[]? ConstructorArgs { get; protected set; }
        public IDictionary<string, object>? PropsMap { get; protected set; }
        public override bool IsDefault => false;
        public override bool HasImageSource => false;

        /* Constructors */
        /// <summary>
        /// Create a new <see cref="ImageReference"/> from a type.
        /// </summary>
        /// <param name="referencedType">The instance of the image object, which will be returned when this reference is resolved</param>
        /// <param name="constructorArgs">The constructor arguments to use when constructing the object instance, if any</param>
        /// <param name="propsMap">The values of the properties to set on the object instance</param>
        public TypedImageReference(Type referencedType, object[]? constructorArgs = null, IDictionary<string, object>? propsMap = null)
        {
            ReferencedType = referencedType ?? throw new ArgumentNullException(nameof(referencedType));
            ConstructorArgs = constructorArgs;
            PropsMap = propsMap;

        }

        /* Methods */
        public override object? GetImageObject(ImageOptions? options = null)
        {
            object? instance;
            try
            {
                instance = CreateInstance();
                ApplyPropsMap(instance, options);
            }
            catch
            {
                instance = null;
            }
            return instance;
        }
        public override BitmapImage? GetImageSource(ImageOptions? options = null)
        {
            return null;
        }

        /* Helpers */
        protected object? CreateInstance()
        {
            var args = ConstructorArgs ?? Array.Empty<object>();

            var instance = args.Length > 0
                ? Activator.CreateInstance(ReferencedType, args)
                : Activator.CreateInstance(ReferencedType);

            return instance;
        }
        protected void ApplyPropsMap(object? target, ImageOptions? options)
        {
            if (target == null) return;

            if (PropsMap != null)
            {
                var type = target.GetType();

                if (options != null)
                {
                    PropsMap["Width"] = options.Width;
                    PropsMap["Height"] = options.Height;
                }

                foreach (var (key, value) in PropsMap)
                {
                    var propertyInfo = type.GetProperty(key);

                    if (propertyInfo != null)
                    {
                        propertyInfo.SetValue(target, value, null);
                    }
                }
            }
        }
    }
}
