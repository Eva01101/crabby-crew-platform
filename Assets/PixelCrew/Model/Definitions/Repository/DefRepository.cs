using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository
{
    public class DefRepository<TDefType> : ScriptableObject where TDefType: IHaveId
    {
        [SerializeField] protected TDefType[] _collection; //поле для сохр данных
        
        public TDefType Get(string id) //возвращ конкретный дефиниш

        {
            if (string.IsNullOrEmpty(id)) //если айди пустой
                return default;//не будем дальше искать
            
            foreach (var itemDef in _collection) //искать будем по коллекциям
            {
                if (itemDef.Id == id) //если найдётся такой предмет
                    return itemDef;
            }

            return default;
        }

        public TDefType[] All => new List<TDefType>(_collection).ToArray();
    }
}