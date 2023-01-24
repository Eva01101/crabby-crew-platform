using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class SpawnListComponent: MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;
       // public bool InvertScale { get; set; }

       public void SpawnAll()
       {
           foreach (var spawnData in _spawners)
           {
               spawnData.Component.Spawn();
           }
       }

        public void Spawn(string id)
        {
            var spawner = _spawners.FirstOrDefault(element => element.Id == id); //в эту функцию будет передавать кажд элемент, зашифрованный в element
            //если айди этого элемента равен идентификатору, который мы передали, то мы вернём данный элемент массива
            spawner?.Component.Spawn(); //вызываем через ? так как spawner может быть null
            
            /*foreach (var data in _spawners) // в _spawners найти конкретный элемент по айдишке
            {
                if (data.Id == id)
                {
                    data.Component.Spawn(); // заспавним нужный нам партикт
                    break;
                }
            } */
        }
        
        [Serializable] //чтобы видеть в инспекторе
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component; //на какой компонент по айди он будет ссылаться
        }
    }
}