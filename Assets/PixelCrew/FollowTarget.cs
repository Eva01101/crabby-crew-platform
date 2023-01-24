using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _damping; // скорость

        private void LateUpdate()
        {
            var destination = new Vector3(_target.position.x, _target.position.y, transform.position.z); //высчитываем желаемую позицию
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * _damping); /*принимаем позицию текущего вектора, вектора куда мы хотим прийти
            третья позиция - интерполяция, которая позволяем плавно перемещать камеру 0 - текущая позиц, 1 - желаемая позиц и мы перемещаем от 0 до 1
            двигаем от текущ поз к желаем через функцию интерполяции (Lerp), умножаем на скорость передвижения (_damping), в зависимости от времени (Time) котор прошло с последнего кадра,
            мы используем DeltaTime, чтобы сгладить скорость перемещения */
            // если нам нужно поменять параметр натуральночисленный (float) мы можем использ пакет Mathf.Lerp
        }
    }

    
}

