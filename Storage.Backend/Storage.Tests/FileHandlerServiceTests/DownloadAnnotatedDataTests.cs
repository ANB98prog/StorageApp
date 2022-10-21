using Newtonsoft.Json;
using Storage.Application.Common.Models;
using Storage.Domain;
using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.FileHandlerServiceTests
{
    [Collection("TestServicesCollection")]
    public class DownloadAnnotatedDataTests : TestServicesFixture
    {
        [Fact]
        public async Task DownloadAnnotatedData_Success()
        {
            var annotatedFilesInfos = new List<AnnotationFileInfo>()
            {
                new AnnotationFileInfo
                {
                    Id = Guid.NewGuid(),
                    Name = "File_1_1.txt",
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>
                        {
                            new Domain.AnnotatedClass(0, "1_1"),
                            new Domain.AnnotatedClass(1, "1_2"),
                        }
                    }
                },
                new AnnotationFileInfo
                {
                    Id = Guid.NewGuid(),
                    Name = "File_1_1.txt",
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>
                        {
                            new Domain.AnnotatedClass(0, "1_1"),
                            new Domain.AnnotatedClass(1, "1_2"),
                        }
                    }
                },new AnnotationFileInfo
                {
                    Id = Guid.NewGuid(),
                    Name = "File_2_1.txt",
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>
                        {
                            new Domain.AnnotatedClass(0, "2_1"),
                            new Domain.AnnotatedClass(1, "2_2"),
                        }
                    }
                },
                new AnnotationFileInfo
                {
                    Id = Guid.NewGuid(),
                    Name = "File_2_1.txt",
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>
                        {
                            new Domain.AnnotatedClass(0, "2_1"),
                            new Domain.AnnotatedClass(1, "2_2"),
                        }
                    }
                }
            };

            var temp = new List<AnnotationFileInfo>(annotatedFilesInfos);
            var groups = new Dictionary<Guid, List<AnnotationFileInfo>>();

            while (temp.Any())
            {
                var group = temp.FirstOrDefault();

                if(group == null)
                {
                    continue;
                }

                temp.Remove(group);

                var groupItems = temp.Where(x => x.Annotation.Equals(group.Annotation)).ToList();

                if(groupItems != null
                    && groupItems.Any())
                {
                    groupItems.Add(group);
                    groups.Add(group.Id, groupItems);
                }

                temp.RemoveAll(x => x.Annotation.Equals(group.Annotation));
            }


        }

        public class StringComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y)
            {
                return x.Equals(y);
            }

            public int GetHashCode([DisallowNull] string obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
