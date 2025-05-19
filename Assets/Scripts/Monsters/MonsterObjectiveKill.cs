using System.Collections;
using System.Collections.Generic;
using Environment;
using UnityEngine;

namespace Monsters
{
    public class MonsterObjectiveKill : MonsterObjective
    {
        [SerializeField] private List<Transform> pathTargets;
        private Coroutine objectiveCoroutine;
        private Coroutine goToTargetCoroutine;

        protected override void Start()
        {
            base.Start();

            pathTargets = MonsterManager.MonsterPath.GetPathTargets();
        }

        protected override IEnumerator StartObjective()
        {
            yield return base.StartObjective();

            //compare the y position of the monster and the target, if the monster is lower than the target, remove it from the target list
            for (int i = 0; i < pathTargets.Count; i++)
            {
                if (pathTargets[i].position.y > transform.position.y)
                {
                    pathTargets.RemoveAt(i);
                    i--;
                }
            }

            //if idleDuringObjective is true, start the idleObjective coroutine, otherwise just the Objective coroutine
            if (IdleDuringObjective)
            {
                yield return objectiveCoroutine = StartCoroutine(IdleObjective());
            }
            else
            {
                yield return objectiveCoroutine = StartCoroutine(Objective());
            }

            // Play idle animation
            MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, "idle", true);
        }

        private IEnumerator Objective()
        {
            while (pathTargets.Count > 0)
            {
                // Move towards the target
                Transform target = pathTargets[0];
                pathTargets.RemoveAt(0);

                WalkVisuals(target);

                yield return goToTargetCoroutine = StartCoroutine(GoToTarget(target));
            }
        }

        private IEnumerator IdleObjective()
        {
            while (pathTargets.Count > 0)
            {
                // Play idle animation
                MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, "idle", true);

                // Wait for a random duration
                yield return new WaitForSeconds(Random.Range(IdleDurationRange.x, IdleDurationRange.y));

                // Move towards the target
                Transform target = pathTargets[0];
                pathTargets.RemoveAt(0);

                WalkVisuals(target);

                yield return goToTargetCoroutine = StartCoroutine(GoToTarget(target));
            }
        }

        private IEnumerator GoToTarget(Transform targetTransform)
        {
            // Move towards the target
            while (Vector3.Distance(transform.position, targetTransform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetTransform.position,
                    MonsterManager.TrueSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void WalkVisuals(Transform targetTransform)
        {
            MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, "walk", true);

            //determine direction of the x axis
            int direction = targetTransform.position.x > transform.position.x ? 1 : -1;

            if (direction > 0)
                MonsterManager.MonsterSkeleton.skeleton.ScaleX = -1;
            else
                MonsterManager.MonsterSkeleton.skeleton.ScaleX = 1;
        }

        public override void AbortObjective()
        {
            base.AbortObjective();
            
            if (objectiveCoroutine != null)
            {
                StopCoroutine(objectiveCoroutine);
                objectiveCoroutine = null;
            }
            
            if (goToTargetCoroutine != null)
            {
                StopCoroutine(goToTargetCoroutine);
                goToTargetCoroutine = null;
            }
            
            pathTargets = MonsterManager.MonsterPath.GetPathTargets();
        }
    }
}
