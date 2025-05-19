using System;
using System.Collections.Generic;
using System.Linq;
using Environment;
using GameLogic;
using Monsters;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DifficultyScalingData difficultyScalingData;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private List<JailCell> unOccupiedJailCells;
    private List<JailCell> occupiedJailCells = new();
    private bool gameOver;

    #region Monster Logic

    [SerializeField] private GameObject[] monsters;

    private List<GameObject> createdMonsters = new();

    #endregion

    #region Currency Logic

    [SerializeField] private float baseCurrency = 100f;
    [SerializeField] private float growthRate = 1.15f;

    #endregion

    #region Shift Logic

    [SerializeField] private TimeProgress timeProgress;
    [SerializeField] private GameObject shiftCompletedUi;

    #endregion

    private void OnValidate()
    {
        if (playerManager == null)
            playerManager = FindObjectOfType<PlayerManager>();

        if (unOccupiedJailCells == null || unOccupiedJailCells.Count == 0)
            unOccupiedJailCells = FindObjectsOfType<JailCell>().ToList();

        if (timeProgress == null)
            timeProgress = GetComponent<TimeProgress>();
    }
    
    private void OnEnable()
    {
        MonsterEvents.OnMonsterKill += OnGameOver;
    }
        
    private void OnDisable()
    {
        MonsterEvents.OnMonsterKill -= OnGameOver;
    }

    private void Awake()
    {
        PersistentData.CurrentShift = 0;
    }

    private void Start()
    {
        if (unOccupiedJailCells.Count == 0)
        {
            Debug.LogError("No jail cells found in the scene.");
            return;
        }

        SpawnMonsters();
    }

    private void Update()
    {
        if(gameOver)
            return;
        
        if (timeProgress != null)
            timeProgress.ProgressShift();
    }

    private void SpawnMonsters()
    {
        
        for (int i = 0; i < GetSpawnCount(); i++)
        {
            if (unOccupiedJailCells.Count == 0)
                break;
            
            int randomCellIndex = Random.Range(0, unOccupiedJailCells.Count);
            JailCell randomCell = unOccupiedJailCells[randomCellIndex];
            unOccupiedJailCells.RemoveAt(randomCellIndex);
            occupiedJailCells.Add(randomCell);

            if (randomCell != null)
            {
                int randomMonsterIndex = Random.Range(0, monsters.Length);
                GameObject monsterPrefab = monsters[randomMonsterIndex];
                MonsterManager monsterManager = monsterPrefab.GetComponent<MonsterManager>();
                monsterManager.SetData(randomCell);
                createdMonsters.Add(Instantiate(monsterPrefab,
                    randomCell.transform.position + monsterManager.MonsterSpawnOffset, Quaternion.identity));
            }
        }
    }
    
    private int GetSpawnCount()
    {
        float scaled = difficultyScalingData.BaseSpawnCount * Mathf.Pow(1f + difficultyScalingData.SpawnGrowthRate, PersistentData.CurrentShift - 1);
        return Mathf.Min(Mathf.CeilToInt(scaled), difficultyScalingData.MaxSpawnCount);
    }

    #region Shift Logic

    public void ShiftFinished()
    {
        playerManager.PlayerInventory.Add((int)(baseCurrency*Mathf.Pow(growthRate,PersistentData.CurrentShift-1)));

        //Everything should stop at this point, the player is safe

        //destroy all monsters
        foreach (GameObject monster in createdMonsters)
        {
            DestroyImmediate(monster);
        }
        
        //add all occupied cells back to the unoccupied list
        foreach (JailCell occupiedCell in occupiedJailCells)
        {
            unOccupiedJailCells.Add(occupiedCell);
        }
        occupiedJailCells.Clear();

        shiftCompletedUi.SetActive(true);
        createdMonsters.Clear();
        playerManager.PlayerMovement.UpdateScrollingListener();
    }

    public void StartNextShift()
    {
        shiftCompletedUi.SetActive(false);
        timeProgress.StartShift();
        SpawnMonsters();
        playerManager.PlayerMovement.UpdateScrollingListener();
        playerManager.PlayerTorch.RechargeBattery();
    }

    #endregion
    
    private void OnGameOver(string animationName)
    {
        gameOver = true;
        
        //kill all monsters
        foreach (GameObject monster in createdMonsters)
        {
            DestroyImmediate(monster);
        }
    }
}
