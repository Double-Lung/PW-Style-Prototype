using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    private string characterName;
    private int strength;
    private bool IsPlayerTeam;
    private EntityHealth entityHealth;
    private EntityAlpha entityAlpha;
    private EntityShake entityShake;
    private EntityUI entityUI;

    private void Awake() {
        entityUI = GetComponent<EntityUI>();
        entityAlpha = GetComponent<EntityAlpha>();
        entityShake = GetComponent<EntityShake>();
    }

    private void OnDisable() {
        entityHealth.OnHealthChanged -= updateHealthBar;
        entityHealth.OnHealthChanged -= updateDamageText;
        entityHealth.OnDead -= SetCD;
    }
    public void EntitySetup(CharacterStats character) {
        IsPlayerTeam = character.isPlayerTeam;
        int health = character.baseHealth;
        characterName = character.characterName;
        strength = character.baseStrength;
        if (!IsPlayerTeam) {
            int level = PlayerPrefs.GetInt("level");
            health += level * 19;
            strength += level * 4;
        }
        entityHealth = new EntityHealth(health);
        entityUI.Setup(characterName, character.characterSprite, character.spriteColor, health, character.baseWeapon);
        entityUI.updateHealthBar(entityHealth.GetHealth());
        entityHealth.OnHealthChanged += updateHealthBar;
        entityHealth.OnHealthChanged += updateDamageText;
        entityHealth.OnDead += SetCD;
    }
    void updateHealthBar(int amount) {
        entityUI.updateHealthBar(entityHealth.GetHealth());
    }
    void updateDamageText(int amount) {
        entityUI.updateDamageText(amount);
    }

    void SetCD() {
        if (IsPlayerTeam) {
            CoolDownManager.SetCoolDown(characterName);
            PlayerPrefs.SetInt(characterName, 0);
            PlayerPrefs.SetInt("Members", PlayerPrefs.GetInt("Members")-1);
        }
    }
    public void Attack(Entity target, Action onAttackComplete) {
        StartCoroutine(AttackRoutine(target, 0.5f, onAttackComplete));
    }
    IEnumerator AttackRoutine(Entity target, float waitTime, Action onAttackComplete) {
        entityAlpha.highLight();
        entityUI.displayWeaponName(true);
        yield return new WaitForSeconds(waitTime);
        target.targetHighlight(true);
        yield return new WaitForSeconds(waitTime);
        entityAlpha.alphaFlicker(() => { 
            target.TakeDamage(strength, ()=> {
                EndAttack(onAttackComplete);
            }); 
        });
    }
    public void TakeDamage(int amount, Action callback) {
        StartCoroutine(DamageRoutine(amount, 0.5f, callback));
    }
    IEnumerator DamageRoutine(int amount, float waitTime, Action callback) {
        entityHealth.takeDamage(amount);
        entityShake.Shake();
        entityUI.displayDamageIndicator(true);
        yield return new WaitForSeconds(waitTime);
        entityUI.displayDamageIndicator(false);
        entityUI.displayTriangle(false);
        if (entityHealth.IsDead()) {
            entityAlpha.alphaDeath();
        }
        callback();
    }
    void EndAttack(Action onAttackComplete) {
        StartCoroutine(EndAttackRoutine(0.5f, onAttackComplete));
    }
    public bool IsDead() {
        return entityHealth.IsDead();
    }
    IEnumerator EndAttackRoutine(float waitTime, Action onAttackComplete) {
        entityAlpha.resetAlpha();
        entityUI.displayWeaponName(false);
        yield return new WaitForSeconds(waitTime);
        onAttackComplete();
    }
    public void targetHighlight(bool state) {
        entityUI.displayTriangle(state);
    }
}