using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform; // точка куда телепортироваться
        [SerializeField] private float _alphaTime = 1; //время исчезновения , время работы наших анимаций
        [SerializeField] private float _moveTime = 1; // время передвижения

        public void Teleport(GameObject target) //метод телепорта, target - объект, который будем перемещать 
        {
            // target.transform.position = _destTransform.position; //телепортируем по позиции 
            StartCoroutine(AnimateTeleport(target)); //запускаем корутину, Coroutine - сущность внутри юнити позволяющ в определ куске метода делать прерывания и оставлять до какого-то кадра
        }

        private IEnumerator AnimateTeleport(GameObject target) //вызываем корутину, корутины должны возвращать такой интерфейс 
            //нужен, чтобы запускать постоянные прерывания
        {
            var sprite = target.GetComponent<SpriteRenderer>(); //target - то, с чем мы заколайдились, получаем спрайт
            var input = target.GetComponent<PlayerInput>();
            SetLockInput(input, true);
            
            yield return AlphaAnimation(sprite, 0); // вошлив портал и отключились
            target.SetActive(false); //вошли в портал и больше ничего от объекта не нужно
            
            yield return MoveAnimation(target); //корутина передвижения
            
            target.SetActive(true); //вклчаем обратно объект
            yield return AlphaAnimation(sprite, 1); //ждём конец корутины вышли из портала
            SetLockInput(input, false);
        }

        private void SetLockInput(PlayerInput input, bool isLocked)
        {
            
            if (input != null)
            {
                input.enabled = !isLocked;
            }
        }

        private IEnumerator MoveAnimation(GameObject target) //корутина с движением героя
        {
            var moveTime = 0f; // заводим переменную со временем
            while (moveTime < _moveTime)
            {
                moveTime += Time.deltaTime;
                var progress = moveTime / _moveTime;
                target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress); //у таргета забираем трансформ
                //target.transform.position,_destTransform - откуда и куда
                yield return null; //ждём кадр (ожидаем какого-то события)
            }
        }

        private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha) //начинаем анимировать спрайт,
        {
            var time = 0f; //у нас есть текущее время анимации
            var spriteAlpha = sprite.color.a; //дефолтное стартовое значение 

            while (time < _alphaTime) //время будет меньше настройки нашего времени 
            {
                time += Time.deltaTime; //прибавим время прошедшее с послед кадра
                var progress = time / _alphaTime;
                var tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progress); //будем интерполировать значение стартовое со знач, куда нужно прийтиб нужно исчезнуть, поэтому 0
                //третье значение - прогресс перехода из одного состояния в другое, _alphaTime - настроечный альфатайм
                var color = sprite.color; //у спрайта возьмем цвет
                color.a = tmpAlpha; //у цвета поменяем альфу
                sprite.color = color; //запихаем обратно в спрайт

                yield return null; //пропускает кадр
            }
        }
    }
}