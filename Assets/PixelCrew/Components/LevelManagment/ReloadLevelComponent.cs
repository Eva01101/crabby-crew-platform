using System.Collections;
using System.Collections.Generic;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelManagment 
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>(); //получаем компонент сессии
             //используем дестрой, чтобы удалил именно сам геймобжект, а не только компонент
            //когда мы снова загрузим наш уровень, мы возьмём дефолтное состояние
            session.LoadLastSave();
            
            var scene = SceneManager.GetActiveScene(); //позволяет манипулировать сценами
            SceneManager.LoadScene(scene.name); //перезагружаем эту же сцену по имени name
        }
    }
}
