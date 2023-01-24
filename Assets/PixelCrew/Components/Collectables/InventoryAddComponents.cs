using PixelCrew.Creatures.Hero;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repository.Items;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Collectables
{
    public class InventoryAddComponents: MonoBehaviour
    {
       [InventoryId] [SerializeField] private string _id; //что мы будем добавлять
        [SerializeField] private int _count; //сколько

        public void Add(GameObject go)
        {
            var hero = go.GetInterface<ICanAddInInventory>(); //метод теперь зависит от абстракции, не от конкретной реализации
            hero?.AddInInventory(_id, _count);
        }
    }
}