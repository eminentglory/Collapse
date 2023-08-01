﻿using Microsoft.UI.Xaml.Controls;

namespace CollapseLauncher.Pages
{
    public sealed partial class NullPage : Page
    {
        public NullPage()
        {
            BackgroundImgChanger.ToggleBackground(true);
            this.InitializeComponent();
        }
    }
}
