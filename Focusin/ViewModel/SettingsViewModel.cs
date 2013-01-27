using System;
using System.Windows.Media;
using Focusin.Helpers;
using Focusin.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Focusin.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public RelayCommand ChangeForegroundCommand { get; set; }
        public RelayCommand ChangeBackgroundCommand { get; set; }
        public RelayCommand DefaultSettingsCommand { get; set; }

        public INavigationService NavigationService { get; set; }

        public bool? EnableVibration
        {
            get { return Settings.EnableVibration.Value; }
            set
            {
                Settings.EnableVibration.Value = value;
                RaisePropertyChanged(() => EnableVibration);
            }
        }

        public bool? EnableSound
        {
            get { return Settings.EnableSound.Value; }
            set
            {
                Settings.EnableSound.Value = value;
                RaisePropertyChanged(() => EnableSound);
            }
        }

        public bool? DisableScreenTimeOut
        {
            get { return Settings.DisableScreenTimeOut.Value; }
            set
            {
                Settings.DisableScreenTimeOut.Value = value;
                RaisePropertyChanged(() => DisableScreenTimeOut);
            }
        }

        public TimeSpan SessionMinutes
        {
            get { return Settings.WorkMinutes.Value; }
            set
            {
                Settings.WorkMinutes.Value = value;
                MessengerInstance.Send(new SessionMinutesChanged("SessionChanged"));
                RaisePropertyChanged(() => SessionMinutes);

            }
        }

        public TimeSpan BreakMinutes
        {
            get { return Settings.BreakMinutes.Value; }
            set
            {
                Settings.BreakMinutes.Value = value;
                RaisePropertyChanged(() => BreakMinutes);
            }
        }

        public TimeSpan LongBreakMinutes
        {
            get { return Settings.LongBreakMinutes.Value; }
            set
            {
                Settings.LongBreakMinutes.Value = value;
                RaisePropertyChanged(() => LongBreakMinutes);
            }
        }

        public int LongBreakFrequency
        {
            get { return Settings.LongBreakFrequency.Value; }
            set
            {
                Settings.LongBreakFrequency.Value = value;
                RaisePropertyChanged(() => LongBreakFrequency);
            }
        }

        public Color BackgroundColor
        {
            get { return Settings.BackgroundColor.Value; }
            set
            {
                Settings.BackgroundColor.Value = value;
                RaisePropertyChanged(() => BackgroundColor);
            }
        }

        public Color ForegroundColor
        {
            get { return Settings.ForegroundColor.Value; }
            set
            {
                Settings.ForegroundColor.Value = value;
                RaisePropertyChanged(() => ForegroundColor);
            }
        }

        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel()
        {
            if (IsInDesignMode)
            {
                EnableSound = true;
                DisableScreenTimeOut = true;
            }
            else
            {
                // Code runs "for real": Connect to service, etc...
            }

            ChangeBackgroundCommand = new RelayCommand(ChangeBackground);
            ChangeForegroundCommand = new RelayCommand(ChangeForeground);
            DefaultSettingsCommand = new RelayCommand(SetDefaultSettings);
        }

        private void SetDefaultSettings()
        {
            ForegroundColor = Settings.ForegroundColor.DefaultValue;
            BackgroundColor = Settings.BackgroundColor.DefaultValue;
            EnableSound = Settings.EnableSound.DefaultValue;
            EnableVibration = Settings.EnableVibration.DefaultValue;
            DisableScreenTimeOut = Settings.DisableScreenTimeOut.DefaultValue;
            SessionMinutes = Settings.WorkMinutes.DefaultValue;
            BreakMinutes = Settings.BreakMinutes.DefaultValue;
            LongBreakFrequency = Settings.LongBreakFrequency.DefaultValue;
            LongBreakMinutes = Settings.LongBreakMinutes.DefaultValue;
        }

        void ChangeBackground()
        {
            var currentColorString = Settings.BackgroundColor.Value.ToString().Substring(1);
            var defaultColorString = Settings.BackgroundColor.Value.ToString().Substring(1);

            Settings.BackgroundColor.ForceRefresh();

            NavigationService.NavigateTo(new Uri("/ColorPicker/ColorPickerPage.xaml?"
                                                            + "&currentColor=" + currentColorString
                                                            + "&defaultColor=" + defaultColorString
                                                            + "&settingName=BackgroundColor", UriKind.Relative));
            
        }

        void ChangeForeground()
        {
            var currentColorString = Settings.ForegroundColor.Value.ToString().Substring(1);
            var defaultColorString = Settings.ForegroundColor.Value.ToString().Substring(1);

            Settings.ForegroundColor.ForceRefresh();

            NavigationService.NavigateTo(new Uri("/ColorPicker/ColorPickerPage.xaml?"
                                                            + "&currentColor=" + currentColorString
                                                            + "&defaultColor=" + defaultColorString
                                                            + "&settingName=ForegroundColor", UriKind.Relative));
        }

        public void RefreshColors()
        {
            RaisePropertyChanged(() => BackgroundColor);
            RaisePropertyChanged(() => ForegroundColor);
        }
    }
}