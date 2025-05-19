using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EmpGunUI : MonoBehaviour
    {
        [SerializeField] private Image empGunImage;
        [SerializeField] private Sprite empOff;
        [SerializeField] private Sprite empOn;
        
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
        }
        
        public void HideBlast()
        {
            empGunImage.sprite = empOff;
        }
    }
}
