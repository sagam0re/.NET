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
            _commandRepo.GetCommand(1,1).Returns(command);

            // Act
            var result = _sut.GetCommandForPlatform(1, 1);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeAssignableTo<CommandReadDto>();
        }
    }
}