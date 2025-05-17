using UnityEngine;

namespace UI
{
    public class EmpGunUI : MonoBehaviour
    {
        [SerializeField] private GameObject empBlast;
        
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
            if (empBlast != null)
            {
                empBlast.SetActive(true);
            }
        }
        
        public void HideBlast()
        {
            if (empBlast != null)
            {
                empBlast.SetActive(false);
            }
        }
    }
}
