using Storage.Application.Common.Models;

namespace Storage.Application.Files.Commands.UpdateManyFiles
{
    /// <summary>
    /// Update many files view model
    /// </summary>
    public class UpdatedManyVm : UpdateBulkFilesAttributesModel
    {
        public UpdatedManyVm()
        {
        }

        public UpdatedManyVm(bool ack)
        {
            Acknowledged = ack;
        }
    }
}
