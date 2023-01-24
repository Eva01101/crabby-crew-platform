using UnityEngine;

namespace PixelCrew.Components.ColliderBased
{
    public class LayerCheck: MonoBehaviour
    {
        [SerializeField] protected LayerMask _layer;
        [SerializeField] protected bool _isTouchingLayer;
        
        public bool IsTouchingLayer => _isTouchingLayer; // чтобы видно IsTouchingLayer в нашем инспекторе
        //но при этом, чтобы мы не могли снаружи на него воздействовать, а только получать через гетер IsTouchingLayer
    }
}