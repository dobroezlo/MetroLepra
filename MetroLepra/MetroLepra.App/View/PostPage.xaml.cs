using System.Windows.Navigation;
using GalaSoft.MvvmLight.Ioc;
using MetroLepra.App.ViewModel;
using Microsoft.Phone.Controls;

namespace MetroLepra.App.View
{
    public partial class PostPage : PhoneApplicationPage
    {
        public PostPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var currentPost = SimpleIoc.Default.GetInstance<PostViewModel>();
            currentPost.CreateBodyXaml();
        }
    }
}