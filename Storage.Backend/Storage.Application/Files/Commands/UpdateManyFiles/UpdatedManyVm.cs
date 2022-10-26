using AutoMapper;
using Mapper;
using Storage.Application.Common.Models;

namespace Storage.Application.Files.Commands.UpdateManyFiles
{
    /// <summary>
    /// Update many files view model
    /// </summary>
    public class UpdatedManyVm : UpdateBulkFilesAttributesModel, IMapWith<UpdateBulkFilesAttributesModel>
    {
        public UpdatedManyVm()
        {
        }

        public UpdatedManyVm(bool ack)
        {
            Acknowledged = ack;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBulkFilesAttributesModel, UpdatedManyVm>()
                .ForMember(vm => vm.Acknowledged, opt => 
                    opt.MapFrom(bulk => bulk.Acknowledged))
                .ForMember(vm => vm.Errors, opt =>
                    opt.MapFrom(bulk => bulk.Errors))
                .ForMember(vm => vm.Count, opt =>
                    opt.MapFrom(bulk => bulk.Count));
        }
    }
}
