using System;

namespace DefaultNamespace.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public State HeroState;
        public WorldData WorldData;
        public Stats HeroStats;
        public KillData KillData;
        public UncollectedLoot UncollectedLoot;

        public PlayerProgress(string inititalLevel)
        {
            WorldData = new WorldData(inititalLevel);
            HeroState = new State();
            HeroStats = new Stats();
            KillData = new KillData();
            UncollectedLoot = new UncollectedLoot();;
        }

    }
}