using System;
using UI;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private CurrencyDisplay currencyDisplay;
        
        [SerializeField] private int currency;

        private void OnValidate()
        {
            if (currencyDisplay == null)
                currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        }

        public bool CanSpend(int amount)
        {
            return currency >= amount;
        }

        //We assume that the player always has enough currency to spend
        public void Spend(int amount)
        {
            currency -= amount;
            currencyDisplay.SetCurrency(currency);
        }
        
        public void Add(int amount)
        {
            currency += amount;
            currencyDisplay.SetCurrency(currency);
        }
        
        public int GetCurrency()
        {
            return currency;
        }
    }
}
