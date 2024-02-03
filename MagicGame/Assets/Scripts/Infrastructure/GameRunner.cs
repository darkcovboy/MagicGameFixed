using System;
using UnityEngine;

namespace Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootsraper _bootsraperPrefab;
        
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootsraper>();

            if (!bootstrapper)
                Instantiate(_bootsraperPrefab);
        }
    }
}