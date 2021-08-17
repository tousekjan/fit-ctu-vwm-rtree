using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Vwm.RTree.Api
{
  public static class ServiceCollectionExtensions
  {
    public static void AddAllTypesOf<TInterface>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      var interfaceType = typeof(TInterface);
      if (!interfaceType.IsInterface)
        throw new ArgumentException($"Generic type '{interfaceType}' is not an interface.", nameof(TInterface));
      if (interfaceType.IsGenericType)
        throw new NotImplementedException("Generic interfaces not implemented.");

      var serviceDescriptors = interfaceType.Assembly.ExportedTypes
        .Where(type => type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
        .Select(type => new ServiceDescriptor(interfaceType, type, lifetime));

      foreach (var serviceDescriptor in serviceDescriptors)
        services.Add(serviceDescriptor);
    }
  }
}
