using System;

namespace PixelCrew.Utils.Disposable
{
    public class ActionDisposable: IDisposable //отписки от подписок
    {
        private Action _onDispose;

        public ActionDisposable(Action onDispose)
        {
            _onDispose = onDispose; 
        }
        public void Dispose()
        {
            _onDispose?.Invoke();
            _onDispose = null; 
        }
    }
}