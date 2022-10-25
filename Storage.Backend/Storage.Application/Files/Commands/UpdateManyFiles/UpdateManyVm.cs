using Storage.Application.Common.Models;

namespace Storage.Application.Files.Commands.UpdateManyFiles
{
    /// <summary>
    /// Update many files view model
    /// </summary>
    public class UpdateManyVm : UpdateBulkFilesAttributesModel
    {
        public UpdateManyVm(bool ack)
        {
            Acknowledged = ack;
        }
    }
}
