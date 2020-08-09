using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityUI : MonoBehaviour
{
    private SpriteRenderer model;
    private TextMeshPro nameText;
    private GameObject triangleHighlighter;
    private Slider HealthSlider;
    private Canvas weaponName;
    private Canvas DamageIndicator;
    private TextMeshProUGUI damageText;

    private void Awake() {
        model = transform.GetChild(0).GetComponent<SpriteRenderer>();
        nameText = transform.GetChild(1).GetComponent<TextMeshPro>();
        triangleHighlighter = transform.GetChild(2).gameObject;
        HealthSlider = transform.GetChild(3).GetComponentInChildren<Slider>();
        weaponName = transform.GetChild(4).GetComponentInChildren<Canvas>();
        DamageIndicator = transform.GetChild(5).GetComponentInChildren<Canvas>();
        damageText = DamageIndicator.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    public void Setup(string name, Sprite sprite, Color color, int maxHealth, Weapons weapon) {
        triangleHighlighter.SetActive(false);
        weaponName.enabled = false;
        DamageIndicator.enabled = false;
        nameText.SetText(name);
        model.sprite = sprite;
        model.color = color;
        HealthSlider.maxValue = maxHealth;
        weaponName.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().SetText(weapon.ToString());
    }
    public void updateHealthBar(int health) {
        HealthSlider.value = health;
    }
    public void updateDamageText(int amount) {
        damageText.SetText(amount.ToString());
    }
    public void displayWeaponName(bool state) {
        weaponName.enabled = state;
    }
    public void displayDamageIndicator(bool state) {
        DamageIndicator.enabled = state;
    }
    public void displayTriangle(bool state) {
        triangleHighlighter.SetActive(state);
    }
}