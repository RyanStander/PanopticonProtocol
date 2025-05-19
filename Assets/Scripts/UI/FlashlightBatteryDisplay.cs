using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FlashlightBatteryDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject batteryBarPrefab;
        [SerializeField] private Transform batteryBarContainer;
        [SerializeField] private GameObject flashlightDisabledDisplay;
        [SerializeField] private Image flashlightDetail;
        [SerializeField] private Sprite flashlightOnSprite;
        [SerializeField] private Sprite flashlightOffSprite;
        private int batteryCount = 5;
        private GameObject[] batteryBars;
        
        public void InitializeBatteryDisplay(float givenBatteryCount)
        {
            batteryCount = Mathf.CeilToInt(givenBatteryCount);
            batteryBars = new GameObject[batteryCount];

            SetupContainer();
        }

        private void SetupContainer()
        {
            //clear the container
            foreach (Transform child in batteryBarContainer)
            {
                Destroy(child.gameObject);
            }
            
            for (int i = 0; i < batteryCount; i++)
            {
                batteryBars[i] = Instantiate(batteryBarPrefab, batteryBarContainer);
                batteryBars[i].SetActive(true);
            }
        }
        
        public void UpdateBatteryDisplay(float currentBattery)
        {
            int batteryToUpdate = Mathf.CeilToInt(currentBattery);
            for (int i = 0; i < batteryCount; i++)
            {
                batteryBars[i].SetActive(i < batteryToUpdate);
            }
        }

        public void SetFlashlightState(bool isOn)
        {
            if (flashlightDetail != null)
            {
                flashlightDetail.sprite = isOn ? flashlightOnSprite : flashlightOffSprite;
            }
        }

        public void DisableFlashlight(bool isDisabled = true)
        {
            flashlightDisabledDisplay.SetActive(isDisabled);
            flashlightDetail.sprite = flashlightOffSprite;
        }
    }
}
