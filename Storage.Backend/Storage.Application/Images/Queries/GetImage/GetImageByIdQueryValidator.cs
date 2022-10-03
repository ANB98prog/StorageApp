using FluentValidation;

namespace Storage.Application.Images.Queries.GetImage
{
    public class GetImageByIdQueryValidator : AbstractValidator<GetImageByIdQuery>
    {
        public GetImageByIdQueryValidator()
        {
            RuleFor(image => image.Id).NotNull();
            /*
             TODO 
            добавить для других полей
             */
        }
    }
}
