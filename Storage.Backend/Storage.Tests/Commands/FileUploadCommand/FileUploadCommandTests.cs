using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Images.Commands.UploadImage;
using Storage.Tests.Common;

namespace Storage.Tests.Commands.FileUploadCommand
{
    [Collection("TestCommandsCollection")]
    public class FileUploadCommandTests : TestCommandsFixture
    {
        [Fact]
        public async Task FileUploadCommand_Success()
        {
            var handler = new UploadImageCommandHandler(FileHandlerServiceMock.Object);

            var dir = Path.Combine(TestConstants.CommandsFilesDirectory);

            Directory.CreateDirectory(dir);

            var stream = File.Create(Path.Combine(dir, "upload.txt"));

            var file = new FormFile(stream, 0, stream.Length, "upload.txt", "upload.txt");
            file.Headers = new HeaderDictionary();
            file.ContentType = "plain/text";

            var request = new UploadImageCommand
            {
                UserId = Guid.NewGuid(),
                Attributes = new List<string> { "human", "boy" },
                IsAnnotated = true,
                ImageFile = file
            };

            var fileId = Guid.NewGuid();

            FileHandlerServiceMock.Setup(mock =>
                mock.UploadFileAsync(It.IsAny<UploadFileRequestModel>(), CancellationToken.None))
                .ReturnsAsync(fileId);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.False(result.Equals(Guid.Empty));

            stream.Dispose();
        }

        [Fact]
        public async Task FileUploadCommand_Error_IfModelIsEmpty()
        {
            var handler = new UploadImageCommandHandler(FileHandlerServiceMock.Object);

            var error = await Assert.ThrowsAsync<FileUploadingException>(async () =>
                            await handler.Handle(null, CancellationToken.None));

            Assert.Equal(ErrorMessages.ArgumentNullExeptionMessage("request"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task FileUploadCommand_Error_IfFileIsEmpty()
        {
            var handler = new UploadImageCommandHandler(FileHandlerServiceMock.Object);

            var error = await Assert.ThrowsAsync<FileUploadingException>(async () =>
                            await handler.Handle(new UploadImageCommand(), CancellationToken.None));

            Assert.Equal(ErrorMessages.ArgumentNullExeptionMessage("ImageFile"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task FileUploadCommand_Error_IfFileServiceThrowsException()
        {
            var handler = new UploadImageCommandHandler(FileHandlerServiceMock.Object);

            var dir = Path.Combine(TestConstants.CommandsFilesDirectory);

            Directory.CreateDirectory(dir);

            var stream = File.Create(Path.Combine(dir, "upload2.txt"));

            var file = new FormFile(stream, 0, stream.Length, "upload.txt", "upload.txt");
            file.Headers = new HeaderDictionary();
            file.ContentType = "plain/text";

            var request = new UploadImageCommand
            {
                UserId = Guid.NewGuid(),
                Attributes = new List<string> { "human", "boy" },
                IsAnnotated = true,
                ImageFile = file
            };

            FileHandlerServiceMock.Setup(mock =>
                mock.UploadFileAsync(It.IsAny<UploadFileRequestModel>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var error = await Assert.ThrowsAsync<FileUploadingException>(async () =>
                            await handler.Handle(request, CancellationToken.None));

            stream.Dispose();

            Assert.Equal(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE, error.UserFriendlyMessage);
        }
    }
}
