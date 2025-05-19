using System;
using System.Collections;
using GameLogic;
using UnityEngine;

namespace Monsters
{
    public class MonsterObjective : MonoBehaviour
    {
        [SerializeField] protected MonsterManager MonsterManager;
        [SerializeField] protected bool RoamBeforeObjective = true;
        [SerializeField] protected float RoamTime = 15f;
        [SerializeField] protected bool IdleDuringObjective = true;
        [SerializeField] protected float RandomIdleTime = 5f;
        protected Vector2 IdleDurationRange = new(2f, 5f);

        private Coroutine objectiveCoroutine;
        private Coroutine roamCoroutine;

        private void OnValidate()
        {
            if (MonsterManager == null)
                MonsterManager = GetComponent<MonsterManager>();
        }

        protected virtual void Start()
        {
        }

        public void HandleObjective()
        {
            if (!MonsterManager.MonsterEscapeLogic.HasEscaped || MonsterManager.MonsterWeakness.IsRetreating) return;
            objectiveCoroutine ??= StartCoroutine(StartObjective());
        }

        protected virtual IEnumerator StartObjective()
        {
            if (RoamBeforeObjective)
            {
                yield return roamCoroutine = StartCoroutine(Roam(RoamTime));
            }
        }

        private IEnumerator Roam(float duration)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;

                // Pick a random direction
                int direction = UnityEngine.Random.value > 0.5f ? 1 : -1;

                // Walk for a short time (randomized)
                float moveDuration = UnityEngine.Random.Range(0.5f, 2f);
                float moveTimer = 0f;

                if (direction > 0)
                    MonsterManager.MonsterSkeleton.skeleton.ScaleX = -1;
                else
                    MonsterManager.MonsterSkeleton.skeleton.ScaleX = 1;

                MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, "walk", true);

                while (moveTimer < moveDuration)
                {
                    transform.Translate(Vector2.right * direction * MonsterManager.TrueSpeed * Time.deltaTime);
                    moveTimer += Time.deltaTime;
                    yield return null; // wait one frame
                }

                MonsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, "idle", true);

                // Wait/idle after moving
                float idleDuration = UnityEngine.Random.Range(IdleDurationRange.x, IdleDurationRange.y);
                idleDuration = Mathf.Max(IdleDurationRange.x,
                    idleDuration * Mathf.Pow(1 - MonsterManager.DifficultyScalingData.IdleTimeDecreaseRate,
                        PersistentData.CurrentShift - 1));
                yield return new WaitForSeconds(idleDuration);
            }
        }

        public virtual void AbortObjective()
        {
            if (objectiveCoroutine != null)
            {
                StopCoroutine(objectiveCoroutine);
                objectiveCoroutine = null;
            }
        }
    }
}
