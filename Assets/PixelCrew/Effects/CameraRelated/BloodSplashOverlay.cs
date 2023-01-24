using System;
using System.Runtime.InteropServices;
using PixelCrew.Model;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils.Disposable;
using UnityEngine;

namespace PixelCrew.Effects.CameraRelated
{
    [RequireComponent(typeof(Animator))]
    
    public class BloodSplashOverlay: MonoBehaviour
    {
        [SerializeField] private Transform _overLay;
        private static readonly int Health = Animator.StringToHash("Health");

        private Animator _animator;
        private Vector3 _overScale;
        private GameSession _session;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _overScale = _overLay.localScale - Vector3.one;

            _session = FindObjectOfType<GameSession>();
            _trash.Retain(_session.Data.Hp.SubscribeAndInvoke(OnHpChanged)); //подпишемся на изменение жизни
        }

        private void OnHpChanged(int newValue, int _)
        {
            var maxHp = _session.StatsModel.GetValue(StatId.Hp);
            var hpNormalized = newValue / maxHp;
            _animator.SetFloat(Health, hpNormalized);

            var overlayModifier = Mathf.Max(hpNormalized - 0.3f, 0f); //сдвигаем значение 
            _overLay.localScale = Vector3.one + _overScale * overlayModifier;
            //считаем зависимость скэйла от нашего текущего здоровья, эту зависимость сдвигаем
            //меньше, чем единичный вектор у нас быть не может, устанавливаем это с помощью конструкции Vector3.one
        }
        

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}