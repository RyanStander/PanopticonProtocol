﻿using System;
using System.Collections;
using System.Collections.Generic;
using Environment;
using Monsters;
using UI;
using UnityEngine;

namespace GameLogic
{
    public class FacilityPower : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private FacilityBatteryDisplay facilityBatteryDisplay;
        [SerializeField] private float powerLevel = 15;
        private float currentPowerLevel;
        [SerializeField, Range(0.001f, 0.1f)] private float batteryDrain = 0.01f;
        [SerializeField, Range(0, 1)] private float drainRate = 0.1f;

        [SerializeField] private JailCell[] jailCells;
        private List<Coroutine> drainCoroutines = new();

        private void OnValidate()
        {
            if (gameManager == null)
                gameManager = GetComponent<GameManager>();
            
            if (facilityBatteryDisplay == null)
                facilityBatteryDisplay = FindObjectOfType<FacilityBatteryDisplay>();

            if (jailCells == null || jailCells.Length == 0)
            {
                jailCells = FindObjectsOfType<JailCell>();
            }
        }

        private void OnEnable()
        {
            MonsterEvents.OnJailCellSealed += OnJailCellSealed;
            MonsterEvents.OnJailCellUnsealed += OnJailCellUnsealed;
        }
        
        private void OnDisable()
        {
            MonsterEvents.OnJailCellSealed -= OnJailCellSealed;
            MonsterEvents.OnJailCellUnsealed -= OnJailCellUnsealed;
        }

        private void Start()
        {
            currentPowerLevel = powerLevel;
            facilityBatteryDisplay.InitializeBatteryDisplay(currentPowerLevel);
        }

        private void OnJailCellSealed(JailCell jailCell)
        {
            if (jailCell != null)
            {
                drainCoroutines.Add(StartCoroutine(DrainBattery()));
            }
        }
        
        private void OnJailCellUnsealed(JailCell jailCell)
        {
            if (jailCell != null && drainCoroutines.Count > 0)
            {
                StopCoroutine(drainCoroutines[0]);
                drainCoroutines.RemoveAt(0);
            }
        }

        private IEnumerator DrainBattery()
        {
            while (true)
            {
                if (currentPowerLevel > 0)
                {
                    currentPowerLevel -= batteryDrain *
                                         Mathf.Pow(
                                             1f + gameManager.DifficultyScalingData.BatteryDrainIncreaseRate,
                                             PersistentData.CurrentShift - 1);

                    facilityBatteryDisplay.UpdateBatteryDisplay(currentPowerLevel);
                }
            
                if (currentPowerLevel <= 0)
                {
                    currentPowerLevel = 0;
                    facilityBatteryDisplay.UpdateBatteryDisplay(currentPowerLevel);
                    MonsterEvents.FacilityNoPower();
                }

                yield return new WaitForSeconds(drainRate);
            }
        }
        
        public void RechargeBattery()
        {
            currentPowerLevel = powerLevel;

            facilityBatteryDisplay.UpdateBatteryDisplay(powerLevel);
        }
    }
}
