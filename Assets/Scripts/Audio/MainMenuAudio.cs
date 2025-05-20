using FMODUnity;
using GameLogic;
using UnityEngine;

namespace Audio
{
    public class MainMenuAudio : MonoBehaviour
    {
        public EventReference MainMenuMusicEvent;
        public EventReference ButtonClickEvent;
                public EventReference ButtonHoverEvent;
        
        private void Start()
        {
            AudioManager.Instance.PlayPersistent(MainMenuMusicEvent);
        }
        
        public void StopMainMenuMusic()
        {
            AudioManager.Instance.StopPersistent(MainMenuMusicEvent);
        }
      
        public void PlayButtonClick()
        {
            AudioManager.Instance.PlayOneShot(ButtonClickEvent, transform.position);
        }

        public void PlayButtonHover()
        {
            AudioManager.Instance.PlayOneShot(ButtonHoverEvent, transform.position);
        }
    }
}
