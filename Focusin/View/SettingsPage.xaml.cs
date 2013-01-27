using System;
using System.Windows;
using Focusin.Resources;
using Focusin.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Focusin.View
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            // We have to localize the app bar manually
            (ApplicationBar.Buttons[0] as IApplicationBarIconButton).Text = Strings.DefaultSettings;
        }

        public SettingsViewModel ViewModel
        {
            get { return DataContext as SettingsViewModel; }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.RefreshColors();
        }

        private void DefaultSettingsButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Strings.SettingsResetMessage, Strings.SettingsReset, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                ViewModel.DefaultSettingsCommand.Execute(null);
        }
    }
}