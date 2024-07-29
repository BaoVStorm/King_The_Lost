using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject Layer1;
    public GameObject Layer2;
    public GameObject Layer3;

    private UnityEngine.Vector3 FirstPos1;
    private UnityEngine.Vector3 FirstPos2;
    private UnityEngine.Vector3 FirstPos3;

    public float Duration = 0.015f;

    //single ton
    private static Background _instance;
    public static Background Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        FirstPos1 = Layer1.transform.position;
        FirstPos2 = Layer2.transform.position;
        FirstPos3 = Layer3.transform.position;
    }

    public void Shake_Background()
    {
        StartCoroutine(ShakeBg());
    }

    private IEnumerator ShakeBg()
    {
        float Times = 0f;

        while(Times <= Duration)
        {
            Times += 1 * Time.deltaTime;

            Layer1.transform.position = FirstPos1 + new UnityEngine.Vector3(Random.Range(-0.015f, 0.015f), Random.Range(-0.015f, 0.015f), 0);
            Layer2.transform.position = FirstPos2 + new UnityEngine.Vector3(Random.Range(-0.015f, 0.015f), Random.Range(-0.015f, 0.015f), 0);
            Layer3.transform.position = FirstPos3 + new UnityEngine.Vector3(Random.Range(0f, 0.01f), Random.Range(0f, 0.01f), 0);

            yield return null;
        }

        Layer1.transform.position = FirstPos1;
        Layer2.transform.position = FirstPos2;
        Layer3.transform.position = FirstPos3;
    }
}
