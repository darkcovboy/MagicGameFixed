using System;
using DefaultNamespace.Enemy;

namespace DefaultNamespace.Data
{
    [Serializable]
    public class LootSaveData
    {
        public string Id;
        public Loot Loot;
        public Vector3Data Vector3;

        public LootSaveData(string id, Loot loot, Vector3Data vector3)
        {
            Id = id;
            Loot = loot;
            Vector3 = vector3;
        }
    }
}