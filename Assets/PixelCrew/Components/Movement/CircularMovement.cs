using System;
using UnityEngine;

namespace PixelCrew.Components.Movement
{
    public class CircularMovement: MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed = 1f;
        private Rigidbody2D[] _bodies;
        private Vector2[] _positions;
        private float _time;

        private void Awake()
        {
            UpdateContent(); //на старте будем собирать все монетки, кот есть внутри
        }

        private void UpdateContent()
        {
            _bodies = GetComponentsInChildren<Rigidbody2D>(); //искать все риджитбоди
            _positions = new Vector2[_bodies.Length]; //массив с позициями 
        }

        private void Update()
        {
            CalculatePositions();
            var isAllDead = true;
            for (var i = 0; i < _bodies.Length; i++)
            {
                if (_bodies[i]) //если тело ещё живое, то будем двигать
                { 
                    _bodies[i].MovePosition(_positions[i]);
                    isAllDead = false;
                }
                
            }

            if (isAllDead)
            {
                enabled = false;
                Destroy(gameObject, 1f);
            }
            
            _time += Time.deltaTime;
        }

        private void CalculatePositions()
        {
            var step = 2 * Mathf.PI / _bodies.Length; //2PI - это полный круг
            //чтобы расставить предметы по кругу, мы сам круг разделим на колич предметов в нём
            Vector2 containerPosition = transform.position;
            
            for (var i = 0; i < _bodies.Length; i++)
            {
                var angle = step * i; //получим угол предметов
                var pos = new Vector2(
                    Mathf.Cos(angle + _time * _speed) * _radius,
                    Mathf.Sin(angle+ _time * _speed) * _radius); //нашли на окр нашего радиуса коорд объекта
                
                _positions[i] = containerPosition + pos;
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateContent();
            CalculatePositions();
            for (var i = 0; i < _bodies.Length; i++)
            {
                _bodies[i].transform.position = _positions[i];
            }
        }

        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);
        }
        
#endif

    }
}