using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.LevelsLoader
{
    public class LevelLoader: MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _transitionTime;
        
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            InitLoader();
        }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private static void InitLoader()
        {
            SceneManager.LoadScene("LevelLoader", LoadSceneMode.Additive); //загружать ничего не нужно, Additive - не перезатираем текущ сцену
        }

        public void LoadLevel(string sceneName) //сцена, кот хотим загрузить
        {
            StartCoroutine(StartAnimation(sceneName));
        }

        private IEnumerator StartAnimation(string sceneName)
        {
          _animator.SetBool(Enabled, true);
          yield return new WaitForSeconds(_transitionTime);
          SceneManager.LoadScene(sceneName);
          _animator.SetBool(Enabled, false);
        }
    }
}