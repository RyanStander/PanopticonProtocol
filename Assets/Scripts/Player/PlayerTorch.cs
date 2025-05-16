using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    public class PlayerTorch : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject torchLight;
        [SerializeField] private Light2D torchLight2D;
        [SerializeField] private float intensity = 1f;
        [SerializeField] private Vector2 lightRange = new Vector2(1f, 2f);
        [SerializeField] private float falloffStrength = 1f;
        [SerializeField] private float shadowStrength = 1f;

        private void OnValidate()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
            
            if(torchLight!=null)
                torchLight2D = torchLight.GetComponent<Light2D>();

            if (torchLight2D == null) 
                return;
            
            torchLight2D.intensity = intensity;
            torchLight2D.pointLightOuterRadius = lightRange.y;
            torchLight2D.pointLightInnerRadius = lightRange.x;
            torchLight2D.falloffIntensity = falloffStrength;
            torchLight2D.shadowIntensity = shadowStrength;
        }

        public void HandleTorch()
        {
            MoveTorch();
        }

        private void MoveTorch()
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            torchLight.transform.position = mousePosition;
        }
    }
}
