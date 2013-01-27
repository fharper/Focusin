using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Focusin.Helpers
{
    public static class SoundEffects
    {
        public static SoundEffect EndSound { get; private set; }

        public static void Initialize()
        {
            StreamResourceInfo info =
                Application.GetResourceStream(new Uri("Content/Audio/endsound.wav", UriKind.Relative));
            EndSound = SoundEffect.FromStream(info.Stream);

            CompositionTarget.Rendering += (s, e) => FrameworkDispatcher.Update();
                                               
            // Call also once at the beginning
            FrameworkDispatcher.Update();
        }
    }
}