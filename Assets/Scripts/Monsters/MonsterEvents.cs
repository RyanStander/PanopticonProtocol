using System;

namespace Monsters
{
    public static class MonsterEvents
    {
        public static event Action<int> OnMonsterHitByPlayerWeapon;

        public static void MonsterHit(int rewardAmount)
        {
            OnMonsterHitByPlayerWeapon?.Invoke(rewardAmount);
        }
    }
}
