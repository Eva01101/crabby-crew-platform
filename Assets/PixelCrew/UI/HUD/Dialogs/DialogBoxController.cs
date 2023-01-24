using System;
using System.Collections;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.UI.HUD.Dialogs
{
    public class DialogBoxController: MonoBehaviour
    {
        
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space] [SerializeField] private float _textSpeed = 0.09f; //скорость текста

        [Header("Sounds")] [SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;
        
        [Space][SerializeField] protected DialogContent _content;
        
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private DialogData _data;
        private int _currentSentence; //шаги
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;
        private UnityEvent _onComplete;

        protected Sentence CurrentSentence => _data.Sentences[_currentSentence];

        private void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource(); //получим звуки
        }

        public void ShowDialog(DialogData data, UnityEvent onComplete)
        {
            _onComplete = onComplete;
            _data = data;
            _currentSentence = 0; //при показе нового диалога будем занулять
            CurrentContent.Text.text = string.Empty; //при показе диалога будет стирать старый текст 
            
            _container.SetActive(true);//всё отображение будет в контейнере
            _sfxSource.PlayOneShot(_open); 
            _animator.SetBool(IsOpen, true);
        }
        private IEnumerator TypeDialogText()
        {
            CurrentContent.Text.text = string.Empty; //перед каждым тайпингом занулим текст
            var sentence = CurrentSentence; //выберем текущ строчку, кот нужно отобраз
            CurrentContent.TrySetIcon(sentence.Icon);

            foreach (var letter in sentence.Valued)
            {
                CurrentContent.Text.text += letter; //добавляем букву к общему тексту 
                _sfxSource.PlayOneShot(_typing); //проигрываем звук тайпинга
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        protected virtual DialogContent CurrentContent => _content;

        public void OnSkip()
        {
            if(_typingRoutine == null) return; // если у нас не происхдит анимация, мы ничего не будем делать

            StopTypeAnimation();

            CurrentContent.Text.text = _data.Sentences[_currentSentence].Valued; //остановили анимация и показываем весь текщий текст
        }

        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentence++; //увеличить, чтобы мы взяли следущ сентенцию

            var isDialogCompleted = _currentSentence >= _data.Sentences.Length;
            //если текущ индекс диалога >= количеству фраз всего
            if (isDialogCompleted)
            {
                HideDialogBox();
                _onComplete?.Invoke();
            }
            else
            {
                OnStartDialogAnimation();//иначе стартуем следущ фразу
            }
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close); //проиграть закрывающий звук
        }

        private void StopTypeAnimation()
        {
            if(_typingRoutine != null) //если у нас анимация есть
                StopCoroutine(_typingRoutine); //мы её остановим
            _typingRoutine = null; 
        }

        protected virtual void OnStartDialogAnimation() //когда начать анимацию печатания   
        {
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        private void OnCloseAnimationComplete() //вызыв, когда заверш анимация закрытия диалога
        {
            
        }
    }
}