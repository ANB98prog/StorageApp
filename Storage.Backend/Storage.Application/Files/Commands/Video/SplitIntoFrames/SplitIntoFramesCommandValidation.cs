using FluentValidation;
using System;

namespace Storage.Application.Files.Commands.Video.SplitIntoFrames
{
    public class SplitIntoFramesCommandValidation
        : AbstractValidator<SplitIntoFramesCommand>
    {
        public SplitIntoFramesCommandValidation()
        {
            //RuleFor(split => split.UserId)
            //    .NotEqual(Guid.Empty);
            RuleFor(split => split.VideoFileId)
               .NotEqual(Guid.Empty);
            RuleFor(split => split.FramesStep)
               .GreaterThanOrEqualTo(0);
        }
    }
}
