using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository.Items.Editor
{
    [CustomPropertyDrawer(typeof(InventoryIdAttribute))]
    public class InventoryIdAttributeDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = DefsFacade.I.Items.ItemsForEditor; //получили все айди предметов из инвенторя
            var ids = new List<string>();
            foreach (var itemDef in defs)
            {
                ids.Add(itemDef.Id); //добавить в список все элементы
            }

            var index = Mathf.Max(ids.IndexOf(property.stringValue), 0); //получаем строковое значение переменной, котор обрабатываем
            //всегда будет как минимум 0, функция макс выберет большее из двух переданных значений 
            index = EditorGUI.Popup(position, property.displayName, index, ids.ToArray()); //передаём позицию - где нужно отрисовать,
            //далее как он будет называться, индекс и список элементов дропдавна
            //мы отрисовали дропдавн лист, если выбрали что-то другое, он вернёт просто индекс
            property.stringValue = ids[index]; //запишем новое значение айди
        }
    }
}