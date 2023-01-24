using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components.GoBased 
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private RestoreStateComponent _state;
        
        public void DestroyObject()
        {
            Destroy(_objectToDestroy); // удаление объекта 

            if (_state != null)
                FindObjectOfType<GameSession>().StoreState(_state.Id);
        }
    }
}
