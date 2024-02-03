using System;

namespace DefaultNamespace.Data
{
    [Serializable]
    public class WorldData
    {
        public PositionOnLevel PositionOnLevel;
        public LootData LootData;

        public WorldData(string inititalLevel)
        {
            PositionOnLevel = new PositionOnLevel(inititalLevel);
        }
    }
}