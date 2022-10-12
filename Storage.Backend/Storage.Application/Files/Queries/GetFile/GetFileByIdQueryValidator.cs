using FluentValidation;

namespace Storage.Application.Files.Queries.GetFile
{
    public class GetFileByIdQueryValidator : AbstractValidator<GetFileByIdQuery>
    {
        public GetFileByIdQueryValidator()
        {
            RuleFor(file => file.Id).NotNull();
            /*
             TODO 
            добавить для других полей
             */
        }
    }
}
