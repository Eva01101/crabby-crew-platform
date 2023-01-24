using System;
using UnityEngine.Events;

namespace PixelCrew.Utils.Disposable
{
    public static class UnityEventExtensions //выбираем эвент на кот хотим подписаться
    {
        public static IDisposable Subscribe(this UnityEvent unityEvent, UnityAction call)
        {
            unityEvent.AddListener(call); //делаем подписку
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
            //мы вынесли отписку отдельно
        }
        public static IDisposable Subscribe<TType>(this UnityEvent<TType> unityEvent, UnityAction<TType> call)
        {
            unityEvent.AddListener(call); //делаем подписку
            return new ActionDisposable(() => unityEvent.RemoveListener(call));
            //мы вынесли отписку отдельно
        }
        
    }
}