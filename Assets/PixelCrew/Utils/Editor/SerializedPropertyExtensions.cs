using System;
using UnityEditor;

namespace PixelCrew.Utils.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static bool GetEnum<TEnumType>(this SerializedProperty property, out TEnumType retValue) 
            where TEnumType: Enum //ограничение на дженерик тип
            //out - с помощью этого слова можем указать возвращаемое значение переменной
        {
            retValue = default;
            var names = property.enumNames; //список всех имён, кот может принимать этот enum
            
            if (names == null || names.Length == 0) //неудачное взятие enum
                return false;

            var enumName = names[property.enumValueIndex]; //получим имя по индексу
            retValue = (TEnumType) Enum.Parse(typeof(TEnumType), enumName); //благодаря тому, что у нас есть имя - enumName, мы можем преобразовать в конкретный enum
            //передаём тип и имя и приводим к типу, который желаем
            return true; //преобразование удалось
        }
    }
}