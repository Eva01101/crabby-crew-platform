using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PixelCrew.Utils
{
    public class CheatController : MonoBehaviour
    {
        private string _currentInput; // будем сохранять в строку
        [SerializeField] private float _inputTimeToLive;
        [SerializeField] private CheatItem[] _cheats; // массив читов
        
        private float _inputTime; // переменная, которая будет сбрасывать текущий инпут 
        
        private void Awake() //нужно получить ввод с клавы
        { //текущий источник ввода
            Keyboard.current.onTextInput += OnTextInput;//подписка на событие 
        }

        private void OnDestroy()
        {
            Keyboard.current.onTextInput -= OnTextInput; //отписка на событие
        }

        private void OnTextInput(char inputChar)// каждый знак мы сохраняем в строку, каждый раз, когда будем вводить что-то с клавы, будем попадать в этот метод и обрабатывать 
        {
            _currentInput += inputChar; // эта строка будет жить какое-то время 
            _inputTime = _inputTimeToLive; // сбрасываем время до сброса этой строки, будет сбрасывать до заданного значения, каждый раз, когда будем тыкать на кнопку
            FindAnyCheats();
        }

        private void FindAnyCheats()
        {
            foreach (var cheatItem in _cheats) // проходимся по каждому элементу в читах 
            {
                if (_currentInput.Contains(cheatItem.Name)) //если _currentInput содержит name
                {
                    cheatItem.Action.Invoke(); //должны вызвать какой-то метод
                    _currentInput = String.Empty; //сброс строки 
                }
                
            }
            
        }

        private void Update()
        {
            if (_inputTime < 0) //если инпут тайм больше 0
            {
                _currentInput = String.Empty; // сбросим текущую строку
            }
            else
            {
                _inputTime -= Time.deltaTime; //каждый кадр мы будем отнимать от инпут тайм время с прошедшего кадра 
            }
        }
        [Serializable]
        public class CheatItem
        {
            public string Name; // содержит последовательность клавишь, которые нужно нажать 
            public UnityEvent Action; //здесь мы смотрим, что мы должны сделать в ответ 

        }
    }

}

