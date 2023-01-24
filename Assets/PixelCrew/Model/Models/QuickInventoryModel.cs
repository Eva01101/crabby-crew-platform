using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.Utils.Disposable;
using UnityEngine;

namespace PixelCrew.Model.Models
{
    public class QuickInventoryModel: IDisposable //класс в котором хранится и обрабатывается наш инвентарь
    //в quickinventory у нас могут попадать предметы только с тэгом usable
    {
        private readonly PlayerData _data;
        
        public  InventoryItemData[] Inventory { get; private set; } //получим инвентарь 
       
        public readonly IntProperty SelectedIndex = new IntProperty();

        public event Action OnChanged; //наши внешние контроллеры, кот подписаны на конкретную модель, должны знать, что что-то изменилось

        public InventoryItemData SelectedItem
        {
            get
            {
                if (Inventory.Length > 0 && Inventory.Length > SelectedIndex.Value) //сделаем, чтобы индекс попадал в границы нашего массива
                    return Inventory[SelectedIndex.Value];
                
                return null;
            }
        }

        public ItemDef SelectedDef => DefsFacade.I.Items.Get(SelectedItem?.Id);
        
        public QuickInventoryModel(PlayerData data) //можем обрабатывать данные инвенторя через эту модель
        {
            _data = data;
            
            Inventory = _data.Inventory.GetAll( ItemTag.Usable); //будем забирать только юзабл
            _data.Inventory.OnChanged += OnChangedInventory; //подписаться на изменения инвенторя
        }

        public IDisposable Subscribe(Action call) //для того, чтобы удобней было подписываться на нашу модель
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        private void OnChangedInventory(string id, int value)//если в инвенторе будет что-то меняться, нужно пересобирать инвентарь быстрого доступа
        {
            //var indexFound = Array.FindIndex(Inventory, x => x.Id == id); //в массиве инвентори есть предмет с таким айди
            //if (indexFound != -1)
            {
                //мы будем пересобирать индекс и выбранный сейчас элемент
                //если у нас что-то меняется, мы обновляем весь массив с данными
                //если у нас выбранный предмет выходит за рамки этого инвентаря, то мы ограничиваем его индекс возможными рамками
                //от 0 до границы инвенторя
                
                Inventory = _data.Inventory.GetAll(ItemTag.Usable);
                SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
                
                //здесь находится только наш инвентарь (из quickinventory), не общий
                OnChanged?.Invoke();
            }
            
            
        }

        public void SetNextItem()//выбрать следующий инвентарь
        {
            SelectedIndex.Value = (int) Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
        }


        public void Dispose()
        {
            _data.Inventory.OnChanged -= OnChangedInventory; 
        }
    }
}