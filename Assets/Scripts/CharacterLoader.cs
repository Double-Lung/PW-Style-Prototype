using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader:MonoBehaviour
{
    public CharacterStats[] allChars;

    public List<CharacterStats> GetActiveCharacters() {
        List<CharacterStats> result = new List<CharacterStats>();
        foreach (CharacterStats character in allChars) {
            if (PlayerPrefs.GetInt(character.characterName) == 1) {
                result.Add(character);
            }
        }
        return result;
    }
}
