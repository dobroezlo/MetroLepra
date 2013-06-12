using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using MetroLepra.Core;
using MetroLepra.Model;
using Microsoft.Phone;

namespace MetroLepra.App.ViewModel
{
    public class PostViewModel : ViewModelBase
    {
        private readonly PostModel _model;
        private WriteableBitmap _headerImage;
        private bool _isBackgroundProccessRunning = true;

        public PostViewModel(PostModel postModel)
        {
            _model = postModel;
        }

        public String Author
        {
            get { return _model.Author.Username; }
            set
            {
                if (value == _model.Author.Username)
                    return;

                _model.Author.Username = value;
                RaisePropertyChanged(() => Author);
            }
        }

        public String HeaderText
        {
            get { return _model.HeaderText; }
            set
            {
                if (value == _model.HeaderText)
                    return;

                _model.HeaderText = value;
                RaisePropertyChanged(() => HeaderText);
            }
        }

        public String Date
        {
            get { return _model.Date; }
            set
            {
                if (value == _model.Date)
                    return;

                _model.Date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        public String Time
        {
            get { return _model.Time; }
            set
            {
                if (value == _model.Time)
                    return;

                _model.Time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        public String Body
        {
            get { return _model.Body; }
            set
            {
                if (value == _model.Body)
                    return;

                _model.Body = value;
                RaisePropertyChanged(() => Body);
            }
        }

        public String Rating
        {
            get { return _model.Rating; }
            set
            {
                if (value == _model.Rating)
                    return;

                _model.Rating = value;
                RaisePropertyChanged(() => Rating);
            }
        }

        public String HeaderImageUrl
        {
            get { return _model.HeaderImageUrl; }
            set
            {
                if (value == _model.HeaderImageUrl)
                    return;

                _model.HeaderImageUrl = value;
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

        public List<UIElement> BodyXaml { get; set; }

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

        public async Task DownloadHeaderImage()
        {
            if (String.IsNullOrEmpty(HeaderImageUrl))
                return;

            var imageStream = await ConnectionAgent.Current.GetImageStream(HeaderImageUrl);
            if (imageStream != null)
                HeaderImage = PictureDecoder.DecodeJpeg(imageStream);

            IsBackgroundProccessRunning = false;
        }
    }
}