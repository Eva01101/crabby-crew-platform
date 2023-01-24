using System;
using PixelCrew.Model.Data.Properties;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")] 
    public class GameSettings: ScriptableObject
    {
        [SerializeField] private FloatPersistentProperty _music;  //фоновая музыка
        
        [SerializeField] private FloatPersistentProperty _sfx;

        public FloatPersistentProperty Music => _music;
        public FloatPersistentProperty Sfx => _sfx;

        private static GameSettings _instance;
        public static GameSettings I => _instance == null ? LoadGameSettngs() : _instance;

        private static GameSettings LoadGameSettngs()
        {
            return _instance = Resources.Load<GameSettings>("GameSettings");
        }


        private void OnEnable()
        {
           // PlayerPrefs.SetFloat("music", 20f) //сохраняет данные в регистр, сохр значение в постоянную память
           // PlayerPrefs.Save(); - один из вариантов решения задачи, сохр отдельно для каждой проперти
           
           _music = new FloatPersistentProperty(1, SoundSetting.Music.ToString());
           _sfx = new FloatPersistentProperty(1, SoundSetting.Sfx.ToString());
        }

        private void OnValidate()
        {
            Music.Validate();
            Sfx.Validate();
        }
    }
    
    public enum SoundSetting
    {
        Music,
        Sfx
    
    }
}