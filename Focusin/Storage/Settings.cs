using System;
using System.Windows;
using System.Windows.Media;
using Focusin.Helpers;
using Focusin.Model;

namespace Focusin.Storage
{
    public static class Settings
    {
        public static readonly Setting<TimeSpan> WorkMinutes = new Setting<TimeSpan>("WorkMinutes", TimeSpan.FromMinutes(25));
        public static readonly Setting<TimeSpan> BreakMinutes = new Setting<TimeSpan>("BreakMinutes", TimeSpan.FromMinutes(5));
        public static readonly Setting<TimeSpan> LongBreakMinutes = new Setting<TimeSpan>("LongBreakMinutes", TimeSpan.FromMinutes(25));
        public static readonly Setting<int> LongBreakFrequency = new Setting<int>("LongBreakFrequency",4); 
        public static readonly Setting<Color> ForegroundColor = new Setting<Color>("ForegroundColor", (Color)Application.Current.Resources["PhoneAccentColor"]);
        public static readonly Setting<Color> BackgroundColor = new Setting<Color>("BackgroundColor", Colors.Black);
        public static readonly Setting<bool?> EnableSound = new Setting<bool?>("EnableSound", true);
        public static readonly Setting<bool?> EnableVibration = new Setting<bool?>("EnableVibration", false);
        public static readonly Setting<bool?> DisableScreenTimeOut = new Setting<bool?>("DisableScreenTimeOut", true);

        // This three clases are for app state
        public static readonly Setting<DateTime> PreviousTick = new Setting<DateTime>("PreviousTick", DateTime.MinValue);
        public static readonly Setting<bool> WasRunning = new Setting<bool>("WasRunning", false);
        public static readonly Setting<Session> RunningSession = new Setting<Session>("RunningSession", new Session(1, WorkMinutes.Value));
    }
}