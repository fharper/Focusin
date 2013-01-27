using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Focusin.Model;
using Focusin.Resources;
using Focusin.Storage;
using Focusin.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Focusin
{
    public partial class MainPage : PhoneApplicationPage
    {
        private IApplicationBarIconButton _playStopButton;
        private bool _isRunning;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            _playStopButton = ApplicationBar.Buttons[0] as IApplicationBarIconButton;

            Messenger.Default.Register<Session>(this, "CurrentSession", UpdateUI);
            Messenger.Default.Register<double>(this, "ActualSeconds", UpdateProgressBar);
            
            ProgressBar.Maximum = Settings.WorkMinutes.Value.TotalSeconds;
            ProgressBar.Value = Settings.WorkMinutes.Value.TotalSeconds;

            // We have to localize the AppBar manually
            (ApplicationBar.Buttons[0] as IApplicationBarIconButton).Text = Strings.Play; // The Play / Stop button
            (ApplicationBar.Buttons[1] as IApplicationBarIconButton).Text = Strings.Settings; // The Settings button
            (ApplicationBar.MenuItems[0] as IApplicationBarMenuItem).Text = Strings.Instructions; // Instructions menu item
            (ApplicationBar.MenuItems[1] as IApplicationBarMenuItem).Text = Strings.About; // About menu item
        }

        private void UpdateProgressBar(double seconds)
        {
            ProgressBar.Value = seconds;
        }

        private void UpdateUI(Session session)
        {
            if (!session.IsFreeTime) // If this is a new Session, the timer is stopped so we update the UI
            {
                ProgressBar.Maximum = Settings.WorkMinutes.Value.TotalSeconds;
                ProgressBar.Value = Settings.WorkMinutes.Value.TotalSeconds;
                Settings.WasRunning.Value = false;
                UpdateButton();
            }
            else
            {
                ProgressBar.Maximum = Settings.BreakMinutes.Value.TotalSeconds;
                ProgressBar.Value = Settings.BreakMinutes.Value.TotalSeconds;
            }

            UpdateSessionType(session.IsFreeTime);
        }

        private void UpdateSessionType(bool isFreeTime)
        {
            if (isFreeTime)
            {
                WorkTextBlock.Opacity = .1;
                BreakTextBlock.Opacity = 1;
            }
            else
            {
                BreakTextBlock.Opacity = .1;
                WorkTextBlock.Opacity = 1;
            }
        }


        public MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        private void PlayStopButton_Click(object sender, EventArgs e)
        {
            if (Settings.WasRunning.Value)
            {
                ViewModel.StopCommand.Execute(null);
                //ProgressbarDecreasingStoryboard.Stop();
                ProgressBar.Maximum = Settings.WorkMinutes.Value.TotalSeconds;
                ProgressBar.Value = Settings.WorkMinutes.Value.TotalSeconds;
                UpdateButton();
            }
            else
            {
                ViewModel.PlayCommmand.Execute(null);
                //ProgressbarDecreasingStoryboard.Begin();
                
                UpdateButton();
            }
        }

        private void UpdateButton()
        {
            if (Settings.WasRunning.Value)
            {
                _playStopButton.IconUri = new Uri("/Content/Images/appbar.stop.png", UriKind.Relative);
                _playStopButton.Text = Strings.Stop;
            }
            else
            {
                _playStopButton.IconUri = new Uri("/Content/Images/appbar.play.png", UriKind.Relative);
                _playStopButton.Text = Strings.Play;  
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // We respect the saved settings
            Foreground = ProgressBar.Foreground = new SolidColorBrush(Settings.ForegroundColor.Value);
            LayoutGrid.Background = new SolidColorBrush(Settings.BackgroundColor.Value);
            ApplicationBar.ForegroundColor = Settings.ForegroundColor.Value;
            ApplicationBar.BackgroundColor = Settings.BackgroundColor.Value;

            if (Settings.DisableScreenTimeOut.Value == true)
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            
            RefreshDisplay();
            UpdateButton();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;

            ViewModel.UpdateSavedSessionCommand.Execute(null);
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            base.OnOrientationChanged(e);
            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            if (IsMatchingOrientation(PageOrientation.Portrait))
            {
                //LeftMargin.Width = new GridLength(15);
                //RightMargin.Width = new GridLength(12);
                //Display.FontSize = 125;
                //Display.DigitWidth = 75;
                //Display.Margin = new Thickness(30, 0, 0, 0);
                //WorkTextBlock.FontSize = BreakTextBlock.FontSize = 30;
                //ProgressBar.Width = 430;
                //CurrentSessionTextBlock.FontSize = SessionNumberTextBlock.FontSize = 27;
            }
            else 
            {
                //LeftMargin.Width = new GridLength(92);
                //RightMargin.Width = new GridLength(92);
                //Display.FontSize = 126;
                //Display.DigitWidth = 100;
                //Display.Margin = new Thickness(30, 0, 0, 0);
                //WorkTextBlock.FontSize = BreakTextBlock.FontSize = 50;
                //ProgressBar.Width = 610;
                //CurrentSessionTextBlock.FontSize = SessionNumberTextBlock.FontSize = 35;
            }
        }

        private bool IsMatchingOrientation(PageOrientation orientation)
        {
            return ((Orientation & orientation) == orientation);
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            ViewModel.OpenSettingsPageCommand.Execute(null);
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            ViewModel.OpenAboutPageCommand.Execute(null);
        }

        private void InstructionsMenuItem_Click(object sender, EventArgs e)
        {
            ViewModel.OpenInstructionsPageCommand.Execute(null);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            ApplicationBar.IsVisible = !ApplicationBar.IsVisible;
        }
    }
}