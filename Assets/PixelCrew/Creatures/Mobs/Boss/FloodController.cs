using System.Collections;
using UnityEngine;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class FloodController: MonoBehaviour
    {
        [SerializeField] private Animator _floodAnimator;//вкл\выкл
        [SerializeField] private float _floodTime; //сколько длится потоп
        
        private static readonly int IsFlooding = Animator.StringToHash("IsFlooding");

        private Coroutine _coroutine;

        public void StartFlooding()
        {
            if(_coroutine != null) return;
            _coroutine = StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _floodAnimator.SetBool(IsFlooding, true); //аниматор поднимает анимацию
            yield return new WaitForSeconds(_floodTime);
            _floodAnimator.SetBool(IsFlooding, false);//опускает анимацию
            _coroutine = null;
        }
    }
}