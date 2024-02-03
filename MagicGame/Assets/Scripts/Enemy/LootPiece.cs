using System;
using System.Collections;
using DefaultNamespace.Data;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        [SerializeField] private GameObject _skull;
        [SerializeField] private GameObject _pickUpFxPrefab;
        [SerializeField] private AudioSource _soundEffect;
        [SerializeField] private TMP_Text _lootText;
        [SerializeField] private GameObject _pickupPopup;
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other) => Pickup();

        private void Pickup()
        {
            if(_picked)
                return;
            
            _picked = true;
            
            UpdateWorldData();
            HideSkull();
            PlayPickupFx();
            ShowText();
            PlaySound();

            StartCoroutine(StartDestroyTimer());
        }

        private void PlaySound() => _soundEffect.Play();

        private void UpdateWorldData() => _worldData.LootData.Collect(_loot);

        private void HideSkull() => _skull.SetActive(false);

        private void PlayPickupFx() => Instantiate(_pickUpFxPrefab, transform.position, Quaternion.identity);

        private void ShowText()
        {
            _lootText.text = $"{_loot.Value}";
            _pickupPopup.SetActive(true);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(1.5f);
            
            Destroy(gameObject);
        }
    }
}