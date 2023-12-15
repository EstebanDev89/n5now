using Application.DTOs;
using Application.Enums;
using Application.Features.Permissions.Messages;
using Application.Features.Permissions.Services;
using AutoMapper;
using FluentAssertions;
using Moq;
using Nest;

namespace n5now.Testing.Core.Application.Features.Permission.Services
{
    public class GetPermissionsServiceTests
    {
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IPermissionEventPublisher> permissionEventPublisherMock;
        private readonly Mock<IElasticClient> elasticClientMock;
        private readonly Mock<ISearchResponse<Domain.Entities.Permission>> mockSearchResponse;
        private readonly PermissionDTO permssionDTO = new PermissionDTO()
        {
            EmployeeForename = "EmployeeForename",
            EmployeeSurname = "EmployeeForename",
            PermissionDate = DateTime.Now,
            Id = 1,
            PermissionTypeId = 1
        };

        private GetPermissionsService Sut { get; set; }

        public GetPermissionsServiceTests()
        {
            mapperMock = new Mock<IMapper>();
            permissionEventPublisherMock = new Mock<IPermissionEventPublisher>();
            elasticClientMock = new Mock<IElasticClient>();

            mapperMock.Setup(s => s.Map<PermissionDTO>(It.IsAny<Domain.Entities.Permission>()))
                .Returns(permssionDTO);

            permissionEventPublisherMock.Setup(pep => pep.ProduceAsync(It.IsAny<PermissionMessage>()));

            var permissions = new List<Domain.Entities.Permission>
            {
                new Domain.Entities.Permission()
                {
                    EmployeeForename = "EmployeeForename",
                    EmployeeSurname = "EmployeeForename",
                    PermissionDate = DateTime.Now,
                    Id = 1,
                    PermissionType = new Domain.Entities.PermissionType() { Id = 1, Description = "Description"},
                    PermissionTypeId = 1
                }
            };

            var hits = new List<IHit<Domain.Entities.Permission>>
            {
                new Mock<IHit<Domain.Entities.Permission>>().Object
            };

            mockSearchResponse = new Mock<ISearchResponse<Domain.Entities.Permission>>();
            mockSearchResponse.Setup(x => x.Documents).Returns(permissions);
            mockSearchResponse.Setup(x => x.Hits).Returns(hits);

            elasticClientMock.Setup(x => x
                .SearchAsync(It.IsAny<Func<SearchDescriptor<Domain.Entities.Permission>, ISearchRequest>>(), It.IsAny<CancellationToken>())
            ).ReturnsAsync(mockSearchResponse.Object);

            Sut = new GetPermissionsService(mapperMock.Object, permissionEventPublisherMock.Object, elasticClientMock.Object);
        }

        public class GetByIdMethod : GetPermissionsServiceTests
        {
            [Fact]
            public async Task SearchAsyncCalled()
            {
                //Act
                var result = await Sut.GetById(1);

                //Assert
                elasticClientMock.Verify(x => x.SearchAsync(
                    It.IsAny<Func<SearchDescriptor<Domain.Entities.Permission>, ISearchRequest>>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async Task ProduceAsyncCalled()
            {
                //Act
                var result = await Sut.GetById(1);

                //Assert
                permissionEventPublisherMock.Verify(
                    x => x.ProduceAsync(It.Is<PermissionMessage>(msg => IsEquivalentTo(msg, new PermissionMessage(Operation.Get)))),
                    Times.Once);
            }

            [Fact]
            public async Task ShouldReturnRightPermission()
            {
                //Act
                var result = await Sut.GetById(1);
                
                //Assert
                result.Should().BeEquivalentTo(permssionDTO);
            }

        }

        private static bool IsEquivalentTo(PermissionMessage actual, PermissionMessage expected)
        {
            actual.Should().BeEquivalentTo(expected, options => options.Excluding(p => p.Id));
            return true;
        }
    }

}