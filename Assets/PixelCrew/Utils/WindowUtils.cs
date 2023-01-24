using UnityEngine;

namespace PixelCrew.Utils
{
    public static class WindowUtils
    {
        public static void CreateWindow(string resourcePath) //передадим путь до нашего окна
        {
            var window = Resources.Load<GameObject>(resourcePath); //путь до настроек 
            var canvas = GameObject.FindWithTag("MainUICanvas").GetComponent<Canvas>();
            Object.Instantiate(window, canvas.transform); //инстанциируем найденный объект в канвасе, создаём объект
            //так как это статический класс, не наследник монобихэйвера, найти можно только с помощью
            //Object
        }
    }
}