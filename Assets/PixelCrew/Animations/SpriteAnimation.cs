using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Animations 
{
    [RequireComponent(typeof(SpriteRenderer))] // когда мы добавляем sprite animation в платформере, у нас сразу добавляется автоматически 
    //sprite render, из-за того, что мы добавили RequireComponent

    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] [Range(1, 30)] private int _frameRate = 10; //кадр , 1 - мин, 30 - макс
        [SerializeField] private UnityEvent<string> _onComplete; // вызывается по окончанию анимации, если анимация будет без лупа
        [SerializeField] private AnimationClip[] _clips;


        private SpriteRenderer _renderer; // необходимо получить ссылку на SpriteRenderer, где будет проигрываться анимация

        private float _secPerFrame; // сколько раз в секунду будет сменяться анимация, сколько сек уходит на показ одного спрайта
        private float _nextFrameTime; // время, когда нам нужно обновить наш спрайт
        private int _currentFrame;
        private bool _isPlaying = true;

        private int _currentClip;


        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secPerFrame = 1f / _frameRate; // из 1 секунды на 1 кадр будет переменная _secondsPerFrame, рассчитываем, сколько у нас длиться один кадр времени

            StartAnimation();
        }

        private void OnBecameVisible() // вызывается, когда объект становится видимым камерой
        {
            enabled = _isPlaying;
        }

        private void OnBecameInvisible() // когда исчезаем из вида, клипы перестаём включать, update = false 
        {
            enabled = false;
        }

        public void SetClip(string clipName)
        {
            for (var i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].Name == clipName)
                {
                    _currentClip = i;
                    StartAnimation();
                    return;
                }

            }

            enabled = _isPlaying = false;
        }

        private void StartAnimation()
        {
            _nextFrameTime = Time.time;
            enabled = _isPlaying = true;
            _currentFrame = 0;
        }

        private void OnEnable()
        {
            _nextFrameTime = Time.time; 
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time) return;
            var clip = _clips[_currentClip]; 
            if (_currentFrame >= clip.Sprites.Length)
            {
                if (clip.Loop) //если у нас клип циклится
                {
                    _currentFrame = 0; // меняем на 0 и снова запускаем
                }
                else //иначе переходим на след стейт
                {
                    enabled = _isPlaying = clip.AllowNextClip; //выключаем
                    clip.OnComplete?.Invoke();
                    _onComplete?.Invoke(clip.Name);
                    if (clip.AllowNextClip)
                    {
                        _currentFrame = 0;
                        _currentClip = (int) Mathf.Repeat(_currentClip + 1, _clips.Length);
                    }
                }

                return;
            }

            _renderer.sprite = clip.Sprites[_currentFrame]; //меняем на спрайт из массива

            _nextFrameTime += _secPerFrame;
            _currentFrame++;
        }
    }

    [Serializable]

    public class AnimationClip
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _allowNextClip; //можно ли перейти на следующий клип
        [SerializeField] private UnityEvent _onComplete;
        
        public string Name => _name;
        public Sprite[] Sprites => _sprites;
        public bool Loop => _loop;
        public bool AllowNextClip => _allowNextClip;
        public UnityEvent OnComplete => _onComplete; 

    }
}
