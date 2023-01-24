using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]

    public class InventoryData //внутри будет список всех предметов, Дата - это изменяемые данные
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value); //объект, который описывает метод, value- общее количество

        public OnInventoryChanged OnChanged; //переменная с типом делегата, это наш ивент

        public void Add(string id, int value)
        {
            if (value <= 0) return; //если...можем ничего не добавлять

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return; //если не существует предмета, который мы запрашиваем, то выходим
            
            if (itemDef.HasTag(ItemTag.Stackable))
            {
               AddToStack(id, value);
            }
            else
            { 
                AddNonStack(id, value);
            }
            OnChanged?.Invoke(id, Count(id)); //передаём айди и считаем значения
            
        }

        public InventoryItemData[] GetAll(params ItemTag[] tags) //получим весь инвентарь
            //благодаря params наш метод может принимать не один тэг, а несколько и передавать в массив
        {
            var retValue = new List<InventoryItemData>();
            foreach (var item in _inventory)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id); //для того, чтобы получить список тэгов нам нужно получить дефинишн
                    //то есть описание нашего предмета
                var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));
                    //проверим, что все условия по тэгам у нас выполняются
                    //получается, что каждая переменная в массиве тэгс есть в нашем itemDef
                if(isAllRequirementsMet) //если это так
                    retValue.Add(item); //то мы добавляем наш предмет
            }

            return retValue.ToArray();
        }

        private void AddToStack(string id, int value)
        {
            var isFull = _inventory.Count >= DefsFacade.I.Player.InventorySize;
            var item = GetItem(id);
            if (item == null) // создадим новый и добавим в инвентарь
            {
                if (isFull) return; //если инвентарь полный, то мы просто выйдем
                    
                item = new InventoryItemData(id);
                _inventory.Add(item);
            }
            item.Value += value; //потом добавим количество этих предметов
        }

        private void AddNonStack(string id, int value)
        {
            var itemLasts = DefsFacade.I.Player.InventorySize - _inventory.Count; //сколько у нас осталось элементов
            value = Mathf.Min(itemLasts, value);
            
            for (var i = 0; i < value; i++)
            {
                var item = new InventoryItemData(id) {Value = 1}; //собрать новый предмет с кол 1
                _inventory.Add(item); //и добавить его
            }
        }
        public void Remove(string id, int value) //можем удалять инвентарь 
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return; //если не существует предмета, который мы запрашиваем, то выходим

            if (itemDef.HasTag(ItemTag.Stackable))
            {
                RemoveFromStack(id, value);
            }
            else
            {
                RemoveNonStack(id, value);
            }
            OnChanged?.Invoke(id, Count(id));
        }

        private void RemoveFromStack (string id, int value)
        {
            var item = GetItem(id); //получаем предмет
            if (item == null) return; //если итем нет, то просто выходим
                
            item.Value -= value; //иначе у итема отнимаем количество
            
            if (item.Value <= 0) // если количество...
                _inventory.Remove(item); //удалить 
        }

        private void RemoveNonStack(string id, int value)
        {
            for (int i = 0; i < value; i++)
            {
                var item = GetItem(id); //получаем предмет
                if (item == null) return; //если итем нет, то просто выходим
                    
                _inventory.Remove(item);
            }
        }

        private InventoryItemData GetItem(string id) //будем смотреть, есть ли такой элемент у нас в инвентаре
        {
            foreach (var itemData in _inventory) //пройдёмся по всему инвентарю 
            {
                if (itemData.Id == id) //если найдём предмет с таким же айди, то вернём его
                    return itemData;
            }

            return null; //иначе
        }

        public int Count(string id)
        {
            var count = 0;

            foreach (var item in _inventory)
            {
                if (item.Id == id) //если item.Id будет равно той айди, которую мы передали 
                    count += item.Value; //прибавим количество элементов в инвентаре
            }

            return count;
        }

        public bool IsEnough(params ItemWithCount[] items)
        {
            var joined = new Dictionary<string, int>();
            
            foreach (var item in items)
            {
                if (joined.ContainsKey(item.ItemId))
                    joined[item.ItemId] += item.Count; //если есть просто прибавляем
                else
                    joined.Add(item.ItemId, item.Count);//иначе создаём
            }

            foreach (var kvp in joined)
            {
                var count = Count(kvp.Key);
                if (count < kvp.Value) return false; //если в одном из элементов нам не хватает необходимого
                //вернём false
            }

            return true;
        }
    }

    [Serializable]

        public class InventoryItemData
        {
           [InventoryId] public string Id; //идентификатор
            public int Value; //количество

            public InventoryItemData(string id)
            {
                Id = id;

            }

        }
}