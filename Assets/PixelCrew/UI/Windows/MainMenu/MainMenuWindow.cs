using System;
using PixelCrew.UI.LevelsLoader;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.Windows.MainMenu
{
    public class MainMenuWindow: AnimatedWindow
    {
        private Action _closeAction;
        
        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow"); //путь до настроек 
        }

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel("Level1");
            };
            Close();
        }

        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
            throw new Exception("Test exception");
        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit(); //редактор не поймёт нас, созданим эдитор ниже

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; //остановим проигрывание
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
            
        }
    }
}