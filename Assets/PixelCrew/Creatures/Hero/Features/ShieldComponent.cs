using PixelCrew.Components.Health;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Creatures.Hero.Features
{
    public class ShieldComponent: MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private Cooldown _cooldown; //количество времени действия щита

        public void Use()
        {
            _health.Immune.Retain(this); //будем делать его имунным
            _cooldown.Reset();// сколько будет работать сам щит
            gameObject.SetActive(true);//будем включать его
        }

        private void Update()
        {
            if(_cooldown.IsReady)//если готов
                gameObject.SetActive(false);//будем выключать объект
        }

        private void OnDisable()//во время выключения
        {
            _health.Immune.Release(this); //отключить, объект освободил иммунитет
        }
    }
}