using Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(DiContainer container, string path);
        GameObject Instantiate(string path, Vector3 at);
    }
}