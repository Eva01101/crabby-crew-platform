using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterCollisionComponent: MonoBehaviour 
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action; // в этот объект мы можем передать метод из др комп и вызвать его

        private void OnCollisionEnter2D (Collision2D other)//коллизия - столкновение двух физич объектов
        {
            if (other.gameObject.CompareTag(_tag)) // мы сравним тэг объекта с которым пересеклись, если тэг совпадёт, мы вызовем action 
            {
                _action?.Invoke(other.gameObject); // action передаём объекту с которым мы заколадилист
                //проверяем на null 
                // компоненты - это то, что навешивается на gameobject
            }
        }
        
    }
    [Serializable]
    public class EnterEvent: UnityEvent<GameObject> //в этот ивент мы передаём геймобжект, с которым столкнулись 
    {
            
    }
}