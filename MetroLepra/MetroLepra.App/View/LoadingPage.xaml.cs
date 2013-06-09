using System;
using System.Threading.Tasks;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using MetroLepra.App.Interfaces;
using MetroLepra.App.ViewModel;
using MetroLepra.Core;
using Microsoft.Phone;
using Microsoft.Phone.Controls;

namespace MetroLepra.App.View
{
    public partial class LoadingPage : PhoneApplicationPage
    {
        private readonly INavigationService _navigationService;

        public LoadingPage()
        {
            _navigationService = SimpleIoc.Default.GetInstance<INavigationService>();

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
                App.MainPageModel = await ConnectionAgent.Current.GetMainPage();

                _navigationService.NavigateTo(ViewModelLocator.MainPageUri);
                //NavigationService.Navigate(new Uri("/View/Test.xaml", UriKind.Relative));
            }
            else
            {
                var loginPageModel = await ConnectionAgent.Current.GetLoginPage();
                var imageStream = await ConnectionAgent.Current.GetImageStream(loginPageModel.CaptchaImageUrl);

                var loginPageViewModel = SimpleIoc.Default.GetInstance<LoginViewModel>();
                loginPageViewModel.CaptchaImage = PictureDecoder.DecodeJpeg(imageStream);
                loginPageViewModel.LoginCode = loginPageModel.LoginCode;

                _navigationService.NavigateTo(ViewModelLocator.LoginPageUri);
            }
        }
    }
}