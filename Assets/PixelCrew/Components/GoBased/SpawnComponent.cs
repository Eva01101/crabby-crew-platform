using PixelCrew.Utils;
using PixelCrew.Utils.ObjectPool;
using UnityEngine;

namespace PixelCrew.Components.GoBased
{
    public class SpawnComponent: MonoBehaviour
    {
        [SerializeField] private Transform _target; //переменная отвеч за позицию, где созд спрайт
        [SerializeField] private GameObject _prefab; //объект с самим партиклом, передаваемый из префаба
        //[SerializeField] private bool _inverXtScale; //если у нас эта переменная, то мы по Х будем переворачивать героя 
        [SerializeField] private bool _usePool;
        
        [ContextMenu("Spawn")] //позволить включать и выключать параметр 
        
        public void Spawn()
        {
            SpawnInstance();
        }

        public GameObject SpawnInstance()
        {
            var targetPosition = _target.position;
            
            var instance = _usePool
                ? Pool.Instance.Get(_prefab, targetPosition)
                : SpawnUtils.Spawn(_prefab, targetPosition); //клонировать и создавать будем префаб, позиция
            //в которой будем создавать префаб, Quaternion - поворот префаба
            
            var scale = _target.lossyScale;
            //scale.x *= _inverXtScale ? -1 : 1; //переворач по Х
            
            instance.transform.localScale = scale; //заменим один комп на другой 
            //localScale - комп относительно родителя, lossyScale - виден относительно всего мира
            instance.SetActive(true);
            return instance;
        }

        public void SetPrefab(GameObject prefab)
        {
            _prefab = prefab; 
        }
    }
}