using System;
using System.Collections.Generic;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposable;
using UnityEngine;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class QuickInventoryController: MonoBehaviour //занимается менеджментом инвенторя и созданием элементов в инвенторе
    {
        [SerializeField] private Transform _container; //трансформ в кот мы будем размножать наши элем
        [SerializeField] private InventoryItemWidget _prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable(); //подписки на изменения инвентаря
        
        private GameSession _session;
        private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();
        //новые элементы будем складывать в отдельный список

        private DataGroup<InventoryItemData, InventoryItemWidget> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidget>(_prefab, _container);
            _session = FindObjectOfType<GameSession>(); //находим сессию
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild)); //находим модель и подписываемся на её изменения
            //если что бы то ни было поменяется, мы пересоберём на UI
            
            //subscribe model

            Rebuild();
        }

        private void Rebuild()//в тот момент, когда модель изменилась, перестроить весь наш инвентарь
        {
            var inventory = _session.QuickInventory.Inventory; //напрямую обратиться к нашей модели
            //создать недостающие элементы
            _dataGroup.SetData(inventory);
            
            
            
            /*for (var i = _createdItem.Count; i < inventory.Length; i++) //от количества созданных элементов до _inventory.Length будем создавать
            {
                //если у нас будет уже создано два элемента, а в списке 3 элемента, создадим один элем, а остальное возьмём из кэша
                var item = Instantiate(_prefab, _container); //создадим через Instantiate
                //_container - то место, где будем создавать
                _createdItem.Add(item);
            }
            
            //обновить данные в созданных элементах
            for (var i = 0; i < inventory.Length; i++) //проходимся по списку
            {
                _createdItem[i].SetData(inventory[i], i); //будем проходиться по _createdItem и вызывать метод SetData
                //и отправлять соответ запись из нашего инвенторя
                _createdItem[i].gameObject.SetActive(true); //активировать элемент
                
            }
            
            //прятание ненужных элементов, неизпользуемых в данный момент
            for (var i = inventory.Length; i < _createdItem.Count; i++) //всё что за пределами инвенторя, уже неиспользуем мы должны их спрятать
            {
                _createdItem[i].gameObject.SetActive(false);
            }*/
            
            
            
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}