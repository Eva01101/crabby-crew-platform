using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable] //чтобы можно было использовать как проперти
    public struct DialogData
    {
        [SerializeField] private Sentence[] _sentences;
        [SerializeField] private DialogType _type;
        public Sentence[] Sentences => _sentences; //элемент защиты, чтобы снаружи данные не могли модифицировать
        
        public DialogType Type => _type;
    }

    [Serializable]
    public struct Sentence
    {
        [SerializeField] private string _valued; //текст
        [SerializeField] private Sprite _icon; //спрайт с иконкой
        [SerializeField] private Side _side; //на какой стороне показывать

        public string Valued => _valued;

        public Sprite Icon => _icon;

        public Side Side => _side;
    }

    public enum Side
    {
        Left,
        Right
    }

    public enum DialogType
    {
        Simple,
        Personalized
    }
}