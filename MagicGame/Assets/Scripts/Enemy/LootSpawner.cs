using System;
using Infrastructure.Factory;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _enemyDeath;
        
        private IGameFactory _factory;
        private int _lootMin;
        private int _lootMax;

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
        }

        private void Start()
        {
            _enemyDeath.Happend += SpawnLoot;
        }

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }

        private void SpawnLoot()
        {
            LootPiece loot = _factory.CreateLoot();
            loot.transform.position = transform.position;
            Loot lootItem = GenerateLoot();

            loot.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            Random random = new Random();

            var lootItem = new Loot()
            {
                Value = random.Next(_lootMin, _lootMax)
            };
            return lootItem;
        }
    }

    public class Loot
    {
        public int Value { get; set; }
    }
}