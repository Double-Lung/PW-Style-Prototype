using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI notification;
    public Canvas notificationCanvas;
    public Canvas wonCanvas;
    public Canvas speedControl;
    public Canvas pauseCanvas;
    private int currentScaledTime;

    private void Awake() {
        if (instance!=null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    private void Start() {
        notificationCanvas.enabled = false;
        wonCanvas.enabled = false;
        speedControl.enabled = false;
        pauseCanvas.enabled = false;
        currentScaledTime = PlayerPrefs.GetInt("Speed");
    }

    public void ChangeNotification(string text) {
        notification.SetText(text);
    }

    public void ToggleCanvas(bool state) {
        notificationCanvas.enabled = state;
    }

    public void fastforward() {
        switch (currentScaledTime) {
            case 1:
                currentScaledTime = 2;
                break;
            case 2:
                currentScaledTime = 4;
                break;
            case 4:
                currentScaledTime = 8;
                break;
            case 8:
                currentScaledTime = 1;
                break;
            default:
                currentScaledTime = 1;
                break;
        }
        Time.timeScale = currentScaledTime;
    }

    public void resume() {
        Time.timeScale = currentScaledTime;
    }

    public void pause() {
        Time.timeScale = 0;
    }

    public void LoadNextLevel() {
        int current = PlayerPrefs.GetInt("level");
        PlayerPrefs.SetInt("level", current+1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Return() {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("Speed", 1);
        SceneManager.LoadScene(0);
    }
    public void toggleWonCanvas(bool state) {
        wonCanvas.enabled = state;
    }

    public void toggleSpeedControl(bool state) {
        speedControl.enabled = state;
    }

    public void ResumeLevel() {
        Time.timeScale = currentScaledTime;
        pauseCanvas.enabled = false;
    }
    public void togglePauseMenu(bool state) {
        pauseCanvas.enabled = state;
    }

}
