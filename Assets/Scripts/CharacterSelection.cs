using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public Vector2[] locations;
    public CharacterStats[] characterStats;
    public GameObject characterPrefab;
    private CharacterView[] characters;
    private int selectedIndex;
    private int maxSelection;
    private int selectedCount;

    private void Start() {
        maxSelection = 6;
        selectedCount = 0;
        characters = new CharacterView[15];
        selectedIndex = 0;
        Setup();
    }
    private void Update() {
        SelectCharacter();
        MoveCursor();
        // update timers
    }
    void Setup() {
        for (int i = 0; i < 15; i++) {
            GameObject CharacterGO = Instantiate(characterPrefab);
            CharacterGO.transform.SetParent(transform);
            CharacterGO.transform.localPosition = locations[i];
            CharacterView cv = CharacterGO.GetComponent<CharacterView>();
            string cvName = characterStats[i].characterName;
            CoolDownManager.InitializeCooldown(cvName);
            cv.Setup(characterStats[i]);
            if (!PlayerPrefs.HasKey(cvName)) {
                PlayerPrefs.SetInt(cvName, 0);
            } else {
                if (PlayerPrefs.GetInt(cvName) == 1) {
                    cv.Highlight(true);
                    selectedCount++;
                } else if (!CoolDownManager.IsCooldownOver(cvName)) {
                    cv.NotActive(true);
                    float time = CoolDownManager.GetCountDown(cvName);
                    cv.ToggleTimer(true);
                    cv.UpdateTimer((int)time);
                    StartCoroutine(CountDownRoutine(cv,time,()=> {
                        cv.ToggleTimer(false);
                        cv.NotActive(false);
                        CoolDownManager.RemoveCooldown(cvName);
                    }));
                }
            }
            characters[i] = cv;
        }
        characters[selectedIndex].ShowTriangle(true);
        PlayerPrefs.SetInt("Members", selectedCount);
    }
    void SelectCharacter() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            CharacterView cv = characters[selectedIndex];
            if (!cv.IsSelected()) {
                if (CoolDownManager.IsCooldownOver(cv.GetName())) {
                    if (selectedCount < maxSelection) {
                        cv.Highlight(true);
                        selectedCount++;
                        PlayerPrefs.SetInt(cv.GetName(), 1);
                        PlayerPrefs.SetInt("Members", selectedCount);
                    }
                }
            } else {
                cv.Highlight(false);
                selectedCount--;
                PlayerPrefs.SetInt(cv.GetName(), 0);
                PlayerPrefs.SetInt("Members", selectedCount);
            }
        }
    }
    void MoveCursor() {
        int newIndex = selectedIndex;
        if (Input.GetKeyDown(KeyCode.D)) {
            int row = getRow(newIndex);
            newIndex++;
            int col = newIndex % 5;
            newIndex = row * 5 + col;
        } else if (Input.GetKeyDown(KeyCode.A)) {
            int row = getRow(newIndex);
            newIndex--;
            if (newIndex < 0) {
                newIndex = 4;
            }
            int col = newIndex % 5;
            newIndex = row * 5 + col;
        } else if (Input.GetKeyDown(KeyCode.S)) {
            newIndex += 5;
            newIndex %= 15;

        } else if (Input.GetKeyDown(KeyCode.W)) {
            newIndex -= 5;
            if (newIndex < 0) {
                newIndex += 15;
            }
            newIndex %= 15;
        }
        if (newIndex != selectedIndex) {
            characters[selectedIndex].ShowTriangle(false);
            characters[newIndex].ShowTriangle(true);
            selectedIndex = newIndex;
        }
    }
    int getRow(int index) {
        return Mathf.FloorToInt((index %= 15) / 5);
    }
    IEnumerator CountDownRoutine(CharacterView cv, float time, Action callback) {
        int prevInt = (int)time;
        while (time > 0) {
            time -= Time.unscaledDeltaTime;
            if (prevInt != (int)time) {
                prevInt = (int)time;
                cv.UpdateTimer(prevInt);
            }
            yield return null;
        }
        cv.UpdateTimer(0);
        callback();
    }
}
