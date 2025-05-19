using System;
using System.Collections;
using Environment;
using GameLogic;
using Spine.Unity;
using UnityEngine;

namespace Monsters
{
    public class MonsterManager : MonoBehaviour
    {
        public DifficultyScalingData DifficultyScalingData;
        public SkeletonAnimation MonsterSkeleton;
        public MeshRenderer MonsterMesh;
        public Vector3 MonsterSpawnOffset = new(0, 0, 0);
        [SerializeField] private MonsterEscapeLogic monsterEscapeLogic;

        private Vector2 idleDurationRange = new(2f, 5f);

        [SerializeField] private float moveSpeed = 5f;
        private float trueSpeed;

        private Coroutine roamingCoroutine;

        public JailCell AssignedJailCell;

        private void OnValidate()
        {
            if (MonsterSkeleton == null)
                MonsterSkeleton = GetComponent<SkeletonAnimation>();

            if (MonsterMesh == null)
                MonsterMesh = GetComponent<MeshRenderer>();

            if (monsterEscapeLogic == null)
                monsterEscapeLogic = GetComponent<MonsterEscapeLogic>();
        }

        private void Start()
        {
            trueSpeed = moveSpeed *
                        MathF.Pow(1f + DifficultyScalingData.SpeedGrowthRate, PersistentData.CurrentShift - 1);
        }

        public void SetData(JailCell jailCell)
        {
            AssignedJailCell = jailCell;
        }

        private void FixedUpdate()
        {
            monsterEscapeLogic.HandleEscape();

            if (!monsterEscapeLogic.HasEscaped) return;
            roamingCoroutine ??= StartCoroutine(Roam());
        }

        private IEnumerator Roam()
        {
            while (true)
            {
                // Pick a random direction
                int direction = UnityEngine.Random.value > 0.5f ? 1 : -1;

                // Walk for a short time (randomized)
                float moveDuration = UnityEngine.Random.Range(0.5f, 2f);
                float moveTimer = 0f;

                if (direction > 0)
                    MonsterSkeleton.skeleton.ScaleX = -1;
                else
                    MonsterSkeleton.skeleton.ScaleX = 1;

                MonsterSkeleton.AnimationState.SetAnimation(0, "walk", true);

                while (moveTimer < moveDuration)
                {
                    transform.Translate(Vector2.right * direction * trueSpeed * Time.deltaTime);
                    moveTimer += Time.deltaTime;
                    yield return null; // wait one frame
                }

                MonsterSkeleton.AnimationState.SetAnimation(0, "idle", true);

                // Wait/idle after moving
                float idleDuration = UnityEngine.Random.Range(idleDurationRange.x, idleDurationRange.y);
                idleDuration = Mathf.Max(idleDurationRange.x,
                    idleDuration * Mathf.Pow(1 - DifficultyScalingData.IdleTimeDecreaseRate,
                        PersistentData.CurrentShift - 1));
                yield return new WaitForSeconds(idleDuration);
            }
        }
    }
}
