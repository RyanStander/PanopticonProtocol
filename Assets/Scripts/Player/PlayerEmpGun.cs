using System;
using System.Collections;
using Monsters;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerEmpGun : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private EmpGunUI empGunUI;
        [SerializeField] private Camera mainCamera;

        [SerializeField] private float empRadius = 1.25f;
        [SerializeField] private LayerMask enemyLayer;
        
        private PlayerInput playerInput;
        private bool isGunActive;
        private Coroutine fireCoroutine;

        private void OnValidate()
        {
            if (playerManager == null)
            {
                playerManager = FindObjectOfType<PlayerManager>();
            }

            if (empGunUI == null)
            {
                empGunUI = FindObjectOfType<EmpGunUI>();
            }
            
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
        }

        private void Start()
        {
            playerInput = playerManager.PlayerInput;
        }

        public void HandleEmpGun()
        {
            switch (playerInput.EquipEmpGun)
            {
                case true when !isGunActive:
                    isGunActive = true;
                    empGunUI.ActivateGun();
                    break;
                case false when isGunActive:
                    isGunActive = false;
                    empGunUI.DeactivateGun();
                    break;
            }

            if (isGunActive && playerInput.FireEmpGun && fireCoroutine == null)
            {
                Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(mouseWorldPosition, empRadius, enemyLayer);

                foreach (Collider2D hit in enemiesHit)
                {
                    //try get the MonsterWeakness component on it
                    if (hit.TryGetComponent(out MonsterWeakness monsterWeakness))
                    {
                        monsterWeakness.ShotByEmp();
                    }
                }
                
                //disable the flashlight
                playerManager.PlayerTorch.DisableFlashlight();
                
                empGunUI.Fire();
                fireCoroutine = StartCoroutine(FireEmpGun());
                playerInput.FireEmpGun = false; // Reset the fire input to prevent continuous firing
            }
        }

        private IEnumerator FireEmpGun()
        {
            empGunUI.Fire();
            yield return new WaitForSeconds(0.5f);
            empGunUI.HideBlast();
            isGunActive = false;
            fireCoroutine = null;
        }
    }
}
