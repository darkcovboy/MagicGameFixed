using System;
using DefaultNamespace.Enemy;

namespace DefaultNamespace.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
        }
    }
}