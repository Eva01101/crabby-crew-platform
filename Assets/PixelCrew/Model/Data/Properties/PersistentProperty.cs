using System;
using PixelCrew.Utils.Disposable;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties //сохр в постоян память на диск
{
    public abstract class PersistentProperty <TPropertyType>: ObservableProperty<TPropertyType> //тип у нас дженерик, который мы определим в наследниках позже
    {

        protected TPropertyType _stored; //это то, что на диске
        
        private TPropertyType _defaultValue; //если какого-то значения нет, берём дефолт
      

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue; 
        }

        public override TPropertyType Value //по этому проперти будем забирать значение 
        {
            get => _stored; // возвращать 
            set //записывать
            {
                var isEquals = _stored.Equals(value);
                if(isEquals) return; //если равные проперти, то ничего не делаем

                var oldValue = _stored; //перед тем как поменять, запишем в переменную 
                Write(value); //иначе записываем значение
                _stored = _value = value;
                InvokeChangedEvent(value, oldValue);
            }
        }

        protected void Init()
        {
            _stored = _value = Read(_defaultValue);
        }
        
        protected abstract void Write(TPropertyType value); //здесь будем записывать значения
        protected abstract TPropertyType Read(TPropertyType defaultValue); // здесь читать значения
            //методы абстрактные, потому что мы хотим реализовать их в наследниках
        public void Validate()
        {
            if (!_stored.Equals(_value)) //если то, что у нас на диске не равно тому, что в инспекторе
                Value = _value; //то мы просто перезапишем
        }
    }
}