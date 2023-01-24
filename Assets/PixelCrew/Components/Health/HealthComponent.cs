using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Health 
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health; // значение здоровья
        [SerializeField] public UnityEvent _onDamage; 
        [SerializeField] private UnityEvent _onHeal; //исцеление
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] public HealthChangeEvent _onChange; //изменение сстояния героя
        
        private Lock _immune = new Lock();

        public int Health => _health;

        public Lock Immune => _immune; //вернём

        public void ModifyHealth (int healthDelta)
        {
            if(healthDelta < 0 && Immune.IsLocked) return; //если мы получаем урок и мы сейчас имунны
            
            if (_health <= 0) return; //ничего не будем делать, если...
            
            _health += healthDelta; // прибавим к текщему здоровью дельту (если дельта у предмета больше нуля - лечит, если меньше - наносит урон
            _onChange?.Invoke(_health); //после того, как поменяли здоровье, вызываем этот метод

            if (healthDelta < 0) // установили у пик значение -1
            {
                _onDamage?.Invoke(); // ?. такой метод проверяет на null
            }

            if (healthDelta > 0)
            {
                _onHeal?.Invoke();
            }

            if (_health <= 0) // если текущее здоровье меньше либо равно нулю
            {
                _onDie?.Invoke(); //вызываем метод _onDie

            }
        }
#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
        
#endif

        
        public void SetHealth(int health) //проинициализируем компонент данными из героя
        {
            _health = health; 
        }

        private void OnDestroy()
        {
            _onDie.RemoveAllListeners(); //очистим память 
        }

        //наследник от юнитиивента, чтобы передать текущее состояние здоровья
        [Serializable]
        public class HealthChangeEvent: UnityEvent<int>
        {
            
        }
    }
}


