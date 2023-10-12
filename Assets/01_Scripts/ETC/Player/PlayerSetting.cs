using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    public PlayerSpawnState state; //spawn pos
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        skinnedMeshRenderer = transform.Find("Visual").GetComponent<SkinnedMeshRenderer>();       
    }

    public void SetVisual(Material material)
    {
        skinnedMeshRenderer.material = material;    
    }

    public void SetPlayerNum(PlayerSpawnState state)
    {
        this.state = state;
    }
}
