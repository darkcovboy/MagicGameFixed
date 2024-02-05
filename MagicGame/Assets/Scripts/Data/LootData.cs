using System;
using System.Collections.Generic;
using DefaultNamespace.Enemy;

namespace DefaultNamespace.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;
        public Action Changed;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            Changed?.Invoke();
        }
    }
}