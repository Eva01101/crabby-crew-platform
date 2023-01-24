using System;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data //здесь инвентарь, в который будем помещать объекты, изменяемые данные
{
    [Serializable] //данные будут видны внутри какого-то объекта 
    
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory; 
        
        public IntProperty Hp = new IntProperty();
        public FloatProperty Fuel = new FloatProperty();//топливо фонарика 
        public PerksData Perks = new PerksData();
        public LevelData Levels = new LevelData();

        public InventoryData Inventory => _inventory;
        

        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json); //еще вариант записи сессии 


            /* return new PlayerData()
            { альтернативный вариант создания клона
                Coins = Coins,
                Hp = Hp,
                IsArmed = IsArmed

            };*/
        }
    }
}