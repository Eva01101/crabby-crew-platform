﻿using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace PixelCrew.Creatures.Mobs.Boss
{
    public class ChangeLightsComponent: MonoBehaviour
    {
        [SerializeField] private Light2D[] _lights;

        [ColorUsage(true, true)] [SerializeField]
        private Color _color;

        [ContextMenu("Setup")]
        public void SetColor()
        {
            foreach (var light2D in _lights)//пройдёмся по всем источникам освещения
            {
                light2D.color = _color; //установим нужный цвет
            }
        }
    }
}