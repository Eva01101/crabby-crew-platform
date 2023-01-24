using System;
using UnityEngine;

namespace PixelCrew.Effects
{
    public class ParallaxEffect: MonoBehaviour
    {
        [SerializeField] private float _effectValue;
        [SerializeField] private Transform _followTarget; //объект, за которым бдем следить

        private float _startX;
        
        private void Start()
        {
            _startX = transform.position.x;//на старте сохраняем дефолтную позицию
        }

        private void LateUpdate()//здесь будем двигать облака
        {
            var currentPosition = transform.position; //заберём текущ позицию
            var deltaX = _followTarget.position.x * _effectValue; //посмотрим, насколько нам нужно сдвинуться
            transform.position = new Vector3(_startX + deltaX, currentPosition.y, currentPosition.z); //задаём новую позицию
        }
    }
}