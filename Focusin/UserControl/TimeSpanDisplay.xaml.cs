using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Focusin
{
    public partial class TimeSpanDisplay : UserControl
    {
        private int _digitWidth;

        public TimeSpanDisplay()
        {
            InitializeComponent();

            // In design mode, show something other than an empty StackPanel
            if (DesignerProperties.IsInDesignTool)
                LayoutRoot.Children.Add(new TextBlock { Text = "25:00"});
        }

        public int DigitWidth
        {
            get { return _digitWidth; }
            set { _digitWidth = value;
                // Force a display update using the new width:
                Time = Time;
            }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time",
                                        typeof (TimeSpan),
                                        typeof (UserControl),
                                        new PropertyMetadata(TimeSpan.Zero, OnTimePropertyChanged));

        private static void OnTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as TimeSpanDisplay;
            var value = (TimeSpan)e.NewValue;

            source.LayoutRoot.Children.Clear();

            // Carve out the appropiate digits and add each individually
            // Support and arbitrary # of minutes digits (including a leading zero if neccesary)
            var minutesString = value.Minutes.ToString();
            if (minutesString.Length == 1)
                source.AddDigitString(0.ToString());
            for (int i = 0; i < minutesString.Length; i++)
                source.AddDigitString(minutesString[i].ToString());

            source.LayoutRoot.Children.Add(new TextBlock { Text = ":" });

            // Seconds (Always to digits, including a leading zero if neccesary)
            source.AddDigitString((value.Seconds / 10).ToString());
            source.AddDigitString((value.Seconds % 10).ToString());
        }

        public TimeSpan Time
        {
            get { return (TimeSpan) GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        private void AddDigitString(string digitString)
        {
            var border = new Border {Width = _digitWidth};
            border.Child = new TextBlock {Text = digitString, HorizontalAlignment = HorizontalAlignment.Center};
            LayoutRoot.Children.Add(border);
        }
    }
}
