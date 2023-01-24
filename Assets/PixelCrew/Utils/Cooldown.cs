using System;
using UnityEngine;

namespace PixelCrew.Utils
{
    [Serializable] //чтобы могли его настраивать из редактора 

    public class Cooldown
    {
        [SerializeField] private float _value; //сколько секунд мы должны будем ждать

        private float _timesUp;

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public void Reset()
        {
            _timesUp = Time.time + _value; //Time.time - время прошедшее с начала нашей игры 
            //складываем и получаем будущее время, проверяем, наступило ли наше событие в будущем

        }

        public float RemainingTime => Mathf.Max(_timesUp - Time.time, 0);//считает, сколько осталось времени
        //до тикания текущего кулдавн

        public bool IsReady => _timesUp <= Time.time; //время, когда этот кулдавн уже выставлен, прощло 

    }
    
}