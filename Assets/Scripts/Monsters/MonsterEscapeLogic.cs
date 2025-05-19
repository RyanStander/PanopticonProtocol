using System;
using GameLogic;
using Spine.Unity;
using UnityEngine;

namespace Monsters
{
    public class MonsterEscapeLogic : MonoBehaviour
    {
        [SerializeField] private string cellDoorShakeAnimation = "M1_CellDoor_shake";
        [SerializeField] private string cellDoorBreakAnimation = "M1_CellDoor_break";
        [SerializeField] private MonsterManager monsterManager;
        [SerializeField] private int jailedLayer = 1;
        [SerializeField] private int escapedLayer = 5;
        [SerializeField] private float baseTime = 5f;
        [SerializeField] private float randomJailTime = 5f;
        private float jailTimeStamp;
        [SerializeField] private float randomEscapeTime = 5f;
        private float escapeTimeStamp;

        private bool isAttemptingToEscape = false;
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
            monsterMesh.sortingOrder = jailedLayer;
            jailTimeStamp = Time.time + GetEscapeTime(randomJailTime);
        }

        public void HandleEscape()
        {
            if (HasEscaped)
                return;

            if (Time.time > jailTimeStamp && !isAttemptingToEscape)
            {
                if (monsterManager.AssignedJailCell.IsSealed)
                {
                    jailTimeStamp = Time.time + GetEscapeTime(randomJailTime);
                    return;
                }

                isAttemptingToEscape = true;
                monsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, "idle_to_break", false);
                monsterManager.MonsterSkeleton.AnimationState.AddAnimation(0, "break_loop", true, 0);
                monsterManager.AssignedJailCell.PlayAnimation(cellDoorShakeAnimation, true);
                escapeTimeStamp = Time.time + GetEscapeTime(randomEscapeTime);
            }

            if (isAttemptingToEscape && Time.time > escapeTimeStamp && !HasEscaped)
            {
                if (monsterManager.AssignedJailCell.IsSealed)
                {
                    isAttemptingToEscape = false;
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
    }
}
