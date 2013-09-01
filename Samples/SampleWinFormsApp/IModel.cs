using System.Collections.Generic;
using System.ComponentModel;

namespace SampleWinFormsApp
{
    public interface IModel : INotifyPropertyChanged
    {
        IEnumerable<string> AvailableEvents { get; }

        string CurrentState { get; }

        bool IsAnyEventAvailable { get; }
        
        void Initialize();
        
        void Transition(string triggeringEvent);
    }
}