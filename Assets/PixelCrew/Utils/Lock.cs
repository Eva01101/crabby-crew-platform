using System.Collections.Generic;

namespace PixelCrew.Utils
{
    public class Lock
    {
        private readonly List<object> _retained = new List<object>();//список объектов

        public void Retain(object item) //включили
        {
            _retained.Add(item);
        }

        public void Release(object item)
        {
            _retained.Remove(item);
        }

        public bool IsLocked => _retained.Count > 0; //количество элем, кот заретейнены, если больше 0 => заблочен объект
    }
}