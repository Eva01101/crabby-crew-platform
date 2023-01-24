using System;
using System.Net.Mime;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposable;

namespace PixelCrew.Model.Models
{
    public class PerksModel: IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        public readonly Cooldown Cooldown = new Cooldown();
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public event Action OnChanged;
        
        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;
            
            _trash.Retain(_data.Perks.Used.Subscribe((x,y ) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x,y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call); //сразу отпишемся 
        }

        public string Used => _data.Perks.Used.Value;
        
        public bool IsSuperThrowSupported => _data.Perks.Used.Value == "super-throw" && Cooldown.IsReady; //определение возможности использов перков
        public bool IsDoubleJumpSupported => _data.Perks.Used.Value == "double-jump" && Cooldown.IsReady;
        public bool IsShieldSupported => _data.Perks.Used.Value == "shield" && Cooldown.IsReady;

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price); //проверяем, хватает ли нам ресурсов

            if (isEnoughResources) //если хватает
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count); //удаляем ресурс
                _data.Perks.AddPerk(id); //добавляем перк
                
                OnChanged?.Invoke();
            }
        }

        public void SelectPerk(string selected)
        {
            var perkDef = DefsFacade.I.Perks.Get(selected);
            Cooldown.Value = perkDef.Cooldown;//будем менять знач, если разн кулдав буду у перков, они буду меняться при изменении
            _data.Perks.Used.Value = selected;
        }

        public bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }

        public bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId); 

        }

        public bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }
        
        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}