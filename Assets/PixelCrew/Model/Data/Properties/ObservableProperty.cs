using System;
using PixelCrew.Utils.Disposable;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class ObservableProperty<TPropertyType>
    {
        [SerializeField] protected TPropertyType _value;
        
        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldValue); //делегат, чтобы отлавливать события изменения нашего проперти
        
        public event OnPropertyChanged OnChanged;
        
         
        public IDisposable Subscribe(OnPropertyChanged call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call); //сразу отпишемся 
       }
        
        public IDisposable SubscribeAndInvoke(OnPropertyChanged call)
        {
            OnChanged += call;
            var dispose = new ActionDisposable(() => OnChanged -= call); //сразу отпишемся 
            call(_value, _value);
            return dispose; //сразу же вызовем метод отписки
        }


        public virtual TPropertyType Value
        {
            get => _value;
            set
            {
                if (_value == null) 
                {
                    _value = value;
                    return;
                }
                var isSame = _value.Equals(value); //сохр если что-то изменилось
                if(isSame) return;
                var oldValue = _value;
                _value = value; //сначала нужно заменить
                InvokeChangedEvent(_value, oldValue);//потом сказать, что оно изменилось
                //это касательно выбора инвентаря 
            }
        }

        protected void InvokeChangedEvent(TPropertyType newValue, TPropertyType oldValue)
        {
            OnChanged?.Invoke(newValue, oldValue);
        }
    }
}