using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerManager : MonoBehaviour
    {
        public PlayerInput PlayerInput;
        public PlayerMovement PlayerMovement;

        private void OnValidate()
        {
            if (PlayerInput == null)
                PlayerInput = GetComponent<PlayerInput>();

            if (PlayerMovement == null)
                PlayerMovement = GetComponent<PlayerMovement>();
        }
    }
}
