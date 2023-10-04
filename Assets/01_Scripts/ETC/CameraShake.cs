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
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] CinemachineVirtualCamera vCam2;

    private CinemachineBasicMultiChannelPerlin perlin;

    [SerializeField] CinemachineFreeLook PlayerFCam;
    [SerializeField] CinemachineFreeLook AIFCam;

    [SerializeField] PlayableDirector introDirector;
    [SerializeField] TrackAsset track;


    [SerializeField] float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;
    Vector3 vec = new Vector3(0.17f, -0.2f, 3.1f);
    private bool isWin = false;
    private bool isLose = false;

    private void Awake()
    {
        track.muted = true;
        vCam = GameObject.Find("IntroCamera").GetComponent<CinemachineVirtualCamera>();
        vCam2 = GameObject.Find("ShakeCamera").GetComponent<CinemachineVirtualCamera>();
        perlin = vCam2.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        PlayerFCam = GameObject.Find("PlayerFreeLook").GetComponent<CinemachineFreeLook>();
        AIFCam = GameObject.Find("AIFreeLook").GetComponent<CinemachineFreeLook>();


        PlayerFCam.m_XAxis.m_InputAxisName = ""; // X축 입력 무시
        PlayerFCam.m_YAxis.m_InputAxisName = ""; // Y축 입력 무시

        AIFCam.m_XAxis.m_InputAxisName = "";
        AIFCam.m_YAxis.m_InputAxisName = "";

        introDirector = GameObject.Find("IntroTimeLine").GetComponent<PlayableDirector>();

        PlayerFCam.gameObject.SetActive(false);
        AIFCam.gameObject.SetActive(false);
    }

    void Start()
    {
        introDirector.Play();
    }
    private void Update()
    {
        if (vCam.transform.position == vec)
        {
            vCam.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            introDirector.Stop();
            StartCoroutine(ShakeCamera());
        }
        if (Input.GetKeyDown(KeyCode.Q) )
        {
            introDirector.Stop();
            StartCoroutine(WinnerRotateCamera());
            vCam2.gameObject.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(LoserRotateCamera());
            vCam2.gameObject.SetActive(false);
        }
    }



    IEnumerator ShakeCamera()
    {
        perlin.m_AmplitudeGain = hitAmplitudeGain;
        perlin.m_FrequencyGain = hitFrequencyGain;
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
        PlayerFCam.gameObject.SetActive(true);

        yield return null;
    }
    IEnumerator LoserRotateCamera()
    {
        AIFCam.gameObject.SetActive(true);
        introDirector.Stop();
        yield return null;
    }
}

