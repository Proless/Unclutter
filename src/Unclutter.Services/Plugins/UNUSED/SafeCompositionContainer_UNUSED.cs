using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Unclutter.Services.Plugins.UNUSED
{
    public class SafeCompositionContainer_UNUSED : CompositionContainer
    {
        public SafeCompositionContainer_UNUSED() : base() { }
        public SafeCompositionContainer_UNUSED(params ExportProvider[] providers) : base(providers) { }
        public SafeCompositionContainer_UNUSED(CompositionOptions compositionOptions, params ExportProvider[] providers) : base(compositionOptions, providers) { }
        public SafeCompositionContainer_UNUSED(ComposablePartCatalog catalog, params ExportProvider[] providers) : base(catalog, providers) { }
        public SafeCompositionContainer_UNUSED(ComposablePartCatalog catalog, bool isThreadSafe, params ExportProvider[] providers) : base(catalog, isThreadSafe, providers) { }
        public SafeCompositionContainer_UNUSED(ComposablePartCatalog catalog, CompositionOptions compositionOptions, params ExportProvider[] providers) : base(catalog, compositionOptions, providers) { }
    }
}
