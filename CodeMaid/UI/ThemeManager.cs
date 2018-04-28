using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SteveCadwallader.CodeMaid.UI
{
    /// <summary>
    /// A helper class for managing the active theme.
    /// </summary>
    public class ThemeManager : Bindable
    {
        #region Fields

        private static Dictionary<ThemeMode, Uri> _themeUris;

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="ThemeManager" /> class.
        /// </summary>
        private static ThemeManager _instance;

        /// <summary>
        /// Gets an instance of the <see cref="ThemeManager" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="ThemeManager" /> class.</returns>
        internal static ThemeManager GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new ThemeManager(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManager" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private ThemeManager(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the active theme.
        /// </summary>
        public ThemeMode ActiveTheme
        {
            get { return GetPropertyValue<ThemeMode>(); }
            private set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets the dictionary of Uris corresponding to <see cref="ThemeMode" /> values.
        /// </summary>
        private static Dictionary<ThemeMode, Uri> ThemeUris
        {
            get
            {
                if (_themeUris == null)
                {
                    _themeUris = new Dictionary<ThemeMode, Uri>();

                    foreach (ThemeMode theme in Enum.GetValues(typeof(ThemeMode)))
                    {
                        var uriString = $@"/SteveCadwallader.CodeMaid;component/UI/Themes/CodeMaid{theme}Theme.xaml";
                        var uri = new Uri(uriString, UriKind.Relative);

                        _themeUris.Add(theme, uri);
                    }
                }

                return _themeUris;
            }
        }

        /// <summary>
        /// Gets the Spade content as a FrameworkElement, may be null.
        /// </summary>
        private FrameworkElement SpadeContent => _package.Spade?.Content as FrameworkElement;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Applies the appropriate theme based on current settings.
        /// </summary>
        public void ApplyTheme()
        {
            ActiveTheme = ResolveActiveTheme();

            ApplyThemeToElement(SpadeContent, ActiveTheme);
        }

        /// <summary>
        /// Resolves which theme should currently be active based on settings.
        /// </summary>
        /// <returns>The resolved theme.</returns>
        private ThemeMode ResolveActiveTheme()
        {
            var theme = (ThemeMode)Settings.Default.General_Theme;

            return theme == ThemeMode.AutoDetect ? AutoDetectTheme() : theme;
        }

        /// <summary>
        /// Auto-detects which theme should be active based on the current IDE settings.
        /// </summary>
        private ThemeMode AutoDetectTheme()
        {
            const int medianColor = 128 * 3;
            var bgColor = GetColorFromUInt(_package.IDE.GetThemeColor(vsThemeColors.vsThemeColorToolWindowBackground));

            return (bgColor.R + bgColor.G + bgColor.B) >= medianColor ? ThemeMode.Light : ThemeMode.Dark;
        }

        /// <summary>
        /// A simple converter for turning a <see cref="uint" /> into a <see cref="Color" />.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        /// <returns>The color.</returns>
        private static Color GetColorFromUInt(uint number)
        {
            return Color.FromRgb((byte)(number >> 16),
                                 (byte)(number >> 8),
                                 (byte)(number >> 0));
        }

        /// <summary>
        /// Applies the specified theme to the specified element.
        /// </summary>
        /// <param name="element">The element to theme.</param>
        /// <param name="theme">The theme to apply.</param>
        private void ApplyThemeToElement(FrameworkElement element, ThemeMode theme)
        {
            if (element == null) return;

            if (element.Resources == null)
            {
                element.Resources = new ResourceDictionary();
            }

            // Search to see if the theme is already applied, or if other themes need removed.
            foreach (var mergedDictionary in element.Resources.MergedDictionaries.ToList())
            {
                if (mergedDictionary.Source == ThemeUris[theme])
                {
                    // Theme is already applied, no further processing necessary.
                    return;
                }

                if (ThemeUris.ContainsValue(mergedDictionary.Source))
                {
                    // Remove any other theme dictionaries.
                    element.Resources.MergedDictionaries.Remove(mergedDictionary);
                }
            }

            // Apply the theme.
            var resourceDictionary = LoadResourceDictionary(ThemeUris[theme]);
            if (resourceDictionary != null)
            {
                element.Resources.MergedDictionaries.Insert(0, resourceDictionary);
            }
        }

        /// <summary>
        /// Attempts to load a resource dictionary for the specified theme URI.
        /// </summary>
        /// <param name="themeUri">The theme URI.</param>
        /// <returns>The loaded resource dictionary, otherwise null.</returns>
        private ResourceDictionary LoadResourceDictionary(Uri themeUri)
        {
            try
            {
                var dictionary = (ResourceDictionary)Application.LoadComponent(themeUri);
                dictionary.Source = themeUri;

                return dictionary;
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine($"Unable to load theme '{themeUri}'", ex);
                _package.IDE.StatusBar.Text = string.Format(Resources.CodeMaidFailedToLoadTheme0SeeOutputWindowForMoreDetails, themeUri);

                return null;
            }
        }

        #endregion Methods
    }
}