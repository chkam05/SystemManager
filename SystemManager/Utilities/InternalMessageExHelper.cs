﻿using chkam05.Tools.ControlsEx.Events;
using chkam05.Tools.ControlsEx.InternalMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.Data.Configuration;

namespace SystemManager.Utilities
{
    public static class InternalMessageExHelper
    {

        //  METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Set internal message appearance. </summary>
        /// <param name="internalMessageEx"> InternalMessageEx. </param>
        public static void SetInternalMessageAppearance(InternalMessageEx internalMessageEx)
        {
            SetBaseInternalMessageAppearance(internalMessageEx);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Set files selector internal message appearance. </summary>
        /// <param name="internalMessageEx"> FilesSelectorInternalMessageEx. </param>
        public static void SetFilesSelectorInternalMessageAppearance(FilesSelectorInternalMessageEx internalMessageEx)
        {
            var appearanceConfig = AppearanceConfig.Instance;

            internalMessageEx.TextBoxMouseOverBackground = appearanceConfig.ThemeMouseOverBrush;
            internalMessageEx.TextBoxMouseOverBorderBrush = appearanceConfig.AccentMouseOverBrush;
            internalMessageEx.TextBoxMouseOverForeground = appearanceConfig.ThemeForegroundBrush;
            internalMessageEx.TextBoxSelectedBackground = appearanceConfig.ThemeSelectedBrush;
            internalMessageEx.TextBoxSelectedBorderBrush = appearanceConfig.AccentSelectedBrush;
            internalMessageEx.TextBoxSelectedForeground = appearanceConfig.ThemeForegroundBrush;
            internalMessageEx.TextBoxSelectedTextBackground = appearanceConfig.AccentColorBrush;

            SetBaseInternalMessageAppearance(internalMessageEx);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Set standard internal message appearance. </summary>
        /// <param name="internalMessageEx"> StandardInternalMessageEx. </param>
        private static void SetBaseInternalMessageAppearance<T>(BaseInternalMessageEx<T> internalMessageEx) where T : InternalMessageCloseEventArgs
        {
            var appearanceConfig = AppearanceConfig.Instance;

            internalMessageEx.Background = appearanceConfig.ThemeBackgroundBrush;
            internalMessageEx.BorderBrush = appearanceConfig.AccentColorBrush;
            internalMessageEx.BottomBackground = appearanceConfig.ThemeBackgroundBrush;
            internalMessageEx.BottomBorderBrush = appearanceConfig.AccentColorBrush;
            internalMessageEx.ButtonBackground = appearanceConfig.AccentColorBrush;
            internalMessageEx.ButtonBorderBrush = appearanceConfig.AccentColorBrush;
            internalMessageEx.ButtonForeground = appearanceConfig.AccentForegroundBrush;
            internalMessageEx.ButtonMouseOverBackground = appearanceConfig.AccentMouseOverBrush;
            internalMessageEx.ButtonMouseOverBorderBrush = appearanceConfig.AccentMouseOverBrush;
            internalMessageEx.ButtonMouseOverForeground = appearanceConfig.AccentForegroundBrush;
            internalMessageEx.ButtonPressedBackground = appearanceConfig.AccentPressedBrush;
            internalMessageEx.ButtonPressedBorderBrush = appearanceConfig.AccentPressedBrush;
            internalMessageEx.ButtonPressedForeground = appearanceConfig.AccentForegroundBrush;
            internalMessageEx.Foreground = appearanceConfig.ThemeForegroundBrush;
            internalMessageEx.HeaderBackground = appearanceConfig.ThemeBackgroundBrush;
            internalMessageEx.HeaderBorderBrush = appearanceConfig.AccentColorBrush;
        }

    }
}
