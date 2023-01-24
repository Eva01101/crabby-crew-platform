﻿using System.Collections;
using PixelCrew.Components.ColliderBased;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Patrolling
{
    public class PlatformPatrol: Patrol
    {
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerCheck _obstacleCheck;
        [SerializeField] private Creature _creature;
        [SerializeField] private int _direction;
        
        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (_groundCheck.IsTouchingLayer && !_obstacleCheck.IsTouchingLayer) //если мы касаемся земли и не кас обс
                {
                    _creature.SetDirection(new Vector2(_direction, 0));
                }
                else
                {
                    _direction = -_direction;
                    _creature.SetDirection(new Vector2(_direction, 0));
                }
                yield return null;
            }
        }
    }
}