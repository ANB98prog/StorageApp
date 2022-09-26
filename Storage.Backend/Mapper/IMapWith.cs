using AutoMapper;

namespace Mapper
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
