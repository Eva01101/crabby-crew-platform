using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.UI.Widgets
{
    public class DataGroup<TDataType, TItemType> where TItemType: MonoBehaviour, IItemRenderer<TDataType>
    {
        protected readonly List<TItemType> CreatedItem = new List<TItemType>();
        private readonly TItemType _prefab;
        private readonly Transform _container; 

        public DataGroup(TItemType prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }

        public virtual void SetData(IList<TDataType> data)
        {
        
            for (var i = CreatedItem.Count; i < data.Count(); i++) //от количества созданных элементов до _inventory.Length будем создавать
            {
                //если у нас будет уже создано два элемента, а в списке 3 элемента, создадим один элем, а остальное возьмём из кэша
                var item = Object.Instantiate(_prefab, _container); //создадим через Instantiate
                //_container - то место, где будем создавать
                CreatedItem.Add(item);
            }

            //обновить данные в созданных элементах
            for (var i = 0; i < data.Count; i++) //проходимся по списку
            {
                CreatedItem[i].SetData(data[i], i); //будем проходиться по _createdItem и вызывать метод SetData
                //и отправлять соответ запись из нашего инвенторя
                CreatedItem[i].gameObject.SetActive(true); //активировать элемент

            }

            //прятание ненужных элементов, неизпользуемых в данный момент
            for (var i = data.Count; i < CreatedItem.Count; i++) //всё что за пределами инвенторя, уже неиспользуем мы должны их спрятать
            {
                CreatedItem[i].gameObject.SetActive(false);
            }
        }
    }

    public interface IItemRenderer<TDataType>
    {
        void SetData(TDataType data, int index);
    }
}