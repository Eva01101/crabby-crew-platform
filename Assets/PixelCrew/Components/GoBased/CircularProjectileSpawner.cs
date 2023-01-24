using System;
using System.Collections;
using PixelCrew.Creatures.Weapons;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class CircularProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private CircularProjectileSettings[] _settings;
        
        public int Stage { get; set; }

        [ContextMenu("Launch!")]
        public void LaunchProjectiles()
        {
            StartCoroutine(SpawnProjectiles());
        }

        private IEnumerator SpawnProjectiles()
        {
            var setting = _settings[Stage];
            var sectorStep = 2 * Mathf.PI / setting.BurstCount;
            for (int i = 0; i < setting.BurstCount; i++)
            {
                var angle = sectorStep * i; //в какой угол нам послать наш прожектайл
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));// x -cos, y - sin

                var instance = SpawnUtils.Spawn(setting.Prefab.gameObject, transform.position);//заспавним
                //спавнер возвращает нам геймобжект => нужно получить компонент
                var projectile = instance.GetComponent<DirectionalProjectile>();
                projectile.Launch(direction);
                
                yield return new WaitForSeconds(setting.Delay);
            } 
        }
    }

    [Serializable]
    public struct CircularProjectileSettings
    {
        [SerializeField] private DirectionalProjectile _prefab;
        [SerializeField] private int _burstCount; //сколько прожектов будем спавнить
        [SerializeField] private float _delay; //сколько удет задержка между спавнами

        public DirectionalProjectile Prefab => _prefab;

        public int BurstCount => _burstCount;

        public float Delay => _delay;
    }
}