using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Repository;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposable;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Perks
{
    public class ManagePerksWindow: AnimatedWindow
    {
        [SerializeField] private Transform _container; //ссылка на трансформ контента
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private Text _infoText; //описание
        [SerializeField] private ItemWidget _price;

        private PredefinedDataGroup<PerkDef, PerkWidget> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;

        protected override void Start() //на старте начнём всё создавать
        {
            base.Start();

            _dataGroup = new PredefinedDataGroup<PerkDef, PerkWidget>(_container);
            _session = FindObjectOfType<GameSession>();
            
            _trash.Retain(_session.PerksModel.Subscribe(OnPerksChanged));
            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));//Subscribe - добавляет lisener 
            //и dispos...
            _trash.Retain(_useButton.onClick.Subscribe(OnUse));

            OnPerksChanged();
        }

        private void OnPerksChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Perks.All);

            var selected = _session.PerksModel.InterfaceSelection.Value; //получим текущий выбранный предмет
            
            _useButton.gameObject.SetActive(_session.PerksModel.IsUnlocked(selected)); //_useButton, если 
            //разблокирован выбранный предмет
            _useButton.interactable = _session.PerksModel.Used != selected;
            
            _buyButton.gameObject.SetActive(!_session.PerksModel.IsUnlocked(selected));//если не разблокирован предмет. значит можем купить
            _buyButton.interactable = _session.PerksModel.CanBuy(selected); //если можем его купить

            var def = DefsFacade.I.Perks.Get(selected);
            _price.SetData(def.Price);

            _infoText.text = LocalizationManager.I.Localize(def.Info);
        }

        private void OnUse()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.SelectPerk(selected);
        }

        private void OnBuy()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value; 
            _session.PerksModel.Unlock(selected); //по клику на кнопку, будем анлочить текущ выбран предмет
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}