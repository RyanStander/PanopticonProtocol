using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerControls playerControls;

        public float Horizontal;
        public bool ToggleTorch;

        public bool EquipEmpGun;
        public bool FireEmpGun;

        private void Awake()
        {
            playerControls = new PlayerControls();
            playerControls.Enable();
            CheckInputs();
        }

        private void CheckInputs()
        {
            playerControls.ObservationControls.Horizontal.performed += horizontalControls =>
                Horizontal = horizontalControls.ReadValue<float>();
            playerControls.ObservationControls.Horizontal.canceled += horizontalControls =>  
                Horizontal = horizontalControls.ReadValue<float>();

            playerControls.ObservationControls.ToggleTorch.performed += i => ToggleTorch = true;
            playerControls.ObservationControls.ToggleTorch.canceled += i => ToggleTorch = false;
            
            playerControls.ObservationControls.EquipEmpGun.performed += i => EquipEmpGun = true;
            playerControls.ObservationControls.EquipEmpGun.canceled += i => EquipEmpGun = false;
            
            playerControls.ObservationControls.FireEmpGun.performed += i => FireEmpGun = true;
            playerControls.ObservationControls.FireEmpGun.canceled += i => FireEmpGun = false;
        }
    }
}
