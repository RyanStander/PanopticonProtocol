using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private ScrollingTexture scrollingBackground;
        [SerializeField] private ScrollingTexture scrollingForeground;
        
        [Header("Movement Settings")]
        [SerializeField] private float scrollingSpeed = 0.5f;
        [SerializeField] private float foregroundSpeed = 0.25f;

        private PlayerInput playerInput;
        
        private void OnValidate()
        {
            if (playerManager == null)
                playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            playerInput = playerManager.PlayerInput;
        }

        private void FixedUpdate()
        {
            switch (playerInput.Horizontal)
            {
                case > 0:
                    scrollingBackground.Scroll(scrollingSpeed);
                    scrollingForeground.Scroll(foregroundSpeed);
                    break;
                case < 0:
                    scrollingBackground.Scroll(-scrollingSpeed);
                    scrollingForeground.Scroll(-foregroundSpeed);
                    break;
            }
        }
    }
}
