using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Tasks;

namespace Combinator
{
    public class AboutViewModel
    {
        public AboutViewModel()
        {

        }

        public void Email()
        {
            EmailComposeTask email = new EmailComposeTask();
            email.To = "miguel.rochefort@gmail.com";
            email.Subject = "Combinator v2.1";
            email.Show();
        }

        public void Twitter()
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri("https://twitter.com/#!/miguelrochefort", UriKind.Absolute);
            browser.Show();
        }

        public void Rate()
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        public void Share()
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();
            shareLinkTask.Title = "Combinator";
            shareLinkTask.LinkUri = new Uri("http://windowsphone.com/s?appid=a5275a3f-6611-48f6-bd62-7382eda4c028", UriKind.Absolute);
            shareLinkTask.Message = "Try Combinator, the best Hacker News client on Windows Phone!";
            shareLinkTask.Show();
        }

        public void CaliburnMicro()
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri("http://caliburnmicro.codeplex.com/", UriKind.Absolute);
            browser.Show();
        }

        public void iHackerNewsAPI()
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri("http://api.ihackernews.com/", UriKind.Absolute);
            browser.Show();
        }

        public void HNDroidAPI()
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri("http://hndroidapi.appspot.com/", UriKind.Absolute);
            browser.Show();
        }

        public void RestSharp()
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri("http://restsharp.org/", UriKind.Absolute);
            browser.Show();
        }

        public void JsonNET()
        {
            WebBrowserTask browser = new WebBrowserTask();
            browser.Uri = new Uri("http://json.codeplex.com/", UriKind.Absolute);
            browser.Show();
        }
    }
}
