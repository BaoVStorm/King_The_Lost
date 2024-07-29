using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Animator Continue;
    
    public Animator Restart;
    
    public Animator Options;

    public Animator Exit;

    public Move player;

    public void Press_Pause()
    {
        SoundManager.Instance.GetUISound("PauseOn").AudioPlay();
        
        if(player.Get_IsDead())
            return;

        this.gameObject.SetActive(true);

        Time.timeScale = 0f;

        player.Set_StopComBo(true);
    }

    public void Push_Continue()
    {
        Time.timeScale = 1f;
        
        SoundManager.Instance.GetUISound("Other").AudioPlay();

        // Debug.Log("Continue");
        Continue.SetBool("IsPush", true);

        this.gameObject.SetActive(false);

        player.Set_StopComBo(false);
    }
    public void Push_Restart()
    {
        Time.timeScale = 1f;

        SoundManager.Instance.GetUISound("Other").AudioPlay();

        // Debug.Log("Restart");
        Restart.SetBool("IsPush", true);

        LevelLoader.Instance.ChangeScene(1);
    }
    public void Push_Options()
    {
        SoundManager.Instance.GetUISound("Other").AudioPlay();

        // Time.timeScale = 1f;

        // Options.SetBool("IsPush", true);
    }
    public void Push_Exit()
    {
        Time.timeScale = 1f;

        SoundManager.Instance.GetUISound("Other").AudioPlay();

        // Debug.Log("Exit");
        Exit.SetBool("IsPush", true);

        LevelLoader.Instance.ChangeScene(0);
    }
    
}
