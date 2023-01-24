using System;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository
{
    [CreateAssetMenu(menuName = "Defs/Throwable", fileName = "Throwable")]
    
    public class ThrowableRepository : DefRepository<ThrowableDef> //описания предметов будут с постфиксом деф
        //ScriptableObject - Благодаря этому объекты появляются на платформе как ассет Defs/InventoryItems
    {
        /*[SerializeField] private ThrowableDef[] _items;
        
        private void OnEnable()
        {
            _collection = _items;
        }

        public ThrowableDef Get(string id)
        {
            foreach (var itemDef in _items)
            {
                if (itemDef.Id == id) //если найдётся такой предмет
                    return itemDef;
            }

            return default;
        }*/
    }

    [Serializable]
    public struct ThrowableDef: IHaveId
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private GameObject _projectile; //который будет спавниться

        public string Id => _id;

        public GameObject Projectile => _projectile;
    }
}