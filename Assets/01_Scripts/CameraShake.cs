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
    [SerializeField] CinemachineFreeLook blueGuyFCam;
    [SerializeField] CinemachineFreeLook redGuyFCam;
    [SerializeField] VisualEffect hitEffect;
    [SerializeField] PlayableDirector winningDirector;
    [SerializeField] PlayableDirector losingDirector;
    [SerializeField] TimelineAsset timeline;

    [SerializeField] float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;
    private bool isWin = false;

    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        perlin = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        blueGuyFCam.m_XAxis.m_InputAxisName = ""; // X축 입력 무시
        blueGuyFCam.m_YAxis.m_InputAxisName = ""; // Y축 입력 무시
        redGuyFCam.m_XAxis.m_InputAxisName = "";
        redGuyFCam.m_YAxis.m_InputAxisName = "";
        winningDirector = GameObject.Find("BlueGuy_123").GetComponent<PlayableDirector>();
        losingDirector = GameObject.Find("RedGuy_123").GetComponent<PlayableDirector>();
    }

    void Start()
    {
        winningDirector.Stop();
        losingDirector.Stop();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)&&!isWin)
        {
            StartCoroutine(ShakeCamera());
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(WinnerRotateCamera());
            vCam.gameObject.SetActive(false);
            winningDirector.Stop();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(LoserRotateCamera());
            vCam.gameObject.SetActive(false);
            losingDirector.Stop();
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
    IEnumerator WinnerRotateCamera()
    {
        winningDirector.Play();
        yield return null;
    }
    IEnumerator LoserRotateCamera()
    {
        losingDirector.Play();
        yield return null;
    }
}

