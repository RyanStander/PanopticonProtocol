using System;
using System.Collections;
using Environment;
using FMODUnity;
using GameLogic;
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
        
        [SerializeField] private EventReference empGunBlastSound;
        [SerializeField] private EventReference empGunActivateSound;
        
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
                    AudioManager.Instance.PlayOneShot(empGunActivateSound);
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
                
                //play the emp gun sound
                AudioManager.Instance.PlayOneShot(empGunBlastSound, mouseWorldPosition);

                foreach (Collider2D hit in enemiesHit)
                {
                    //try get the MonsterWeakness component on it
                    if (hit.TryGetComponent(out MonsterWeakness monsterWeakness))
                    {
                        monsterWeakness.ShotByEmp();
                    }
                    else if (hit.TryGetComponent(out JailCell jailCell))
                    {
                        jailCell.EmpDoor();  
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
