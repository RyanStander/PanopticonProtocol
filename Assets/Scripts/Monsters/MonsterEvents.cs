using System;

namespace Monsters
{
    public static class MonsterEvents
    {
        public static event Action<int> OnMonsterHitByPlayerWeapon;
        public static event Action<string> OnMonsterKill;
        public static event Action<ScrollingObject> OnNewScrollingObject; 
        public static event Action OnScrollingObjectDelete;
        
        public static void MonsterHit(int rewardAmount)
        {
            OnMonsterHitByPlayerWeapon?.Invoke(rewardAmount);
        }

        public static void MonsterKill(string animationName)
        {
            OnMonsterKill?.Invoke(animationName);
        }
        
        public static void NewScrollingObject(ScrollingObject scrollingObject)
        {
            OnNewScrollingObject?.Invoke(scrollingObject);
        }
        
        public static void ScrollingObjectDelete()
        {
            OnScrollingObjectDelete?.Invoke();
        }
    }
}
