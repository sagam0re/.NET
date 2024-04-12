using PlatformService.Dtos;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto platformReadDto);
    }
}