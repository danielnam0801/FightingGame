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

    [SerializeField] CinemachineFreeLook Player1FCam;
    [SerializeField] CinemachineFreeLook Player2FCam;

    [SerializeField] PlayableDirector introDirector;
    [SerializeField] PlayableDirector winDirector;

    private TrackAsset winTrack = null;
    private TrackAsset winCameraTrack = null;
    private TrackAsset loseTrack = null;
    private TrackAsset loseCameraTrack = null;

    private TimelineAsset winTimeline;
    private TimelineAsset loseTimeline;
    private FightManager fightManager;

    string winTrackName = "Winning";
    string loseTrackName = "Losing";
    string winCameraTrackName = "WinCameraSpin";
    string loseCameraTrackName = "LoseCameraSpin";

    public bool Able { get; set; } = false;
    public bool IntroTimeLineDone = false;


    [SerializeField] float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;
    Vector3 vec = new Vector3(0.17f, -0.2f, 3.1f);

    public void Init(GameObject player1, GameObject player2)
    {
        fightManager = GameObject.Find("FightManager").GetComponent<FightManager>();
        Able = true;
        introDirector.Play();
        CameraInit(player1, player2);
    }

    public void CameraInit(GameObject player1, GameObject player2)
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

        Player1FCam = GameObject.Find("PlayerFreeLook").GetComponent<CinemachineFreeLook>();
        Player2FCam = GameObject.Find("Player2FreeLook").GetComponent<CinemachineFreeLook>();

        Player1FCam.Follow = player1.transform;
        Player1FCam.LookAt = player1.transform.Find("Chest");
        Player2FCam.Follow = player2.transform;
        Player2FCam.LookAt = player2.transform.Find("Chest");

        Player1FCam.m_XAxis.m_InputAxisName = ""; // X축 입력 무시
        Player1FCam.m_YAxis.m_InputAxisName = ""; // Y축 입력 무시

        Player2FCam.m_XAxis.m_InputAxisName = "";
        Player2FCam.m_YAxis.m_InputAxisName = "";

        introDirector = GameObject.Find("IntroTimeLine").GetComponent<PlayableDirector>();

        //Player1FCam.gameObject.SetActive(false);
        //Player2FCam.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Vector3.Distance(introCam.transform.localPosition, vec) <= 0.01f)
        {
            if(IntroTimeLineDone == false)
            {
                IntroTimeLineDone = true;
                introCam.gameObject.SetActive(false);
                fightManager.StartUI();
            }
        }
    }

    public void Shake(bool flag)
    {
        if (flag)
            StartCoroutine(nameof(ShakeCamera));
        else
            StartCoroutine(nameof(StopShake));
    }

    public void Win(PlayerType winPlayer)
    {
        if(winPlayer == PlayerType.player1)
        {
            StartCoroutine(WinnerRotateCamera(Player1FCam));
        }
        else if(winPlayer == PlayerType.player2)
        {
            StartCoroutine(WinnerRotateCamera(Player2FCam));
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
    IEnumerator WinnerRotateCamera(CinemachineFreeLook cam)
    {
        cam.gameObject.SetActive(true);
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

    IEnumerator LoserRotateCamera(CinemachineFreeLook cam)
    {
        cam.gameObject.SetActive(true);
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
