using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components.Audio
{
    public class PlaySfxSound: MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        private AudioSource _source; //через сорс будем проигрывать адиоклип

        public void Play()
        {
            if (_source == null) //если сорса нет
                _source = AudioUtils.FindSfxSource();
            
            _source.PlayOneShot(_clip);
        }
    }
}