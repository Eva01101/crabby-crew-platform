using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.Windows.InGameMenu
{
    public class InGameMenuWindow: AnimatedWindow //чтобы сохранилась анимация
    {
        private float _defaultTimeScale;
        
        protected override void Start()
        {
            base.Start();

            _defaultTimeScale = Time.timeScale; //нужно сохранить текущее значение, когда мы только стартанули
            Time.timeScale = 0; //ставим игру на паузу
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow"); 
        }

        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");
            
            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);//нам нужно потереть сессию
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale; //а потом его вернуть, чтобы заново запустилась игра
        }
    }
}