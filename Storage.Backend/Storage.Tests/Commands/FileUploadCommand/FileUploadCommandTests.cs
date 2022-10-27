using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Files.Commands.UploadFile;
using Storage.Tests.Common;

namespace Storage.Tests.Commands.FileUploadCommand
{
    [Collection("TestCommandsCollection")]
    public class FileUploadCommandTests : TestCommandsFixture
    {
        [Fact]
        public async Task FileUploadCommand_Success()
        {
            var handler = new UploadFileCommandHandler(FileHandlerServiceMock.Object);

            var dir = Path.Combine(TestConstants.CommandsFilesDirectory);

            Directory.CreateDirectory(dir);

            var stream = File.Create(Path.Combine(dir, "upload.txt"));

            var file = new FormFile(stream, 0, stream.Length, "upload.txt", "upload.txt");
            file.Headers = new HeaderDictionary();
            file.ContentType = "plain/text";

            var request = new UploadFileCommand
            {
                UserId = Guid.NewGuid(),
                Attributes = new List<string> { "human", "boy" },
                IsAnnotated = true,
                File = file
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
            var handler = new UploadFileCommandHandler(FileHandlerServiceMock.Object);

            var error = await Assert.ThrowsAsync<UserException>(async () =>
                            await handler.Handle(null, CancellationToken.None));

            Assert.Equal(ErrorMessages.ArgumentNullExeptionMessage("request"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task FileUploadCommand_Error_IfFileIsEmpty()
        {
            var handler = new UploadFileCommandHandler(FileHandlerServiceMock.Object);

            var error = await Assert.ThrowsAsync<UserException>(async () =>
                            await handler.Handle(new UploadFileCommand(), CancellationToken.None));

            Assert.Equal(ErrorMessages.ArgumentNullExeptionMessage("File"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task FileUploadCommand_Error_IfFileServiceThrowsException()
        {
            var handler = new UploadFileCommandHandler(FileHandlerServiceMock.Object);

            var dir = Path.Combine(TestConstants.CommandsFilesDirectory);

            Directory.CreateDirectory(dir);

            var stream = File.Create(Path.Combine(dir, "upload2.txt"));

            var file = new FormFile(stream, 0, stream.Length, "upload.txt", "upload.txt");
            file.Headers = new HeaderDictionary();
            file.ContentType = "plain/text";

            var request = new UploadFileCommand
            {
                UserId = Guid.NewGuid(),
                Attributes = new List<string> { "human", "boy" },
                IsAnnotated = true,
                File = file
            };

            FileHandlerServiceMock.Setup(mock =>
                mock.UploadFileAsync(It.IsAny<UploadFileRequestModel>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var error = await Assert.ThrowsAsync<CommandExecutionException>(async () =>
                            await handler.Handle(request, CancellationToken.None));

            stream.Dispose();

            Assert.Equal(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE, error.UserFriendlyMessage);
        }
    }
}
