using System;
using System.Collections;
using FMODUnity;
using GameLogic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Monsters
{
    public class MonsterEscapeLogic : MonoBehaviour
    {
        [SerializeField] private bool isBasic;
        [SerializeField] private bool isBaiter;
        [SerializeField] private string idleToBreakAnimation = "idle_to_break";
        [SerializeField] private string breakLoopAnimation = "break_loop";
        [SerializeField] private string breakOpenAnimation = "break_open";
        [SerializeField] private MonsterManager monsterManager;
        public int JailedLayer = 1;
        [SerializeField] private int escapedLayer = 5;
        [SerializeField] private float baseTime = 5f;
        [SerializeField] private float randomJailTime = 5f;
        private float jailTimeStamp;
        [SerializeField] private float randomEscapeTime = 5f;
        private float escapeTimeStamp;

        [SerializeField] private EventReference sealBang;

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
                    if (monsterManager.AssignedJailCell.IsSealed)
                        StartCoroutine(DelayedBang());

                    jailTimeStamp = Time.time + GetEscapeTime(randomJailTime);
                    return;
                }

                IsAttemptingToEscape = true;
                if (!string.IsNullOrEmpty(idleToBreakAnimation))
                {
                    TrackEntry entry =
                        monsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, idleToBreakAnimation, false);

                    entry.Complete += (_) =>
                    {
                        if (isBaiter)
                            monsterManager.AssignedJailCell.ShakeBaiter();
                        else if (isBasic)
                            monsterManager.AssignedJailCell.Shake();
                    };
                }

                if (!string.IsNullOrEmpty(breakLoopAnimation))
                    monsterManager.MonsterSkeleton.AnimationState.AddAnimation(0, breakLoopAnimation, true, 0);


                escapeTimeStamp = Time.time + GetEscapeTime(randomEscapeTime);
            }

            if (IsAttemptingToEscape && Time.time > escapeTimeStamp && !HasEscaped)
            {
                if (monsterManager.AssignedJailCell.IsSealed || monsterManager.MonsterWeakness.IsRetreating)
                {
                    if (monsterManager.AssignedJailCell.IsSealed)
                        StartCoroutine(DelayedBang());

                    IsAttemptingToEscape = false;
                    jailTimeStamp = Time.time + GetEscapeTime(randomJailTime);
                    return;
                }

                HasEscaped = true;
                if (!string.IsNullOrEmpty(breakOpenAnimation))
                    monsterManager.MonsterSkeleton.AnimationState.SetAnimation(0, breakOpenAnimation, false);

                if (isBaiter)
                    monsterManager.AssignedJailCell.BreakDoorBaiter();
                else if (isBasic)
                    monsterManager.AssignedJailCell.BreakDoor();

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
            jailTimeStamp = Time.time + GetEscapeTime(randomJailTime) + 5;
            escapeTimeStamp = Time.time + GetEscapeTime(randomEscapeTime) + 5;
        }
        
        private IEnumerator DelayedBang()
        {
            float randomDelay = UnityEngine.Random.Range(0.01f, 0.1f);
            yield return new WaitForSeconds(randomDelay);
            AudioManager.Instance.PlayOneShot(sealBang, monsterManager.transform.position);
        }
    }
}
