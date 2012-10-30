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
using System.Collections.Generic;
using Microsoft.Phone.Tasks;

namespace Combinator
{
    public class PostViewModel : PropertyChangedBase
    {
        public PostViewModel()
        {
        }

        INavigationService _navigationService;
        public PostViewModel(PostItem post, INavigationService navigationService)
        {
            Post = post;
            _navigationService = navigationService;
            Load();
        }

        private string _ErrorMessage;
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                _ErrorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
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
            }
        }

        public PostItem _Post;
        public PostItem Post
        {
            get
            {
                return _Post;
            }
            set
            {
                _Post = value;
                NotifyOfPropertyChange(() => Post);
            }
        }

        BindableCollection<CommentItem> _Comments;

        public BindableCollection<CommentItem> Comments
        {
            get { return _Comments; }
            set
            {
                _Comments = value;
                NotifyOfPropertyChange(() => Comments);
            }
        }

        public void Load()
        {
            HackerNewsApi api = new HackerNewsApi();

            ErrorMessage = "";
            IsLoading = true;
            api.GetComments(Post.Item_ID, (comments) =>
            {
                comments = OrderComments(comments);
                Comments = new BindableCollection<CommentItem>(comments);
                IsLoading = false;
            }, (error) =>
            {
                IsLoading = false;
            });
        }

        // put all comments in one single dimension list (no nested children), and add the indentation level to each item
        private List<CommentItem> OrderComments(List<CommentItem> comments)
        {
            List<CommentItem> newComments = new List<CommentItem>();

            foreach (CommentItem comment in comments)
            {
                comment.IndentationLevel = 0;
                newComments.Add(comment);

                foreach (CommentItem comment1 in comment.Children)
                {
                    comment1.IndentationLevel = 1;
                    newComments.Add(comment1);

                    foreach (CommentItem comment2 in comment1.Children)
                    {
                        comment2.IndentationLevel = 2;
                        newComments.Add(comment2);

                        foreach (CommentItem comment3 in comment2.Children)
                        {
                            comment3.IndentationLevel = 3;
                            newComments.Add(comment3);

                            foreach (CommentItem comment4 in comment3.Children)
                            {
                                comment4.IndentationLevel = 4;
                                newComments.Add(comment4);

                                foreach (CommentItem comment5 in comment4.Children)
                                {
                                    comment5.IndentationLevel = 5;
                                    newComments.Add(comment5);
                                }
                            }
                        }
                    }
                }
            }

            return newComments;
        }

        public void Read()
        {
            Post.ShowPost();
        }

        public string SupportedOrientationsText
        {
            get
            {
                return Combinator.Settings.Current.SupportedOrientations.ToString();
            }
        }

        public void Settings()
        {
            _navigationService.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.RelativeOrAbsolute));
            FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
            root.DataContext = new SettingsViewModel(_navigationService);
        }
    }
}
