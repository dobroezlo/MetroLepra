using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MetroLepra.Core;
using MetroLepra.Model;
using Microsoft.Phone;

namespace MetroLepra.App.ViewModel
{
    public class PostViewModel : ViewModelBase
    {
        private ObservableCollection<UIElement> _bodyXaml;
        private WriteableBitmap _headerImage;
        private bool _isBackgroundProccessRunning;

        [PreferredConstructor]
        public PostViewModel()
        {
        }

        public PostViewModel(PostModel postModel)
        {
            Model = postModel;
        }

        public String Author
        {
            get { return Model.Author.Username; }
            set
            {
                if (value == Model.Author.Username)
                    return;

                Model.Author.Username = value;
                RaisePropertyChanged(() => Author);
            }
        }

        public String HeaderText
        {
            get { return Model.HeaderText; }
            set
            {
                if (value == Model.HeaderText)
                    return;

                Model.HeaderText = value;
                RaisePropertyChanged(() => HeaderText);
            }
        }

        public String Date
        {
            get { return Model.Date; }
            set
            {
                if (value == Model.Date)
                    return;

                Model.Date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        public String Time
        {
            get { return Model.Time; }
            set
            {
                if (value == Model.Time)
                    return;

                Model.Time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        public String Body
        {
            get { return Model.Body; }
            set
            {
                if (value == Model.Body)
                    return;

                Model.Body = value;
                RaisePropertyChanged(() => Body);
            }
        }

        public String Rating
        {
            get { return Model.Rating; }
            set
            {
                if (value == Model.Rating)
                    return;

                Model.Rating = value;
                RaisePropertyChanged(() => Rating);
            }
        }

        public String HeaderImageUrl
        {
            get { return Model.HeaderImageUrl; }
            set
            {
                if (value == Model.HeaderImageUrl)
                    return;

                Model.HeaderImageUrl = value;
                RaisePropertyChanged(() => HeaderImageUrl);
            }
        }

        public WriteableBitmap HeaderImage
        {
            get { return _headerImage; }
            set
            {
                if (value == _headerImage)
                    return;

                _headerImage = value;
                RaisePropertyChanged(() => HeaderImage);
            }
        }

        public ObservableCollection<UIElement> BodyXaml
        {
            get { return _bodyXaml; }
            set
            {
                if (value == _bodyXaml)
                    return;

                _bodyXaml = value;
                RaisePropertyChanged(() => BodyXaml);
            }
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

        public PostModel Model { get; set; }

        public async Task DownloadHeaderImage()
        {
            if (String.IsNullOrEmpty(HeaderImageUrl))
                return;

            IsBackgroundProccessRunning = true;

            var imageStream = await ConnectionAgent.Current.GetImageStream(HeaderImageUrl);
            if (imageStream != null)
                HeaderImage = PictureDecoder.DecodeJpeg(imageStream);

            IsBackgroundProccessRunning = false;
        }

        public async Task CreateBodyXaml()
        {
            if (String.IsNullOrEmpty(Body))
                return;

            IsBackgroundProccessRunning = true;

            if(BodyXaml != null)
                BodyXaml.Clear();
            else
                BodyXaml = new ObservableCollection<UIElement>();

            var elements = await HtmlParser.ConvertHtmlToXaml(Body);
            foreach (var element in elements)
            {
                BodyXaml.Add(element);
            }

            IsBackgroundProccessRunning = false;
        }
    }
}