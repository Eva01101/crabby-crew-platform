using System;
using System.Collections.Generic;

namespace PixelCrew.Utils.Disposable
{
    public class CompositeDisposable: IDisposable //этот класс хранит в себе реализации IDisposable
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        
        public void Retain(IDisposable disposable) //будем принимать наши подписки
        {
            _disposables.Add(disposable);
        }
        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();//удаляем подписки
            }
            _disposables.Clear(); //сам массив нужно очистить
        }
    }
}