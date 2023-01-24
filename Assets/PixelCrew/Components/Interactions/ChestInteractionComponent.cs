using System.Collections.Generic;
using PixelCrew.Model.Definitions.Repository.Items;
using UnityEngine;
using UnityEngine.EventSystems;


namespace PixelCrew.Components.Interactions
{
    public class ChestInteractionComponent: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private List<ItemUsableComponent> _itemUsable;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Instantiate(_itemUsable[Random.Range(0, _itemUsable.Count)], transform.position, Quaternion.identity);
        }
    }
}