using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorLerp : MonoBehaviour
{
    public Color startColor = Color.black;
    public Color endColor = Color.white;
    [Range(0, 10)]
    public float speed = 1;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {

    }
    public void ColorChange()
    {
        float lerpValue = Mathf.PingPong(Time.time * speed, 1.0f);
        image.color = Color.Lerp(startColor, endColor, lerpValue);
    }
}
