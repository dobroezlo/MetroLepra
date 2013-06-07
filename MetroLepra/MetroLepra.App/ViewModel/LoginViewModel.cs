using System;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MetroLepra.App.Interfaces;
using MetroLepra.Core;

namespace MetroLepra.App.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private WriteableBitmap _captchaImage;
        private String _captchaText;
        private String _errorMessage;
        private bool _isLoginProcessRunning;
        private bool _isProgressMessageVisible;

        private String _password;
        private String _username;

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            OkCommand = new RelayCommand(OnLogin, CanLogin);
        }

        public String Username
        {
            get { return _username; }
            set
            {
                if (value == _username)
                    return;

                _username = value;
                RaisePropertyChanged(() => Username);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public String Password
        {
            get { return _password; }
            set
            {
                if (value == _password)
                    return;

                _password = value;
                RaisePropertyChanged(() => Password);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public String CaptchaText
        {
            get { return _captchaText; }
            set
            {
                if (value == _captchaText)
                    return;

                _captchaText = value;
                RaisePropertyChanged(() => CaptchaText);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public WriteableBitmap CaptchaImage
        {
            get { return _captchaImage; }
            set
            {
                if (value == _captchaImage)
                    return;

                _captchaImage = value;
                RaisePropertyChanged(() => CaptchaImage);
            }
        }

        public String ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (value == _errorMessage)
                    return;

                _errorMessage = value;
                RaisePropertyChanged(() => ErrorMessage);
            }
        }

        public bool IsProgressMessageVisible
        {
            get { return _isProgressMessageVisible; }
            set
            {
                if (value == _isProgressMessageVisible)
                    return;

                _isProgressMessageVisible = value;
                RaisePropertyChanged(() => IsProgressMessageVisible);
            }
        }

        public RelayCommand OkCommand { get; set; }

        public string LoginCode { get; set; }

        private bool CanLogin()
        {
            return !String.IsNullOrWhiteSpace(Username) && !String.IsNullOrWhiteSpace(Password) && !String.IsNullOrWhiteSpace(CaptchaText) &&
                   !_isLoginProcessRunning;
        }

        private async void OnLogin()
        {
            IsProgressMessageVisible = true;
            ErrorMessage = String.Empty;
            _isLoginProcessRunning = true;
            OkCommand.RaiseCanExecuteChanged();

            var error = await ConnectionAgent.Current.Login(Username, Password, CaptchaText, LoginCode);

            if (!String.IsNullOrEmpty(error))
            {
                ErrorMessage = error;
                IsProgressMessageVisible = false;
                _isLoginProcessRunning = false;
                OkCommand.RaiseCanExecuteChanged();
                return;
            }

            App.MainPageModel = await ConnectionAgent.Current.GetMainPage();

            _navigationService.NavigateTo(ViewModelLocator.MainPageUri);
        }
    }
}