using System;
using System.Collections;
using System.Collections.Generic;
using Environment;
using Spine.Unity;
using UnityEngine;

namespace Monsters
{
    public class MonsterWeakness : MonoBehaviour
    {
        [SerializeField] protected MonsterManager MonsterManager;

        [SerializeField] private bool isVulnerableToLight;
        [SerializeField] protected bool IsVulnerableToEmp;
        [SerializeField] private float lightStunDuration = 2f;
        [SerializeField] private GameObject flashlightHitEffect;

        private JailCell assignedJailCell;
        protected SkeletonAnimation MonsterSkeleton;
        private List<Transform> pathTargets;
        public bool IsRetreating;
        
        protected Coroutine FlashlightHoldCoroutine;
        protected bool IsBeingLit;


        private void OnValidate()
        {
            if (MonsterManager == null)
                MonsterManager = GetComponent<MonsterManager>();
        }

        private void Start()
        {
            MonsterSkeleton = MonsterManager.MonsterSkeleton;
            assignedJailCell = MonsterManager.AssignedJailCell;
        }

        #region Flashlight

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isVulnerableToLight && other.CompareTag("Light") && !IsRetreating)
            {
                IsBeingLit = true;

                if (FlashlightHoldCoroutine == null)
                {
                    FlashlightHoldCoroutine = StartCoroutine(HoldFlashlightToStun());
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (isVulnerableToLight && other.CompareTag("Light") && !IsRetreating)
            {
                IsBeingLit = true; // Ensure flag is true if still inside
                if (flashlightHitEffect != null)
                {
                    flashlightHitEffect.SetActive(true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isVulnerableToLight && other.CompareTag("Light"))
            {
                IsBeingLit = false;

                if (FlashlightHoldCoroutine != null)
                {
                    StopCoroutine(FlashlightHoldCoroutine);
                    FlashlightHoldCoroutine = null;
                }
                
                if (flashlightHitEffect != null)
                {
                    flashlightHitEffect.SetActive(true);
                }
            }
        }
        
        protected virtual IEnumerator HoldFlashlightToStun()
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
            MonsterSkeleton.AnimationState.AddAnimation(0, "flashlight_down_loop", true, 0);

            MonsterEvents.MonsterHit(MonsterManager.DifficultyScalingData.DetainedMonsterReward);
            StartCoroutine(RetreatToPrisonCell(lightStunDuration));
        }

        #endregion
        
        public virtual void ShotByEmp()
        {
            if (IsRetreating)
                return;

            if (!MonsterManager.MonsterEscapeLogic.IsAttemptingToEscape)
                return;

            if (IsVulnerableToEmp)
            {
                IsRetreating = true;
                MonsterSkeleton.AnimationState.SetAnimation(0, "emp_hit", false);
                MonsterEvents.MonsterHit(MonsterManager.DifficultyScalingData.DetainedMonsterReward);
                StartCoroutine(RetreatToPrisonCell(MonsterSkeleton.skeleton.Data.FindAnimation("emp_hit").Duration));
            }
        }

        private IEnumerator RetreatToPrisonCell(float stunDuration)
        {
            yield return new WaitForSeconds(stunDuration);

            GetViableTargets();

            while (pathTargets.Count > 0)
            {
                // Move towards the target
                Transform target = pathTargets[0];
                pathTargets.RemoveAt(0);

                WalkVisuals(target);

                yield return StartCoroutine(GoToTarget(target));
            }

            WalkVisuals(assignedJailCell.transform);
            // Move the monster back to its assigned jail cell
            while (Vector3.Distance(transform.position,
                       assignedJailCell.transform.position + MonsterManager.MonsterSpawnOffset) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    assignedJailCell.transform.position + MonsterManager.MonsterSpawnOffset,
                    MonsterManager.TrueSpeed * Time.deltaTime);
                yield return null;
            }

            // Play the idle animation
            MonsterSkeleton.AnimationState.SetAnimation(0, "idle", true);
            MonsterManager.MonsterMesh.sortingOrder = MonsterManager.MonsterEscapeLogic.JailedLayer;
            //TODO: This is where we put an unbreak
            //assignedJailCell.PlayAnimation("M1_CellDoor_break");
            IsRetreating = false;
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

        private void GetViableTargets()
        {
            pathTargets = MonsterManager.MonsterPath.GetPathTargets();

            float jailY = assignedJailCell.transform.position.y;
            float monsterY = transform.position.y;

            float verticalDistance = Mathf.Abs(jailY - monsterY);

            // If jail cell is very close vertically, skip pathing
            if (verticalDistance < 1f)
            {
                pathTargets.Clear();
                return;
            }

            if (jailY > monsterY)
            {
                // Going up: Keep targets between monster and jail
                for (int i = 0; i < pathTargets.Count; i++)
                {
                    float y = pathTargets[i].position.y;
                    if (y < monsterY || y > jailY)
                    {
                        pathTargets.RemoveAt(i);
                        i--;
                    }
                }

                // Reverse so we move upward in order
                pathTargets.Reverse();
            }
            else
            {
                // Going down: Keep targets between monster and jail
                for (int i = 0; i < pathTargets.Count; i++)
                {
                    float y = pathTargets[i].position.y;
                    if (y > monsterY || y < jailY)
                    {
                        pathTargets.RemoveAt(i);
                        i--;
                    }
                }

                // No reverse needed for downward path
            }
        }


        private void WalkVisuals(Transform targetTransform)
        {
            MonsterSkeleton.AnimationState.SetAnimation(0, "walk", true);

            //determine direction of the x axis
            int direction = targetTransform.position.x > transform.position.x ? 1 : -1;

            if (direction > 0)
                MonsterSkeleton.skeleton.ScaleX = -1;
            else
                MonsterSkeleton.skeleton.ScaleX = 1;
        }
    }
}
