using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace MetroLepra.App.Services
{
    public static class WebBrowserHelper
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", typeof(string), typeof(WebBrowserHelper), new PropertyMetadata(OnHtmlChanged));

        public static string GetHtml(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(HtmlProperty, value);
        }

        private static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebBrowser;

            if (browser == null)
                return;

            var html = e.NewValue.ToString();

            html = FastConvertExtendedASCII(html);

            browser.NavigateToString(String.Format("<html><body>{0}</body></html>", html));
        }

        private static string FastConvertExtendedASCII(string HTML)
        {
            char[] s = HTML.ToCharArray();

            // Getting number of characters to be converted
            // and calculate extra space
            int n = 0;
            int value;
            foreach (char c in s)
            {
                if ((value = Convert.ToInt32(c)) > 127)
                {
                    if (value > 9999)
                        n += 7;
                    else if (value > 999)
                        n += 6;
                    else
                        n += 5;
                }
            }

            // To avoid new string instantiating
            // allocate memory buffer for final string
            char[] res = new char[HTML.Length + n];

            // Conversion
            int i = 0;
            int div;
            const int zero = (int)'0';
            foreach (char c in s)
            {
                if ((value = Convert.ToInt32(c)) > 127)
                {
                    res[i++] = '&';
                    res[i++] = '#';

                    if (value > 9999)
                        div = 10000;
                    else if (value > 999)
                        div = 1000;
                    else
                        div = 100;

                    while (div > 0)
                    {
                        res[i++] = (char)(zero + value / div);
                        value %= div;
                        div /= 10;
                    }

                    res[i++] = ';';
                }
                else
                {
                    res[i] = c;
                    i++;
                }
            }

            return new string(res);
        }
    }
}