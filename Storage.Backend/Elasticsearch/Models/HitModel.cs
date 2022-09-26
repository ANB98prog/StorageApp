using Mapper;
using Nest;

namespace Elasticsearch.Models
{
    /// <summary>
    /// Elastic hit model
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    public class HitModel<T> : IMapWith<IHit<T>>
        where T : class
    {
        /// <summary>
        /// Hit score
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Document data
        /// </summary>
        public T Document { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<IHit<T>, HitModel<T>>()
                .ForMember(m => m.Score, opt => opt.MapFrom(o => o.Score))
                .ForMember(m => m.Document, opt => opt.MapFrom(o => o.Source));
        }

    }
}
