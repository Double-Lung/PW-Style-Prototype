using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    private TextMeshProUGUI Notification;

    private void Awake() {
        Notification = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Notification.enabled = false;
    }
    public void LevelLocked() {
        Notification.color = Color.red;
        Notification.SetText("Level Locked");
        Notification.enabled = true;
    }
    public void NoMember() {
        Notification.color = Color.red;
        Notification.SetText("No Character");
        Notification.enabled = true;
    }
    public void CharacterSelection() {
        SceneManager.LoadScene("CharacterSelection");
    }
    public void ClearSave() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Progress", 0);
        PlayerPrefs.SetInt("Members", 0);
        Notification.color = Color.green;
        Notification.SetText("Save Cleared");
        Notification.enabled = true;
    }
    public void Quit() {
        CoolDownManager.ExitWork();
        Application.Quit();
    }
}
