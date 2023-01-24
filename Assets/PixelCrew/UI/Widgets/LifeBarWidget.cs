using System;
using PixelCrew.Components.Health;
using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposable;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class LifeBarWidget: MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _lifeBar;
        [SerializeField] private HealthComponent _hp;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private int _maxHp;//сохраним на момент старта

        private void Start()
        {
            if (_hp == null)
                _hp = GetComponentInParent<HealthComponent>();//будем пытаться найти компонен в родителях

            _maxHp = _hp.Health;

           _trash.Retain(_hp._onDie.Subscribe(OnDie)) ;//удалять значение прогресс бара
            _trash.Retain(_hp._onChange.Subscribe(OnHpChanged)); //обновлять знач прогресс бара
        }

        private void OnDie()
        {
            Destroy(gameObject);
        }

        private void OnHpChanged(int hp)
        {
            var progress = (float) hp / _maxHp; 
            _lifeBar.SetProgress(progress);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}