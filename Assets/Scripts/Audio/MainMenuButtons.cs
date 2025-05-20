using FMODUnity;
using GameLogic;
using UnityEngine;

namespace Audio
{
    public class MainMenuButtons : MonoBehaviour
    {
        public EventReference ButtonClickEvent;
        public EventReference ButtonHoverEvent;

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
