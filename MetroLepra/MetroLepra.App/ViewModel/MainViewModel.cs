using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MetroLepra.App.Interfaces;
using MetroLepra.Core;

namespace MetroLepra.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private List<PostViewModel> _generalPosts;

        private bool _isBackgroundProccessRunning;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool IsBackgroundProccessRunning
        {
            get { return _isBackgroundProccessRunning; }
            set
            {
                if (value == _isBackgroundProccessRunning)
                    return;

                _isBackgroundProccessRunning = value;
                RaisePropertyChanged(() => IsBackgroundProccessRunning);
            }
        }

        public List<PostViewModel> GeneralPosts
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

        public async Task LoadGeneralPosts()
        {
            IsBackgroundProccessRunning = true;
            var latestPosts = await ConnectionAgent.Current.GetLatestPosts();

            var latestPostsViewModel = latestPosts.Select(x => new PostViewModel(x)).ToList();
            foreach (var postViewModel in latestPostsViewModel)
            {
                await postViewModel.DownloadHeaderImage();
            }

            GeneralPosts = latestPostsViewModel;
            IsBackgroundProccessRunning = false;
        }
    }
}