using AutoMapper;

namespace Storage.Application.Common.Mappings
{
    /// <summary>
    /// Interface of mapping configuration
    /// </summary>
    /// <typeparam name="T">Map with type</typeparam>
    public interface IMapWith<T>
    {
        void Mapping(Profile profile) =>
            profile.CreateMap(typeof(T), GetType());
    }
}
