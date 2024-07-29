using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    //single ton
    private static LevelLoader instance_;

    public static LevelLoader Instance
    {
        get
        {
            return instance_;
        }
    }

    public Animator LoadScene;

    void Awake()
    {
        instance_ = this;

        LoadScene.SetBool("IsEnd", true);
    }

    public void ChangeScene(int Index)
    {
        // SceneManager.GetActiveScene().buildIndex  -Get Scene index
        StartCoroutine(ChangeScene_(Index));
    }

    IEnumerator ChangeScene_(int Index)
    {
        LoadScene.SetBool("IsStart", true);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(Index);
    }
}
