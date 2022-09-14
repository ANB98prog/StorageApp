﻿using AutoMapper;
using Moq;
using Ninject;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.Common
{
    public class TestCommandsFixture : IDisposable
    {
        protected readonly Mock<IFileHandlerService> FileHandlerServiceMock;

        public TestCommandsFixture()
        {
            if (!Directory.Exists(TestConstants.TestFilesDirectory))
                Directory.CreateDirectory(TestConstants.TestFilesDirectory);

            FileHandlerServiceMock = new Mock<IFileHandlerService>();
        }

        public void Dispose()
        {
            TestHelper.RemoveTestData(TestConstants.TestFilesDirectory);
            TestHelper.RemoveTestData(TestConstants.CommandsFilesDirectory);
            TestHelper.RemoveTestData(TestConstants.StorageDirectory);
        }

        [CollectionDefinition("TestCommandsCollection")]
        public class TestCommandsCollection : ICollectionFixture<TestCommandsFixture> { }
    }
}
