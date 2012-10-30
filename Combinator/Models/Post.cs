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
using System.Collections.Generic;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;

namespace Combinator
{
    public class PostItem
    {
        string _Title;
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = Cleaner.CleanText(value);
            }
        }

        public string Url { get; set; }
        public string Item_ID { get; set; }
        public string Comments { get; set; }
        public string Score { get; set; }
        public string Time { get; set; }
        public string User { get; set; }
        public int CommentCount
        {
            get
            {
                int comments;
                if (Comments != null && Comments.Contains(" "))
                {
                    comments = int.Parse(Comments.Split(' ')[0]);
                }
                else
                {
                    comments = 0;
                }

                return comments > 99 ? 99 : comments;
            }
        }


        public string Description
        {
            get
            {
                return (Score != "" ? Score : "0 point") + " by " + (User != "" ? User : "unknown") + " " + (Time != "" ? Time : "a moment ago");
            }
        }

        public Caliburn.Micro.INavigationService NavigationService { get; set; }

        public void ShowComments()
        {
            NavigationService.Navigate(new Uri("/Views/PostView.xaml", UriKind.RelativeOrAbsolute));
            FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
            PostViewModel vm = new PostViewModel(this, NavigationService);
            root.DataContext = vm;
        }

        public void ShowPost()
        {
            string url;

            if (Url.StartsWith("item?id="))
            {
                string postId = Url.Split('=')[1];
                url = "http://ihackernews.com/comments/" + postId;
            }
            else
            {
                switch (Settings.Current.RenderingService)
                {
                    case RenderingService.iHackerNews:
                        url = "http://ihackernews.com/comments/" + Item_ID;
                        break;
                    case RenderingService.Instapaper:
                        url = "http://www.instapaper.com/text?u=" + Url;
                        break;
                    //case RenderingService.Readability:
                    //    url = "
                    //    break;
                    case RenderingService.ViewText:
                        url = "http://viewtext.org/article?url=" + Url;
                        break;
                    case RenderingService.Default:
                    default:
                        url = Url;
                        break;
                }
            }

            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new Uri(url, UriKind.Absolute);
            task.Show();
        }
    }

    public class CommentItem
    {
        public CommentItem()
        {
            IndentationLevel = 0;
        }

        public string Username { get; set; }

        private string _comment;
        public string Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = Cleaner.CleanText(value);
            }
        }
        public string ID { get; set; }
        public string Time { get; set; }
        public List<CommentItem> Children { get; set; }

        public string Description
        {
            get
            {
                return Username + " " + Time;
            }
        }

        // used to indent children comments
        public int IndentationLevel { get; set; }

        const int INDENTATION_BLOCK = 8;
        public string IndentationSize
        {
            get
            {
                return (INDENTATION_BLOCK * IndentationLevel).ToString() + ",0,24,6";
            }
        }

        //public List<string> Comments
        //{
        //    get
        //    {
                
        //    }
        //}
    }

    public static class Cleaner
    {
        public static string CleanText(string text)
        {
            // get rid of euro
            text = text.Replace("&euro;", "&amp;");
            // remove duplicate amp
            text = text.Replace("&amp;&amp;", "&amp;");

            // convertion
            text = text.Replace("&amp;sbquo;", "€");
            text = text.Replace("&sbquo;", "€"); // what?

            text = text.Replace("&amp;#62;", ">");
            text = text.Replace("&amp;#60;", "<");
            text = text.Replace("&amp;#38;", "&");
            text = text.Replace("&amp;rdquo;", "—");

            text = text.Replace("&amp;&trade;", "'");

            text = text.Replace("&amp;&oelig;", "\"");

            text = text.Replace("&amp;", "\""); //keep it last

            text = text.Replace("\\\"", "\"");

            text = text.Replace("__BR__", "\n\n");

            // proved to work, not optimized
            //text = text.Replace("&amp; rdquo;", "—");
            //text = text.Replace("&amp;rdquo;", "—");

            //text = text.Replace("&euro;&trade;", "'");
            //text = text.Replace("&amp;&trade;", "'");

            //text = text.Replace("&euro;&oelig;", "\"");
            //text = text.Replace("&amp;&oelig;", "\"");
            //text = text.Replace("&euro;&amp;&oelig;", "\"");
            //text = text.Replace("&amp;euro;", "\"");
            //text = text.Replace("\\\"", "\"");

            //text = text.Replace("&amp;sbquo;", "€");
            //text = text.Replace("&sbquo;", "€");

            //text = text.Replace("&amp;#62;", ">");
            //text = text.Replace("&amp;#60;", "<");

            //text = text.Replace("&amp;#38;", "&");
            
            //text = text.Replace("__BR__", "\n\n");

            return text;
        }
    }
}