using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace MetroLepra.App.View
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Previous page was LoadingPage, remove it from journal
            NavigationService.RemoveBackEntry();
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            // Update the binding source
            var bindingExpr = textBox.GetBindingExpression(TextBox.TextProperty);
            bindingExpr.UpdateSource();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var textBox = sender as PasswordBox;
            // Update the binding source
            var bindingExpr = textBox.GetBindingExpression(PasswordBox.PasswordProperty);
            bindingExpr.UpdateSource();
        }
    }
}