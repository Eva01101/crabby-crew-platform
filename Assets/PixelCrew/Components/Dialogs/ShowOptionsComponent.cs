using PixelCrew.UI.HUD.Dialogs;
using UnityEngine;

namespace PixelCrew.Components.Dialogs
{
    public class ShowOptionsComponent: MonoBehaviour
    {
        [SerializeField] private OptionDialogData _data;
        private OptionDialogController _dialogBox;

        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<OptionDialogController>(); //если диалог бокс null - будем его искать
           
            _dialogBox.Show(_data);
        }
    }
}