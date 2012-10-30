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
using Caliburn.Micro;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;

namespace Combinator
{
    public class PostListViewModel : PropertyChangedBase
    {
        private INavigationService _navigationService;

        public PostListViewModel(PostType postType, INavigationService navigationService, System.Action<bool> NotifyLoading)
        {
            _navigationService = navigationService;

            _ErrorMessage = "";
            _Posts = new BindableCollection<PostItem>();
            _IsLoading = false;
            NotifyLoadingAction = NotifyLoading;

            PostType = postType;
            Load();
        }

        private System.Action<bool> NotifyLoadingAction
        {
            get;
            set;
        }

        public PostType PostType
        {
            get;
            set;
        }

        string _ErrorMessage;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        BindableCollection<PostItem> _Posts;
        public BindableCollection<PostItem> Posts
        {
            get { return _Posts; }
            set
            {
                _Posts = value;
                NotifyOfPropertyChange(() => Posts);
            }
        }

        bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
                NotifyLoadingAction(_IsLoading);
            }
        }

        public void Load()
        {
            ErrorMessage = "";
            Posts.Clear();
            IsLoading = true;
            HackerNewsApi api = new HackerNewsApi();
            api.GetPosts(PostType, (posts) =>
            {
                // last item is a reference to the next page, could fix by specifying a page (or use this with a "more" button)
                posts.RemoveAt(posts.Count - 1);
                
                // used to make the object know how to navigate... // hack
                foreach (PostItem post in posts)
                {
                    post.NavigationService = _navigationService;
                }

                Posts = new BindableCollection<PostItem>(posts);
                IsLoading = false;
            }, (error) =>
            {
                ErrorMessage = error;
                IsLoading = false;
            });
        }

        public int SelectedIndex
        {
            get { return -1; }
            set
            {
                if (value != -1)
                {
                    Posts[value].ShowPost();
                }
            }
        }
    }
}
