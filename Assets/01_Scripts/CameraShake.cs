using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.VFX;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin perlin;
    [SerializeField] VisualEffect hitEffect;

   [SerializeField]  float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;
    bool isShaking = false;
    float shakeTimeElapsed = 0f;

    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(ShakeCamera());
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(RotateCamera());
        }
    }
    IEnumerator ShakeCamera()
    {
        perlin.m_AmplitudeGain = hitAmplitudeGain;
        perlin.m_FrequencyGain = hitFrequencyGain;
        hitEffect.Play();
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(StopShake());
    }
    IEnumerator StopShake()
    {
        perlin.m_AmplitudeGain = 0;
        perlin.m_FrequencyGain = 0;
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator RotateCamera()
    {
        transform.Rotate(Vector3.one, 90);
        yield return null;
    }
}
