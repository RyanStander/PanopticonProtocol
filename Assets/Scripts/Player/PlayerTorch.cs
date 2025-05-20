using System;
using System.Collections;
using FMODUnity;
using GameLogic;
using UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    public class PlayerTorch : MonoBehaviour
    {
        #region General Settings

        [Header("General Settings")] [SerializeField]
        private PlayerManager playerManager;

        [SerializeField] private Camera mainCamera;
        private PlayerInput playerInput;

        #endregion

        #region Torch Mechanics Settings

        [Header("Torch Mechanics Settings")] [SerializeField]
        private float torchBattery = 5f;

        private float torchBatteryRemaining;

        [SerializeField, Range(0.001f, 0.1f)] private float torchBatteryDrain = 0.01f;
        [SerializeField, Range(0, 1)] private float drainRate = 0.1f;
        private Coroutine torchDrainCoroutine;
        private bool isTorchActive;

        private bool flashlightDisabled;

        #endregion

        #region Torch Visual Settings

        [Header("Torch Visual Settings")] [SerializeField]
        private GameObject torchLight;

        [SerializeField] private Light2D torchLight2D;
        [SerializeField] private CircleCollider2D torchCollider;
        [SerializeField] private float intensity = 1f;
        [SerializeField] private Vector2 lightRange = new Vector2(1f, 2f);
        [SerializeField] private float falloffStrength = 1f;
        [SerializeField] private float shadowStrength = 1f;

        [SerializeField] private FlashlightBatteryDisplay flashlightBatteryDisplay;

        #endregion
        
        [Header("Torch Audio Settings")]
        [SerializeField] private EventReference flashlightOnSound;
        [SerializeField] private EventReference flashlightOffSound;

        private void OnValidate()
        {
            if (playerManager == null)
                playerManager = GetComponent<PlayerManager>();

            if (mainCamera == null)
                mainCamera = Camera.main;

            if (flashlightBatteryDisplay == null)
                flashlightBatteryDisplay = FindObjectOfType<FlashlightBatteryDisplay>();

            if (torchLight != null)
            {
                torchLight2D = torchLight.GetComponent<Light2D>();
                torchCollider = torchLight.GetComponent<CircleCollider2D>();
            }

            if (torchLight2D == null)
                return;

            torchLight2D.intensity = intensity;
            torchLight2D.pointLightOuterRadius = lightRange.y;
            if (torchCollider != null)
                torchCollider.radius = lightRange.y;
            torchLight2D.pointLightInnerRadius = lightRange.x;
            torchLight2D.falloffIntensity = falloffStrength;
            torchLight2D.shadowIntensity = shadowStrength;
        }

        private void Start()
        {
            playerInput = playerManager.PlayerInput;
            torchBatteryRemaining = torchBattery;
            isTorchActive = true;
            flashlightBatteryDisplay.InitializeBatteryDisplay(torchBattery);
            torchDrainCoroutine = StartCoroutine(DrainBattery());
        }

        public void HandleTorch()
        {
            MoveTorch();
            ToggleTorch();
        }

        private void MoveTorch()
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            torchLight.transform.position = mousePosition;
        }

        private void ToggleTorch()
        {
            if (flashlightDisabled)
                return;

            if (playerInput.ToggleTorch)
            {
                torchLight.SetActive(!torchLight.activeSelf);
                playerInput.ToggleTorch = false;
                isTorchActive = torchLight.activeSelf;

                if (isTorchActive)
                {
                    if (torchDrainCoroutine != null)
                        StopCoroutine(torchDrainCoroutine);
                    torchDrainCoroutine = StartCoroutine(DrainBattery());
                    flashlightBatteryDisplay.SetFlashlightState(true);
                    AudioManager.Instance.PlayOneShot(flashlightOnSound);
                }
                else
                {
                    if (torchDrainCoroutine != null)
                        StopCoroutine(torchDrainCoroutine);
                    flashlightBatteryDisplay.SetFlashlightState(false);
                    AudioManager.Instance.PlayOneShot(flashlightOffSound);
                }
            }
        }

        private IEnumerator DrainBattery()
        {
            while (isTorchActive)
            {
                if (torchBatteryRemaining > 0)
                {
                    torchBatteryRemaining -= torchBatteryDrain *
                                             Mathf.Pow(
                                                 1f + playerManager.DifficultyScalingData.BatteryDrainIncreaseRate,
                                                 PersistentData.CurrentShift - 1);
                    flashlightBatteryDisplay.UpdateBatteryDisplay(torchBatteryRemaining);
                }
                else
                {
                    isTorchActive = false;
                    torchLight.SetActive(false);
                    break;
                }

                yield return new WaitForSeconds(drainRate);
            }
        }

        public void DisableFlashlight()
        {
            flashlightDisabled = true;
            torchLight.SetActive(false);
            isTorchActive = false;
            flashlightBatteryDisplay.DisableFlashlight();
            StartCoroutine(FlashlightDisabledCoroutine());
        }

        private IEnumerator FlashlightDisabledCoroutine()
        {
            yield return new WaitForSeconds(3f);
            flashlightDisabled = false;
            flashlightBatteryDisplay.DisableFlashlight(false);
        }

        public void RechargeBattery()
        {
            //set torch to max
                torchBatteryRemaining = torchBattery;
        }
    }
}
