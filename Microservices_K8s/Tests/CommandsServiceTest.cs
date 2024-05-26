using AutoMapper;
using CommandsService.Controllers;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class CommandsServiceTest
    {
        private readonly ICommandRepo _commandRepo;
        private readonly IMapper _mapper;
        private readonly CommandsController _sut;

        public CommandsServiceTest()
        {
            _commandRepo = Substitute.For<ICommandRepo>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Command, CommandReadDto>();
                cfg.CreateMap<CommandCreateDto, Command>();
            });
            _mapper = configuration.CreateMapper();

            _sut = new CommandsController(_commandRepo, _mapper);
        }

        // GetCommandsForPlatform
        [Fact]
        public void GetCommandsForPlatform_ShouldReturnNotFound_WhenPlatformDoesNotExist()
        {
            // Arrange
            _commandRepo.PlaformExits(1).Returns(false);

            // Act
            var result = _sut.GetCommandsForPlatform(1);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetCommandsForPlatform_ShouldReturnEmptyList_WhenNoCommandsExist()
        {

            _commandRepo.PlaformExits(1).Returns(true);
            _commandRepo.GetCommandsForPlatform(1).Returns(Enumerable.Empty<Command>());

            // Act
            var commands = _sut.GetCommandsForPlatform(1);

            // Arrange
            _commandRepo.PlaformExits(1).Returns(true);
            _commandRepo.GetCommandsForPlatform(1).Returns(Enumerable.Empty<Command>());

            // Act
            var result = _sut.GetCommandsForPlatform(1);

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
                new Command { Id = 1, HowTo = "How to do something", CommandLine = "dotnet run" },
                new Command { Id = 2, HowTo = "How to do another thing", CommandLine = "dotnet build" }
            };

            _commandRepo.PlaformExits(1).Returns(true);
            _commandRepo.GetCommandsForPlatform(1).Returns(commands);

            // Act
            var result = _sut.GetCommandsForPlatform(1);

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
            _commandRepo.PlaformExits(1).Returns(false);

            // Act
            var result = _sut.GetCommandForPlatform(1, 1);
            // Assert

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetCommandForPlatform_ShouldReturnNotFound_WhenCommandDoesNotExist()
        {
            // Arrange
            _commandRepo.PlaformExits(1).Returns(true);
            _commandRepo.GetCommand(1, 1).Returns((Command)null);

            // Act
            var result = _sut.GetCommandForPlatform(1, 1);

            // 
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetCommandForPlatform_ShouldReturnCommand_WhenCommandExist()
        {

            Command command = new()
            {
                Id = 1,
                HowTo = "Do something",
                PlatformId = 1,
                CommandLine = "dotnet run",
            };
            // Arrange
            _commandRepo.PlaformExits(1).Returns(true);
            _commandRepo.GetCommand(1, 1).Returns(command);

            // Act
            var result = _sut.GetCommandForPlatform(1, 1);

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
            _commandRepo.PlaformExits(1).Returns(false);

            // Act
            var result = _sut.CreateCommandForPlatform(1, command);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CreateCommandForPlatform_ShouldReturnCreatedAtRoute_WhenCommandIsCreated()
        {
            // Arrange
            _commandRepo.PlaformExits(1).Returns(true);
            var commandCreateDto = new CommandCreateDto { HowTo = "Do something", CommandLine = "dotnet run" };
            var command = new Command { Id = 1, HowTo = "Do something", CommandLine = "dotnet run" };
            _commandRepo.When(x => x.CreateCommand(1, Arg.Any<Command>()))
                        .Do(callInfo =>
                        {
                            var cmd = callInfo.Arg<Command>();
                            cmd.Id = command.Id;
                        });
            _commandRepo.SaveChanges().Returns(true);

            // Act
            var result = _sut.CreateCommandForPlatform(1, commandCreateDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtRouteResult>();
            var createdAtRouteResult = result.Result as CreatedAtRouteResult;
            createdAtRouteResult.RouteName.Should().Be(nameof(_sut.GetCommandForPlatform));
            createdAtRouteResult.RouteValues["platformId"].Should().Be(1);
            createdAtRouteResult.RouteValues["commandId"].Should().Be(1);
            var commandReadDto = createdAtRouteResult.Value as CommandReadDto;
            commandReadDto.Should().NotBeNull();
            commandReadDto.Id.Should().Be(1);
            commandReadDto.HowTo.Should().Be("Do something");
            commandReadDto.CommandLine.Should().Be("dotnet run");
        }
    }
}