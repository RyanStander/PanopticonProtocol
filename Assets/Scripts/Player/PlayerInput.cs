using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerControls playerControls;

        public float Horizontal;
        public bool ToggleTorch;

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
        }
    }
}
