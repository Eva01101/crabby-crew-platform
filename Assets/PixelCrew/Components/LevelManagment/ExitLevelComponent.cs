using PixelCrew.Model;
using PixelCrew.UI.LevelsLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelManagment 
{
    public class ExitLevelComponent: MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        
        public void Exit() //переход с уровня на другой уровень
        {
            var session = FindObjectOfType<GameSession>();
            session.Save();
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);//передаём сцену
        }
    }
}