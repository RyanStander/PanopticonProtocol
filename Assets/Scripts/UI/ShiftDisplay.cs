using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ShiftDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI shiftText;

        private void OnValidate()
        {
            if (shiftText == null)
                shiftText = GetComponent<TextMeshProUGUI>();
        }

        public void SetShift(int shift)
        {
            string shiftString = shift.ToString();
            shiftText.text = $"Shift: {shiftString}";
        }
    }
}
