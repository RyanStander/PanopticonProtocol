using System.Collections;
using Spine;
using UnityEngine;

namespace Monsters
{
    public class MonsterWeaknessClone : MonsterWeakness
    {
        protected override IEnumerator HoldFlashlightToStun()
        {
            float timer = 0f;

            while (timer < 2f)
            {
                if (!IsBeingLit)
                {
                    FlashlightHoldCoroutine = null;
                    yield break; // Exit early if flashlight leaves
                }

                timer += Time.deltaTime;
                yield return null;
            }

            FlashlightHoldCoroutine = null;

            if (IsRetreating || !MonsterManager.MonsterEscapeLogic.IsAttemptingToEscape)
                yield break;

            IsRetreating = true;
            MonsterSkeleton.AnimationState.SetAnimation(0, "flashlight", false);
            MonsterSkeleton.AnimationState.AddAnimation(0, "flashlight_down_loop", false, 0);

            TrackEntry entry = MonsterSkeleton.AnimationState.AddAnimation(0, "poof", false, 0);

            MonsterEvents.ScrollingObjectDelete();
            entry.Complete += (_) => { Destroy(gameObject); };
        }
        
        public override void ShotByEmp()
        {
            if (IsVulnerableToEmp)
            {
                IsRetreating = true;
                MonsterSkeleton.AnimationState.SetAnimation(0, "emp_hit", false);
                
                TrackEntry entry = MonsterSkeleton.AnimationState.AddAnimation(0, "poof", false, 0);

                MonsterEvents.ScrollingObjectDelete();
                entry.Complete += (_) => { Destroy(gameObject); };
            }
        }
    }
}
