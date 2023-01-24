using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
    
    public class LocaleDef: ScriptableObject
    {
        // en https://docs.google.com/spreadsheets/d/e/2PACX-1vQRopjGzFgGnLiavauzMFnxqrYNLMwpIqNHbKAYurupkQvBsQikGNB10JSOfOlaAKMp0q5dgcLXugjI/pub?gid=0&single=true&output=tsv
        // ru https://docs.google.com/spreadsheets/d/e/2PACX-1vQRopjGzFgGnLiavauzMFnxqrYNLMwpIqNHbKAYurupkQvBsQikGNB10JSOfOlaAKMp0q5dgcLXugjI/pub?gid=2145717567&single=true&output=tsv

        [SerializeField] private string _url;
        [SerializeField] private List<LocaleItem> _localeItems;

        private UnityWebRequest _request;

        public Dictionary<string, string> GetData() //преобразуем _localeItems в Dictionary - структура ключ/значение
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var localeItem in _localeItems) //проходимся по всем строкам локализации
            {
                dictionary.Add(localeItem.Key, localeItem.Value);
            }

            return dictionary;
        }

        [ContextMenu("Update locale")]
        public void UpdateLocale()
        {
            if(_request != null) return; //если у нас есть запрос, то ничего делать не будем
            
            _request = UnityWebRequest.Get(_url);
            _request.SendWebRequest().completed += OnDataLoaded; 

        }

#if UNITY_EDITOR // так как испол UnityEditor
        [ContextMenu("Update locale from file")]
        public void UpdateLocaleFromFile()
        {
            var path = UnityEditor.EditorUtility.OpenFilePanel("Open locale file", "", "tsv");
            if (path.Length != 0)
            {
                var data = File.ReadAllText(path);
                ParseData(data);
            }
        }
#endif
        
        private void OnDataLoaded(AsyncOperation operation)//обработаем данные, кот нам пришли
        {
            if (operation.isDone)//если операция прошла успешно
            {
                var data = _request.downloadHandler.text; //можем обрабатывать данные, кот хранятся в downloadHandler
                //сначала получим строки
               ParseData(data);
            }
        }

        private void ParseData(string data)
        {
            var rows = data.Split('\n'); //Split - разделяет строку на кусочки и возвращает массив 
            _localeItems.Clear();
            foreach (var row in rows) //пройтись по всем строкам
            {
                AddLocaleItem(row);
            }
        }

        private void AddLocaleItem(string row)
        {
            try
            {
              var parts = row.Split('\t');
              _localeItems.Add(new LocaleItem{Key = parts[0], Value = parts[1]}); // из таблицы menu_title - ключ, меню - значение
            }
            catch (Exception e)
            {
                Debug.LogError($"Can`t parse row: {row}.\n {e}");
            }
        }

        [Serializable]
        private class LocaleItem
        {
            public string Key;
            public string Value;
        }
    }
}