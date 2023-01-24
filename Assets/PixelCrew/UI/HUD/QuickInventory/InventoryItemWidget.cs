using System;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposable;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD.QuickInventory
{
    public class InventoryItemWidget: MonoBehaviour, IItemRenderer<InventoryItemData> //прокинуть всё, что мы будем использовать 
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection; //выбран или не выбран наш объект
        [SerializeField] private Text _value; //количество ресурсов
        
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private int _index;

        private void Start()
        {
            var session = FindObjectOfType<GameSession>();
            var index = session.QuickInventory.SelectedIndex;
            _trash.Retain(index.SubscribeAndInvoke(OnIndexChanged));
            //найти сессия, найти нужную нам модель и подписаться на изменения селектед индекс
        }

        private void OnIndexChanged(int newValue, int _)
        {
            _selection.SetActive(_index == newValue); //если текущий индекс равен ньювалью
            // в одну единицу времени у нас будет выбран только один элемент
        }

        public void SetData(InventoryItemData item, int index) //передаём элемент нашего инвентаря и текущ индекс
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id); //через фасад получим по айди дефенишн
            _icon.sprite = def.Icon; //заберём 
            _value.text = def.HasTag(ItemTag.Stackable) ? item.Value.ToString() : string.Empty;
            
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}