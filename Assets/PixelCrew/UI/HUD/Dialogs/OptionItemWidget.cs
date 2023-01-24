using System;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD.Dialogs
{
    public class OptionItemWidget: MonoBehaviour, IItemRenderer<OptionData>
    {
        [SerializeField] private Text _label;
        [SerializeField] private SelectOptions _onSelect;
        
        private OptionData _data;

        public void SetData(OptionData data, int index)
        {
            _data = data;
            _label.text = data.Text;
        }

        public void OnSelect()
        {
            _onSelect.Invoke(_data);//будем вызывать селект, когда будем выбирать конкретную функцию 
        }
        
        [Serializable]
        public class SelectOptions: UnityEvent<OptionData>
        {
            
        }
    }
}