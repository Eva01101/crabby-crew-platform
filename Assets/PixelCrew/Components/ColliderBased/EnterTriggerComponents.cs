using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased
{
    public class EnterTriggerComponents : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action; // в этот объект мы можем передать метод из др комп и вызвать его

        private void OnTriggerEnter2D(Collider2D other)//объект войдёт в этот триггер
        {
            if(!other.gameObject.IsInLayer(_layer)) return; //если объект с кот заколайдились не в этом слое, мы прерываем
            
            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag)) return; // если у нас есть тэг и если тэг не сопадает, прерываем
            // мы сравним тэг объекта с которым пересеклись, если тэг совпадёт, мы вызовем action 
            
            _action?.Invoke(other.gameObject); //проверяем на null 
                            // компоннты - это то, что навешивается на gameobject, протягиваем из коллизии EnterEvent

        }
    }
}
