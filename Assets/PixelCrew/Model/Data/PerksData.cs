using System;
using System.Collections.Generic;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class PerksData
    {
        [SerializeField] private StringProperty _used = new StringProperty();
        [SerializeField] private List<string> _unlocked; //перки, которые уже купили

        public StringProperty Used => _used;

        public void AddPerk(string id)
        {
            if(!_unlocked.Contains(id))
                _unlocked.Add(id); //если у нас такого перка ещё нет, добавим
        }

        public bool IsUnlocked(string id) //проверка, анлоченый или нет перк
        {
            return _unlocked.Contains(id);
        }
    }
}