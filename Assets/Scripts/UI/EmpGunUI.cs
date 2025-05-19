using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EmpGunUI : MonoBehaviour
    {
        [SerializeField] private Image empGunImage;
        [SerializeField] private Sprite empOff;
        [SerializeField] private Sprite empOn;
        [SerializeField] private SkeletonGraphic empBolt;
        
        public void ActivateGun()
        {
            gameObject.SetActive(true);
        }
        
        public void DeactivateGun()
        {
            gameObject.SetActive(false);
        }

        public void Fire()
        {
            empGunImage.sprite = empOn;
            empBolt.gameObject.SetActive(true);
            empBolt.AnimationState.SetAnimation(0, "bolt", false);
        }
        
        public void HideBlast()
        {
            empGunImage.sprite = empOff;
            empBolt.gameObject.SetActive(false);
        }
    }
}
