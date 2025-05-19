using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CurrencyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyText;

        private void OnValidate()
        {
            if (currencyText == null)
                currencyText = GetComponent<TextMeshProUGUI>();
        
        }

        public void SetCurrency(int currency)
        {
            if (currencyText == null)
            {
                Debug.LogError("Currency Text is not assigned.");
                return;
            }
            
            currencyText.text = currency.ToString();
        }
    }
}
