using System;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Audio
{
    public class PlaySoundsComponent: MonoBehaviour
    {
        public const string SfxSourceTag = "SfxAudioSource";
        
        [SerializeField] private AudioData[] _sounds; //список звуков
        private AudioSource _source;

        public void Play(string id) // передадим идентификатор нашего звука
        {
            foreach (var audioData in _sounds)
            {
                if (audioData.Id != id) continue;  // если айди не равен айди, который мы передали

                if (_source == null)
                    _source = AudioUtils.FindSfxSource();
                
                _source.PlayOneShot(audioData.Clip);
                break;
            }
        }
                                  
        
        [Serializable]
        public class AudioData
        {
            [SerializeField] private string _id;
            [SerializeField] private AudioClip _clip; //это тот клип, который мы передаём в аудио сорс
            
            //сделаем открытыми через ридонли проперти
            public string Id => _id;
            public AudioClip Clip => _clip; 
        }
    }
}