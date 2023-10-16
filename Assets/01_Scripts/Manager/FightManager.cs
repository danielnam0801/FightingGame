using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System;
using TMPro;

public class FightManager : Singleton<FightManager>
{
    [Space(30)]
    [Header("Player")]
    [SerializeField] GameObject aiModel;
    [SerializeField] GameObject playerModel;
    [SerializeField] Transform p1SpawnPoint;
    [SerializeField] Transform p2SpawnPoint;
    [SerializeField] Image roundUI;
    [SerializeField] Image fightUI;
    [SerializeField] RawImage player1face;
    [SerializeField] RawImage player2face;
    [SerializeField] TextMeshProUGUI timer;

    PlayerInfo player1Info;
    PlayerInfo player2Info;
    CameraManager camManager;

    GameObject player1;
    GameObject player2;

    public bool RoundStart = false;
    public bool RoundEnd = false;
    public bool PlayerWin1 = false;
    public bool PlayerWin2 = false;

    [SerializeField]
    private float gameTime = 90f;


    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        player1Info = DataManager<PlayerInfo>.LoadData(Core.Path.Player1Path);
        player2Info = DataManager<PlayerInfo>.LoadData(Core.Path.Player2Path);

        SetPlayer();
        SoundManager.Instance.PlaySound(SoundType.BGM);
    }

    private void SetPlayer()
    {
        player1 = (player1Info.mode == PlayerType.AI)
            ? Instantiate(aiModel, p1SpawnPoint.position, Quaternion.Euler(0, 0, 0))
            : Instantiate(playerModel, p1SpawnPoint.position, Quaternion.Euler(0, 0, 0));

        player2 = (player2Info.mode == PlayerType.AI)
            ? Instantiate(aiModel, p2SpawnPoint.position, Quaternion.Euler(0, 180, 0))
            : Instantiate(playerModel, p2SpawnPoint.position, Quaternion.Euler(0, 180, 0));


        PlayerSetting p1Setting = player1.GetComponent<PlayerSetting>();
        p1Setting.SetPlayerNum(PlayerSpawnState.left);
        p1Setting.SetVisual(player1Info.character.material);

        PlayerSetting p2Setting = player2.GetComponent<PlayerSetting>();
        p2Setting.SetPlayerNum(PlayerSpawnState.right);
        p2Setting.SetVisual(player2Info.character.material);

        player1face.texture = player1Info.character.GetRenderTexture();
        player2face.texture = player2Info.character.GetRenderTexture();

        camManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        camManager.Init(player1, player2);
    }

    public void StartUI()
    {
        SoundManager.Instance.PlaySound(SoundType.Start);

        roundUI.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(roundUI.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutQuad));
        seq.Append(roundUI.transform.DOScale(Vector3.zero, 0.7f).SetEase(Ease.InQuart)).AppendCallback(() => fightUI.gameObject.SetActive(true));
        seq.Append(fightUI.rectTransform.DOScale(Vector3.one, 1f).SetEase(Ease.OutQuad));
        seq.Append(fightUI.rectTransform.DOScale(Vector3.zero, 0.7f).SetEase(Ease.InQuart));
        seq.onComplete = GameStart;
    }
       
    private void GameStart()
    {
        RoundStart = true;
        player1.GetComponent<Unit>().Able = true;
        player2.GetComponent<Unit>().Able = true;
    }

    private void Update()
    {
        if (RoundEnd == true || RoundStart == false) return;
    
        TimeOn();

        if(gameTime < 0)
        {
            GameOver();
        }
    }

    public void Lose(PlayerSpawnState state)
    {
        if(state == PlayerSpawnState.left)
        {
            PlayerWin2 = true;
            camManager.Win(PlayerType.player2);
        }
        else
        {
            PlayerWin1 = true;
            camManager.Win(PlayerType.player1);
        }
        RoundEnd = true;

    }  

    private void GameOver()
    {
        RoundEnd = true;
    }

    private void TimeOn()
    {
        gameTime -= Time.deltaTime;
        timer.text = Math.Ceiling(gameTime).ToString();
    }

    private void OnDisable()
    {
        player1Info = null;
        player2Info = null;
    }
}
