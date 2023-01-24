using System;
using PixelCrew.Model.Definitions.Repository;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Player
{
    [Serializable]
    public struct StatDef
    {
        [SerializeField] private string _name;
        [SerializeField] private StatId _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private StatLevelDef[] _levels; //распределение по уровню

        public StatId ID => _id;

        public string Name => _name;

        public Sprite Icon => _icon;

        public StatLevelDef[] Levels => _levels;
    }

    [Serializable]
    public struct StatLevelDef
    {
        [SerializeField] private float _value; //то, какой значение будет принимать параметр героя
        [SerializeField] private ItemWithCount _price; //сколько стоит прокачать до этого уровня

        public float Value => _value;

        public ItemWithCount Price => _price;
    }

    public enum StatId
    {
        Hp,
        Speed,
        RangeDamage,
        CriticalDamage
    }
}