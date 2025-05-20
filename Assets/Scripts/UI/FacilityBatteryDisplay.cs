using UnityEngine;

namespace UI
{
    public class FacilityBatteryDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject batteryBarPrefab;
        [SerializeField] private Transform batteryBarContainer;
        private int batteryCount;
        [SerializeField] private GameObject[] batteryBars;

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
    }
}
