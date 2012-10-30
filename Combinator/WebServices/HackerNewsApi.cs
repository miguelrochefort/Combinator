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
using RestSharp;
using System.Collections.Generic;


namespace Combinator
{
    public enum PostType { Top, New, Ask }

    public class HackerNewsApi
    {
        private const string BASE_URL = "http://hndroidapi.appspot.com";
        private const string APP_ID = "combinator";
        private const string FORMAT = "json";

        private const string POSTS_ROOT_ELEMENT = "items";
        private const string COMMENTS_ROOT_ELEMENT = "items";

        private const int MAX_ATTEMPT_COUNT = 10;

        public HackerNewsApi()
        {
        }

        public void GetPosts(PostType postType, Action<List<PostItem>> success, Action<string> error)
        {
            string postTypeString = PostTypeString(postType);

            var request = new RestRequest();
            request.Resource = BASE_URL + "/" + postTypeString + "/" + "format" + "/" + FORMAT + "/" + "page" + "/" + "?" + "appid=" + APP_ID;
            request.RootElement = POSTS_ROOT_ELEMENT;

            // common
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            //request.Resource += "?time=" + DateTime.Now.Ticks;

            Execute<List<PostItem>>(request, success, error);
        }

        public void GetComments(string postId, Action<List<CommentItem>> success, Action<string> error)
        {
            //http://hndroidapi.appspot.com/nestedcomments/format/json/id/4060629?appid=combinator

            var request = new RestRequest();
            request.Resource = BASE_URL + "/" + "nestedcomments" + "/" + "format" + "/" + FORMAT + "/" + "id" + "/" + postId + "?" + "appid=" + APP_ID;
            request.RootElement = COMMENTS_ROOT_ELEMENT;

            // common
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.GET;
            //request.Resource += "?time=" + DateTime.Now.Ticks;

            Execute<List<CommentItem>>(request, success, error);
        }

        private string PostTypeString(PostType type)
        {
            switch (type)
            {
                case PostType.Top:
                    return "news";
                case PostType.New:
                    return "newest";
                case PostType.Ask:
                    return "ask";
                default:
                    return "";
            }
        }

        public void Execute<T>(RestRequest request, Action<T> callback, Action<string> error) where T : new()
        {
            var client = new RestClient();
            //client.BaseUrl = BaseUrl;

            if (request.Attempts >= MAX_ATTEMPT_COUNT)
            {
                error("An error occured. Please try again later.");
            }
            else
            {
                client.ExecuteAsync<T>(request, (response) =>
                    {
                        if (response.Data == null)
                        {
                            Execute<T>(request, callback, error);
                        }
                        else
                        {
                            callback(response.Data);
                        }
                    });
            }

            request.IncreaseNumAttempts();
        }
    }
}
