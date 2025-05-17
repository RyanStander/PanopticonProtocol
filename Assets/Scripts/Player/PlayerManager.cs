using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))] [RequireComponent(typeof(PlayerTorch))] [RequireComponent(typeof(PlayerEmpGun))]
    public class PlayerManager : MonoBehaviour
    {
        public PlayerInput PlayerInput;
        public PlayerMovement PlayerMovement;
        public PlayerTorch PlayerTorch;
        public PlayerEmpGun PlayerEmpGun;

        private void OnValidate()
        {
            if (PlayerInput == null)
                PlayerInput = GetComponent<PlayerInput>();

            if (PlayerMovement == null)
                PlayerMovement = GetComponent<PlayerMovement>();
            
            if (PlayerTorch == null)
                PlayerTorch = GetComponent<PlayerTorch>();
            
            if (PlayerEmpGun == null)
                PlayerEmpGun = GetComponent<PlayerEmpGun>();
        }

        private void FixedUpdate()
        {
            PlayerMovement.HandleMovement();
            PlayerTorch.HandleTorch();
            PlayerEmpGun.HandleEmpGun();
        }
    }
}
