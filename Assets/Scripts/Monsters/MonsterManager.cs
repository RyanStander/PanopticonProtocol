using System;
using System.Collections;
using Environment;
using UnityEngine;

namespace Monsters
{
    public class MonsterManager : MonoBehaviour
    {
        public SpriteRenderer MonsterSprite;
        public Vector3 MonsterSpawnOffset = new(0, 0, 0);
        [SerializeField] private MonsterEscapeLogic monsterEscapeLogic;

        
        [SerializeField] private float moveSpeed = 5f;
        
        private Coroutine roamingCoroutine;
        
        public JailCell AssignedJailCell;

        private void OnValidate()
        {
            if (MonsterSprite == null)
                MonsterSprite = GetComponent<SpriteRenderer>();
            
            if (monsterEscapeLogic == null)
                monsterEscapeLogic = GetComponent<MonsterEscapeLogic>();
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

                // Optional: flip the sprite
                if (MonsterSprite != null)
                    MonsterSprite.flipX = direction > 0;

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
