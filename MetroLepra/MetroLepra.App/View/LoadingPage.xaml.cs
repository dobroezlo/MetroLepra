using System;
using System.Threading.Tasks;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using MetroLepra.App.ViewModel;
using MetroLepra.Core;
using Microsoft.Phone;
using Microsoft.Phone.Controls;

namespace MetroLepra.App.View
{
    public partial class LoadingPage : PhoneApplicationPage
    {
        public LoadingPage()
        {
            InitializeComponent();
            this.progressBar.IsIndeterminate = true;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await Initialise();
        }

        private async Task Initialise()
        {
            if (ConnectionAgent.Current.IsAuthenticated)
            {
                var htmlPageModel = await ConnectionAgent.Current.GetMainPage();
                //DisplayMainPage(htmlPageModel);
            }
            else
            {
                var loginPageModel = await ConnectionAgent.Current.GetLoginPage();
                var imageStream = await ConnectionAgent.Current.GetImageStream(loginPageModel.CaptchaImageUrl);

                var loginPageViewModel = SimpleIoc.Default.GetInstance<LoginViewModel>();
                loginPageViewModel.CaptchaImage = PictureDecoder.DecodeJpeg(imageStream);
                loginPageViewModel.LoginCode = loginPageModel.LoginCode;
                
                NavigationService.Navigate(new Uri("/View/LoginPage.xaml", UriKind.Relative));
            }
        }
    }
}