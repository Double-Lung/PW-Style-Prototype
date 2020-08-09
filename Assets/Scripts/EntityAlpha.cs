using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityAlpha : MonoBehaviour
{
    [SerializeField] private float fadeDuration;
    [SerializeField] private Image healthBarBG;
    [SerializeField] private Image healthBarFill;
    private TextMeshPro nameText;
    private SpriteRenderer model;

    private void Awake() {
        model = GetComponentInChildren<SpriteRenderer>();
        nameText = GetComponentInChildren<TextMeshPro>();
    }
    public void resetAlpha() {
        UnityEngine.Color color = model.color;
        color.a = 0.5f;
        model.color = color;
    }
    public void highLight() {
        UnityEngine.Color color = model.color;
        color.a = 1f;
        model.color = color;
    }
    public void alphaFlicker(Action onFlickerComplete) {
        StartCoroutine(alphaFlickerRoutine(onFlickerComplete));
    }
    IEnumerator alphaFlickerRoutine(Action onFlickerComplete) {
        UnityEngine.Color color = model.color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0.5f;
        model.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        model.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0.5f;
        model.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        model.color = color;
        yield return new WaitForSeconds(0.1f);
        onFlickerComplete();
    }
    public void alphaDeath() {
        StartCoroutine(alphaDeathRoutine());
    }
    IEnumerator alphaDeathRoutine() {
        UnityEngine.Color color1 = model.color;
        UnityEngine.Color color2 = healthBarBG.color;
        UnityEngine.Color color3 = healthBarFill.color;
        UnityEngine.Color color4 = nameText.color;
        float originalAlpha1 = color1.a;
        float originalAlpha2 = color2.a;
        float originalAlpha3 = color3.a;
        float originalAlpha4 = color4.a;
        while (color1.a>0) {
            color1.a -= originalAlpha1 * Time.deltaTime / fadeDuration;
            model.color = color1;
            color2.a -= originalAlpha2 * Time.deltaTime / fadeDuration;
            healthBarBG.color = color2;
            color3.a -= originalAlpha3 * Time.deltaTime / fadeDuration;
            healthBarFill.color = color3;
            color4.a -= originalAlpha4 * Time.deltaTime / fadeDuration;
            nameText.color = color4;
            yield return null;
        }
        color1.a = 0;
        model.color = color1;
        color2.a = 0;
        healthBarBG.color = color2;
        color3.a = 0;
        healthBarFill.color = color3;
        color4.a = 0;
        nameText.color = color4;
        gameObject.SetActive(false);
    }
}
