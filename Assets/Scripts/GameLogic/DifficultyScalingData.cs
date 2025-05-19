using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "DifficultyScale", menuName = "PP/DifficultyScale", order = 0)]
    public class DifficultyScalingData : ScriptableObject
    {
        [Header("Monster Spawn Scaling")] [Tooltip("Base amount of monsters to spawn on shift 1.")]
        public int BaseSpawnCount = 5;

        [Tooltip("Percent growth per shift (e.g., 0.15 for +15% per shift).")] [Range(0f, 1f)]
        public float SpawnGrowthRate = 0.15f;

        [Tooltip("Maximum number of monsters that can be spawned (e.g., number of cells).")]
        public int MaxSpawnCount = 30;

        [Header("Monster Type Weighting")] [Tooltip("How much weight increases per shift for rare monsters.")]
        public AnimationCurve MonsterWeightCurve; // Define in editor to curve how types become more common

        [Header("Escape Difficulty Scaling")]
        [Tooltip("Percent reduction in escape time per shift.")] [Range(0f, 1f)]
        public float EscapeTimeDecreaseRate = 0.05f;

        [Header("Monster Speed Scaling")]
        [Tooltip("Percent increase in speed per shift.")] [Range(0f, 1f)]
        public float SpeedGrowthRate = 0.10f;

        [Header("Idle Time Scaling")]
        [Tooltip("Percent decrease in idle time per shift.")] [Range(0f, 1f)]
        public float IdleTimeDecreaseRate = 0.05f;

        [Header("Battery Drain Scaling")]
        [Tooltip("How much the player's battery drains scaled based on shifts. Use with caution.")]
        public float BatteryDrainIncreaseRate = 0.01f; // Increase as shifts progress if needed
    }
}
