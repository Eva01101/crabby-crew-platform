using System;
using UnityEngine;

namespace PixelCrew.Creatures.Weapons
{
    public class Projectile: BaseProjectile
    {
        protected override void Start()
        {
            base.Start();
            
            var force = new Vector2(Direction * _speed, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse);  // втыкать в землю меч, при таком способе
            //нужно поменять с кинематик на динамик и увеличить скорость броска
        }

       /*private void FixedUpdate()
        {
            var position = Rigidbody.position; //получим текущую позицию
            position.x += Direction * _speed; 
            Rigidbody.MovePosition(position); //новая позиция
        } 
        */
    }
}