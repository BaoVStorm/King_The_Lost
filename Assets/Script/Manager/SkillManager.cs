using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    //single ton
    private static SkillManager _instance;
    public static SkillManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // Smoke
    public SmokeSkill SkillSmoke;

    // Elec
    public ElecSkill SkillElec;
    private float PosY;
    private float DistanceSkill;

    void Start()
    {
        _instance = this;

        PosY = -2.8f;

        DistanceSkill = 2.5f;

        Start_PoolSmokeSkill();

        Start_PoolElecSkill();
    }

    // void Update()
    // {
        
    // }

    // Skill Smoke
    public void ActiveSkill_Smoke(int Direct)
    {
        if(SkillSmoke == null)
            return;

        SoundManager.Instance.GetSkillSound("Smoke").AudioPlay();

        SmokeSkill Skill = GetPoolSmokeSkill();

        Skill.gameObject.SetActive(true);

        Skill.Set_DirectionMove(Direct);
    }

    // Skill Elec
    public void ActiveSkill_Elec()
    {
        if(SkillElec == null)
            return;

        StartCoroutine(ActiveSkill());
    }

    IEnumerator ActiveSkill()
    {
        ElecSkill ElecLeft, ElecRight;

        for(int i = 1; i <= 3; i++)
        {
            SoundManager.Instance.GetSkillSound("Elec").AudioPlay();

            ElecLeft = GetPoolElecSkill();
            ElecLeft.transform.position = new UnityEngine.Vector3(DistanceSkill * i, PosY, 0f);
            ElecLeft.gameObject.SetActive(true);

            ElecRight = GetPoolElecSkill();
            ElecRight.transform.position = new UnityEngine.Vector3(- DistanceSkill * i, PosY, 0f);
            ElecRight.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(0.3f);

        }
    }

    // Pooling Smoke
    private List<SmokeSkill> PoolSmokeSkill = new List<SmokeSkill>();
    private int AmountPoolSmokeSkill = 2;

    private void Start_PoolSmokeSkill()
    {
        for(int i = 0; i < AmountPoolSmokeSkill; i++)
        {
            SmokeSkill Skill = Instantiate(SkillSmoke);

            Skill.gameObject.SetActive(false);

            Skill.Reset_DirectionMove();

            PoolSmokeSkill.Add(Skill);
        }
    }

    private SmokeSkill GetPoolSmokeSkill()
    {
        for(int i = 0; i < PoolSmokeSkill.Count; i++)
        {
            if(!PoolSmokeSkill[i].gameObject.activeInHierarchy)
            {
                return PoolSmokeSkill[i];
            }
        }

        SmokeSkill Skill = Instantiate(SkillSmoke);

        Skill.gameObject.SetActive(false);

        Skill.Reset_DirectionMove();

        PoolSmokeSkill.Add(Skill);

        return Skill;
    }

    // Pooling Elec
    private List<ElecSkill> PoolElecSkill = new List<ElecSkill>();
    private int AmountPoolElecSkill = 4;

    private void Start_PoolElecSkill()
    {
        for(int i = 0; i < AmountPoolElecSkill; i++)
        {
            ElecSkill Skill = Instantiate(SkillElec);

            Skill.gameObject.SetActive(false);
        
            PoolElecSkill.Add(Skill);
        }
    }

    private ElecSkill GetPoolElecSkill()
    {
        for(int i = 0; i < PoolElecSkill.Count; i++)
        {
            if(!PoolElecSkill[i].gameObject.activeInHierarchy)
            {
                return PoolElecSkill[i];
            }
        }

        ElecSkill Skill = Instantiate(SkillElec);

        Skill.gameObject.SetActive(false);

        PoolElecSkill.Add(Skill);

        return Skill;
    }
}
