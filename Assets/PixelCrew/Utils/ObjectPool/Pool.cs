using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Utils.ObjectPool
{
    public class Pool: MonoBehaviour
    {
        private readonly Dictionary<int, Queue<PoolItem>> _items = new Dictionary<int, Queue<PoolItem>>();
       //ключ значение, ключ - айдишка, значение - очередь из элементов 

       private static Pool _instance;

       public static Pool Instance
       {
           get
           {
               if (_instance == null)
               {
                   var go = new GameObject("###MAIN_POOL###");
                   _instance = go.AddComponent<Pool>();
               }

               return _instance;
           }
       }

       public GameObject Get(GameObject go, Vector3 position)
       {
           var id = go.GetInstanceID(); //получим уникальный идентификатор
           var queue = RequireQueue(id);

           if (queue.Count > 0) //если есть свободные элементы
           {
               var pooledItem = queue.Dequeue(); //вернуть из очереди
               pooledItem.transform.position = position;
               pooledItem.gameObject.SetActive(true);
               pooledItem.Restart();
               return pooledItem.gameObject;
           }

           var instance = SpawnUtils.Spawn(go, position, gameObject.name);
           var poolItem = instance.GetComponent<PoolItem>();
           poolItem.Retain(id,this);

           return instance;
       }

       private Queue<PoolItem> RequireQueue(int id)
       {
           if (!_items.TryGetValue(id, out var queue))//если нет очереди
           {
               queue = new Queue<PoolItem>(); //создаём
               _items.Add(id, queue);
           }

           return queue;
       }

       public void Release(int id, PoolItem poolItem)
       {
           var queue = RequireQueue(id);
           queue.Enqueue(poolItem);//добавим
           
           poolItem.gameObject.SetActive(false);
       }
    }
}