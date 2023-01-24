using System;
using UnityEngine;

namespace PixelCrew.Components.Interactions
{
    public class SwitchComponent: MonoBehaviour
    { //компронент для двери
        
        [SerializeField] private Animator _animator; //сам аниматор
        [SerializeField] private bool _state; //состояние объекта, включён или выключен
        [SerializeField]private string _animationKey; //проперти, который будем вызывать
        [SerializeField] private bool _updateOnStart;

        private void Start()
        {
            if(_updateOnStart)
                _animator.SetBool(_animationKey, _state);
        }

        public void Switch() //метод, который будем у него вызывать 
        {
            _state = !_state; //будем менять состояние 
            _animator.SetBool(_animationKey, _state); //пихать в аниматор соответствующий бул 
            
        }

        [ContextMenu("Switch")] //отладочный метод, теперь он есть на платформе в SwitchComponent, где три точки
        public void SwitchIt()
        {
            Switch();
        }
        
        
    }
}