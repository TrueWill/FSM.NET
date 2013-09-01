using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Configuration;
using TrueWill.Fsm;

namespace SampleWcfService
{
    public class StateMachineService : IStateMachineService
    {
        private static readonly StateMachine _stateMachine;

        static StateMachineService()
        {
            // Cheesy initialization - I'd probably use Unity.WCF in production.
            // Not handling errors.
            // No security.

            // Read state machine from Web.config.

            var config = WebConfigurationManager.OpenWebConfiguration("~");
            var tableText = config.AppSettings.Settings["transitionTable"].Value;
            var transitions = TransitionTableParser.Parse(tableText);
            _stateMachine = new StateMachine(transitions);
        }

        public string GetInitialState()
        {
            return _stateMachine.InitialState;
        }

        public IEnumerable<string> GetAvailableEvents(string currentState)
        {
            try
            {
                return _stateMachine.GetAvailableEvents(currentState);
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is ArgumentNullException)
                {
                    throw new FaultException(ex.Message);
                }

                throw;
            }
        }

        public string GetNewState(string currentState, string triggeringEvent)
        {
            try
            {
                return _stateMachine.GetNewState(currentState, triggeringEvent);
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is ArgumentNullException)
                {
                    throw new FaultException(ex.Message);
                }

                throw;
            }
        }
    }
}
