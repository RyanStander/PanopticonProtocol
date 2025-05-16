using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerControls playerControls;
        
        public float Horizontal => playerControls.ObservationControls.Horizontal.ReadValue<float>();
        
        private void Awake()
        {
            playerControls = new PlayerControls();
            playerControls.Enable();
        }
    }
}
