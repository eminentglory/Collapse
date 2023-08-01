﻿using CollapseLauncher.Interfaces;
using Hi3Helper;
using Microsoft.Win32;
using System;
using System.Text;
using System.Text.Json;
using static CollapseLauncher.GameSettings.Base.SettingsBase;
using static Hi3Helper.Logger;

namespace CollapseLauncher.GameSettings.Universal
{
    internal class CollapseScreenSetting : IGameSettingsValue<CollapseScreenSetting>
    {
        #region Fields
        private const string _ValueName = "CollapseLauncher_ScreenSetting";
        #endregion

        #region Properties
        /// <summary>
        /// This defines if the game should run in a custom resolution.<br/><br/>
        /// Default: false
        /// </summary>
        public bool UseCustomResolution { get; set; } = false;

        /// <summary>
        /// This defines if the game should run in Exclusive Fullscreen mode.<br/><br/>
        /// Default: false
        /// </summary>
        public bool UseExclusiveFullscreen { get; set; } = false;

        /// <summary>
        /// This defines if the game should run in Borderless Screen mode. <br/><br/>
        /// Default: false
        /// </summary>
        public bool UseBorderlessScreen { get; set; } = false;

        /// <summary>
        /// This defines the Graphics API will be used for the game to run.<br/><br/>
        /// Values:<br/>
        ///     - 0 = DirectX 11 (Feature Level: 10.1)<br/>
        ///     - 1 = DirectX 11 (Feature Level: 11.0) No Single-thread<br/>
        ///     - 2 = DirectX 11 (Feature Level: 11.1)<br/>
        ///     - 3 = DirectX 11 (Feature Level: 11.1) No Single-thread<br/>
        ///     - 4 = DirectX 12 (Feature Level: 12.0) [Experimental]<br/><br/>
        /// Default: 3
        /// </summary>
        public byte GameGraphicsAPI { get; set; } = 3;
        #endregion

        #region Methods
#nullable enable
        public static CollapseScreenSetting Load()
        {
            try
            {
                if (RegistryRoot == null) throw new NullReferenceException($"Cannot load {_ValueName} RegistryKey is unexpectedly not initialized!");

                object? value = RegistryRoot.GetValue(_ValueName, null);

                if (value != null)
                {
                    ReadOnlySpan<byte> byteStr = (byte[])value;
#if DEBUG
                    LogWriteLine($"Loaded Collapse Screen Settings:\r\n{Encoding.UTF8.GetString((byte[])value, 0, ((byte[])value).Length - 1)}", LogType.Debug, true);
#endif
                    return (CollapseScreenSetting?)JsonSerializer.Deserialize(byteStr.Slice(0, byteStr.Length - 1), typeof(CollapseScreenSetting), UniversalSettingsJSONContext.Default) ?? new CollapseScreenSetting();
                }
            }
            catch (Exception ex)
            {
                LogWriteLine($"Failed while reading {_ValueName}\r\n{ex}", LogType.Error, true);
            }

            return new CollapseScreenSetting();
        }

        public void Save()
        {
            try
            {
                if (RegistryRoot == null) throw new NullReferenceException($"Cannot save {_ValueName} since RegistryKey is unexpectedly not initialized!");

                string data = JsonSerializer.Serialize(this, typeof(CollapseScreenSetting), UniversalSettingsJSONContext.Default) + '\0';
                byte[] dataByte = Encoding.UTF8.GetBytes(data);
#if DEBUG
                LogWriteLine($"Saved Collapse Screen Settings:\r\n{data}", LogType.Debug, true);
#endif
                RegistryRoot.SetValue(_ValueName, dataByte, RegistryValueKind.Binary);
            }
            catch (Exception ex)
            {
                LogWriteLine($"Failed to save {_ValueName}!\r\n{ex}", LogType.Error, true);
            }
        }

        public bool Equals(CollapseScreenSetting? comparedTo)
        {
            if (ReferenceEquals(this, comparedTo)) return true;
            if (comparedTo == null) return false;

            return comparedTo.UseCustomResolution == this.UseCustomResolution &&
                comparedTo.UseExclusiveFullscreen == this.UseExclusiveFullscreen &&
                comparedTo.GameGraphicsAPI == this.GameGraphicsAPI;
        }
#nullable disable
        #endregion
    }
}
