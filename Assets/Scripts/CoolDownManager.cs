using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoolDownManager
{
    private static float coolDownDuration = 300;
    private static List<string> InCoolDown = new List<string>();

    public static void InitializeCooldown(string key) {
        string realKey = key + "Cooldown";
        if (!PlayerPrefs.HasKey(realKey)) {
            PlayerPrefs.SetFloat(realKey, 0);
            return;
        }
        float exitTime = PlayerPrefs.GetFloat(realKey);
        if (exitTime > 0) {
            if (!InCoolDown.Contains(realKey)) {
                InCoolDown.Add(realKey);
            }
        } else {
            PlayerPrefs.SetFloat(realKey, 0);
            if (InCoolDown.Contains(realKey)) {
                InCoolDown.Remove(realKey);
            }
        }
    }

    public static void ExitWork() {

        List<string> tempList = new List<string>();
        foreach (string realkey in InCoolDown) {
            float exitTime = PlayerPrefs.GetFloat(realkey);
            float timeLeft = exitTime - Time.realtimeSinceStartup;
            tempList.Add(realkey);
            if (timeLeft > 0) {
                PlayerPrefs.SetFloat(realkey, timeLeft);
            } else {
                PlayerPrefs.SetFloat(realkey, 0);
            }
        }
        InCoolDown = tempList;
    }
    
    public static void SetCoolDown(string key) {
        string realKey = key + "Cooldown";
        PlayerPrefs.SetFloat(realKey, Time.realtimeSinceStartup+ coolDownDuration);
        InCoolDown.Add(realKey);
    }

    public static void RemoveCooldown(string key) {
        string realKey = key + "Cooldown";
        InCoolDown.Remove(realKey);
        PlayerPrefs.SetFloat(realKey, 0);
    }

    public static bool IsCooldownOver(string key) {
        string realKey = key + "Cooldown";
        float exitTime = PlayerPrefs.GetFloat(realKey);
        return (exitTime <= 0);
    }

    public static float GetCountDown(string key) {
        string realKey = key + "Cooldown";
        float exitTime = PlayerPrefs.GetFloat(realKey);
        float timeLeft = exitTime - Time.realtimeSinceStartup;
        if (timeLeft < 0) {
            return 0;
        }
        return timeLeft;
    }
}
