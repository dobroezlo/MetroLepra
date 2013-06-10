using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<PostViewModel> _generalPosts;

        private ObservableCollection<PostViewModel> _myStuffPosts;

        /// <summary>
        ///   Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

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

        public async Task LoadGeneralPosts()
        {
            var latestPosts = await ConnectionAgent.Current.GetLatestPosts();

            var latestPostsViewModel = latestPosts.Select(x => new PostViewModel(x)).ToList();
            GeneralPosts = new ObservableCollection<PostViewModel>(latestPostsViewModel);

            foreach (var postViewModel in GeneralPosts)
            {
                await postViewModel.DownloadHeaderImage();
            }
        }

        public async Task LoadMyStuffPosts()
        {
            var myStuff = await ConnectionAgent.Current.GetMyStuff();

            var latestPostsViewModel = myStuff.Select(x => new PostViewModel(x)).ToList();
            MyStuffPosts = new ObservableCollection<PostViewModel>(latestPostsViewModel);

            foreach (var postViewModel in MyStuffPosts)
            {
                await postViewModel.DownloadHeaderImage();
            }
        }
    }
}