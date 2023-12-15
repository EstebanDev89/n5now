using Application.DTOs;
using Application.Enums;
using Application.Features.Permissions.Commands.ModifyPermission;
using Application.Features.Permissions.Messages;
using Application.Features.Permissions.Services;
using AutoMapper;
using FluentAssertions;
using Moq;
using Nest;
using Persistence.Repository;

namespace n5now.Testing.Core.Application.Features.Permission.Services
{
    public class ModifyPermissionServiceTest
    {
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IPermissionEventPublisher> permissionEventPublisherMock;
        private readonly Mock<IElasticClient> elasticClientMock;
        private readonly Mock<IRepositoryAsync<Domain.Entities.Permission>> repositoryAsynctMock;
        private readonly Mock<IReadRepositoryAsync<Domain.Entities.Permission>> readRepositoryAsynctMock;

        private readonly Domain.Entities.Permission permission = new Domain.Entities.Permission()
        {
            EmployeeForename = "EmployeeForename",
            EmployeeSurname = "EmployeeForename",
            PermissionDate = DateTime.Now,
            Id = 1,
            PermissionType = new Domain.Entities.PermissionType() { Id = 1, Description = "Description" },
            PermissionTypeId = 1
        };

        private readonly Domain.Entities.Permission permissionModify = new Domain.Entities.Permission()
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

        private readonly ModifyPermissionCommand command = new ModifyPermissionCommand(1, "EmployeeForename2", "EmployeeSurname", 2);

        private ModifyPermissionService Sut { get; set; }

        public ModifyPermissionServiceTest()
        {
            mapperMock = new Mock<IMapper>();
            permissionEventPublisherMock = new Mock<IPermissionEventPublisher>();
            elasticClientMock = new Mock<IElasticClient>();
            repositoryAsynctMock = new Mock<IRepositoryAsync<Domain.Entities.Permission>>();
            readRepositoryAsynctMock = new Mock<IReadRepositoryAsync<Domain.Entities.Permission>>();

            readRepositoryAsynctMock.Setup(rr => rr.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(permission);
            repositoryAsynctMock.Setup(rr => rr.UpdateAsync(It.IsAny<Domain.Entities.Permission>())).ReturnsAsync(permissionModify);

            mapperMock.Setup(s => s.Map<PermissionDTO>(It.IsAny<Domain.Entities.Permission>()))
                .Returns(permssionDTO);

            permissionEventPublisherMock.Setup(pep => pep.ProduceAsync(It.IsAny<PermissionMessage>()));

            Sut = new ModifyPermissionService(repositoryAsynctMock.Object, readRepositoryAsynctMock.Object, mapperMock.Object, permissionEventPublisherMock.Object, elasticClientMock.Object);
        }

        [Fact]
        public async Task ProduceAsyncCalled()
        {
            //Act
            await Sut.Update(command);

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
            var result = await Sut.Update(command);

            //Assert
            result.Should().BeEquivalentTo(permssionDTO);
        }

    }
}