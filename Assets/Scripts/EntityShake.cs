using System.Collections;
using UnityEngine;

public class EntityShake : MonoBehaviour
{
    [SerializeField] private float magnitude;
    [SerializeField] private float duration;
    private Transform model;

    private void Awake() {
        model = transform.GetChild(0);
    }

    public void Shake() {
        StartCoroutine(ShakeRoutine());
    }

    float GetLerpValue(float t) {
        float lerp = 0.5f;
        if (t < duration/5) {
            lerp += f1(t);
        } else if (t < duration * 2 / 5) {
            lerp = 1 + f2(t);
        } else if (t< duration * 3 / 5) {
            lerp = f3(t);
        } else if (t < duration * 4 / 5) {
            lerp = 0.75f+f4(t);
        } else if (t < duration) {
            lerp = 0.25f+f5(t);
        }
        return lerp;
    }

    float f1(float x) {
        return 0.5f / (duration / 5) * x;
    }

    float f2(float x) {
        return -1 / (duration / 5) * (x- duration / 5);
    }

    float f3(float x) {
        return 0.75f / (duration / 5) * (x - duration*2 / 5);
    }

    float f4(float x) {
        return -0.5f / (duration / 5) * (x - duration * 3 / 5);
    }

    float f5(float x) {
        return 0.25f / (duration / 5) * (x - duration * 4 / 5);
    }

    IEnumerator ShakeRoutine() {
        Vector3 originalPos = model.localPosition;
        float a = -magnitude / 2;
        float b = magnitude / 2;
        float elapsed = 0;
        while (elapsed <= duration) {
            float lerpValue = GetLerpValue(elapsed);
            float xOffset = Mathf.Lerp(a,b, lerpValue);
            model.localPosition = originalPos+ new Vector3(xOffset, originalPos.y);
            elapsed += Time.deltaTime;
            yield return null;
        }
        model.localPosition = originalPos;
    }
}
