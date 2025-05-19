using System;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;

namespace Monsters
{
    public class MonsterCloneSpawner : MonoBehaviour
    {
        [SerializeField] private MonsterManager monsterManager;
        [SerializeField] private GameObject monsterClonePrefab;
        [SerializeField] private float baseSpawnTime = 5f;
        [SerializeField] private float randomSpawnTime = 5f;
        [SerializeField] private int maxClones = 3;

        private List<GameObject> clones = new();
        private float spawnTimeStamp;
        private GameManager gameManager;

        private void OnValidate()
        {
            if (monsterManager == null)
                monsterManager = GetComponent<MonsterManager>();
        }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            spawnTimeStamp = Time.time + GetSpawnTime(randomSpawnTime);
        }

        private void FixedUpdate()
        {
            //make sure the clones are not null
            for (int i = clones.Count - 1; i >= 0; i--)
            {
                if (clones[i] == null)
                {
                    clones.RemoveAt(i);
                }
            }

            if (Time.time > spawnTimeStamp && clones.Count < maxClones)
            {
                clones.Add(gameManager.SpawnClone(monsterClonePrefab));
                MonsterEvents.NewScrollingObject(clones[^1].GetComponent<ScrollingObject>());
                spawnTimeStamp = Time.time + GetSpawnTime(randomSpawnTime);
            }

            //if the monster is retreating, destroy the clones
            if (monsterManager.MonsterWeakness.IsRetreating)
            {
                foreach (GameObject clone in clones)
                {
                    Destroy(clone);
                }

                clones.Clear();
                MonsterEvents.ScrollingObjectDelete();
            }
        }

        private float GetSpawnTime(float randomTime)
        {
            return baseSpawnTime + UnityEngine.Random.Range(0, randomTime) *
                (1 - monsterManager.DifficultyScalingData.SpawnGrowthRate * PersistentData.CurrentShift);
        }
    }
}
