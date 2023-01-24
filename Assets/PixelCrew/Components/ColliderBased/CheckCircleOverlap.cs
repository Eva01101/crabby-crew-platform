using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components.ColliderBased 
{
    public class CheckCircleOverlap: MonoBehaviour
    {
        [SerializeField] private float _radius = 1f; //радиус нашего круга
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags; 
        [SerializeField] private OnOverlapEvent _onOverlap;
        
        private readonly Collider2D[] _interactionResult = new Collider2D[10];

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TransparentRed;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius); //теперь мы видим радиус
        }
#endif
        public void Check()
        {
            // создали круг с определён радиусом и чекаем массив объектов, которые вошли в этот радиус
            var size = Physics2D.OverlapCircleNonAlloc(transform.position,
                _radius,
                _interactionResult,
                _mask);

            for (var i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i]; //если у нас элемент с которым пересеклись будет иметь тэг один из перечиленных
                var IsInTags =
                    _tags.Any(tag => overlapResult.CompareTag(tag)); //any возвращает тру если хотя бы один элемент  тоже тру
                if (IsInTags)
                {
                    _onOverlap?.Invoke(overlapResult.gameObject); //то профильтруем результаты
                }
            }
        }

        [Serializable]
        public class OnOverlapEvent: UnityEvent<GameObject>
        {
            
        }
    }
}