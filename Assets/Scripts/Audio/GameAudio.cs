using System;
using FMOD.Studio;
using FMODUnity;
using GameLogic;
using UnityEngine;

namespace Audio
{
    public class GameAudio : MonoBehaviour
    {
        [SerializeField] private EventReference gameMusic;
        [SerializeField] private string timeParameterName = "ShiftTimeElapsed";
        
        private EventInstance musicInstance;

        private void Start()
        {
            musicInstance = RuntimeManager.CreateInstance(gameMusic);
            musicInstance.start();
        }
        
        public void UpdateTimeParameter(float normalizedTime)
        {
            RuntimeManager.StudioSystem.setParameterByName(timeParameterName, normalizedTime);
        }

        
        private void OnDestroy()
        {
            if (musicInstance.isValid())
            {
                musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                musicInstance.release();
            }
        }
    }
}
