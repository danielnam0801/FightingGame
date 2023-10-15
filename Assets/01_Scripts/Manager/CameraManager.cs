using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.VFX;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera introCam;
    [SerializeField] CinemachineVirtualCamera shakeCam;

    private CinemachineBasicMultiChannelPerlin perlin;

    [SerializeField] CinemachineFreeLook PlayerFCam;
    [SerializeField] CinemachineFreeLook AIFCam;

    [SerializeField] PlayableDirector introDirector;
    [SerializeField] PlayableDirector winDirector;



    private TrackAsset winTrack = null;
    private TrackAsset winCameraTrack = null;
    private TrackAsset loseTrack = null;
    private TrackAsset loseCameraTrack = null;

    private TimelineAsset winTimeline;
    private TimelineAsset loseTimeline;

    string winTrackName = "Winning";
    string loseTrackName = "Losing";
    string winCameraTrackName = "WinCameraSpin";
    string loseCameraTrackName = "LoseCameraSpin";

    public bool Able { get; set; } = false;
    public bool IntroTimeLineDone = false;


    [SerializeField] float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;
    Vector3 vec = new Vector3(0.17f, -0.2f, 3.1f);

    private void Awake()
    {
        winTimeline = winDirector.playableAsset as TimelineAsset;
        loseTimeline = winDirector.playableAsset as TimelineAsset;



        winTrack = winTimeline.GetOutputTrack(0);
        winCameraTrack = winTimeline.GetOutputTrack(0);
        loseTrack = loseTimeline.GetOutputTrack(0);
        loseCameraTrack = loseTimeline.GetOutputTrack(0);

        introCam = GameObject.Find("IntroCamera").GetComponent<CinemachineVirtualCamera>();
        shakeCam = GameObject.Find("ShakeCamera").GetComponent<CinemachineVirtualCamera>();
        perlin = shakeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        PlayerFCam = GameObject.Find("PlayerFreeLook").GetComponent<CinemachineFreeLook>();
        AIFCam = GameObject.Find("AIFreeLook").GetComponent<CinemachineFreeLook>();



        PlayerFCam.m_XAxis.m_InputAxisName = ""; // X�� �Է� ����
        PlayerFCam.m_YAxis.m_InputAxisName = ""; // Y�� �Է� ����

        AIFCam.m_XAxis.m_InputAxisName = "";
        AIFCam.m_YAxis.m_InputAxisName = "";

        introDirector = GameObject.Find("IntroTimeLine").GetComponent<PlayableDirector>();

        PlayerFCam.gameObject.SetActive(false);
        AIFCam.gameObject.SetActive(false);
    }

    public void Init()
    {
        Able = true;
        introDirector.Play();
    }

    private void Update()
    {
        if (!Able) return;

        if (introCam.transform.position == vec)
        {
            IntroTimeLineDone = true;
            introCam.gameObject.SetActive(false);
        }
        #region Debug�ڵ�
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    introDirector.Stop();
        //    StartCoroutine(ShakeCamera());
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    introDirector.Stop();
        //    StartCoroutine(WinnerRotateCamera());
        //    shakeCam.gameObject.SetActive(false);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    introDirector.Stop();
        //    StartCoroutine(LoserRotateCamera());
        //    shakeCam.gameObject.SetActive(false);
        //}
        #endregion
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
        introDirector.Stop();
        foreach (TrackAsset track in winTimeline.GetOutputTracks())
        {
            if (track.name == loseTrackName)
            {
                track.locked = true;
                track.muted = true;
                loseTrack = track;
                loseCameraTrack = track;
            }
        }
        winDirector.Play();

        yield return null;
    }
    IEnumerator LoserRotateCamera()
    {
        PlayerFCam.gameObject.SetActive(true);
        introDirector.Stop();
        foreach (TrackAsset track in loseTimeline.GetOutputTracks())
        {
            if (track.name == winTrackName)
            {
                track.locked = true;
                track.muted = true;
                winTrack = track;
                winCameraTrack = track;
            }
        }
        winDirector.Play();

        yield return null;
    }
}