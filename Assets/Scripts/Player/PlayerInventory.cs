using System;
using Monsters;
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

        private void OnEnable()
        {
            MonsterEvents.OnMonsterHitByPlayerWeapon += AddCurrencyOnMonsterHit;
        }

        private void OnDisable()
        {
            MonsterEvents.OnMonsterHitByPlayerWeapon -= AddCurrencyOnMonsterHit;
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
        
        private void AddCurrencyOnMonsterHit(int amount)
        {
            Add(amount);
        }
    }
}
