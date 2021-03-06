﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MetroLepra.Core;
using MetroLepra.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Lab.Resources;
using System.Runtime.InteropServices;

namespace Lab
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            TestLogin();
        }

        private async void TestLogin()
        {
            if (ConnectionAgent.Current.IsAuthenticated)
            {
                var htmlPageModel = await ConnectionAgent.Current.GetMainPage();
                DisplayMainPage(htmlPageModel);
            }
            else
            {
                var loginPageModel = await ConnectionAgent.Current.GetLoginPage();

                var error = await ConnectionAgent.Current.Login("dobroe-z", "aaaa", "aaaa", "aaaa");
            }
        }

        private void DisplayMainPage(MainPageModel htmlPageModel)
        {
            
        }

        private void RedirectToLoginPage()
        {
            
        }
    }
}