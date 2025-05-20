using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace GameLogic
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private Dictionary<EventReference, EventInstance> instances = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void PlayOneShot(EventReference sound, Vector3 position)
        {
            if (!sound.IsNull)
            {
                RuntimeManager.PlayOneShot(sound, position);
            }
            else
            {
                Debug.LogWarning("Attempted to play null FMOD event.");
            }
        }

        public void PlayPersistent(EventReference sound)
        {
            if (sound.IsNull)
            {
                Debug.LogWarning("Attempted to play null FMOD event.");
                return;
            }

            if (instances.ContainsKey(sound))
            {
                Debug.LogWarning("Sound already playing.");
                return;
            }

            EventInstance instance = RuntimeManager.CreateInstance(sound);
            instance.start();
            instances[sound] = instance;
        }

        public void StopPersistent(EventReference sound, STOP_MODE mode = STOP_MODE.ALLOWFADEOUT)
        {
            if (instances.TryGetValue(sound, out EventInstance instance))
            {
                instance.stop(mode);
                instance.release();
                instances.Remove(sound);
            }
        }

        public void SetParameter(EventReference sound, string parameterName, float value)
        {
            if (instances.TryGetValue(sound, out EventInstance instance))
            {
                instance.setParameterByName(parameterName, value);
            }
        }

        public bool IsPlaying(EventReference sound)
        {
            if (instances.TryGetValue(sound, out EventInstance instance))
            {
                instance.getPlaybackState(out PLAYBACK_STATE state);
                return state == PLAYBACK_STATE.PLAYING || state == PLAYBACK_STATE.STARTING;
            }

            return false;
        }
    }
}
