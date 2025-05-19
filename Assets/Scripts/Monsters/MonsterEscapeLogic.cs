using System;
using GameLogic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Monsters
{
    public class MonsterEscapeLogic : MonoBehaviour
    {
        [SerializeField] private string cellDoorShakeAnimation = "M1_CellDoor_shake";
        [SerializeField] private string cellDoorBreakAnimation = "M1_CellDoor_break";
        [SerializeField] private MonsterManager monsterManager;
        public int JailedLayer = 1;
        [SerializeField] private int escapedLayer = 5;
        [SerializeField] private float baseTime = 5f;
        [SerializeField] private float randomJailTime = 5f;
        private float jailTimeStamp;
        [SerializeField] private float randomEscapeTime = 5f;
        private float escapeTimeStamp;

        public bool IsAttemptingToEscape = false;
        public bool HasEscaped;

        private MeshRenderer monsterMesh;

        private DifficultyScalingData difficultyScalingData;

        private void OnValidate()
        {
            if (monsterManager == null)
                monsterManager = GetComponent<MonsterManager>();
        }

        private void Start()
        {
            // Set the monster to the jailed layer
            difficultyScalingData = monsterManager.DifficultyScalingData;
            monsterMesh = monsterManager.MonsterMesh;
            monsterMesh.sortingOrder = JailedLayer;
            jailTimeStamp = Time.time + GetEscapeTime(randomJailTime);
        }

        public void HandleEscape()
        {
            if (HasEscaped || monsterManager.MonsterWeakness.IsRetreating)
                return;

            if (Time.time > jailTimeStamp && !IsAttemptingToEscape)
            {
                if (monsterManager.AssignedJailCell.IsSealed || monsterManager.MonsterWeakness.IsRetreating)
                {
                    jailTimeStamp = Time.time + GetEscapeTime(randomJailTime);
                    return;
                }

                IsAttemptingToEscape = true;
                TrackEntry entry =
                    monsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, "idle_to_break", false);
                monsterManager.MonsterSkeleton.AnimationState.AddAnimation(0, "break_loop", true, 0);

                entry.Complete += (_) =>
                {
                    monsterManager.AssignedJailCell.PlayAnimation(cellDoorShakeAnimation, true);
                };

                escapeTimeStamp = Time.time + GetEscapeTime(randomEscapeTime);
            }

            if (IsAttemptingToEscape && Time.time > escapeTimeStamp && !HasEscaped)
            {
                if (monsterManager.AssignedJailCell.IsSealed || monsterManager.MonsterWeakness.IsRetreating)
                {
                    IsAttemptingToEscape = false;
                    jailTimeStamp = Time.time + GetEscapeTime(randomJailTime);
                    return;
                }

                HasEscaped = true;
                monsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, "break_open", false);
                monsterManager.AssignedJailCell.PlayAnimation(cellDoorBreakAnimation);
                monsterMesh.sortingOrder = escapedLayer;
            }
        }

        private float GetEscapeTime(float randomTime)
        {
            return Mathf.Max(3f,
                (baseTime + UnityEngine.Random.Range(0, randomTime)) *
                Mathf.Pow(1f - difficultyScalingData.EscapeTimeDecreaseRate, PersistentData.CurrentShift - 1));
        }

        public void AbortEscape()
        {
            IsAttemptingToEscape = false;
            HasEscaped = false;
            //set times
            jailTimeStamp = Time.time + GetEscapeTime(randomJailTime) + 10;
            escapeTimeStamp = Time.time + GetEscapeTime(randomEscapeTime) + 10;
        }
    }
}
