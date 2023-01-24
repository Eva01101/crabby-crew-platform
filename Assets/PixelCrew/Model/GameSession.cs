using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PixelCrew.Components.LevelManagment;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Models;
using PixelCrew.Utils.Disposable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession: MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;
        
       // public static GameSession Instance { get; private set; }
        
        public PlayerData Data => _data; //доступ к полю извне 
        private PlayerData _save;
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        public QuickInventoryModel QuickInventory { get; private set; } //аксессор с публичным доступом
        //но с приватной модификацией для того, чтобы из вне мы не могли её изменить
        //public 
        
        public PerksModel PerksModel { get; private set; }
        
        public StatsModel StatsModel { get; private set; }
        
        private readonly List<string> _checkpoints = new List<string>();

        private void Awake()
        {
            var existsSession = GetExistsSession();
            if (existsSession != null) //если у нас существует сессия, значит она появилась второй и её нужно уничтожить 
            {
                existsSession.StartSession(_defaultCheckPoint);
                Destroy(gameObject); 
            }
            else //если у нас сессии нет, то она первая и её нужно сохранить междусценами
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this); //передаём наш объект 
                //Instance = this; //всегда будем иметь ссылку на нашу сессию
                StartSession(_defaultCheckPoint);
            }
        }

        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);
            
            LoadUIs();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints =  FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkpoints.Last();
            foreach (var checkPoint in checkpoints)
            {
                if (checkPoint.Id == lastCheckPoint)//посмотреть, совпадает ли айди с последним чек поинтом
                {
                    checkPoint.SpawnHero();//когда мы находим последний чек поинт - спавним героя
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);
 
            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);

            StatsModel = new StatsModel(_data); 
            _trash.Retain(StatsModel);

            _data.Hp.Value = (int) StatsModel.GetValue(StatId.Hp);
        }

        private void LoadUIs()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
            //LoadOnScreenControls(); 
        }
        
         //[Conditional("USE_ON_SCREEN_CONTROLS")]
         //private void LoadOnScreenControls()
         //{
             
         //} 

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>(); //ищем объект сессии в сцене
            
            foreach (var gameSession in sessions) //пройтись по всем объектам
            {
                if (gameSession != this) //не равно текущему объекту, если у нас есть сессия в сцене, кот не равна не текущ сессии
                    return gameSession; //вернём тру - другие сессии существуют у нас
            }

            return null; 
        }

        public void Save()
        {
            _save = _data.Clone(); //когда мы сохраняемся, записать
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            
            _trash.Dispose();//очистимся от всего
            InitModels(); //затем инит, чтобы все модельки пересоздались
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id); //смотрим, есть ли у нас такой чекпоинт 
        }
        
        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id)) //если у нас нет такой айди, добавим
            {
                Save();
                _checkpoints.Add(id);
            }
        }

        private void OnDestroy()
        {
            //if (Instance == this)
               // Instance = null;
               //поменять код можно теперь; session = GameSession.Instance
            _trash.Dispose();
        }

        private readonly List<string> _removedItems = new List<string>();

        public bool RestoreState(string itemId)
        {
            return _removedItems.Contains(itemId);
        }

        public void StoreState(string itemId)
        {
            if(!_removedItems.Contains(itemId)) //если у нас нет такого айди, добавим
                _removedItems.Add(itemId);
        }
    }
}