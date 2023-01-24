using System;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.Interactions
{
    public class RequireItemComponent: MonoBehaviour
    { //если у нас будет удовлетворять необходимое количество, то мы удалим из инвентаря

        [SerializeField] private InventoryItemData[] _required;
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSuccess; 
        [SerializeField] private UnityEvent _onFail; 

        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            var areAllRequirementsMet = true; //все рекваеры сходятся
            
            foreach (var item in _required)
            {
                var numItems = session.Data.Inventory.Count(item.Id); //будем проверять кажд предм
                if (numItems < item.Value)
                    areAllRequirementsMet = false; 
            }
            
            if (areAllRequirementsMet) //если количество предметов больше либо равно _count
            {
                if (_removeAfterUse) //удаляем
                {
                    foreach (var item in _required)
                        session.Data.Inventory.Remove(item.Id, item.Value);
                }
                
                _onSuccess?.Invoke();
            }
            else
            {
                _onFail?.Invoke();
            }
        }
    }
}