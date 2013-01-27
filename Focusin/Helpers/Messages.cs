using GalaSoft.MvvmLight.Messaging;

namespace Focusin.Helpers
{
    public class SessionMinutesChanged : NotificationMessage
    {
        public SessionMinutesChanged(string notification) : base(notification)
        {
            
        }
    }
}