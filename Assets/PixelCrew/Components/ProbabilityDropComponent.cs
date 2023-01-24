using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class ProbabilityDropComponent: MonoBehaviour
    {
        [SerializeField] private int _count; //количество выпадющих элементов
        [SerializeField] private DropData[] _drop;
        [SerializeField] private DropEvent _onDropCalculated;
        [SerializeField] private bool _spawnOnEnable;

        private void OnEnable()
        {
            if (_spawnOnEnable)
            {
                CalculateDrop();
            }
        }
        
        [ContextMenu("CalculateDrop")] //
        public void CalculateDrop()
        {
            var itemsToDrop = new GameObject[_count]; //массив, размер - количество элементов
            var itemCount = 0;
            var total = _drop.Sum(dropData => dropData.Probability);
            var sortedDrop = _drop.OrderBy(dropData => dropData.Probability); 

            while (itemCount < _count)
            {
                var random = UnityEngine.Random.value * total; //получим какой-то рандом в рамках нашей полной вероятности
                var current = 0f; 
                
                foreach (var dropData in sortedDrop) //пройдёмся по всем элементам
                {
                    current += dropData.Probability; 
                    if (current >= random) 
                    {
                        itemsToDrop[itemCount] = dropData.Drop;
                        itemCount++;
                        break;
                    }
                }
            }
            _onDropCalculated ?.Invoke(itemsToDrop);
        }
        
        [Serializable]
        public class DropData
        {
            public GameObject Drop; //ссылка на префаб 
            [Range(0f, 100f)] public float Probability; //вероятность, Range(0f, 100f) - ограничитель 
        }
        
        public void SetCount(int count) //передадим кол монеток, кот доожны выкинуть
        {
            _count = count; 
        }
    }
    
    [Serializable]
    public class DropEvent: UnityEvent<GameObject[]> //принимать массив объектов, всё, что необходимо заспавнить
    {
            
    }
}