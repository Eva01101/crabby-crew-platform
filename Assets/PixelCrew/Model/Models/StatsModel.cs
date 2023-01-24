using System;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils.Disposable;

namespace PixelCrew.Model.Models
{
    public class StatsModel: IDisposable
    {
        private readonly PlayerData _data;
        public event Action OnChanged;
        public event Action<StatId> OnUpgraded; 

        public readonly ObservableProperty<StatId> InterfaceSelectedStat = new ObservableProperty<StatId>();

        private readonly CompositeDisposable _trash = new CompositeDisposable(); 

        public StatsModel(PlayerData data)
        {
            _data = data;
            _trash.Retain(InterfaceSelectedStat.Subscribe((x, y) => OnChanged?.Invoke()));

        }
        
        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call); //сразу отпишемся 
        }

        public void LevelUp(StatId id) //проверить цену
        {
            var def = DefsFacade.I.Player.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;

            if (def.Levels.Length <= nextLevel) return;

            var price = def.Levels[nextLevel].Price; //возьмём цену покупки уровня
            if (!_data.Inventory.IsEnough(price)) return; //если у нас не хватает чего-то, прерываем функцию
            
            _data.Inventory.Remove(price.ItemId, price.Count); //если хватает, тратим всё, что есть
            _data.Levels.LevelUp(id);

            OnChanged?.Invoke();//сообщить о том, что изменилось что-то
            OnUpgraded?.Invoke(id);
        }

        public float GetValue(StatId id, int level = -1)//получать ото всюду текущее знач
        {
            return GetLevelDef(id, level).Value;//вынули его из списка всех уровней
        }

        public StatLevelDef GetLevelDef(StatId id, int  level = -1)
        {
            if (level == -1) level = GetCurrentLevel(id);
            var def = DefsFacade.I.Player.GetStat(id); //по айди нашли описание, распределение всех уровней
            
            if(def.Levels.Length > level)
                return def.Levels[level]; //получим текущий уровень
           
            return default;
        }
        
        public int GetCurrentLevel(StatId id) => _data.Levels.GetLevel(id); 
        
        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}