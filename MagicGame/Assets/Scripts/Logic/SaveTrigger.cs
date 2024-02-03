using System;
using Infrastructure.Services.PersistentProgress;
using Infrastructure.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.Logic
{
    public class SaveTrigger : MonoBehaviour
    {
        [SerializeField] private BoxCollider _boxCollider;
        
        private ISavedLoadService _saveLoadProgress;

        [Inject]
        public void Constructor(ISavedLoadService savedLoadService)
        {
            _saveLoadProgress = savedLoadService;
        }

        private void OnTriggerEnter(Collider other)
        {
            _saveLoadProgress.SaveProgress();
            Debug.Log("Progress Saved.");
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if(!_boxCollider)
                return;
            
            Gizmos.color = new Color(30,200,30,130);
            Gizmos.DrawCube(transform.position + _boxCollider.center, _boxCollider.size);
        }
    }
}