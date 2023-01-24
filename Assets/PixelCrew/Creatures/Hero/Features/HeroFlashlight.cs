using System;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures.Hero.Features
{
    public class HeroFlashlight: MonoBehaviour
    {
        [SerializeField] private float _consumePerSecond; //потребление топлива в сек
        [SerializeField] private Light2D _light;
        
        private GameSession _session;
        private float _defaultIntensity;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _defaultIntensity = _light.intensity; //возьмём за 100%
        }

        private void Update()
        {
            var consumed = Time.deltaTime * _consumePerSecond; //время прошедшее с последнего кадра
            var currentValue = _session.Data.Fuel.Value;
            var nextValue = currentValue - consumed;
            nextValue = Mathf.Max(nextValue, 0);
            _session.Data.Fuel.Value = nextValue;

            var progress = Mathf.Clamp(nextValue / 20, 0, 1);
            _light.intensity = _defaultIntensity * progress;
        }
    }
}