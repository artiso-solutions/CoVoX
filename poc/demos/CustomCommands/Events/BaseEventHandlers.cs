using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Dialog;

namespace CustomCommands.Events
{
    internal abstract class BaseEventHandlers
    {
        public abstract void OnTurnStatusReceivedHandler(object sender, TurnStatusReceivedEventArgs e);
        
        public abstract void OnRecognizedHandler(object sender, SpeechRecognitionEventArgs e);
        
        public abstract void OnRecognizingHandler(object sender, SpeechRecognitionEventArgs e);
        
        public abstract void OnActivityReceivedHandler(object sender, ActivityReceivedEventArgs e);
        
        public abstract void OnCanceledHandler(object sender, SpeechRecognitionCanceledEventArgs e);
    }
}
