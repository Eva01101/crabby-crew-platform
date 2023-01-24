using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;

namespace PixelCrew.Model.Definitions //объект, который хранит в себе описания предметов и кот мы можем запрашивать 
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")] //не изменяемые данные
    //нужен для того, чтобы мы могли из любого места в приложении достучаться до этого описания, списка итемов
    
    public class DefsFacade: ScriptableObject //проверить, есть ли предметы, получить на них ссылки
    {
        [SerializeField] private ItemsRepository _items;
        [SerializeField] private ThrowableRepository _throwableItems;
        [SerializeField] private PotionRepository _potions;
        [SerializeField] private PerkRepository _perks;
        [SerializeField] private PlayerDef _player;
        

        public ItemsRepository Items => _items;
        
        public ThrowableRepository Throwable => _throwableItems;
        
        public PotionRepository Potions => _potions;

        public PerkRepository Perks => _perks;
        
        public PlayerDef Player => _player;
            
            
        //далее делаем статические поля, чтобы мы могли по полям обращаться из проекта 
        private static DefsFacade _instance; //делаем статические поля, что мы могли обращаться по полям
        //создадим проперти и будем забирать
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance; //делаем проверку
        //если инст = 0, загружаем, иначе возвращаем инст

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade"); //Resources.Load - спец директория
            //загрузка
        }
        
    }
}