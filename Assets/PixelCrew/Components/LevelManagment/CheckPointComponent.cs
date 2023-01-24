using System;
using PixelCrew.Components.GoBased;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.LevelManagment
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckPointComponent: MonoBehaviour
    {
        [SerializeField] private string _id; //идентификатор нашего чекпоита
        [SerializeField] private SpawnComponent _heroSpawner;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;
        
        public string Id => _id;
        private GameSession _session;
       
        private void Start() //нужно получить сессию, на старте она уже будет готова
        {
            _session = FindObjectOfType<GameSession>();
            if (_session.IsChecked(_id))
                _setChecked?.Invoke();
            else
                _setUnchecked?.Invoke();
        }

        public void Check()//добавляем чекпоинт в список
        {
            _session.SetChecked(_id);
            _setChecked?.Invoke();//не только сессии сказать, что мы зачекинились 
        }

        public void SpawnHero()
        {
            _heroSpawner.Spawn();
        }
    }
}