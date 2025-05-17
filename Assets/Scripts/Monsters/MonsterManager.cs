using System;
using System.Collections;
using UnityEngine;

namespace Monsters
{
    public class MonsterManager : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer monsterSprite;
        public Vector3 MonsterSpawnOffset = new Vector3(0, 0, 0);

        [SerializeField] private int jailedLayer = 1;
        [SerializeField] private int escapedLayer = 5;
        [SerializeField] private float timeRandomizer = 2f;
        [SerializeField] private float jailTime = 5f;
        private float jailTimeStamp;
        [SerializeField] private float escapeTime = 5f;
        private float escapeTimeStamp;
        [SerializeField] private float moveSpeed = 5f;
        private bool isAttemptingToEscape = false;
        private bool hasEscaped = false;
        private Coroutine roamingCoroutine;

        private void OnValidate()
        {
            if (monsterSprite == null)
                monsterSprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // Set the monster to the jailed layer
            monsterSprite.sortingOrder = jailedLayer;
            jailTimeStamp = Time.time + jailTime + UnityEngine.Random.Range(0, timeRandomizer);
        }

        //monster will attempt to escape from the jail after waiting its jail time + a random time, then it will start attempting to escape
        //for the duration of the escape time + a random time, thereafter it will be considered free and will roam left and right with its move speed

        private void FixedUpdate()
        {
            if (Time.time > jailTimeStamp && !isAttemptingToEscape)
            {
                isAttemptingToEscape = true;
                escapeTimeStamp = Time.time + escapeTime + UnityEngine.Random.Range(0, timeRandomizer);
            }

            if (isAttemptingToEscape)
            {
                if (Time.time > escapeTimeStamp && !hasEscaped)
                {
                    hasEscaped = true;
                    monsterSprite.sortingOrder = escapedLayer;
                    // Start roaming left and right
                    if (roamingCoroutine == null)
                    {
                        roamingCoroutine = StartCoroutine(Roam());
                    }
                }
            }
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

                // Optional: flip the sprite
                if (monsterSprite != null)
                    monsterSprite.flipX = direction > 0;

                while (moveTimer < moveDuration)
                {
                    transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
                    moveTimer += Time.deltaTime;
                    yield return null; // wait one frame
                }

                // Wait/idle after moving
                float idleDuration = UnityEngine.Random.Range(0.5f, 2f);
                yield return new WaitForSeconds(idleDuration);
            }
        }
    }
}
