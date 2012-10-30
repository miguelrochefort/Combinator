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
using System.ComponentModel;
using System.Windows.Navigation;

namespace Combinator
{
    public class MainViewModel : PropertyChangedBase
    {
        private RenderingService RenderingService
        {
            get
            {
                return Combinator.Settings.Current.RenderingService;
            }
        }

        public SupportedOrientations SupportedOrientations
        {
            get
            {
                return Combinator.Settings.Current.SupportedOrientations;
            }
        }

        public string SupportedOrientationsText
        {
            get
            {
                return SupportedOrientations.ToString();
            }
        }
        
        private INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            TopPosts = new PostListViewModel(PostType.Top, navigationService, NotifyLoading);
            NewPosts = new PostListViewModel(PostType.New, navigationService, NotifyLoading);
            AskPosts = new PostListViewModel(PostType.Ask, navigationService, NotifyLoading);

            _navigationService = navigationService;
        }

        int _Loadings;
        private void NotifyLoading(bool loading)
        {
            if (loading)
                _Loadings += 1;
            else
                _Loadings -= 1;
            NotifyOfPropertyChange(() => IsLoading);
        }

        bool _IsLoading;
        public bool IsLoading
        {
            get
            {
                return _Loadings != 0;
            }
            set
            {
                _IsLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        PostListViewModel topPosts;
        public PostListViewModel TopPosts
        {
            get { return topPosts; }
            set
            {
                topPosts = value;
                NotifyOfPropertyChange(() => TopPosts);
            }
        }

        PostListViewModel newPosts;
        public PostListViewModel NewPosts
        {
            get { return newPosts; }
            set
            {
                newPosts = value;
                NotifyOfPropertyChange(() => NewPosts);
            }
        }

        PostListViewModel askPosts;
        public PostListViewModel AskPosts
        {
            get { return askPosts; }
            set
            {
                askPosts = value;
                NotifyOfPropertyChange(() => AskPosts);
            }
        }

        public void Refresh()
        {
            if (IsLoading)
                return;

            Load(PostType.Top);
            Load(PostType.New);
            Load(PostType.Ask);
            NotifyOfPropertyChange(() => IsLoading);
        }

        public void About()
        {
            _navigationService.Navigate(new Uri("/Views/AboutView.xaml", UriKind.RelativeOrAbsolute));
            FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
            root.DataContext = new AboutViewModel();
        }

        public void Settings()
        {
            _navigationService.Navigate(new Uri("/Views/SettingsView.xaml", UriKind.RelativeOrAbsolute));
            FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
            root.DataContext = new SettingsViewModel(_navigationService);
        }

        public void Load(PostType type)
        {
            switch (type)
            {
                case PostType.Top:
                    TopPosts.Load();
                    break;
                case PostType.New:
                    NewPosts.Load();
                    break;
                case PostType.Ask:
                    AskPosts.Load();
                    break;
                default:
                    break;
            }
        }
    }
}
