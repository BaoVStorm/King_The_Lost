using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject Logo;
    public SettingBar Play;
    
    public SettingBar Options;

    public SettingBar Exit;

    public GameObject Slide1;
    public GameObject Slide2;
    public GameObject Slide3;
    public GameObject Slide4;
    public GameObject Slide5;

    private Sequence seq;

    private bool CheckELogo;

    void Start()
    {
        CheckELogo = true;
        
        seq = DOTween.Sequence();

    }

    // Update is called once per frame
    void Update()
    {
        if(CheckELogo)
        {   
            seq = DOTween.Sequence();

            CheckELogo = false;

            seq.Join(Logo.transform.DOScale(Logo.transform.localScale + new UnityEngine.Vector3(0.2f, 0.2f, 0.2f), 0.2f)).OnComplete(() =>
            {
                Logo.transform.DOScale(Logo.transform.localScale - new UnityEngine.Vector3(0.4f, 0.4f, 0.4f), 0.4f).OnComplete(() =>
                {
                    Logo.transform.DOScale(Logo.transform.localScale + new UnityEngine.Vector3(0.2f, 0.2f, 0.2f), 0.2f).OnComplete(() =>
                    {
                        Logo.transform.DOScale(Logo.transform.localScale, 5f).OnComplete(() =>
                        {
                            CheckELogo = true;
                        });

                    });
                });
            });
        }
         
    }
}
