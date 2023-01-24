using System;
using System.Collections;
using Cinemachine;
using PixelCrew.Components;
using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.GoBased;
using PixelCrew.Components.Health;
using PixelCrew.Creatures.Hero.Features;
using PixelCrew.Effects.CameraRelated;
using PixelCrew.Model;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.Utils;
using UnityEngine;
using Random = UnityEngine.Random;


namespace PixelCrew.Creatures.Hero 
{
    public class Heroes : Creature, ICanAddInInventory
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private ColliderCheck _wallCheck;
        
        [SerializeField] private float _slamDownVelocity;

        [SerializeField] private Cooldown _throwCoolDown;
        [SerializeField] private RuntimeAnimatorController _armed;
        [SerializeField] private RuntimeAnimatorController _disarmed;

        [Header("Super Throw")] [SerializeField]
        private Cooldown _superThrowCooldown;
        
        [SerializeField] private int _superThrowParticles; //количество мечей, кот будем выбрасывать
        [SerializeField] private float _superThrowDelay; //сколько сек должно пройти между бросками
        [SerializeField] private ProbabilityDropComponent _hitDrop;
        [SerializeField] private SpawnComponent _throwSpawner;
        [SerializeField] private ShieldComponent _shield;
        [SerializeField] private HeroFlashlight _flashlight;
        
        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");
        
        private bool _allowDoubleJump;
        private bool _isOnWall;
        private bool _superThrow;

        private GameSession _session;
        private HealthComponent _health;
        private CameraShakeEffect _cameraShake;
        private float _defaultGravityScale;

        private const string SwordId = "Sword";
        
        private int CoinsCount => _session.Data.Inventory.Count("Coin");
        
        private int SwordCount => _session.Data.Inventory.Count(SwordId);

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;
                
                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable); //иначе, если это у нас что-либо кидаемое, мы можем его кинуть 
            }
        }

        protected override void Awake()
        {
            base.Awake(); //с помощью ключевого слова base вызываем метод в базовом классе
            _defaultGravityScale = Rigidbody.gravityScale;
        }
        
        private void Start() //старт вызывается позже, чем awake
        {
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _session = FindObjectOfType<GameSession>(); //ищем в сцене нужный нам объект
            _health = GetComponent<HealthComponent>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged; //подпишемся на события +=
            _session.StatsModel.OnUpgraded += OnHeroUpgraded; 
            
            _health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon(); //обновить представление нашего героя о себе из этих данных 
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId) //перезаписываем здоровье
            {
                case StatId.Hp:
                   var health = (int) _session.StatsModel.GetValue(statId);
                   _session.Data.Hp.Value = health;
                   _health.SetHealth(health);
                    break;
            }
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged; //также нужно обязательно отписаться от события -= 
            
        }
        
        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
        }
        
        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth; //обновим наши данные в сессии
        }
        
        protected override void Update()
        {
            base.Update();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            
            if (_wallCheck.IsTouchingLayer && moveToSameDirection) //не просто прислонился к стене, а давит на неё
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0; //гравитация 0
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale; 
            }
            Animator.SetBool(IsOnWall, _isOnWall);
        }
        
        protected override float CalculateYVelocity() //рассчёт y
        {
            //var yVelocity = _rigidbody.velocity.y;берём текущую вертикальную силу, если y Y нас не модифицирован никак оставляем как есть 
            // velocity - свойство физического тела, которое показывает величину ускорения объекта.
            // Свойство отлично подходит для задач, когда нужно замедлить или ускорить объект без резких толчков
            
            var isJumpPressing = Direction.y > 0;
            
            if (IsGrounded || _isOnWall) 
            {
                _allowDoubleJump = true; //если мы будем стоять на земле, то мы будем сбрасывать флаг allow, нам будет доступен двойной прыжок
            }

            if (!isJumpPressing && _isOnWall) //если мы не прыгаем и находимся на сцене
            {
               return 0f; //вернуть велосити равное 0, никуда не двигаемся по вертикальной координате
            }

            return base.CalculateYVelocity(); //иначе рассчитать как в базовом классе
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported && !_isOnWall) // если мы не на земле и разрешён дабл $$ иначе если нам доступен двойной прыжок $$ мы совершаем двойной прыжок 
            //также запрещать прыгать, когда мы на стене
            {
                _session.PerksModel.Cooldown.Reset();
                _allowDoubleJump = false; // и запретим прыгать второй раз 
                DoJumpVfx();
                return _jumpSpeed; 
            }

            return base.CalculateJumpVelocity(yVelocity); // и теперь вернём скорость в рассчёт 
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }
        
        
        public override void TakeDamage()
        {
            base.TakeDamage();
            _cameraShake?.Shake();
            if(CoinsCount  > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            
            var numCoinsToDispose = Mathf.Min(CoinsCount , 5); //посчитаем количество коинов, которые мы можем выкинуть
            //текущее значение коинов и максимум монет, если будет больше 5, то вернём пять, если меньше, то вернём текущее колич монет
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            _hitDrop.SetCount(numCoinsToDispose);
            _hitDrop.CalculateDrop(); //посчитаем дроп
            
            
            /*var burst = _hitParticles.emission.GetBurst(0); // emission - метод из пактикл системы, доступ
            burst.count = numCoinsToDispose; // каждый раз будет выплёвываться нужное количество монет
            _hitParticles.emission.SetBurst(0, burst); //нам нужно поставить burst в нужный нам индекс 
            
            _hitParticles.gameObject.SetActive(true); //сначала включим наш объект
            _hitParticles.Play(); // скажем проиграться нашей системе партиклов */
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        private void OnCollisionEnter2D(Collision2D other) //вызовется в тот момент, когда герой соприкоснётся с чем-бы то ни было ещё
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0]; // нужно понять с какой скоростью
                if (contact.relativeVelocity.y >=
                    _slamDownVelocity) //relativeVelocity - скорость относительно двух коллайдеров
                {
                    _particles.Spawn("SlamDown"); //приземление героя 
                }
            }
        }
        

        public override void Attack() //здесь мы запускаем триггер
        {
            if (SwordCount <= 0) return; //
            
            base.Attack();
        }
        
        
        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
            
            /*if (_session.Data.IsArmed) //если мы вооружены
            {
                _animator.runtimeAnimatorController = _armed;
            }
            else
            {
                _animator.runtimeAnimatorController = _disarmed;
            } */
                
        }

        public void OnDoThrow()
        {
            if (_superThrow && _session.PerksModel.IsSuperThrowSupported)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId); //сначала получим всё выбранное
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;
                
                var numThrows = Mathf.Min(_superThrowParticles, possibleCount); //-1 для того, чтобы всегда оставался 1 меч
                _session.PerksModel.Cooldown.Reset();
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory(); //в случае обычного броска вызываем этот метод
            }

            _superThrow = false; //супер бросок снять 

        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory(); //будем выбрасывать меч
                yield return new WaitForSeconds(_superThrowDelay); //и ожидать 
            }
            
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");
            //_particles.Spawn("Throw");
            //который мы получим из дефинишн
            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId); //заходим в фасад и получаем кидабельные предметы
            _throwSpawner.SetPrefab(throwableDef.Projectile);
            var instance = _throwSpawner.SpawnInstance();//потом только спавн
            ApplyRangeDamageStat(instance);
            
            _session.Data.Inventory.Remove(throwableId, 1);//взять текущие данные инвенторя и удалить 1
            //удалить то, что кинули
        }

        private void ApplyRangeDamageStat(GameObject projectile)
        {
           var hpModify = projectile.GetComponent<ModifyHealthComponent>();
           var damageValue =(int) _session.StatsModel.GetValue(StatId.RangeDamage);
           damageValue = ModifyDamageByCrit(damageValue);
           hpModify.SetDelta(-damageValue);
        }

        private int ModifyDamageByCrit(int damage) //критический удар
        {
            var critChance = _session.StatsModel.GetValue(StatId.CriticalDamage);
            if (Random.value * 100 <= critChance)
            {
                return damage * 2;
            }

            return damage;
        }


        public void StartThrowing()
        {
            _superThrowCooldown.Reset();
        }

        public void UseInventory()
        {
            if (IsSelectedItem(ItemTag.Throwable))//если можем бросать предмет - бросим
                PerformThrowing();
            else if (IsSelectedItem(ItemTag.Potion))//если есть тэг пошн, то применим как пошн
                UsePotion();
        }

        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
               case Effect.AddHp:
                   _session.Data.Hp.Value += (int) potion.Value; //добавляет здоровье
                   break;
               case Effect.SpeedUp:
                   _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + potion.Time; //используем, увеличим время работы эффекта
                   _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);//выбираем максимальное знач из того, какой эффект действует
                   _speedUpCooldown.Reset();
                   break;
            }
            
            _session.Data.Inventory.Remove(potion.Id, 1);
        }

        private readonly Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed; 

        protected override float CalculateSpeed()
        {
            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;

            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _additionalSpeed;
        }

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        private void PerformThrowing()
        {
            if (!_throwCoolDown.IsReady || !CanThrow) return; //если мы не готовы к броску, то выйдем
            
            if (_superThrowCooldown.IsReady) _superThrow = true;
            
            Animator.SetTrigger(ThrowKey); //то стартуем нашу анимацию 
            _throwCoolDown.Reset(); //ресетим наш кулдавн, чтобы начал заново считаться
        }

        /*public void UsePotion()
        {
            var potionCount = _session.Data.Inventory.Count("HealthPotion");
            if (potionCount > 0)
            {
                _health.ModifyHealth(5);
                _session.Data.Inventory.Remove("HealthPotion", 1);
            }
        }*/

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }
        
       /* public void DropDown()
        {
            var endPosition = transform.position + new Vector3(0, -1);
            var hit = Physics2D.Linecast(transform.position, endPosition, _groundLayer);
            if (hit.collider == null) return;
            var component = hit.collider.GetComponent<TmpDisableColliderComponent>();
            if (component == null) return;
            component.DisableCollider();
        }*/

        public void UsePerk()
        {
            if (_session.PerksModel.IsShieldSupported)
            {
                _shield.Use();
                _session.PerksModel.Cooldown.Reset();
            }
        }

        public void ToggleFlashlight()//фонарик
        {
            var isActive = _flashlight.gameObject.activeSelf;
            _flashlight.gameObject.SetActive(!isActive);

        }
    }
}