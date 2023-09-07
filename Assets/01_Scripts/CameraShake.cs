using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.VFX;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin perlin;
    [SerializeField] CinemachineFreeLook fCam;
    [SerializeField] VisualEffect hitEffect;
    [SerializeField] PlayableDirector winningDirector;
    [SerializeField] TimelineAsset timeline;

    [SerializeField] float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;
    private bool isWin = false;

    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        fCam.m_XAxis.m_InputAxisName = ""; // X축 입력 무시
        fCam.m_YAxis.m_InputAxisName = ""; // Y축 입력 무시
        winningDirector = GameObject.Find("BlueGuy_123").GetComponent<PlayableDirector>();
        //timeline = 
    }

    void Start()
    {

        winningDirector.Stop();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)&&!isWin)
        {
            StartCoroutine(ShakeCamera());
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(RotateCamera());
            vCam.gameObject.SetActive(false);
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
        winningDirector.Play();
        yield return null;
    }
}

