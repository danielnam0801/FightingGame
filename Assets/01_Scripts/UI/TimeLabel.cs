using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using SelectScene;

public class TimeLabel
{
    Label _label;
    
    float t;
    float maxtime;
    float delay = 1f;

    public bool Stop = false;

    public SelectUI SelectUI { get; }

    public TimeLabel(SelectUI selectUI, Label label, float time)
    {
        SelectUI = selectUI;
        _label = label;
        maxtime = time;
        t = maxtime + delay;
        SetTimer(90);
    }

    public void Update()
    {
        if (Stop == true) return;

        t -= Time.deltaTime;
        if(t < maxtime)
        {
            SetTimer(t);
        }
        if(t < 0f)
        {
            Debug.Log("시간 초과 메뉴로 돌아갑니다");
            SelectUI.GoIntro();
        }
    }

    public void SetTimer(float t)
    {
        _label.text = t.ToString("#");
    }
}
