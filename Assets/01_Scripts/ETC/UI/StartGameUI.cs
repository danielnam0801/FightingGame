using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class StartGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _onePlayer;
    [SerializeField] private GameObject _twoPlayer;
    [SerializeField] private Image _onePlayerFace;
    [SerializeField] private Image _twoPlayerFace;
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private Image readyImg;
    [SerializeField] private Image fightImg;

    public UnityEvent startEvent; //게임 시작하면
    public UnityEvent playingEvent; //게임 중일때

    public int Round = 0;

    private void Awake()
    {
        _onePlayer.SetActive(false);
        _twoPlayer.SetActive(false);
        _onePlayerFace.enabled = false;
        _twoPlayerFace.enabled = false;
        _timer.enabled = false;
        readyImg.enabled = false;
        fightImg.enabled = false;

        Invoke("GameStart", 2f);
    }

    public void GameStart()
    {
        startEvent.Invoke();
        StartCoroutine(GameStartCoroutine());
    }

    private IEnumerator GameStartCoroutine()
    {
        _onePlayer.SetActive(true);
        _twoPlayer.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        FadeFaceImg(_onePlayerFace);
        FadeFaceImg(_twoPlayerFace);
        yield return new WaitForSeconds(1.1f);
        StartRound();
    }

    private void FadeFaceImg(Image img)
    {
        img.enabled = true;
        img.DOFade(1, 1f);
    }

    public void StartRound()
    {
        _timer.enabled = false;
        readyImg.enabled = false;
        fightImg.enabled = false;

        _timer.enabled = false;
        StartCoroutine(RoundStartCoroutine());
    }

    private IEnumerator RoundStartCoroutine()
    {
        readyImg.enabled = true;
        Round++;
        yield return new WaitForSeconds(1.6f);
        readyImg.enabled = false;
        fightImg.enabled = true;
        yield return new WaitForSeconds(0.5f);
        fightImg.enabled = false;
        yield return new WaitForSeconds(0.5f);
        _timer.enabled = true;
    }
}
