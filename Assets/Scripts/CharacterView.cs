using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private CharacterStats characterStats;
    private SpriteRenderer model;
    private GameObject triangle;
    private GameObject cdMark;
    private TextMeshPro nameText;
    private TextMeshPro timer;
    private bool selected;
    //private string characterName;
    //private float characterStrength;
    //private float characterHealth;
    //private Weapons characterWeapon;
    //private float characterHit;
    //private float characterDodge;
    //private int characterLevel;

    private void Awake() {
        model = transform.GetChild(0).GetComponent<SpriteRenderer>();
        triangle = transform.GetChild(1).gameObject;
        cdMark = transform.GetChild(2).gameObject;
        nameText = transform.GetChild(3).GetComponent<TextMeshPro>();
        timer = transform.GetChild(4).GetComponent<TextMeshPro>();
        triangle.SetActive(false);
        cdMark.SetActive(false);
        selected = false;
        timer.enabled = false;
    }
    public void Setup(CharacterStats characterStats) {
        this.characterStats = characterStats;
        model.sprite = this.characterStats.characterSprite;
        model.color = this.characterStats.spriteColor;
        nameText.SetText(this.characterStats.characterName);
        //characterName = this.characterStats.characterName;
        //characterStrength = this.characterStats.baseStrength;
        //characterHealth = this.characterStats.baseHealth;
        //characterWeapon = this.characterStats.baseWeapon;
        //characterHit = this.characterStats.baseHit;
        //characterDodge = this.characterStats.baseDodge;
        //characterLevel = this.characterStats.baseLevel;
    }
    public void Highlight(bool state) {
        Color color = model.color;
        color.a = state ? 1 : 0.5f;
        model.color = color;
        selected = state;
    }
    public void NotActive(bool state) {
        cdMark.SetActive(state);
    }
    public void ShowTriangle(bool state) {
        triangle.SetActive(state);
    }
    public string GetName() {
        return characterStats.characterName;
    }
    public bool IsSelected() {
        return selected;
    }
    public void ToggleTimer(bool state) {
        timer.enabled = state;
    }
    public void UpdateTimer(int time) {
        timer.SetText(time.ToString());
    }
}
