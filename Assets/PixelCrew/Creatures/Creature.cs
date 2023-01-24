using System;
using PixelCrew.Components;
using PixelCrew.Components.Audio;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using UnityEngine;
using UnityEngine.Profiling;

namespace PixelCrew.Creatures
{
    public class Creature: MonoBehaviour
    {
        [Header("Params")] [SerializeField] private bool _invertScale; 
        [SerializeField] private float _speed;
        [SerializeField] protected float _jumpSpeed;
        [SerializeField] private float _damageVelocity; //то, на сколько нас подкидывает при каком-то дамаге

        [Header("Checkers")]
        [SerializeField] protected LayerMask _groundLayer;//передаём какой комп явл землёй,
        //земляобщая для всех и для героев и для мобов
        [SerializeField] private ColliderCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent _particles; //компонент с партиклами
        
        protected Rigidbody2D Rigidbody; //тело нашего героя
        protected Vector2 Direction;
        protected Animator Animator;
        protected PlaySoundsComponent Sounds;
        protected bool IsGrounded; // булевая переменная, так как либо на земле, либо нет
        private bool _isJumping;
        
        private static readonly int IsGroundKey = Animator.StringToHash("is-ground"); //переменная прочитается только один раз
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void  Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>(); //забираем компонент rigidbody
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
            
        }
        
        public void SetDirection(Vector2 direction) //направление задаём и у героя и у моба
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }
        
        private void FixedUpdate() //так как это физика, вычисления происходят в методе FixedUpDate
        {
            var xVelocity = CalculateXVelocity(); //рассчитываем исходя из направления, куда мы движемся
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);
            
            Animator.SetBool(IsGroundKey, IsGrounded); // устанавливаем через комп animator 
            Animator.SetBool(IsRunning, Direction.x != 0); // если is-run не равен нулю, то мы бежим
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);
            
            UpdateSpriteDirection(Direction);
        }

        protected virtual float CalculateXVelocity()
        {
            return Direction.x * CalculateSpeed(); 
        }

        protected virtual float CalculateSpeed()
        {
            return _speed;
        }
        
        protected virtual float CalculateYVelocity() //рассчёт y
        {
            var yVelocity = Rigidbody.velocity.y; // берём текущую вертикальную силу, если y Y нас не модифицирован никак оставляем как есть 
            // velocity - свойство физического тела, которое показывает величину ускорения объекта.
            // Свойство отлично подходит для задач, когда нужно замедлить или ускорить объект без резких толчков
            var isJumpPressing = Direction.y > 0;
            
            if (IsGrounded) //если на земле
            {
                _isJumping = false; //отпускать будем переменную, когда будем приземляться
            }
            
            //когда мы второй раз прыгнем, будем говорить false 
            
            if (isJumpPressing) // если нажата кнопка прыжка
            {
                _isJumping = true;
                
                var isFalling = Rigidbody.velocity.y <= 0.001f; //проверяем, что здесь наше тело падает, не прыг, когда герой летит наверх
                
                //if (!isFalling) return yVelocity; // если мы не падаем, то возвращаем, что было по дефолту
                //будем поднимат эту переменную, если нажали прыжок
                
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity; //рассчитаем скорость прыжка, как мы модифиц нашу вертикальную силу
            } // если у нас isFalling то мы будем рассчитывать, иначе вернём yVelocity
            
            else if (Rigidbody.velocity.y > 0 && _isJumping) //сброс скорости работает, если мы всё-таки прыгаем
                // если мы не прыгаем, но скорость больше нуля 
            {
                yVelocity *= 0.5f; // если мы отпустили кнопку прыжка и все равно двигаемся вверх, то мы замедляемся
                
            }

            return yVelocity; 
        }
        
        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded) // если мы всё же падаем, если мы на земле
            {
                yVelocity = _jumpSpeed; // то прибавляем _jumpSpeed, мы просто прыгаем 
                DoJumpVfx();
            }

            return yVelocity; // и теперь вернём скорость в рассчёт 
        }

        protected void DoJumpVfx()
        {
            Profiler.BeginSample("JumpVFXSample");
            _particles.Spawn("Jump");
            Profiler.EndSample();
            
            Sounds.Play("Jump");
        }
        
        public void UpdateSpriteDirection(Vector2 direction) // поворот героя
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1); // это константа, эквивалент единичному вектору
                    
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1); 
                    
            }
        }
        
        public virtual void TakeDamage()
        {
            _isJumping = false; //во время получения урона, вертик перемещение не должны считать за прыжок, герою наносится урон
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity); //изменим скорость с которой герой летит наверх
            
        }
        
        public virtual void Attack() //здесь мы запускаем триггер
        {
            Animator.SetTrigger(AttackKey); //здесь триггер срабатывает
            Sounds.Play("Melee"); //проигрываем в тот момент, когда мы только запускаем атаку
        }
        
        public void OnDoAttack()
        {
            _attackRange.Check(); 
            _particles.Spawn("Slash");
            
        }
    }
}