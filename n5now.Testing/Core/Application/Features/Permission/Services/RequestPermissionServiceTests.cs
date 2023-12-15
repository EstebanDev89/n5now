using Application.DTOs;
using Application.Enums;
using Application.Features.Permissions.Commands.RequestPermission;
using Application.Features.Permissions.Messages;
using Application.Features.Permissions.Services;
using AutoMapper;
using FluentAssertions;
using Moq;
using Nest;
using Persistence.Repository;

namespace n5now.Testing.Core.Application.Features.Permission.Services
{
    public class RequestPermissionServiceTests
    {
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IPermissionEventPublisher> permissionEventPublisherMock;
        private readonly Mock<IElasticClient> elasticClientMock;
        private readonly Mock<IRepositoryAsync<Domain.Entities.Permission>> repositoryAsynctMock;

        private readonly Domain.Entities.Permission permission = new Domain.Entities.Permission()
        {
            EmployeeForename = "EmployeeForename",
            EmployeeSurname = "EmployeeForename",
            PermissionDate = DateTime.Now,
            Id = 1,
            PermissionType = new Domain.Entities.PermissionType() { Id = 1, Description = "Description" },
            PermissionTypeId = 1
        };

        private readonly Domain.Entities.Permission permissionAdded = new Domain.Entities.Permission()
        {
            EmployeeForename = "EmployeeForename2",
            EmployeeSurname = "EmployeeForename2",
            PermissionDate = DateTime.Now,
            Id = 1,
            PermissionType = new Domain.Entities.PermissionType() { Id = 2, Description = "Description2" },
            PermissionTypeId = 2
        };

        private readonly PermissionDTO permssionDTO = new PermissionDTO()
        {
            EmployeeForename = "EmployeeForename2",
            EmployeeSurname = "EmployeeForename2",
            PermissionDate = DateTime.Now,
            Id = 1,
            PermissionTypeId = 2
        };

        private readonly RequestPermissionCommand command = new RequestPermissionCommand("EmployeeForename2", "EmployeeSurname", 2);

        private RequestPermissionService Sut { get; set; }

        public RequestPermissionServiceTests()
        {
            mapperMock = new Mock<IMapper>();
            permissionEventPublisherMock = new Mock<IPermissionEventPublisher>();
            elasticClientMock = new Mock<IElasticClient>();
            repositoryAsynctMock = new Mock<IRepositoryAsync<Domain.Entities.Permission>>();

            repositoryAsynctMock.Setup(rr => rr.AddAsync(It.IsAny<Domain.Entities.Permission>())).ReturnsAsync(permissionAdded);

            mapperMock.Setup(s => s.Map<PermissionDTO>(It.IsAny<Domain.Entities.Permission>()))
                .Returns(permssionDTO);

            mapperMock.Setup(s => s.Map<Domain.Entities.Permission>(It.IsAny<RequestPermissionCommand>()))
              .Returns(permission);

            permissionEventPublisherMock.Setup(pep => pep.ProduceAsync(It.IsAny<PermissionMessage>()));

            Sut = new RequestPermissionService(repositoryAsynctMock.Object, mapperMock.Object, permissionEventPublisherMock.Object, elasticClientMock.Object);
        }

        [Fact]
        public async Task ProduceAsyncCalled()
        {
            //Act
            await Sut.Create(command);

            //Assert
            permissionEventPublisherMock.Verify(
                x => x.ProduceAsync(It.Is<PermissionMessage>(msg => IsEquivalentTo(msg, new PermissionMessage(Operation.Modify)))),
                Times.Once);
        }

        private static bool IsEquivalentTo(PermissionMessage actual, PermissionMessage expected)
        {
            actual.Should().BeEquivalentTo(expected, options => options.Excluding(p => p.Id));
            return true;
        }


        [Fact]
        public async Task ShouldReturnRightPermission()
        {
            //Act
            var result = await Sut.Create(command);

            //Assert
            result.Should().BeEquivalentTo(permssionDTO);
        }

    }
}