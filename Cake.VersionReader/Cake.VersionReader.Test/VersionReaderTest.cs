using System;
using System.Diagnostics.CodeAnalysis;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cake.VersionReader.Tests
{
    public class VersionReaderTest
    {
        [Test]
        public void RetrieveVersionNumber()
        {
            // arrange
            const string filePath = @"./TestData/SemVerTest.dll";
            var fixture = new VersionReaderFixture();
            var sut = fixture.CakeContext;

            // act
            var result = sut.GetVersionNumber(filePath);

            // assert
            result.Should().Be("1.1.2");
        }

        [Test]
        public void RetrieveBuildServerVersionNumber()
        {
            // arrange
            const string filePath = @"./TestData/SemVerTest.dll";
            var fixture = new VersionReaderFixture();
            var sut = fixture.CakeContext;

            // act
            var result = sut.GetVersionNumberWithContinuesIntegrationNumberAppended(filePath, 5);

            // assert
            result.Should().Be("1.1.2-CI00005");
        }

        [Test]
        public void RetrieveFullVersionNumber()
        {
            // arrange
            const string filePath = @"./TestData/SemVerTest.dll";
            var fixture = new VersionReaderFixture();
            var sut = fixture.CakeContext;

            // act
            var result = sut.GetFullVersionNumber(filePath);

            // assert
            result.Should().Be("1.1.2.0");
        }

        [Test]
        public void RetrieveFullBuildServerVersionNumber()
        {
            // arrange
            const string filePath = @"./TestData/SemVerTest.dll";
            var fixture = new VersionReaderFixture();
            var sut = fixture.CakeContext;

            // act
            var result = sut.GetFullVersionNumberWithContinuesIntegrationNumberAppended(filePath, 12);
            
            // assert
            result.Should().Be("1.1.2.0-CI00012");
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local", Justification = "Test Fixture - Unit tests require access to Mocks")]
        private class VersionReaderFixture
        {
            public VersionReaderFixture()
            {
                var appPath = AppDomain.CurrentDomain.BaseDirectory;
                CakeEnviromentMock
                    .Setup(t => t.WorkingDirectory)
                    .Returns(new DirectoryPath(appPath));
                CakeContextMock
                    .Setup(t => t.ProcessRunner)
                    .Returns(ProcessRunner);
                CakeContextMock
                    .Setup(t => t.FileSystem)
                    .Returns(FileSystem);
                CakeContextMock
                    .Setup(t => t.Log)
                    .Returns(CakeLog);
                CakeContextMock
                    .Setup(t => t.Environment)
                    .Returns(CakeEnvironment);
                ProcessRunnerMock
                    .Setup(t => t.Start(It.IsAny<FilePath>(), It.IsAny<ProcessSettings>()))
                    .Returns(ProcessMock.Object);
                ProcessMock
                    .Setup(t => t.GetExitCode())
                    .Returns(0);

            }

            public Mock<ICakeContext> CakeContextMock { get; } = new Mock<ICakeContext>();
            public ICakeContext CakeContext => CakeContextMock.Object;

            public Mock<IProcessRunner> ProcessRunnerMock { get; } = new Mock<IProcessRunner>();
            public IProcessRunner ProcessRunner => ProcessRunnerMock.Object;

            public Mock<IFileSystem> FileSystemMock { get; } = new Mock<IFileSystem>();
            public IFileSystem FileSystem => FileSystemMock.Object;

            public Mock<ICakeLog> CakeLogMock { get; } = new Mock<ICakeLog>();
            public ICakeLog CakeLog => CakeLogMock.Object;

            public Mock<IProcess> ProcessMock { get; } = new Mock<IProcess>();
            public IProcess Process => ProcessMock.Object;

            public Mock<ICakeEnvironment> CakeEnviromentMock { get; } = new Mock<ICakeEnvironment>();
            public ICakeEnvironment CakeEnvironment => CakeEnviromentMock.Object;
        }
    }
}