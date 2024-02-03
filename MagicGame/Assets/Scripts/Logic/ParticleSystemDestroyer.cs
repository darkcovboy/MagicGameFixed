
using System.Collections;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemDestroyer : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        
        private void Awake()
        {
            _particleSystem = gameObject.GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            StartCoroutine(DestroyTimer(_particleSystem.main.duration));
        }

        private IEnumerator DestroyTimer(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            Destroy(gameObject);
        }
    }
}