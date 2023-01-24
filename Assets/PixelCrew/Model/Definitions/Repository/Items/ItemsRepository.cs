using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository.Items
{
    [CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")] //описание того, какие предметы у нас есть 
    //не изменяемые данные

    public class ItemsRepository : DefRepository<ItemDef> //описания предметов будут с постфиксом деф
        //ScriptableObject - Благодаря этому объекты появляются на платформе как ассет Defs/InventoryItems
    {
        /*[SerializeField] private ItemDef[] _items;

        private void OnEnable()
        {
            _collection = _items;
        }

        public ItemDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id) //если найдётся такой предмет
                        return itemDef;
            }
            return default;
        }*/
#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _collection;
#endif
    }

    [Serializable]
    public struct ItemDef: IHaveId //описание, меняться не должно, объявим как структуру 
    { //в структурах нет нула
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags; //добавляем список из тэгов
        
        public string Id => _id;
        public bool IsVoid => string.IsNullOrEmpty(_id); //если у нас айди нул или пустое, значит будет пустой айтемдеф
        public Sprite Icon => _icon;

        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false; //посмотрим, есть ли такой тэг
            //? - проверка на null 
        }
    }
}