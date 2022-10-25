﻿using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
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
        public Task<T> GetFileInfoAsync<T>(Guid id) where T : class;

        /// <summary>
        /// Gets files infos
        /// </summary>
        /// <param name="ids">Files ids</param>
        /// <exception cref="ElasticStorageServiceException"></exception>
        /// <returns>Files infos</returns>
        public Task<List<T>> GetFilesInfoAsync<T>(List<Guid> ids) where T : class;

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

        /// <summary>
        /// Updates file's attributes
        /// </summary>
        /// <param name="update">Update model</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated result</returns>
        /// <exception cref="ElasticStorageServiceException"></exception>
        public Task<UpdatedFileAttributesResponseModel> UpdateFileAttributesAsync(UpdateFileAttributesModel update, CancellationToken cancellationToken);

        /// <summary>
        /// Updates bulk files attributes
        /// </summary>
        /// <param name="updates">Bulk updates</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated result</returns>
        /// <exception cref="ElasticStorageServiceException"></exception>
        public Task<UpdateBulkFilesAttributesModel> UpdateBulkFilesAttributesAsync(List<UpdateFileAttributesModel> updates, CancellationToken cancellationToken);
    }
}
