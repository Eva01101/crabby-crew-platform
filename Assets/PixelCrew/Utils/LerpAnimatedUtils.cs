using System;
using System.Collections;
using UnityEngine;

namespace PixelCrew.Utils
{
    public static class LerpAnimatedUtils
    {
        public static Coroutine LerpAnimated(this MonoBehaviour behaviour, float start, float end, float time, Action<float> OnFrame)
        //time - время изменений от старта до конца
        {
            return behaviour.StartCoroutine(Animate(start, end, time, OnFrame));
        }

        private static IEnumerator Animate(float start, float end, float animationTime, Action<float> onFrame)
        {
            var time = 0f; //у нас есть текущее время анимации
            onFrame(start); //от стартового значения
            while (time < animationTime) //пока время не вышло
            {
                time += Time.deltaTime; //
                var progress = time / animationTime;//будем скипать, пока не достигнем animationTime, получ текущ прогресс по времени
                var value = Mathf.Lerp(start, end, progress); //будем интерполировать значение стартовое со знач, куда нужно прийтиб нужно исчезнуть, поэтому 0
                //третье значение - прогресс перехода из одного состояния в другое, _alphaTime - настроечный альфатайм
                onFrame(value);
                
                yield return null; //пропускает кадр
            }

            onFrame(end);
        }
    }
}