using System;
using System.Collections.Generic;

namespace DefaultNamespace.Data
{
    [Serializable]
    public class UncollectedLoot
    {
        public List<LootSaveData> UntakedLoot = new List<LootSaveData>();
    }
}