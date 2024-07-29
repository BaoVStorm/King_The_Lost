using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // singleton

    private static GameOver _instance;
    public static GameOver Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;

        this.gameObject.SetActive(false);
    }

    public void Activate_GameOver()
    {
        this.gameObject.SetActive(true);
        Debug.Log("Press GameOver");
    }

    public void Press_Revive()
    {
        // LevelLoader.Instance.ChangeScene(0);
        SoundManager.Instance.GetUISound("Revive").AudioPlay();

        Debug.Log("Revive");
    }

    public void Press_Restart()
    {
        LevelLoader.Instance.ChangeScene(1);

        SoundManager.Instance.GetUISound("Start").AudioPlay();
    }

    public void Press_Exit()
    {
        SoundManager.Instance.GetUISound("End").AudioPlay();

        LevelLoader.Instance.ChangeScene(0);
    }

}
