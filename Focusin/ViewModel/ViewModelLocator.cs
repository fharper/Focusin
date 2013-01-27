using System;
using Focusin.Helpers;
using GalaSoft.MvvmLight.Ioc;

namespace Focusin.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public static readonly Uri SettingsPageUri = new Uri("/View/SettingsPage.xaml", UriKind.Relative);
        public static readonly Uri AboutPageUri = new Uri("/View/AboutPage.xaml", UriKind.Relative);
        public static readonly Uri InstructionsPageUri = new Uri("/View/InstructionsPage.xaml", UriKind.Relative);

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            // Ensure VM
            var main = SimpleIoc.Default.GetInstance<MainViewModel>();

            SimpleIoc.Default.Register<SettingsViewModel>();
            // Ensure VM
            var settings = SimpleIoc.Default.GetInstance<SettingsViewModel>();

            SimpleIoc.Default.Register<AboutViewModel>();
            // Ensure VM
            SimpleIoc.Default.GetInstance<AboutViewModel>();

            main.NavigationService = new NavigationService();
            settings.NavigationService = new NavigationService();
        }

        /// <summary>
        /// Gets the Main property which defines the main viewmodel.
        /// </summary>
        public MainViewModel Main
        {
            get { return SimpleIoc.Default.GetInstance<MainViewModel>(); }
        }

        public SettingsViewModel Settings
        {
            get { return SimpleIoc.Default.GetInstance<SettingsViewModel>(); }
        }

        public AboutViewModel About
        {
            get { return SimpleIoc.Default.GetInstance<AboutViewModel>(); }
        }
    }
}