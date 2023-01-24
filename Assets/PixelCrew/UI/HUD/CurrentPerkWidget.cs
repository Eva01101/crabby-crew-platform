using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD
{
    public class CurrentPerkWidget: MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownImage; //заливка кулдавн
        
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();//получим сесси, чтобы достучаться до модели
        }

        public void Set(PerkDef perkDef)
        {
            _icon.sprite = perkDef.Icon;
        }

        private void Update()//будем смотреть значение кулдавна и обновлять fillAmount _cooldownImage
        {
            var cooldown = _session.PerksModel.Cooldown;
            _cooldownImage.fillAmount = cooldown.RemainingTime / cooldown.Value; //fillAmount - это от 0 до 1 нормализованное знач заполнения нашего имэджа
            //RemainingTime - оставшееся время
            //получаем нормализированное значение заполнения имэйджа
        }
    }
}