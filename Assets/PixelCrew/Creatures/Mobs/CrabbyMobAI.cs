using System;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class CrabbyMobAI : MonoBehaviour
    {
        [SerializeField] private ColliderCheck _visionCheck;
        
        [Header("Closely")]
        [SerializeField] private Cooldown _closeCooldown;
        [SerializeField] private CheckCircleOverlap _closeAttack;
        [SerializeField] private ColliderCheck _closeCanAttack;

        [Header("Attack")] 
        [SerializeField] private Cooldown _rangedCooldown;
        [SerializeField] private SpawnComponent _rangedAttack;
        
        private static readonly int CloseKey = Animator.StringToHash("closely");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        private static readonly int DeadKey = Animator.StringToHash("dead");

        private Animator _animator;
        //private Creature _creature;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            if (_visionCheck.IsTouchingLayer) //если у нас в вижене что-то есть
            {
                if (_closeCanAttack.IsTouchingLayer) //если можем атаковать
                {
                    if (_closeCooldown.IsReady)
                        MobCloseAttack();
                    return;
                }

                if (_rangedCooldown.IsReady)
                    RangedAttack();
            }
        }

        private void RangedAttack()
        {
            _rangedCooldown.Reset();
            _animator.SetTrigger(AttackKey);
        }

        private void MobCloseAttack()
        {
            _closeCooldown.Reset();
            _animator.SetTrigger(CloseKey);
        }

        public void OnMobCloseAttack()
        {
            _closeAttack.Check();
        }

        public void OnMobRangedAttack()
        {
            _rangedAttack.Spawn();
        }

        public void OnDie()
        {
            _animator.SetBool(DeadKey, true);

            //_creature.SetDirection(Vector2.zero); //чтобы герой не двигался после смерти
        }
    }
}