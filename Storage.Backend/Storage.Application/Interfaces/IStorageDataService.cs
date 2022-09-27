using System;
using System.Threading.Tasks;

namespace Storage.Application.Interfaces
{
    /// <summary>
    /// Storage interface to work with elastic
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public interface IStorageDataService<T> where T : class
    {
        /// <summary>
        /// Adds data to elastic storage
        /// </summary>
        /// <param name="data">Data to add</param>
        /// <returns>Item id</returns>
        public Task<Guid> AddDataToStorageAsync(T data);
    }
}
