using System;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class SeashellTrapAI: MonoBehaviour
    {
        [SerializeField] private ColliderCheck _vision;
        
        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCoolDown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private ColliderCheck _meleeCanAttack;
        
        [Header("Range")]
        [SerializeField] private Cooldown _rangeCoolDown;
        [SerializeField] private SpawnComponent _rangeAttack;
        
        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");
        
        
        private Animator _animator;

        private void Awake()
        { 
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer) //если у нас в вижене что-то есть
            {
                if (_meleeCanAttack.IsTouchingLayer) // и если мы можем атаковать
                {
                    if (_meleeCoolDown.IsReady)
                        _MeleeAttack();//вызоветь функцию
                    return; //ранний выход из функции, если не переходим в рэнж атаку
                }
                
                if(_rangeCoolDown.IsReady)// а если мы уже кого-то видим и можем атаковать
                    RangeAttack();
            }
        }

        private void RangeAttack()
        {
            _rangeCoolDown.Reset();
            _animator.SetTrigger(Range);
        }

        private void _MeleeAttack()
        {
            _meleeCoolDown.Reset();
            _animator.SetTrigger(Melee);
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
            

    }
}