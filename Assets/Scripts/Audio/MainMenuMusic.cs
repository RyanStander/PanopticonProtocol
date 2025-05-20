using FMODUnity;
using GameLogic;
using UnityEngine;

namespace Audio
{
    public class MainMenuMusic : MonoBehaviour
    {
        public EventReference MainMenuMusicEvent;
        
        private void Start()
        {
            AudioManager.Instance.PlayPersistent(MainMenuMusicEvent);
        }
    }
}
