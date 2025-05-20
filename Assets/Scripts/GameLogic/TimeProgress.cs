using System;
using Audio;
using UI;
using UnityEngine;

namespace GameLogic
{
    public class TimeProgress : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private ClockTimeDisplay clockTimeDisplay;
        [SerializeField] private ShiftDisplay shiftDisplay;
        
        [SerializeField] private float timeInMinutes = 6.25f;
        [SerializeField] private Vector2 timeRange = new Vector2(20, 24);
        [SerializeField] private GameAudio gameAudio;
        
        private float timeInSeconds;
        private float startTime;
        private int clockHours;
        private int clockMinutes;

        private bool shiftFinished;

        private void OnValidate()
        {
            if (gameManager == null)
                gameManager = GetComponent<GameManager>();
            
            if (clockTimeDisplay == null)
                clockTimeDisplay = FindObjectOfType<ClockTimeDisplay>();

            if (shiftDisplay == null)
                shiftDisplay = FindObjectOfType<ShiftDisplay>();
            
            if (gameAudio == null)
                gameAudio = FindObjectOfType<GameAudio>();
        }

        private void Start()
        {
            timeInSeconds = timeInMinutes * 60;

            StartShift();
        }

        public void StartShift()
        {
            startTime = Time.time;
            PersistentData.CurrentShift++;
            shiftDisplay.SetShift(PersistentData.CurrentShift);
            shiftFinished = false;
        }

        public void ProgressShift()
        {
            if (shiftFinished)
                return;

            float elapsed = Time.time - startTime;
            float progress = Mathf.Clamp01(elapsed / timeInSeconds);

            float fakeHour = Mathf.Lerp(timeRange.x, timeRange.y, progress);
            
            float normalizedTime = Mathf.InverseLerp(timeRange.x, timeRange.y, fakeHour);
            gameAudio?.UpdateTimeParameter(normalizedTime);

            
            int rawHour = Mathf.FloorToInt(fakeHour);
            clockHours = rawHour % 24;
            clockMinutes = Mathf.FloorToInt((fakeHour - rawHour) * 60);
            
            clockTimeDisplay.SetTime(clockHours, clockMinutes);
            
            if (Time.time - startTime >= timeInSeconds)
            {
                shiftFinished = true;
                gameManager.ShiftFinished();
            }
        }
    }
}
