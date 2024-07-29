using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBar : MonoBehaviour
{
    Animator ani;

    void Start()
    {
        ani = this.GetComponent<Animator>();
    }

    public void Push_Button()
    {
        ani.SetBool("IsPush", true);
    }

    public void Set_IsPush_False()
    {
        ani.SetBool("IsPush", false);
    }

    public void Play()
    {
        SoundManager.Instance.GetUISound("Other").AudioPlay();

        Debug.Log("Play");
        LevelLoader.Instance.ChangeScene(1);
    }

    public void Options()
    {
        SoundManager.Instance.GetUISound("Other").AudioPlay();

        Debug.Log("Options");
    }
    public void Exit()
    {
        SoundManager.Instance.GetUISound("Other").AudioPlay();

        Debug.Log("Exit");
    }
}
