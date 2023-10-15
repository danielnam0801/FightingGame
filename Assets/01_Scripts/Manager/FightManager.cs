using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public class FightManager : MonoBehaviour
{   
    [Space(30)]
    [Header("Player")]
    [SerializeField] GameObject aiModel;
    [SerializeField] GameObject playerModel;
    [SerializeField] Transform p1SpawnPoint;
    [SerializeField] Transform p2SpawnPoint;
    [SerializeField] PlayableDirector introTimeline;
    [SerializeField] PlayableDirector winningTimeline;
    [SerializeField] TimelineAsset winningTimeLineAsset;
    [SerializeField] Image roundUI;
    [SerializeField] Image fightUI;

    private List<GameObject> tracks = new List<GameObject>();

    PlayerInfo player1Info;
    PlayerInfo player2Info;

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        player1Info = DataManager<PlayerInfo>.LoadData(Core.Path.Player1Path);
        player2Info = DataManager<PlayerInfo>.LoadData(Core.Path.Player2Path);

        SetPlayer();
    }

    private void SetPlayer()
    {
        GameObject p1 = (player1Info.mode == PlayerType.AI)
            ? Instantiate(aiModel, p1SpawnPoint.position, Quaternion.Euler(0, 0, 0))
            : Instantiate(playerModel, p1SpawnPoint.position, Quaternion.Euler(0, 0, 0));

        GameObject p2 = (player2Info.mode == PlayerType.AI)
            ? Instantiate(aiModel, p2SpawnPoint.position, Quaternion.Euler(0, 180, 0))
            : Instantiate(playerModel, p2SpawnPoint.position, Quaternion.Euler(0, 180, 0));


        PlayerSetting p1Setting = p1.GetComponent<PlayerSetting>();
        p1Setting.SetPlayerNum(PlayerSpawnState.left);
        p1Setting.SetVisual(player1Info.character.material);
        
        PlayerSetting p2Setting = p2.GetComponent<PlayerSetting>();
        p2Setting.SetPlayerNum(PlayerSpawnState.right);
        p2Setting.SetVisual(player2Info.character.material);


        tracks.Add(GameObject.Find("PlayerFreeLook"));
        tracks.Add(p1);

        PlayableDirector p1TimeLine = p1.GetComponent<PlayableDirector>();
        PlayableDirector p2TimeLine = p1.GetComponent<PlayableDirector>();
        BindTimelineTracks(p1TimeLine);
        BindTimelineTracks(p2TimeLine);
        
    }

    public void BindTimelineTracks(PlayableDirector timeline)
    {
        Debug.Log("Binding Timeline Tracks!");
        winningTimeLineAsset = (TimelineAsset)timeline.playableAsset;
        // iterate through tracks and map the objects appropriately
     
        List<PlayableBinding> binding = new List<PlayableBinding>(winningTimeLineAsset.outputs);
        for (int i = 0; i < tracks.Count * 2; i++)
        {
            if (tracks[i] != null)
            {
                if(i < 2)
                {
                    var track = binding[i].sourceObject;
                    timeline.SetGenericBinding(track, tracks[i]);
                }
                else
                {
                    var track = binding[i - 2].sourceObject;
                    timeline.SetGenericBinding(track, tracks[i - 2]);
                }
            }
        }
    }

    private void OnDisable()
    {
        player1Info = null;
        player2Info = null;
    }
}
