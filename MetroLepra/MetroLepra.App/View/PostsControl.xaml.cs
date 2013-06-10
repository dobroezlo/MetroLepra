using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MetroLepra.App.ViewModel;

namespace MetroLepra.App.View
{
    public partial class PostsControl : UserControl
    {
        public static readonly DependencyProperty PostsDataSourceProperty =
            DependencyProperty.Register("PostsDataSource", typeof (ObservableCollection<PostViewModel>), typeof (PostsControl),
                                        new PropertyMetadata(null, OnPostsDataSourceChanged));

        public static readonly DependencyProperty IsDataSourceLoadingProperty =
            DependencyProperty.Register("IsDataSourceLoading", typeof (bool), typeof (PostsControl), new PropertyMetadata(true));

        public bool IsDataSourceLoading
        {
            get { return (bool) GetValue(IsDataSourceLoadingProperty); }
            set { SetValue(IsDataSourceLoadingProperty, value); }
        }

        public PostsControl()
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
            var control = (PostsControl) dependencyObject;
            control.PostsDataSourceChanged();
        }

        private void PostsDataSourceChanged()
        {
            if (PostsDataSource != null)
                IsDataSourceLoading = false;
        }
    }
}