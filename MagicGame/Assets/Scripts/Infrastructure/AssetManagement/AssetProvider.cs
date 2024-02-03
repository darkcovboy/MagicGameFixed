using UnityEngine;
using Zenject;

namespace Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private DiContainer _container;

        [Inject]
        public void Constructor(DiContainer container)
        {
            _container = container;
        }

        public GameObject Instantiate(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return _container.InstantiatePrefab(prefab);
        }

        public GameObject Instantiate(DiContainer container,string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return container.InstantiatePrefab(prefab);
        }

        public GameObject Instantiate(string path, Vector3 at)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            return _container.InstantiatePrefab(prefab, at, Quaternion.identity, null);
        }
    }
}