using System.Collections;
using Spine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Monsters
{
    public class MonsterObjectiveBait : MonsterObjective
    {
        [SerializeField] private float timeBetweenBaits;
        [SerializeField] private string idleToBreakAnimationName = "idle_To_Break";
        [SerializeField] private string breakLoopAnimationName = "break_Loop";
        [SerializeField] private string breakOpenAnimationName = "break_Open";
        [SerializeField] private float breakOpenAnimationDuration = 2f;
        private Coroutine breakDoorCoroutine;

        [SerializeField] private Light2D anglerLight;
        [SerializeField] private float targetFalloff = 6f;
        private float brightenBulbDuration = 5f;
        private float originalFalloff;
        private Coroutine shineBulbCoroutine;

        protected override void OnValidate()
        {
            base.OnValidate();

            if (anglerLight == null)
                anglerLight = GetComponentInChildren<Light2D>();
        }

        protected override void Start()
        {
            base.Start();
            originalFalloff = anglerLight.pointLightOuterRadius;
        }

        protected override IEnumerator StartObjective()
        {
            //randomly select one of his distractions every set amount of time
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenBaits);

                int randomBait = Random.Range(0, 2);

                switch (randomBait)
                {
                    //Break dour distraction
                    case 0:
                        breakDoorCoroutine = StartCoroutine(BreakDoor());
                        yield return breakDoorCoroutine;
                        break;
                    case 1:
                        shineBulbCoroutine = StartCoroutine(ShineBulb());
                        yield return shineBulbCoroutine;
                        break;
                    default:
                        break;
                }
            }

            yield return null;
        }

        private IEnumerator BreakDoor()
        {
            MonsterManager.AssignedJailCell.Idle();
            
            TrackEntry entry =
                MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, idleToBreakAnimationName, false);

            entry.Complete += (_) => { MonsterManager.AssignedJailCell.ShakeBaiter(); };
            MonsterManager.MonsterSkeleton.AnimationState.AddAnimation(0, breakLoopAnimationName, true, 0);

            yield return new WaitForSeconds(breakOpenAnimationDuration);

            MonsterManager.AssignedJailCell.BreakDoorBaiter();
            MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, breakOpenAnimationName, false);

            yield return new WaitForSeconds(2);
            MonsterManager.AssignedJailCell.Idle();
        }

        private IEnumerator ShineBulb()
        {
            MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, breakLoopAnimationName, gameObject);

            float timer = 0f;
            while (timer < brightenBulbDuration)
            {
                timer += Time.deltaTime;
                anglerLight.falloffIntensity =
                    Mathf.Lerp(originalFalloff, targetFalloff, timer / brightenBulbDuration);
                yield return null;
            }

            //Reset the light
            timer = 0f;
            while (timer < brightenBulbDuration)
            {
                timer += Time.deltaTime;
                anglerLight.shapeLightFalloffSize =
                    Mathf.Lerp(targetFalloff, originalFalloff, timer / brightenBulbDuration);
                yield return null;
            }

            MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, IdleAnimationName, true);
        }
    }
}
