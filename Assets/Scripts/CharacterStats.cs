using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Weapons {
    Koucha,
    Pistol,
    Rifle,
    AR,
    Knife,
    Fist,
    Shotgun,
    SMG,
    MG
}
[CreateAssetMenu(fileName ="Generic Character", menuName = "ScriptableObjects/CharacterStats", order=0)]
public class CharacterStats : ScriptableObject
{
    public string characterName;
    public int baseLevel;
    public int baseHealth;
    public int baseStrength;
    public int baseDodge;
    public int baseHit;
    public Weapons baseWeapon;
    public Sprite characterSprite;
    public Color spriteColor;
    public bool isPlayerTeam;
}
