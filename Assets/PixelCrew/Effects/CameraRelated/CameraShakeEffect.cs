using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace PixelCrew.Effects.CameraRelated
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    
    public class CameraShakeEffect: MonoBehaviour
    {
        [SerializeField] private float _animationTime = 0.3f; //время тряски камеры
        [SerializeField] private float _intensity = 3f;//интенсивность тряски
        
        private CinemachineBasicMultiChannelPerlin _camersnNoise;
        
        private Coroutine _coroutine;

        private void Awake()
        {
            var virtualCamera = GetComponent<CinemachineVirtualCamera>();
           _camersnNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake()
        {
            if (_coroutine != null)
                StopAnimation();
            _coroutine = StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            _camersnNoise.m_FrequencyGain = _intensity;
            yield return new WaitForSeconds(_animationTime);
            StopAnimation();

        }
        
        private void StopAnimation()
        {
            _camersnNoise.m_FrequencyGain = 0f;
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}