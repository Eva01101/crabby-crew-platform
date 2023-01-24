using System;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Utils.Disposable;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class AudioSettingsWidget: MonoBehaviour

    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _value;

        private FloatPersistentProperty _model;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private void Start()
        {
            _trash.Retain(_slider.onValueChanged.Subscribe(OnSliderValueChanged)); //изменяется значение слайдера
            
        }
        
        public void SetModel(FloatPersistentProperty model)
        {
            _model = model;
            _trash.Retain(model.Subscribe(OnValueChanged)); //подписка на события
            OnValueChanged(model.Value, model.Value);
        }

        private void OnSliderValueChanged(float value)
        {
            _model.Value = value; //обновим значение модели
        }
        
        private void OnValueChanged(float newValue, float oldValue)
        {
            var textValue = Mathf.Round(newValue * 100); //значение от 0 до 100 и округлим 
            _value.text = textValue.ToString();
            
            _slider.normalizedValue = newValue; //обновляем слайдер в обратную сторону
        }

        private void OnDestroy()
        {
            _trash.Dispose();//чтобы отписаться
        }
    }
}