using System;
using System.Threading.Tasks;

namespace Storage.Application.Interfaces
{
    /// <summary>
    /// Storage interface to work with elastic
    /// </summary>
    public interface IStorageDataService
    {
        /// <summary>
        /// Adds data to elastic storage
        /// </summary>
        /// <param name="data">Data to add</param>
        /// <typeparam name="T">Data type</typeparam>
        /// <returns>Item id</returns>
        public Task<Guid> AddDataToStorageAsync<T>(T data) where T : class;
    }
}
