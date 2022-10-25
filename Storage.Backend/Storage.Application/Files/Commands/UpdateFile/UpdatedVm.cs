using AutoMapper;
using Mapper;
using Storage.Application.Common.Models;

namespace Storage.Application.Files.Commands.UpdateFile
{
    /// <summary>
    /// Updated view model
    /// </summary>
    public class UpdatedVm : BaseFileActionModel
    {
        public UpdatedVm(bool ack)
        {
            Acknowledged = ack;
        }
    }
}
