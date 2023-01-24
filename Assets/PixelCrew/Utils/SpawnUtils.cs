using UnityEngine;

namespace PixelCrew.Utils
{
    public class SpawnUtils
    {
        private const string ContainerName = "###SPAWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, string containerName = ContainerName)
        {
            var container = GameObject.Find(containerName);
            if (container == null)
                container = new GameObject(containerName); //если контейнера нет, создадим
            return Object.Instantiate(prefab, position, Quaternion.identity, container.transform);
        }
    }
}