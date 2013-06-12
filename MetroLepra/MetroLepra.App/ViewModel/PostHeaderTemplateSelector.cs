using System;
using System.Windows;
using MetroLepra.App.Helpers;

namespace MetroLepra.App.ViewModel
{
    public class PostHeaderTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageAndTextHeaderTemplate { get; set; }
        public DataTemplate ImageHeaderTemplate { get; set; }
        public DataTemplate TextHeaderTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var postHeaderViewModel = (PostViewModel) item;

            if (String.IsNullOrEmpty(postHeaderViewModel.HeaderText))
                return ImageHeaderTemplate;

            if (postHeaderViewModel.HeaderImage == null)
                return TextHeaderTemplate;

            return ImageAndTextHeaderTemplate;
        }
    }
}