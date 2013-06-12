/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MetroLepra.App"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System;
using GalaSoft.MvvmLight.Ioc;
using MetroLepra.App.Interfaces;
using MetroLepra.App.Services;
using Microsoft.Practices.ServiceLocation;

namespace MetroLepra.App.ViewModel
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        public static readonly Uri MainPageUri = new Uri("/View/MainPage.xaml", UriKind.Relative);
        public static Uri LoginPageUri = new Uri("/View/LoginPage.xaml", UriKind.Relative);
        public static Uri PostPageUri = new Uri("/View/PostPage.xaml", UriKind.Relative);

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<PostViewModel>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
        }

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public LoginViewModel Login
        {
            get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
        }

        public PostViewModel Post
        {
            get { return ServiceLocator.Current.GetInstance<PostViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}