using System.Collections.Generic;

namespace SampleWinFormsApp
{
    public interface IServiceClient
    {
        string GetInitialState();

        IEnumerable<string> GetAvailableEvents(string currentState);
        
        string GetNewState(string currentState, string triggeringEvent);
    }
}