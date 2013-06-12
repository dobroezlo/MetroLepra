using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MetroLepra.App.ViewModel;

namespace MetroLepra.App.Controls
{
    public partial class PostsListControl : UserControl
    {
        public static readonly DependencyProperty PostsDataSourceProperty =
            DependencyProperty.Register("PostsDataSource", typeof (ObservableCollection<PostViewModel>), typeof (PostsListControl),
                                        new PropertyMetadata(null, OnPostsDataSourceChanged));

        public static readonly DependencyProperty IsDataSourceLoadingProperty =
            DependencyProperty.Register("IsDataSourceLoading", typeof (bool), typeof (PostsListControl), new PropertyMetadata(true));

        public bool IsDataSourceLoading
        {
            get { return (bool) GetValue(IsDataSourceLoadingProperty); }
            set { SetValue(IsDataSourceLoadingProperty, value); }
        }

        public PostsListControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<PostViewModel> PostsDataSource
        {
            get { return (ObservableCollection<PostViewModel>) GetValue(PostsDataSourceProperty); }
            set { SetValue(PostsDataSourceProperty, value); }
        }

        private static void OnPostsDataSourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var control = (PostsListControl) dependencyObject;
            control.PostsDataSourceChanged();
        }

        private async void PostsDataSourceChanged()
        {
            if (PostsDataSource == null) return;

            IsDataSourceLoading = false;
        }
    }
}