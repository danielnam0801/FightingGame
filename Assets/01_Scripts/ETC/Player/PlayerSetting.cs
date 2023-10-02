using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    private void Awake()
    {
        skinnedMeshRenderer = transform.Find("Visual").GetComponent<SkinnedMeshRenderer>();       
    }

    public void SetVisual(Material material)
    {
        skinnedMeshRenderer.material = material;    
    }
}
