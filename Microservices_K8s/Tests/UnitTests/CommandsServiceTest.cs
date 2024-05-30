using AutoMapper;
using CommandsService.Controllers;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.UnitTests
{
    [Collection("TestCollectionFixture")] // Allows Data Sharing with different Test Clases, But blocks Parralel exicution
    public class CommandsServiceTest
    {
        private readonly ICommandRepo _commandRepo = Substitute.For<ICommandRepo>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly CommandsController _sut;
        private readonly ClassFixture _fixture;

        public CommandsServiceTest(ClassFixture fixture)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Command, CommandReadDto>();
                cfg.CreateMap<CommandReadDto, Command>();
                cfg.CreateMap<CommandCreateDto, Command>();
            });
            _mapper = configuration.CreateMapper();
            _sut = new CommandsController(_commandRepo, _mapper);
            _fixture = fixture;
        }

        // GetCommandsForPlatform
        [Fact]
        public void GetCommandsForPlatform_ShouldReturnNotFound_WhenPlatformDoesNotExist()
        {
            // Arrange
            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(false);

            // Act
            var result = _sut.GetCommandsForPlatform(_fixture.PlatformId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetCommandsForPlatform_ShouldReturnEmptyList_WhenNoCommandsExist()
        {

            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(true);
            _commandRepo.GetCommandsForPlatform(_fixture.PlatformId).Returns(Enumerable.Empty<Command>());

            // Act
            var commands = _sut.GetCommandsForPlatform(_fixture.PlatformId);

            // Arrange
            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(true);
            _commandRepo.GetCommandsForPlatform(_fixture.PlatformId).Returns(Enumerable.Empty<Command>());

            // Act
            var result = _sut.GetCommandsForPlatform(_fixture.PlatformId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeAssignableTo<IEnumerable<CommandReadDto>>().Which.Should().BeEmpty();
        }

        [Fact]
        public void GetCommandsForPlatform_ShouldReturnCommands_WhenCommandsExist()
        {
            // Arrange
            var commands = new List<Command>
            {
                new Command { Id = _fixture.CommandId, HowTo = "How to do something", CommandLine = "dotnet run" },
                new Command { Id = _fixture.CommandId, HowTo = "How to do another thing", CommandLine = "dotnet build" }
            };

            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(true);
            _commandRepo.GetCommandsForPlatform(_fixture.PlatformId).Returns(commands);

            // Act
            var result = _sut.GetCommandsForPlatform(_fixture.PlatformId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeAssignableTo<IEnumerable<CommandReadDto>>();
            var commandReadDtos = okResult.Value as IEnumerable<CommandReadDto>;
            commandReadDtos.Should().HaveCount(2);
            commandReadDtos.First().HowTo.Should().Be("How to do something");
            commandReadDtos.First().CommandLine.Should().Be("dotnet run");
        }

        // GetCommandForPlatform
        [Fact]
        public void GetCommandForPlatform_ShouldReturnNotFound_WhenPlatformDoesNotExist()
        {
            // Arrange
            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(false);

            // Act
            var result = _sut.GetCommandForPlatform(_fixture.PlatformId, _fixture.CommandId);
            // Assert

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetCommandForPlatform_ShouldReturnNotFound_WhenCommandDoesNotExist()
        {
            // Arrange
            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(true);
            _commandRepo.GetCommand(_fixture.PlatformId, _fixture.CommandId).Returns((Command)null);

            // Act
            var result = _sut.GetCommandForPlatform(_fixture.PlatformId, _fixture.CommandId);

            // 
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetCommandForPlatform_ShouldReturnCommand_WhenCommandExist()
        {

            Command command = new()
            {
                Id = _fixture.CommandId,
                HowTo = "Do something",
                PlatformId = 1,
                CommandLine = "dotnet run",
            };
            // Arrange
            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(true);
            _commandRepo.GetCommand(_fixture.PlatformId, _fixture.CommandId).Returns(command);

            // Act
            var result = _sut.GetCommandForPlatform(_fixture.PlatformId, _fixture.CommandId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeAssignableTo<CommandReadDto>();
        }

        // CreateCommandForPlatform
        [Fact]
        public void CreateCommandForPlatform_ShouldReturnNotFound_WhenPlatformDoesNotExist()
        {
            // Arrange
            var command = new CommandCreateDto { HowTo = "How to do something", CommandLine = "dotnet run" };
            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(false);

            // Act
            var result = _sut.CreateCommandForPlatform(_fixture.PlatformId, command);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CreateCommandForPlatform_ShouldReturnCreatedAtRoute_WhenCommandIsCreated()
        {
            // Arrange
            _commandRepo.PlaformExits(_fixture.PlatformId).Returns(true);
            var commandCreateDto = new CommandCreateDto { HowTo = "Do something", CommandLine = "dotnet run" };
            var command = new Command { Id = _fixture.CommandId, HowTo = "Do something", CommandLine = "dotnet run" };
            _commandRepo.When(x => x.CreateCommand(_fixture.PlatformId, Arg.Any<Command>()))
                        .Do(callInfo =>
                        {
                            var cmd = callInfo.Arg<Command>();
                            cmd.Id = command.Id;
                        });
            _commandRepo.SaveChanges().Returns(true);

            // Act
            var result = _sut.CreateCommandForPlatform(_fixture.PlatformId, commandCreateDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtRouteResult>();
            var createdAtRouteResult = result.Result as CreatedAtRouteResult;
            createdAtRouteResult.RouteName.Should().Be(nameof(_sut.GetCommandForPlatform));
            createdAtRouteResult.RouteValues["platformId"].Should().Be(_fixture.PlatformId);
            createdAtRouteResult.RouteValues["commandId"].Should().Be(_fixture.CommandId);
            var commandReadDto = createdAtRouteResult.Value as CommandReadDto;
            commandReadDto.Should().NotBeNull();
            commandReadDto.Id.Should().Be(_fixture.CommandId);
            commandReadDto.HowTo.Should().Be("Do something");
            commandReadDto.CommandLine.Should().Be("dotnet run");
        }
    }

}