using System.Collections.Generic;
using System.ServiceModel;

namespace SampleWcfService
{
    [ServiceContract]
    public interface IStateMachineService
    {
        [OperationContract]
        string GetInitialState();

        [OperationContract]
        IEnumerable<string> GetAvailableEvents(string currentState);

        [OperationContract]
        string GetNewState(string currentState, string triggeringEvent);
    }
}
