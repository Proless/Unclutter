using DryIoc;
using Prism.Ioc;
using Prism.Ioc.Internals;

namespace Unclutter.Services.Container
{
    public class DryIocAdapterContainerExtensions : AdapterContainerExtensions<IContainer>
    {
        public DryIocAdapterContainerExtensions(IContainerExtension<IContainer> containerExtension, IContainerInfo containerInfo)
            : base(containerExtension, containerInfo) { }
    }
}
