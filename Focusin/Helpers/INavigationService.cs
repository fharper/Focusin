using System;
using System.Windows.Navigation;

namespace Focusin.Helpers
{
    public interface INavigationService
    {
        event NavigatingCancelEventHandler Navigating;
        void NavigateTo(Uri uri);
        void GoBack();
    }
}