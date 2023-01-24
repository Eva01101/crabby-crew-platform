using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.Health
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _hpDelta; //значение hp, которое будет принимать

        public void SetDelta(int delta)
        {
            _hpDelta = delta;
        }
        
        //вызовем публичный метод, который этот дамаг нанесёт
        public void Apply(GameObject target) 
        {
            var healthComponent = target.GetComponent<HealthComponent>(); // получаем компонент, который создали
            if (healthComponent != null) // мы находим компонет здоровья у героя (компонент) мы добавили
            {
                healthComponent.ModifyHealth(_hpDelta); //нашли компонент, применили hp 
            }
        }
    }

}
