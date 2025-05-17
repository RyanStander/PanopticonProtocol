using System;
using Environment;
using Monsters;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private JailCell[] jailCells;
    [SerializeField] private GameObject[] monsters;
    [SerializeField] private int monsterSpawnCount = 5;

    private void OnValidate()
    {
        if (jailCells == null || jailCells.Length == 0)
            jailCells = FindObjectsOfType<JailCell>();
    }

    private void Start()
    {
        if (jailCells.Length == 0)
        {
            Debug.LogError("No jail cells found in the scene.");
            return;
        }

        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < monsterSpawnCount; i++)
        {
            int randomCellIndex = UnityEngine.Random.Range(0, jailCells.Length);
            JailCell randomCell = jailCells[randomCellIndex];

            if (randomCell != null)
            {
                int randomMonsterIndex = UnityEngine.Random.Range(0, monsters.Length);
                GameObject monsterPrefab = monsters[randomMonsterIndex];
                MonsterManager monsterManager = monsterPrefab.GetComponent<MonsterManager>();
                monsterManager.SetData(randomCell);
                Instantiate(monsterPrefab, randomCell.transform.position + monsterManager.MonsterSpawnOffset, Quaternion.identity);
            }
        }
    }
}
