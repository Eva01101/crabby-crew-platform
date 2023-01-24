using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposable;
using UnityEngine;

namespace PixelCrew.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;
        [SerializeField] private CurrentPerkWidget _currentPerk;
        
        private GameSession _session;
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            if(_session == null) return;
            
            _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHealthChanged)); //подписываемся на изменения здоровья

            _trash.Retain(_session.PerksModel.Subscribe(OnPerkChanged));
            
            //OnHealthChanged(_session.Data.Hp.Value,0); //запустим, чтобы обновить прогресс бар
            OnPerkChanged();
        }

        private void OnPerkChanged()
        {
            var usedPerkId =_session.PerksModel.Used;//получим используемый перк
            var hasPerk = !string.IsNullOrEmpty(usedPerkId);
            if (hasPerk)
            {
                var perkDef = DefsFacade.I.Perks.Get(usedPerkId);
                _currentPerk.Set(perkDef); //если у нас перк выбран не будет, виджет спрячем
            }
            
            _currentPerk.gameObject.SetActive(hasPerk);//если перк выбр будет, покажем
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = _session.StatsModel.GetValue(StatId.Hp);
            var value = (float) newValue / maxHealth; //получим нормализированное значение прогресс бара от 0 до 1
                //это два инта, нужно, чтобы хот один был приведён в флоат, чтобы процесс деления был не целочисленным
                _healthBar.SetProgress(value);
        }

        public void OnSettings()
        {
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
        }

        private void OnDestroy()
        {
            _trash.Dispose();
            //_session.Data.Hp.OnChanged -= OnHealthChanged;
        }

        public void OnDebug()
        {
            WindowUtils.CreateWindow("UI/PlayerStatsWindow"); 
        }
    }
}