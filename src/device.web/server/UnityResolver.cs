#region

using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

#endregion

namespace device.web.server
{
    /// <summary>
    /// Represents Unity dependency resolver.
    /// </summary>
    public class UnityResolver : IDependencyResolver
    {
        public static void CreateDefault(IUnityContainer container)
        {
            Default = new UnityResolver(container);
        }

        public static UnityResolver Default { get; private set; }

        /// <summary>
        /// The Unity container
        /// </summary>
        protected IUnityContainer Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        private UnityResolver(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            Container = container;
        }

        /// <summary>
        /// Gets the service by type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The service instance or null.</returns>
        public object GetService(Type serviceType)
        {
            try
            {
                return Container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the services by type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The service instances or empty collection.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return Container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// Begins the scope.
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            var child = Container.CreateChildContainer();
            return new UnityResolver(child);
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Container.Dispose();
        }
    }
}