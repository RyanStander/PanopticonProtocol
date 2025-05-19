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
        [SerializeField] private MonsterManager monsterManager;

        [SerializeField] private bool isVulnerableToLight;
        [SerializeField] private bool isVulnerableToEmp;
        [SerializeField] private float lightStunDuration = 2f;

        private JailCell assignedJailCell;
        private SkeletonAnimation monsterSkeleton;
        private List<Transform> pathTargets;
        public bool IsRetreating;
        
        private Coroutine flashlightHoldCoroutine;
        private bool isBeingLit;


        private void OnValidate()
        {
            if (monsterManager == null)
                monsterManager = GetComponent<MonsterManager>();
        }

        private void Start()
        {
            monsterSkeleton = monsterManager.MonsterSkeleton;
            assignedJailCell = monsterManager.AssignedJailCell;
        }

        #region Flashlight

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isVulnerableToLight && other.CompareTag("Light") && !IsRetreating)
            {
                isBeingLit = true;

                if (flashlightHoldCoroutine == null)
                {
                    flashlightHoldCoroutine = StartCoroutine(HoldFlashlightToStun());
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (isVulnerableToLight && other.CompareTag("Light") && !IsRetreating)
            {
                isBeingLit = true; // Ensure flag is true if still inside
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isVulnerableToLight && other.CompareTag("Light"))
            {
                isBeingLit = false;

                if (flashlightHoldCoroutine != null)
                {
                    StopCoroutine(flashlightHoldCoroutine);
                    flashlightHoldCoroutine = null;
                }
            }
        }
        
        private IEnumerator HoldFlashlightToStun()
        {
            float timer = 0f;

            while (timer < 2f)
            {
                if (!isBeingLit)
                {
                    flashlightHoldCoroutine = null;
                    yield break; // Exit early if flashlight leaves
                }

                timer += Time.deltaTime;
                yield return null;
            }

            flashlightHoldCoroutine = null;

            if (IsRetreating || !monsterManager.MonsterEscapeLogic.IsAttemptingToEscape)
                yield break;

            IsRetreating = true;
            monsterSkeleton.AnimationState.SetAnimation(0, "flashlight", false);
            monsterSkeleton.AnimationState.AddAnimation(0, "flashlight_down_loop", true, 0);

            MonsterEvents.MonsterHit(monsterManager.DifficultyScalingData.DetainedMonsterReward);
            StartCoroutine(RetreatToPrisonCell(lightStunDuration));
        }

        #endregion
        
        public void ShotByEmp()
        {
            if (IsRetreating)
                return;

            if (!monsterManager.MonsterEscapeLogic.IsAttemptingToEscape)
                return;

            if (isVulnerableToEmp)
            {
                IsRetreating = true;
                monsterSkeleton.AnimationState.SetAnimation(0, "emp_hit", false);
                MonsterEvents.MonsterHit(monsterManager.DifficultyScalingData.DetainedMonsterReward);
                StartCoroutine(RetreatToPrisonCell(monsterSkeleton.skeleton.Data.FindAnimation("emp_hit").Duration));
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
                       assignedJailCell.transform.position + monsterManager.MonsterSpawnOffset) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    assignedJailCell.transform.position + monsterManager.MonsterSpawnOffset,
                    monsterManager.TrueSpeed * Time.deltaTime);
                yield return null;
            }

            // Play the idle animation
            monsterSkeleton.AnimationState.SetAnimation(0, "idle", true);
            monsterManager.MonsterMesh.sortingOrder = monsterManager.MonsterEscapeLogic.JailedLayer;
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
                    monsterManager.TrueSpeed * Time.deltaTime);
                yield return null;
            }
        }

        private void GetViableTargets()
        {
            pathTargets = monsterManager.MonsterPath.GetPathTargets();

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
            monsterSkeleton.AnimationState.SetAnimation(0, "walk", true);

            //determine direction of the x axis
            int direction = targetTransform.position.x > transform.position.x ? 1 : -1;

            if (direction > 0)
                monsterSkeleton.skeleton.ScaleX = -1;
            else
                monsterSkeleton.skeleton.ScaleX = 1;
        }
    }
}
