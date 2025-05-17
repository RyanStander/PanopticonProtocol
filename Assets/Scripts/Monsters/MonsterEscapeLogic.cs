using System;
using UnityEngine;

namespace Monsters
{
    public class MonsterEscapeLogic : MonoBehaviour
    {
        [SerializeField] private MonsterManager monsterManager;
        [SerializeField] private int jailedLayer = 1;
        [SerializeField] private int escapedLayer = 5;
        [SerializeField] private float timeRandomizer = 2f;
        [SerializeField] private float jailTime = 5f;
        private float jailTimeStamp;
        [SerializeField] private float escapeTime = 5f;
        private float escapeTimeStamp;

        private bool isAttemptingToEscape = false;
        public bool HasEscaped;

        private SpriteRenderer monsterSprite;

        private void OnValidate()
        {
            if (monsterManager == null)
                monsterManager = GetComponent<MonsterManager>();
        }

        private void Start()
        {
            // Set the monster to the jailed layer
            monsterSprite = monsterManager.MonsterSprite;
            monsterSprite.sortingOrder = jailedLayer;
            jailTimeStamp = Time.time + jailTime + UnityEngine.Random.Range(0, timeRandomizer);
        }

        public void HandleEscape()
        {
            if (HasEscaped)
                return;

            if (Time.time > jailTimeStamp && !isAttemptingToEscape)
            {
                if (monsterManager.AssignedJailCell.IsSealed)
                {
                    jailTimeStamp = Time.time + jailTime + UnityEngine.Random.Range(0, timeRandomizer);
                    return;
                }

                isAttemptingToEscape = true;
                escapeTimeStamp = Time.time + escapeTime + UnityEngine.Random.Range(0, timeRandomizer);
            }

            if (isAttemptingToEscape && Time.time > escapeTimeStamp && !HasEscaped)
            {
                if (monsterManager.AssignedJailCell.IsSealed)
                {
                    isAttemptingToEscape = false;
                    jailTimeStamp = Time.time + jailTime + UnityEngine.Random.Range(0, timeRandomizer);
                    return;
                }

                HasEscaped = true;
                monsterSprite.sortingOrder = escapedLayer;
            }
        }
    }
}
