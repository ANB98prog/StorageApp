using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Gets file info
        /// </summary>
        /// <param name="id">File id</param>
        /// <returns>File info</returns>
        public Task<FileInfoModel?> GetFileInfoAsync(Guid id);

        /// <summary>
        /// Gets files infos
        /// </summary>
        /// <param name="ids">Files ids</param>
        /// <exception cref="ElasticStorageServiceException"></exception>
        /// <returns>Files infos</returns>
        public Task<List<FileInfoModel>> GetFilesInfoAsync(List<Guid> ids);

        /// <summary>
        /// Removes file from storage
        /// </summary>
        /// <param name="id">Item id</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexNotFoundException"></exception>
        /// <exception cref="DeleteDocumentException"></exception>
        /// <exception cref="UnexpectedElasticException"></exception>
        /// <returns>Acknowledged</returns>
        public Task<bool> RemoveFileFromStorageAsync(Guid id);

        /// <summary>
        /// Removes files from elastic storage
        /// </summary>
        /// <param name="id">File id</param>
        /// <returns>Acknowledged</returns>
        public Task<bool> RemoveFilesFromStorageAsync(List<Guid> id);
    }
}
