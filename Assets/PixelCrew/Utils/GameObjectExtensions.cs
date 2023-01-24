using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject go, LayerMask layer) //будем чекать находится ли слой этого геймобжекта в этой лэйермаске

        {   //0001 go.layer
            //0110 mask
            //0111 побитовое смещение вернёт такой результат 
            return layer == (layer | 1 << go.layer); 
        }

        public static TInterfaceType GetInterface<TInterfaceType>(this GameObject go) //<TInterfaceType> - тип класса
        { //метод, позвол вернуть интерфейсы
            var components = go.GetComponents<Component>();
            foreach (var component in components)
            {
                if (component is TInterfaceType type) //если это наш интерфейс
                {
                    return type;
                }
            }

            return default;
        }
    }
}