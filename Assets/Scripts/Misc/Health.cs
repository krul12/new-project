using System;
using UnityEngine;

namespace Misc
{
    public class Health :  MonoBehaviour
    {
        [NonSerialized]
        public int MaxHealth;
        public int currentHealth;
    
        public delegate void HealthChanged(int current, int max);
        public event HealthChanged OnHealthChanged;
        public delegate void DieDelegate();
        public event DieDelegate OnDie;

        private void Start()
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
            if (currentHealth == 0)
            {
                currentHealth = MaxHealth;
            }
        
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);
        }
    
        public void AddHealth(int amount)
        {
            if( amount < 0 ) return;
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);
        }

        public void ResetHealth()
        {
            currentHealth = MaxHealth;
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);
        }
    
        public void RemoveHealth(int amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);

            if (currentHealth <= 0f)
            {
                Die();
            }
        }
        private void Die()
        {
            OnDie?.Invoke();
        }
    }
}