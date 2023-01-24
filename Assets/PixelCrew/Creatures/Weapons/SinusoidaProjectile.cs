using System;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class SinusoidaProjectile: BaseProjectile
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f; 
        private float _originalY;
        private float _time;
        
        protected override void Start()
        {
            base.Start();
            _originalY = Rigidbody.position.y; //сохраним на старте 
        }

        private void FixedUpdate() //изменять координату в каждом кадре 
        {
            var position = Rigidbody.position;
            position.x += Direction * _speed;
            position.y = _originalY + Mathf.Sin(_time * _frequency) * _amplitude; 
            //теперь мы стартуем с нуля и прожектайл вылетает из одного места, Mathf.Sin(_time * _frequency) * _amplitude;
            //формула, чтобы движение шло по амплитуде 
            Rigidbody.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }
    }
}