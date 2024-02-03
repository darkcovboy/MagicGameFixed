using Cinemachine;
using UnityEngine;
using Zenject;

namespace DefaultNamespace.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _following;

        private void Start()
        {
            gameObject.GetComponent<CinemachineVirtualCamera>().Follow = _following;
        }
        public void Follow(GameObject following) => _following = following.transform;
    }
}