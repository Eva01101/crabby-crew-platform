using System;
using PixelCrew.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Creatures.Hero
{
    public class HeroInPutReader : MonoBehaviour
    {
        [SerializeField] private Heroes _heroes;
        
        
        public void OnMovement(InputAction.CallbackContext context) //если будет privet - не будет отображаться в проекте

        {
           var direction = context.ReadValue<Vector2>();
           _heroes.SetDirection(direction); 
        }
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _heroes.Interact(); 
            }
        }
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _heroes.Attack(); 
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started) //вызоветься тогда, когда действие уже совершится 
            {
                _heroes.StartThrowing(); //зажали кнопку
            }

            if (context.canceled) //значит мы отпустили кнопку
            {
                _heroes.UseInventory();
            }
        }

        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _heroes.NextItem(); 
            }
        }
        
       /* public void OnDropDown(InputAction.CallbackContext context)
        {
            if (context.performed)
                _heroes.DropDown();//
        }*/

        public void OnUsePerk(InputAction.CallbackContext context)
        {
            if(context.performed)
                _heroes.UsePerk();
        }

        public void OnToggleFlashlight(InputAction.CallbackContext context)
        {
            if(context.performed)
                _heroes.ToggleFlashlight();
        }
    }
}


