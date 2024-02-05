using System;
using System.Collections;
using System.Linq;
using Data;
using DefaultNamespace.Data;
using Infrastructure.Factory;
using Infrastructure.Services.PersistentProgress;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.Enemy
{
    public class LootPiece : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private GameObject _skull;
        [SerializeField] private GameObject _pickUpFxPrefab;
        [SerializeField] private AudioSource _soundEffect;
        [SerializeField] private TMP_Text _lootText;
        [SerializeField] private GameObject _pickupPopup;
        
        private string _id;
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;
        private IGameFactory _gameFactory;

        public void Construct(WorldData worldData, IGameFactory gameFactory)
        {
            _worldData = worldData;
            _gameFactory = gameFactory;
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

        private void PlayPickupFx()
        {
            if (_pickUpFxPrefab != null)
            {
                Instantiate(_pickUpFxPrefab, transform.position, Quaternion.identity);
            }
        }

        private void ShowText()
        {
            _lootText.text = $"{_loot.Value}";
            _pickupPopup.SetActive(true);
        }

        private IEnumerator StartDestroyTimer()
        {
            _gameFactory.ProgressReaders.Remove(this);
            _gameFactory.ProgressWriters.Remove(this);
            
            yield return new WaitForSeconds(1.5f);
            
            Destroy(gameObject);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            LootSaveData lootSaveData = progress.UncollectedLoot.UntakedLoot.FirstOrDefault(x => x.Id == _id);

            if (lootSaveData != null)
            {
                progress.UncollectedLoot.UntakedLoot.Remove(lootSaveData);
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (gameObject != null)
            {
                LootSaveData lootSaveData = new LootSaveData(_id, _loot, transform.position.AsVectorData());
                progress.UncollectedLoot.UntakedLoot.Add(lootSaveData);
            }
            
        }
    }
}