using System.Collections.Generic;
using SampleWinFormsApp.SampleService;

namespace SampleWinFormsApp
{
    public class ServiceClient : IServiceClient
    {
        public string GetInitialState()
        {
            string result = null;

            UsingServiceClient.Do(
                new StateMachineServiceClient(),
                client =>
                result = client.GetInitialState());

            return result;
        }

        public IEnumerable<string> GetAvailableEvents(string currentState)
        {
            string[] result = null;

            UsingServiceClient.Do(
                new StateMachineServiceClient(),
                client =>
                result = client.GetAvailableEvents(currentState));

            return result;
        }

        public string GetNewState(string currentState, string triggeringEvent)
        {
            string result = null;

            UsingServiceClient.Do(
                new StateMachineServiceClient(),
                client =>
                result = client.GetNewState(currentState, triggeringEvent));

            return result;
        }
    }
}