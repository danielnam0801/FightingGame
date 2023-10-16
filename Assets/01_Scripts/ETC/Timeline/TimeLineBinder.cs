using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineBinder : MonoBehaviour
{
    public PlayerSpawnState State;
    public PlayableDirector playableDirector; //the playable director attached to this object

    public Animator firstObjectToBind; //the first game Object in scenes Animator to Bind
    public Animator secondObjectToBind; //the second game Object in scenes Animator to Bind

    private void Start()
    {
        State = GetComponent<PlayerSetting>().state;
        //Find the game objects in the scene with the tags "Team1" and "Team2"
        if (State == PlayerSpawnState.left)
        {
            GameObject playerfreeLook = GameObject.Find("PlayerFreeLook");
            firstObjectToBind = playerfreeLook.GetComponent<Animator>();
            playerfreeLook.SetActive(false);
        } else
        {
            GameObject playerfreeLook = GameObject.Find("Player2FreeLook");
            firstObjectToBind = playerfreeLook.GetComponent<Animator>();
            playerfreeLook.SetActive(false);
        }
        secondObjectToBind = GetComponent<Animator>();
        //Find the playable director components attached to the object
        playableDirector = GetComponent<PlayableDirector>();
        PlayTimeLine();

    }

    public void PlayTimeLine()
    {
        //The tracks in the playable director timeline
        foreach (var playableAssetOutput in playableDirector.playableAsset.outputs)
        {

            // Debug.Log(playableAssetOutput.streamName.ToString());
            if (playableAssetOutput.streamName == firstObjectToBind.tag.ToString())
            {
                playableDirector.SetGenericBinding(playableAssetOutput.sourceObject, firstObjectToBind);
            }
            else if (playableAssetOutput.streamName == secondObjectToBind.tag.ToString())
            {
                playableDirector.SetGenericBinding(playableAssetOutput.sourceObject, secondObjectToBind);
            }
        }
    }

}//end class