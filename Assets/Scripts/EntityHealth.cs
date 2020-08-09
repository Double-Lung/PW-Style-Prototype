using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth {
    private int maxHealth;
    private int health;
    public event Action<int> OnHealthChanged;
    public event Action OnDead;

    public EntityHealth(int maxHealth) {
        this.maxHealth = maxHealth;
        health = this.maxHealth;
    }
    public int GetHealth() {
        return health;
    }
    public void takeDamage(int amount) {
        health -= amount;
        if (health < 0) {
            health = 0;
        }
        OnHealthChanged?.Invoke(amount);
        if (health <= 0) {
            Die();
        }
    }
    void Die() {
        OnDead?.Invoke();
    }
    public bool IsDead() {
        return health <= 0;
    }
}