using FMODUnity;
using GameLogic;
using UnityEngine;

namespace Audio
{
    public class JumpscareSfx : MonoBehaviour
    {
        [SerializeField] private EventReference basicJumpscareSfx;
        [SerializeField] private EventReference cloneJumpscareSfx;

        public void PlayBasicJumpscareSfx()
        {
            AudioManager.Instance.PlayOneShot(basicJumpscareSfx);
        }

        public void PlayCloneJumpscareSfx()
        {
            AudioManager.Instance.PlayOneShot(cloneJumpscareSfx);
        }
    }
}
