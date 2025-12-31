// SPDX-License-Identifier: AGPL-3.0-only
/**
* Digital Voice Modem - Desktop Dispatch Console
* AGPLv3 Open Source. Use is subject to license terms.
* DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
*
* @package DVM / Desktop Dispatch Console
* @license AGPLv3 License (https://opensource.org/licenses/AGPL-3.0)
*
*   Copyright (C) 2025 Caleb, K4PHP
*   Copyright (C) 2025 Bryan Biedenkapp, N2PLL
*
*/

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dvmconsole
{
    /// <summary>
    /// Interaction logic for PatchesWindow.xaml.
    /// </summary>
    public partial class PatchesWindow : Window
    {
        private SettingsManager settingsManager;
        private Codeplug codeplug;

        /*
        ** Methods
        */

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchesWindow"/> class.
        /// </summary>
        /// <param name="settingsManager"></param>
        /// <param name="codeplug"></param>
        public PatchesWindow(SettingsManager settingsManager, Codeplug codeplug)
        {
            InitializeComponent();
            this.settingsManager = settingsManager;
            this.codeplug = codeplug;
            
            LoadPatches();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// Loads patches from the codeplug and displays them in tabs.
        /// </summary>
        public void LoadPatches()
        {
            patchesTabControl.Items.Clear();

            if (codeplug?.Patches == null)
                return;

            foreach (var patch in codeplug.Patches)
            {
                TabItem tabItem = new TabItem
                {
                    Header = patch.Name
                };

                // Create a scroll viewer for the patch content
                ScrollViewer scrollViewer = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
                };

                // Create a stack panel to hold the channels
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(10)
                };

                if (patch.Channels != null)
                {
                    foreach (var channel in patch.Channels)
                    {
                        // Create a horizontal panel for each channel
                        StackPanel channelPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(0, 5, 0, 5)
                        };

                        // Add the transmit busy icon
                        Image icon = new Image
                        {
                            Source = new System.Windows.Media.Imaging.BitmapImage(
                                new System.Uri("/dvmconsole;component/Assets/ind_transmit_busy.png", UriKind.Relative)),
                            Width = 24,
                            Height = 24,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(0, 0, 10, 0)
                        };
                        channelPanel.Children.Add(icon);

                        // Add the channel name
                        TextBlock channelName = new TextBlock
                        {
                            Text = channel.Name,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 14
                        };
                        channelPanel.Children.Add(channelName);

                        stackPanel.Children.Add(channelPanel);
                    }
                }

                scrollViewer.Content = stackPanel;
                tabItem.Content = scrollViewer;
                patchesTabControl.Items.Add(tabItem);
            }
        }

        /// <summary>
        /// Updates the patches display when codeplug changes.
        /// </summary>
        /// <param name="newCodeplug"></param>
        public void UpdateCodeplug(Codeplug newCodeplug)
        {
            this.codeplug = newCodeplug;
            LoadPatches();
        }
    } // public partial class PatchesWindow : Window
} // namespace dvmconsole

