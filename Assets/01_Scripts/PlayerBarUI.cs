using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBarUI : MonoBehaviour
{
    [SerializeField] private float chipSpeed = 2f;
    [SerializeField] private float _duration;
    [SerializeField] private float _power;

    [Header("Health")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [Header("Ultimate")]
    [SerializeField] private Image frontUltimateBar;
    [SerializeField] private Image backUltimateBar;

    public void UpdateHealthUI(float health, float maxHealth, float lerpTimer)
    {
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            BarShake();
            frontHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
    }

    private void BarShake()
    {
        transform.DOShakePosition(_duration, _power);
    }

    public void UpdateUltimateUI(float power, float maxPower, float lerpTimer)
    {
        float fillF = frontUltimateBar.fillAmount;
        float hFraction = power / maxPower;

        if (fillF < hFraction)
        {
            backUltimateBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontUltimateBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }
}
