namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        void PorcessEvent(string message);
    }
}