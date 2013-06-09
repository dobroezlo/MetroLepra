using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using HtmlAgilityPack;
using MetroLepra.Core;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MetroLepra.App.View
{
    public partial class Test : PhoneApplicationPage
    {
        public Test()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var posts = await ConnectionAgent.Current.GetInbox();

            var post = posts[0];

            var body = post.Body;

            var doc = new HtmlDocument();
            doc.LoadHtml(body);

            var nodes = doc.DocumentNode.DescendantNodes().ToList();

            var elements = new List<UIElement>();
            foreach (var htmlNode in nodes)
            {
                if (htmlNode.Name == "#text")
                {
                    var text = new TextBlock();
                    text.TextWrapping = TextWrapping.Wrap;
                    text.Text = htmlNode.InnerText;
                    elements.Add(text);
                    continue;
                }
                else if (htmlNode.Name == "p")
                {
                    var text = new TextBlock();
                    text.Inlines.Add(new LineBreak());
                    elements.Add(text);
                    continue;
                }
                else if (htmlNode.Name == "img")
                {
                    var src = Regex.Match(htmlNode.OuterHtml, "<img src=\"(.+?)\"").Groups[1].Value;

                    var image = new Image();
                    var imageStream = await ConnectionAgent.Current.GetImageStream(src);
                    image.Source = PictureDecoder.DecodeJpeg(imageStream);

                    elements.Add(image);
                }
            }

            list.ItemsSource = new ObservableCollection<UIElement>(elements);
        }
    }
}