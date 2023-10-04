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
    [SerializeField] CinemachineFreeLook blueGuyFCam;
    [SerializeField] CinemachineFreeLook redGuyFCam;
    //[SerializeField] VisualEffect hitEffect;
    [SerializeField] PlayableDirector introDirector;
    [SerializeField] PlayableDirector winningDirector;
    [SerializeField] PlayableDirector losingDirector;

    [SerializeField] float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;
    Vector3 vec = new Vector3(0.17f, -0.2f, 3.1f);
    private bool isWin = false;
    private bool isLose = false;

    private void Awake()
    {

        vCam = GameObject.Find("IntroCamera").GetComponent<CinemachineVirtualCamera>();
        vCam2 = GameObject.Find("ShakeCamera").GetComponent<CinemachineVirtualCamera>();
        perlin = vCam2.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        blueGuyFCam = GameObject.Find("BlueGuyFreeLook").GetComponent<CinemachineFreeLook>();
        redGuyFCam = GameObject.Find("RedGuyFreeLook").GetComponent<CinemachineFreeLook>();


        blueGuyFCam.m_XAxis.m_InputAxisName = ""; // X축 입력 무시
        blueGuyFCam.m_YAxis.m_InputAxisName = ""; // Y축 입력 무시

        redGuyFCam.m_XAxis.m_InputAxisName = "";
        redGuyFCam.m_YAxis.m_InputAxisName = "";

        winningDirector = GameObject.Find("PlayerUnit").GetComponent<PlayableDirector>();
        losingDirector = GameObject.Find("AIUnit").GetComponent<PlayableDirector>();
        introDirector = GameObject.Find("IntroTimeLine").GetComponent<PlayableDirector>();

        blueGuyFCam.gameObject.SetActive(false);
        redGuyFCam.gameObject.SetActive(false);
    }

    void Start()
    {
        introDirector.Play();
        //winningDirector.Stop();
        //losingDirector.Stop();

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
        //hitEffect.Play();
    }
    IEnumerator StopShake()
    {
        perlin.m_AmplitudeGain = 0;
        perlin.m_FrequencyGain = 0;
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator WinnerRotateCamera()
    {
        blueGuyFCam.gameObject.SetActive(true);
        introDirector.Stop();
        winningDirector.Play();
        yield return null;
    }
    IEnumerator LoserRotateCamera()
    {
        redGuyFCam.gameObject.SetActive(true);
        introDirector.Stop();
        losingDirector.Play();
        yield return null;
    }
}

