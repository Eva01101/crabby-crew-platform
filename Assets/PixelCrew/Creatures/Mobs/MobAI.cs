using System;
using System.Collections;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Creatures.Mobs.Patrolling;
//using PixelCrew.Creatures.Patrolling; 
using UnityEngine;

namespace PixelCrew.Creatures.Mobs 
{
    public class MobAI: MonoBehaviour
    {
        [SerializeField] private ColliderCheck _vision;
        [SerializeField] private ColliderCheck _canAttack;

        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _attackCoolDown = 1f;
        [SerializeField] private float _missHeroCoolDown = 0.5f;

        [SerializeField] private float _horizontalTreshold = 0.2f;
        
        private IEnumerator _current; //текущая запущенная корутина
        private GameObject _target;
        
        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");
        
        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private bool _isDead;
        private Patrol _patrol;


        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>(); 
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }

        private void Start()
        {
            StartState(_patrol.DoPatrol()); //вызовем на старте патроллинг 
        }

        public void OnHeroInVision(GameObject go) //мы увидели героя
        {
            if(_isDead) return; //если мёртв, выйдем и не запустим ничего
            var cast = Physics2D.LinecastAll(transform.position, _target.transform.position);
            
            _target = go; //мы должны как-то пойти к герою 
            
            StartState(AgroToHero()); //с агримся на героя
        }

        private IEnumerator AgroToHero()
        {
            LookAtHero();
                
            _particles.Spawn("Exclamation");
            
            yield return new WaitForSeconds(_alarmDelay); //подождём несколько секунд
            
            StartState(GoToHero()); //начинаем идти к герою
        }

        private void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero); //сначала остановить, потом обновить дирекшен
            _creature.UpdateSpriteDirection(direction);
        }

        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer) //если у нас в вижене герой
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    var horizontalDelta = Mathf.Abs(_target.transform.position.x - transform.position.x);
                    if(horizontalDelta <= _horizontalTreshold)
                        _creature.SetDirection(Vector2.zero);
                    else
                        SetDirectionToTarget(); //будем двигаться к герою
                }
                
                yield return null;// возвращаем нал, чтобы пропустить один кадр
            }
            
            _creature.SetDirection(Vector2.zero);//когда мы потеряли героя, остановиться
            _particles.Spawn("MissHero");
            yield return new WaitForSeconds(_missHeroCoolDown); //после состояния ожидания перейти
            
            StartState(_patrol.DoPatrol()); //в состояние патрулирования
        }

        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer) //пока мы можем атаковать
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCoolDown);
            }
            StartState(GoToHero()); //если мы не можем атаковать, то двигаемся к герою 
        }

        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position; //от текущ точки таргета отнимаем нашу позицию
            direction.y = 0;
            return direction.normalized;

        }
        
        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            
            if (_current != null) //если текущая корутина не равна нулю
                StopCoroutine(_current); // будем её останавливать

            _current = coroutine;
                StartCoroutine(coroutine);
        }
        
        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            
            _creature.SetDirection(Vector2.zero); //чтобы герой не двигался после смерти
            
            if (_current != null) //если текущая корутина не равна нулю
                StopCoroutine(_current); // будем её останавливать
        }
    }
}