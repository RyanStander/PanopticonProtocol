using System;

namespace Monsters
{
    public static class MonsterEvents
    {
        public static event Action<int> OnMonsterHitByPlayerWeapon;
        public static event Action<string> OnMonsterKill;
        
        public static void MonsterHit(int rewardAmount)
        {
            OnMonsterHitByPlayerWeapon?.Invoke(rewardAmount);
        }

        public static void MonsterKill(string animationName)
        {
            OnMonsterKill?.Invoke(animationName);
        }
    }
}
