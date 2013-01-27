using GalaSoft.MvvmLight.Command;

namespace Focusin.ViewModel
{
    public class AboutViewModel
    {
        public RelayCommand ViewMoreAppsCommand { get; set; }
        public RelayCommand<string> SupportCommand { get; set; }

        public AboutViewModel()
        {
            ViewMoreAppsCommand = new RelayCommand(ViewMoreApps);
            SupportCommand = new RelayCommand<string>(Support);
        }

        void ViewMoreApps()
        {
            var t = new Microsoft.Phone.Tasks.MarketplaceSearchTask();
            t.ContentType = Microsoft.Phone.Tasks.MarketplaceContentType.Applications;
            t.SearchTerms = "Foxandxss";
            t.Show();
        }

        void Support(string appVersion)
        {
            var t = new Microsoft.Phone.Tasks.EmailComposeTask
                        {
                            Subject = "Feedback on " + "Focusin " + appVersion.Substring("version ".Length),
                            To = "foxandxss@gmail.net"
                        };
            t.Show();
        }
    }
}