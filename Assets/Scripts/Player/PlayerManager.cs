using System;
using GameLogic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))] [RequireComponent(typeof(PlayerTorch))] [RequireComponent(typeof(PlayerEmpGun))] [RequireComponent(typeof(PlayerInventory))]
    public class PlayerManager : MonoBehaviour
    {
        public DifficultyScalingData DifficultyScalingData;
        public PlayerInput PlayerInput;
        public PlayerMovement PlayerMovement;
        public PlayerTorch PlayerTorch;
        public PlayerEmpGun PlayerEmpGun;
        public PlayerInventory PlayerInventory;

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
            
            if (PlayerInventory == null)
                PlayerInventory = GetComponent<PlayerInventory>();
        }

        private void FixedUpdate()
        {
            PlayerMovement.HandleMovement();
            PlayerTorch.HandleTorch();
            PlayerEmpGun.HandleEmpGun();
        }
    }
}
