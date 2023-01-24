using PixelCrew.Components.Audio;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelCrew.UI.Widgets
{
    public class ButtonSound: MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private AudioClip _audioClip; //будем проигрывать этот звук

        private AudioSource _source;
        
        public void OnPointerClick(PointerEventData eventData) //звуки кнопок
        {
            if (_source == null)
                _source = AudioUtils.FindSfxSource();
            
            _source.PlayOneShot(_audioClip);
        }
    }
}