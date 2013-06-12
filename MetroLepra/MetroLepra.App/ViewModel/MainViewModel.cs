using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MetroLepra.App.Interfaces;
using MetroLepra.Core;
using Microsoft.Phone.Controls;

namespace MetroLepra.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private ObservableCollection<PostViewModel> _generalPosts;
        private ObservableCollection<PostViewModel> _inboxPosts;

        private ObservableCollection<PostViewModel> _myStuffPosts;
        private PanoramaItem _selectedPanoramaItem;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            PostTappedCommand = new RelayCommand<PostViewModel>(OnPostTapped);
        }

        public RelayCommand<PostViewModel> PostTappedCommand { get; set; }

        public ObservableCollection<PostViewModel> GeneralPosts
        {
            get { return _generalPosts; }
            set
            {
                if (value == _generalPosts)
                    return;

                _generalPosts = value;
                RaisePropertyChanged(() => GeneralPosts);
            }
        }

        public ObservableCollection<PostViewModel> MyStuffPosts
        {
            get { return _myStuffPosts; }
            set
            {
                if (value == _myStuffPosts)
                    return;

                _myStuffPosts = value;
                RaisePropertyChanged(() => MyStuffPosts);
            }
        }

        public PanoramaItem SelectedPanoramaItem
        {
            get { return _selectedPanoramaItem; }
            set
            {
                if (value == _selectedPanoramaItem)
                    return;

                _selectedPanoramaItem = value;
                RaisePropertyChanged(() => SelectedPanoramaItem);
                OnSelectedPanoramaItemChanged();
            }
        }

        public ObservableCollection<PostViewModel> InboxPosts
        {
            get { return _inboxPosts; }
            set
            {
                if (value == _inboxPosts)
                    return;

                _inboxPosts = value;
                RaisePropertyChanged(() => InboxPosts);
            }
        }

        private void OnPostTapped(PostViewModel post)
        {
            var currentPost = SimpleIoc.Default.GetInstance<PostViewModel>();
            currentPost.Model = post.Model;

            _navigationService.NavigateTo(ViewModelLocator.PostPageUri);
        }

        private void OnSelectedPanoramaItemChanged()
        {
            if (SelectedPanoramaItem.Name == "mainItem")
            {
                LoadGeneralPosts();
                LoadMyStuffPosts();
            }
            else if (SelectedPanoramaItem.Name == "menuItem")
            {
                LoadGeneralPosts();
                LoadInboxPosts();
            }
            else if (SelectedPanoramaItem.Name == "myStuffItem")
            {
                LoadMyStuffPosts();
                LoadInboxPosts();
                LoadGeneralPosts();
            }
        }

        private async Task LoadGeneralPosts()
        {
            var latestPosts = await ConnectionAgent.Current.GetLatestPosts();

            var latestPostsViewModel = latestPosts.Select(x => new PostViewModel(x)).ToList();
            if (latestPostsViewModel.Count == 0)
            {
                GeneralPosts = new ObservableCollection<PostViewModel>();
                return;
            }

            if (GeneralPosts != null)
                GeneralPosts.Clear();

            foreach (var postViewModel in latestPostsViewModel)
            {
                await postViewModel.DownloadHeaderImage();

                if (GeneralPosts == null)
                    GeneralPosts = new ObservableCollection<PostViewModel>();

                GeneralPosts.Add(postViewModel);
            }
        }

        private async Task LoadMyStuffPosts()
        {
            var myStuff = await ConnectionAgent.Current.GetMyStuff();

            var latestPostsViewModel = myStuff.Select(x => new PostViewModel(x)).ToList();
            if (latestPostsViewModel.Count == 0)
            {
                MyStuffPosts = new ObservableCollection<PostViewModel>();
                return;
            }

            if (MyStuffPosts != null)
                MyStuffPosts.Clear();

            foreach (var postViewModel in latestPostsViewModel)
            {
                await postViewModel.DownloadHeaderImage();

                if (MyStuffPosts == null)
                    MyStuffPosts = new ObservableCollection<PostViewModel>();
                MyStuffPosts.Add(postViewModel);
            }
        }

        private async Task LoadInboxPosts()
        {
            var inbox = await ConnectionAgent.Current.GetInbox();

            var inboxPostsViewModel = inbox.Select(x => new PostViewModel(x)).ToList();
            if (inboxPostsViewModel.Count == 0)
            {
                InboxPosts = new ObservableCollection<PostViewModel>();
                return;
            }

            if (InboxPosts != null)
                InboxPosts.Clear();

            foreach (var postViewModel in inboxPostsViewModel)
            {
                await postViewModel.DownloadHeaderImage();

                if (InboxPosts == null)
                    InboxPosts = new ObservableCollection<PostViewModel>();
                InboxPosts.Add(postViewModel);
            }
        }
    }
}