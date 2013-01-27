using System;
using System.Windows.Threading;
using Focusin.Helpers;
using Focusin.Model;
using Focusin.Storage;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Audio;

namespace Focusin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private DispatcherTimer _timer;
        private SoundEffectInstance _endSound;

        private TimeSpan _timeElapsedInBreakTime;

        public RelayCommand PlayCommmand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand OpenSettingsPageCommand { get; set; }
        public RelayCommand OpenAboutPageCommand { get; set; }
        public RelayCommand OpenInstructionsPageCommand { get; set; }
        public RelayCommand UpdateSavedSessionCommand { get; set; }

        public INavigationService NavigationService { get; set; }

        private Session _currentSession;

        public Session CurrentSession
        {
            get { return _currentSession; }
            set
            {
                _currentSession = value;
                Messenger.Default.Send(CurrentSession, "CurrentSession");
                RaisePropertyChanged(() => CurrentSession);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                CurrentSession = new Session(5, TimeSpan.FromMinutes(25));
            }
            else
            {
                CurrentSession = new Session(1, Settings.WorkMinutes.Value);
                if (Settings.WasRunning.Value)
                {
                    CurrentSession = Settings.RunningSession.Value;
                }
                    
                else
                {
                    CurrentSession = new Session(1, Settings.WorkMinutes.Value);
                }
            }
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(.1) };
            _timer.Tick += (s, e) =>
                               {
                                   var delta = DateTime.UtcNow - Settings.PreviousTick.Value;
                                   Settings.PreviousTick.Value += delta;
                                   CurrentSession.Minutes -= delta;
                                   Messenger.Default.Send(CurrentSession.Minutes.TotalSeconds, "ActualSeconds");
                                   if (CurrentSession.Minutes <= TimeSpan.FromSeconds(0))
                                   {
                                       _timeElapsedInBreakTime = TimeSpan.FromSeconds(Math.Abs(CurrentSession.Minutes.TotalSeconds));
                                       _timer.Stop();
                                       if (Settings.EnableSound.Value == true)
                                           _endSound.Play();
                                       if (Settings.EnableVibration.Value == true)
                                           VibrateController.Default.Start(TimeSpan.FromSeconds(1));
                                       UpdateSession();
                                   }
                               };
            PlayCommmand = new RelayCommand(StartSession);
            StopCommand = new RelayCommand(StopSession);
            OpenSettingsPageCommand = new RelayCommand(() => NavigationService.NavigateTo(ViewModelLocator.SettingsPageUri));
            OpenAboutPageCommand = new RelayCommand(() => NavigationService.NavigateTo(ViewModelLocator.AboutPageUri));
            OpenInstructionsPageCommand = new RelayCommand(() => NavigationService.NavigateTo(ViewModelLocator.InstructionsPageUri));
            UpdateSavedSessionCommand = new RelayCommand(UpdateSavedSession);

            SoundEffects.Initialize();
            _endSound = SoundEffects.EndSound.CreateInstance();
            _endSound.Volume = 1;

            Messenger.Default.Register<SessionMinutesChanged>(this, UpdateCurrentSession);

            if (Settings.WasRunning.Value)
                _timer.Start();
        }

        private void UpdateSavedSession()
        {
            if (Settings.WasRunning.Value)
                Settings.RunningSession.Value = CurrentSession;
        }

        private void UpdateCurrentSession(SessionMinutesChanged obj)
        {
            if (_timer.IsEnabled)
                return;

            CurrentSession = new Session(1, Settings.WorkMinutes.Value);
        }

        private void UpdateSession()
        {
            
            CurrentSession.IsFreeTime = !CurrentSession.IsFreeTime;
            if (CurrentSession.IsFreeTime)
            {
                if ((CurrentSession.Number % Settings.LongBreakFrequency.Value) == 0) // AKA is big break time
                    //CurrentSession.Minutes =
                    //    Settings.BreakMinutes.Value = Settings.LongBreakMinutes.Value - _timeElapsedInBreakTime;
                    CurrentSession.Minutes = Settings.LongBreakMinutes.Value - _timeElapsedInBreakTime;
                else
                    CurrentSession.Minutes = Settings.BreakMinutes.Value - _timeElapsedInBreakTime;
            }
            else
            {
                CurrentSession.Number++;
                CurrentSession.Minutes = Settings.WorkMinutes.Value;
            }

            // If we ended our break, the timer will stop waiting for the user click start again.
            if (!CurrentSession.IsFreeTime)
                _timer.Stop();
            else
            {
                _timer.Start();
            }

            // We send the CurrentSession's information to update the storyboard and
            // To update the UI if the CurrentSession ended
            Messenger.Default.Send(CurrentSession, "CurrentSession");
        }

        private void StartSession()
        {
            Settings.PreviousTick.Value = DateTime.UtcNow;
            _timer.Start();
            Settings.WasRunning.Value = true;
        }

        private void StopSession()
        {
            _timer.Stop();
            CurrentSession = new Session(1, Settings.WorkMinutes.Value);
            Settings.WasRunning.Value = false;
        }
    }
}