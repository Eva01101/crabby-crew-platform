﻿using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.UI.Windows.Settings
{
    public class SettingsWindow: AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sfx;

            
        protected override void Start()
        {
            base.Start();
            
            _music.SetModel(GameSettings.I.Music); //передаём
            _sfx.SetModel(GameSettings.I.Sfx);//соответст модели
            
        }
    }
}