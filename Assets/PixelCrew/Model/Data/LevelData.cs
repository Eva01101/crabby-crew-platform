using System;
using System.Collections.Generic;
using System.Linq;
using PixelCrew.Model.Definitions.Player;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] private List<LevelProgress> _progress;

        public int GetLevel(StatId id)
        {
            
            foreach (var levelProgress in _progress)
            {
                if (levelProgress.Id == id)
                {
                    return levelProgress.Level;
                }
            }

            return 0;
            
            //var progress = _progress.FirstOrDefault(x => x.Id == id);
            //если нашли с такой айди элемент, возвр Level, если нет, вернём 0
            //return progress?.Level ?? 0;
            //можно написать так: 
            //if(progress == null)
            //return 0;
            //return progress.Level;
        }

        public void LevelUp(StatId id) //будет добавлять элем в массив, если есть элем, будет его LevelUp
        {
            var progress = _progress.FirstOrDefault(x => x.Id == id);
            if (progress == null)
                _progress.Add(new LevelProgress(id, 1));//прогресс начинается с 1, чтобы на старте у героя были значения
            else
                progress.Level++;

        }
    }

    [Serializable]
    public class LevelProgress
    {
        public StatId Id;
        public int Level; //текущий левел нашего героя

        public LevelProgress(StatId id, int level)
        {
            Id = id;
            Level = level;
        }
    }
}