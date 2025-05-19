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
        public MonsterEscapeLogic MonsterEscapeLogic;
        public MonsterObjective MonsterObjective;
        public MonsterWeakness MonsterWeakness;

        public JailCell AssignedJailCell;
        public MonsterPath MonsterPath;
        
        public float MoveSpeed = 5f;
        public float TrueSpeed;

        private void OnValidate()
        {
            if (MonsterSkeleton == null)
                MonsterSkeleton = GetComponent<SkeletonAnimation>();

            if (MonsterMesh == null)
                MonsterMesh = GetComponent<MeshRenderer>();

            if (MonsterEscapeLogic == null)
                MonsterEscapeLogic = GetComponent<MonsterEscapeLogic>();
            
            if (MonsterObjective == null)
                MonsterObjective = GetComponent<MonsterObjective>();
            
            if (MonsterWeakness == null)
                MonsterWeakness = GetComponent<MonsterWeakness>();
        }

        private void Start()
        {
            MonsterPath = FindObjectOfType<MonsterPath>();
            
            TrueSpeed = MoveSpeed *
                        MathF.Pow(1f + DifficultyScalingData.SpeedGrowthRate,
                            PersistentData.CurrentShift - 1);
        }

        public void SetData(JailCell jailCell)
        {
            AssignedJailCell = jailCell;
        }

        private void FixedUpdate()
        {
            if(MonsterWeakness.IsRetreating)
            {
                MonsterObjective.AbortObjective();
                MonsterEscapeLogic.AbortEscape();
                return;
            }
            
            MonsterEscapeLogic.HandleEscape();
            MonsterObjective.HandleObjective();
        }

        
    }
}
