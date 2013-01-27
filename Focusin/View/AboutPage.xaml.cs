// Copyright (c) Adam Nathan.  All rights reserved.
//
// By purchasing the book that this source code belongs to, you may use and
// modify this code for commercial and non-commercial applications, but you
// may not publish the source code.
// THE SOURCE CODE IS PROVIDED "AS IS", WITH NO WARRANTIES OR INDEMNITIES.
using System.Windows.Navigation;
using Focusin.Resources;
using Microsoft.Phone.Controls;

namespace Focusin.View
{
  public partial class AboutPage : PhoneApplicationPage
  {
    public AboutPage()
    {
      InitializeComponent();

      // Fill out the version number.
      // We don't have permission to do this:
      // VersionTextBlock.Text = "version " + 
      //   typeof(AboutPage).Assembly.GetName().Version.ToString();
      // so do this:
      try
      {
        string s = typeof(AboutPage).Assembly.ToString();
        if (s != null && s.IndexOf("Version=") >= 0)
        {
          s = s.Substring(s.IndexOf("Version=") + "Version=".Length);
          s = s.Substring(0, s.IndexOf(","));
          this.VersionTextBlock.Text = Strings.Version + " " + s;
        }
      }
      catch { /* Never mind! */ }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      // Set the application name in the header
      if (this.NavigationContext.QueryString.ContainsKey("appName"))
      {
        this.ApplicationName.Text =
          this.NavigationContext.QueryString["appName"].ToUpperInvariant();
      }
    }
  }
}