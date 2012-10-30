using System;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Combinator
{
    public enum RenderingService { Default = 0, iHackerNews, Instapaper, /*Readability,*/ ViewText };
    public enum SupportedOrientations { Portrait = 0, Landscape, Both };

    public class Settings
    {
        // Our isolated storage settings
        IsolatedStorageSettings settings;

        // The isolated storage key names of our settings
        const string SupportedOrientationsSettingKeyName = "Orientation";
        const string RenderingServiceSettingKeyName = "RenderingService";

        // The default value of our settings
        const SupportedOrientations SupportedOrientationsSettingDefault = SupportedOrientations.Both;
        const RenderingService RenderingServiceSettingDefault = RenderingService.Default;

        /// <summary>
        /// Constructor that gets the application settings.
        /// </summary>
        public Settings()
        {
            // Get the settings for this application.
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
           return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            settings.Save();
        }

        /// <summary>
        /// Property to get and set a SupportedOrientations Setting Key.
        /// </summary>
        public SupportedOrientations SupportedOrientations
        {
            get
            {
                return GetValueOrDefault<SupportedOrientations>(SupportedOrientationsSettingKeyName, SupportedOrientationsSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(SupportedOrientationsSettingKeyName, value))
                {
                   Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a RenderingService Setting Key.
        /// </summary>
        public RenderingService RenderingService
        {
            get
            {
                return GetValueOrDefault<RenderingService>(RenderingServiceSettingKeyName, RenderingServiceSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(RenderingServiceSettingKeyName, value))
                {
                   Save();
                }
            }
        }

        private static Settings _instance;
        public static Settings Current
        {
            get
            {
                 if (_instance == null)
                 {
                    _instance = new Settings();
                 }
                 return _instance;
            }
        }
    }
}
