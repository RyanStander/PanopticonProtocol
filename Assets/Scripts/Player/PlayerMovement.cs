using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private ScrollingTexture scrollingBackground;
        [SerializeField] private ScrollingTexture scrollingForeground;
        private ScrollingObject[] scrollingObjects;
        
        [Header("Movement Settings")]
        [SerializeField] private float backgroundSpeed = 0.5f;
        [SerializeField] private float scrollingSpeed = 5f;
        [SerializeField] private float foregroundSpeed = 0.25f;
        [SerializeField] private float loopingXPos = 17;

        private PlayerInput playerInput;
        
        private void OnValidate()
        {
            if (playerManager == null)
                playerManager = GetComponent<PlayerManager>();
        }

        private void Start()
        {
            playerInput = playerManager.PlayerInput;

            scrollingObjects = FindObjectsByType<ScrollingObject>(FindObjectsSortMode.None);
            
            foreach (ScrollingObject scrollingObject in scrollingObjects)
            {
                scrollingObject.SetLoopingPos(loopingXPos);
            }
        }

        public void HandleMovement()
        {
            switch (playerInput.Horizontal)
            {
                case > 0:
                    scrollingBackground.Scroll(backgroundSpeed);
                    scrollingForeground.Scroll(foregroundSpeed);
                    foreach (ScrollingObject scrollingObject in scrollingObjects)
                    {
                        scrollingObject.Scroll(-scrollingSpeed);
                    }
                    break;
                case < 0:
                    scrollingBackground.Scroll(-backgroundSpeed);
                    scrollingForeground.Scroll(-foregroundSpeed);
                    foreach (ScrollingObject scrollingObject in scrollingObjects)
                    {
                        scrollingObject.Scroll(scrollingSpeed);
                    }
                    break;
            }
        }
    }
}
