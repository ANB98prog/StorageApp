using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace Mapper
{
    /// <summary>
    /// Assebles mapping profiles
    /// </summary>
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly) =>
            ApplyMappingsFromAssembly(assembly);

        /// <summary>
        /// Applies mapping from assembly
        /// </summary>
        /// <param name="assembly">Assembly</param>
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                        .Where(t => t.GetInterfaces()
                            .Any(i => i.IsGenericType 
                                && i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
                        .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
