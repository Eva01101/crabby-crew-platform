using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class GoContainerComponent: MonoBehaviour
    {
        [SerializeField] private GameObject[] _gos; //здесь объекты, кот будут выстреливать
        [SerializeField] private DropEvent _onDrop;

        [ContextMenu("Drop")] 
        public void Drop()
        {
            _onDrop.Invoke(_gos);
        }
    }
}