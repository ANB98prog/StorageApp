﻿using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.WebApi.Models;
using Storage.Domain;

namespace Storage.WebApi.Controllers.AnnotatedFiles
{
    [Produces("application/json")]
    [Route("api/files/annotated")]
    public class AnnotatedFilesController : BaseController
    {
        /// <summary>
        /// Uploads annotated files
        /// </summary>
        /// <param name="request">Upload request</param>
        /// <returns></returns>
        [HttpPut("upload")]
        public async Task<ActionResult> UploadAnnotatedDataAsync([FromBody] UploadAnnotatedDataRequestModel request)
        {
            return Created("", "");
        }

        /// <summary>
        /// Downloads annotated data
        /// </summary>
        /// <param name="ids">Annotated files ids</param>
        /// <returns>Zip archive</returns>
        [HttpPost("download")]
        [Produces("application/zip")]
        public async Task<FileContentResult> DownloadAnnotatedDataAsync([FromBody] string[] ids)
        {
            using(var file = System.IO.File.Create("temp.zip"))
            {
            }

            return File(System.IO.File.ReadAllBytes("temp.zip"),  "application/zip", "annotatedData.zip");

        }

        /// <summary>
        /// Gets annotation formats
        /// </summary>
        /// <returns>List of formats</returns>
        [HttpGet("formats")]
        public async Task<ActionResult<List<string>>> GetAnnotationsFormatsAsync()
        {
            return Ok(Enum.GetNames(typeof(AnnotationFormats)));
        }
    }
}
