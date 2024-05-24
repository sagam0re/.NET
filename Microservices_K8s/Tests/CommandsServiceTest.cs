using CommandsService.Models;
using FluentAssertions;
using System;
using Xunit;


namespace Tests
{
    public class CommandsServiceTest
    {
        private readonly Command _sut = new();

        [Fact]
        public void Command_ShouldHave_AllTheRequiredProperties()
        {
            Command expected = new()
            {
                Id = 1,
                HowTo = "",
                CommandLine = "",
                PlatformId = 1
            };

            expected.Should().BeEquivalentTo(new Command() { Id = 1, HowTo = "", CommandLine = "", PlatformId = 1 });
            expected.CommandLine.Should().NotBeNull();
            expected.Id.Should().BePositive();
            expected.HowTo.Should().NotBeNull();
            expected.PlatformId.Should().BePositive();
        }
    }
}
