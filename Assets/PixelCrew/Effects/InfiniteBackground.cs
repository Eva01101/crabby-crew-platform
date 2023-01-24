using System;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Effects
{
    public class InfiniteBackground: MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _container; //ссылка на контейнер

        private Bounds _containerBounds;
        private Bounds _allBounds; //границы всех контейнеров, кот будем расширять
        
        private Vector3 _boundsToTransformDelta;
        private Vector3 _containerDelta;
        private Vector3 _screenSize;

        private void Start() //взять все спрайты из контейнера и посчитать границы
        {
            var sprites = _container.GetComponentsInChildren<SpriteRenderer>(); //получим все спрайты
            _containerBounds = sprites[0].bounds;

            foreach (var sprite in sprites)
            {
                _containerBounds.Encapsulate(sprite.bounds); //bounds - границы нашего спрайта
                //Encapsulate - расширяет границы
                //это границы одного контенера 
            }

            _allBounds = _containerBounds; //на старте будут равны дефолт значен

            _boundsToTransformDelta = transform.position - _allBounds.center;
            _containerDelta = _container.position - _allBounds.center; 
        }

        private void LateUpdate()
        {
            var min = _camera.ViewportToWorldPoint(Vector3.zero); //получим размер нашего экрана
            var max = _camera.ViewportToWorldPoint(Vector3.one);
            //ViewportToWorldPoint - измеряется от 0 до 1
            _screenSize = new Vector3(max.x - min.x, max.y - min.y);

            //считаем, вылез наш текущ
            //контейнер за границы или нет
            _allBounds.center = transform.position - _boundsToTransformDelta; //подвинем allBounds
            var camPosition = _camera.transform.position.x; //найдём текущ позицию нашей камеры
            var screenLeft = new Vector3(camPosition - _screenSize.x / 2, _containerBounds.center.y);
            var screenRight = new Vector3(camPosition + _screenSize.x / 2, _containerBounds.center.y);
            //позиция камеры -+ половина размера скрина 

            if (!_allBounds.Contains(screenLeft))
            {
                InstantiateContainer(_allBounds.min.x - _containerBounds.extents.x);
            }

            if (!_allBounds.Contains(screenRight))
            {
                InstantiateContainer(_allBounds.max.x + _containerBounds.extents.x); 
                //extents - половина размера наших границ
            }
        }

        private void InstantiateContainer(float boundCenterX)//получ центральную координату, в кот мы поместим наш контей
        {
            var newBounds = new Bounds(new Vector3(boundCenterX, _containerBounds.center.y), _containerBounds.size);
            _allBounds.Encapsulate(newBounds);

            _boundsToTransformDelta = transform.position - _allBounds.center; //сдвигаем дельту, так как изменился размер
            var newContainerXPos = boundCenterX + _containerDelta.x;
            var newPosition = new Vector3(newContainerXPos, _container.transform.position.y); //считаем позицию нашего объекта 

            Instantiate(_container, newPosition, Quaternion.identity, transform);
        }

        private void OnDrawGizmosSelected()
        {
            GizmosUtils.DrawBounds(_allBounds, Color.magenta);
        }
    }
}