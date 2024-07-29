using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    public Player Player_;

    // Hp Bar
    public GameObject HpBar;
    public BarAni AniHpBar;
    private float HpRoot; // Setting Heath animation

    // Energy Bar
    public GameObject EnergyBar;
    public BarAni AniEnergyBar;
    private float EnergyRoot;


    // Start is called before the first frame update
    void Start()
    {
        HpRoot = Player_.Hp;

        EnergyRoot = Player_.Energy;
    }

    // Update is called once per frame
    void Update()
    {
        Setting_HpBar();
        Setting_EnergyBar();
    }

    private void Setting_HpBar()
    {
        if(HpRoot != Player_.Hp)
        {
            float ScaleBar;

            HpRoot = Player_.Hp;
            AniHpBar.Set_IsAct_True();

            if(Player_.Hp <= 0)
            {
                ScaleBar = 0f;
            }
            else
            {
                ScaleBar = Player_.Hp / Player_.HpBase;
            }
            
            HpBar.transform.localScale = new UnityEngine.Vector3(ScaleBar, HpBar.transform.localScale.y, HpBar.transform.localScale.z);
        }
    }

    private void Setting_EnergyBar()
    {
        float ScaleBar;

        if(Player_.Energy <= 0)
        {
                ScaleBar = 0f;
        }
        else
        {
                ScaleBar = Player_.Energy / Player_.EnergyBase;
        }
            
        EnergyBar.transform.localScale = new UnityEngine.Vector3(ScaleBar, EnergyBar.transform.localScale.y, EnergyBar.transform.localScale.z);

        if(Player_.Energy < EnergyRoot)
        {
            AniEnergyBar.Set_IsAct_True();
        }

        EnergyRoot = Player_.Energy;
    }
}
