using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public class PointPatrol: Patrol
    {
        [SerializeField] private Transform[] _points; //массив точек
        [SerializeField] private float _treshold = 1f;
        
        private Creature _creature;
        private int _destinationPointIndex; //точка до которой мы должны дойти
        

        private void Awake()
        {
            _creature = GetComponent<Creature>(); //потому что нужно его двигать 
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled) //если компонент включён
            {
                if (IsOnPoint()) //если мы дошли до этой точки
                {
                    _destinationPointIndex =
                        (int) Mathf.Repeat(_destinationPointIndex + 1,
                            _points.Length); //то мы должны перейти на следующую точку
                }

                var direction = _points[_destinationPointIndex].position - transform.position; //моба направляем до точки
                direction.y = 0; 
                _creature.SetDirection(direction.normalized); //идём через сетдирекшен 

                yield return null;

            }
        }

        private bool IsOnPoint()
        {
            return (_points[_destinationPointIndex].position - transform.position).magnitude <
                   _treshold; //magnitude - длина вектора
            //сравнить позицию текущую и позицию до точки, найти длину и посмотреть больше или меньше она порогового значения
        }
        
    }
}