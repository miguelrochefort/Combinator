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
using Caliburn.Micro;

namespace Combinator
{
    public class SettingsViewModel : PropertyChangedBase
    {
        private INavigationService _navigationService;
        public SettingsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SelectedSupportedOrientationsIndex = (int)Settings.Current.SupportedOrientations;
            SelectedRenderingServiceIndex = (int)Settings.Current.RenderingService;
        }

        public string SupportedOrientationsHeader
        {
            get
            {
                return "Supported orientations";
            }
        }

        public List<SupportedOrientations> SupportedOrientationsList
        {
            get
            {
                return EnumHelper.GetValues<SupportedOrientations>();
            }
        }

        private int _SelectedSupportedOrientationsIndex;
        public int SelectedSupportedOrientationsIndex
        {
            get
            {
                return _SelectedSupportedOrientationsIndex;
            }
            set
            {
                _SelectedSupportedOrientationsIndex = value;
                Settings.Current.SupportedOrientations = (SupportedOrientations)(SelectedSupportedOrientationsIndex);
            }
        }

        public string RenderingServiceHeader
        {
            get
            {
                return "Rendering service";
            }
        }

        public List<RenderingService> RenderingServiceList
        {
            get
            {
                return EnumHelper.GetValues<RenderingService>();
            }
        }

        private int _SelectedRenderingServiceIndex;
        public int SelectedRenderingServiceIndex
        {
            get
            {
                return _SelectedRenderingServiceIndex;
            }
            set
            {
                _SelectedRenderingServiceIndex = value;
                Settings.Current.RenderingService = (RenderingService)(SelectedRenderingServiceIndex);
            }
        }
    }
}
