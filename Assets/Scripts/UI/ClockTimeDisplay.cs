using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ClockTimeDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        private void OnValidate()
        {
            if (timeText == null)
                timeText = GetComponent<TextMeshProUGUI>();
        }

        public void SetTime(float hour, float minutes)
        {
            string hourString = hour.ToString("00");
            string minuteString = minutes.ToString("00");
            timeText.text = $"{hourString}:{minuteString}";
        }
    }
}
