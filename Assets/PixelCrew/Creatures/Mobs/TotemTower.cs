using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Components.Health;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs
{
    public class TotemTower : MonoBehaviour
    {
        [SerializeField] private List<ShootingTrapAI> _traps; //массив из частей, использ лист, потому что ловушки будут со 
        //временем убиваться
        [SerializeField] private Cooldown _cooldown; //кулдавн между выстрелами

        private int _currentTrap; //текущая стреляющая ловушка

        private void Start()
        {
            foreach (var shootingTrapAI in _traps) //на старте пройдёмся по всем ловушкам
            {
                shootingTrapAI.enabled = false; //и выключим, чтобы у них зря не дергался апдейт
                var hp = shootingTrapAI.GetComponent<HealthComponent>();
                hp._onDie.AddListener(() => OnTrapDead(shootingTrapAI)); //здесь у нас анонимная ссылка на кот
                //мы подписываемся, она пустая 
            }
        }

        private void OnTrapDead(ShootingTrapAI shootingTrapAI)
        {
            var index = _traps.IndexOf(shootingTrapAI); //находим индекс компонента
            _traps.Remove(shootingTrapAI);
            if (index < _currentTrap) //если индекс меньше текущего
            {
                _currentTrap--; 
            }
        }

        private void Update()
        {
            if (_traps.Count == 0)
            {
                enabled = false; //выключаем, чтобы апдейты не слал дальше
                Destroy(gameObject, 1f); //удалим через 1 сек, чтобы успеть заспавнить весь мусор
            }

            var hasAnyTarget = HasAnyTarget();
            // _traps.Any(x => x._vision.IsTouchingLayer); //если какая-то из наших
            //ловушек будет видеть нашего героя, то получим тру
            if (hasAnyTarget)
            {
                if (_cooldown.IsReady)
                {
                    _traps[_currentTrap].Shoot(); //возьмём текущую ловушку и скажем шут
                    _cooldown.Reset();
                    _currentTrap = (int) Mathf.Repeat(_currentTrap + 1, _traps.Count); //если ловушек
                    //будет больше, чем всего, скатимся к нулю
                }
            }
        }

        private bool HasAnyTarget()
        {
            foreach (var shootingTrapAI in _traps)
            {
                if (shootingTrapAI._vision.IsTouchingLayer)
                    return true;
            }

            return false;
        }
    }
}