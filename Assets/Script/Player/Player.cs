using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Base Info
    public float HpBase;
    public float Hp;

    public float EnergyBase;
    public float Energy;

    public Move PlayerMove;

    // AttackEffect
    private GameObject AttackEffect;

    // BeHit
    private bool Is_DelayBeHit;

    // Dead
    private bool IsDead;

    // Skill Smoke
    private float TimeSkillSmoke;
    private bool IsPauseSkillSmoke;
    private float EnergyCostForSkillSmoke = 35f;

    // Skill Elec
    private float TimeSkillElec;
    private bool IsPauseSkillElec;
    private float EnergyCostForSkillElec = 65f;

    // God Mode
    public bool IsGodHp;

    public bool IsGodEnergy;

    void Start()
    {
        HpBase = 10;

        EnergyBase = 100;

        IsDead = false;

        Hp = HpBase;

        Energy = EnergyBase;        

        AttackEffect = Resources.Load<GameObject>("Refabs/AttackEffect/HitEffect/AE");
        Start_PoolingAttackEffect();

        Is_DelayBeHit = false;

        IsGodEnergy = false;

        IsGodHp = false;
    }

    void Update()
    {
        if(IsDead)
            return;

        if(Energy < EnergyBase && IsGodEnergy == false)
        {
            Energy += 2f * Time.deltaTime;
        }
        else
        {
            Energy = EnergyBase;
        }

        if(Input.touchCount == 1)
        {
            if(PlayerMove.DoubleJump == 2)
            {
                Touch touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Began)
                {
                    IsPauseSkillSmoke = false;
                    TimeSkillSmoke = 0.35f;
                } 

                if(touch.phase == TouchPhase.Stationary && !IsPauseSkillSmoke)
                {
                    if(TimeSkillSmoke > 0f)
                    {
                        TimeSkillSmoke -= 1 * Time.deltaTime;
                    }
                    else
                    {
                        IsPauseSkillSmoke = true;

                        if(Energy - EnergyCostForSkillSmoke >= 0)
                        {
                            Energy -= EnergyCostForSkillSmoke;
                            
                            SkillManager.Instance.ActiveSkill_Smoke(PlayerMove.Get_LookDirection());
                        }
                    }
                }
            }
        }

        if(Input.touchCount == 2)
        {
            if(PlayerMove.DoubleJump == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if(touch2.phase == TouchPhase.Began)
                {
                    IsPauseSkillElec = false;
                    TimeSkillElec = 0.35f;
                } 

                if(touch1.phase == TouchPhase.Stationary && touch2.phase == TouchPhase.Stationary && !IsPauseSkillElec)
                {
                    if(TimeSkillElec > 0f)
                    {
                        TimeSkillElec -= 1 * Time.deltaTime;
                    }
                    else
                    {
                        IsPauseSkillElec = true;

                        if(Energy - EnergyCostForSkillElec >= 0)
                        {
                            Energy -= EnergyCostForSkillElec;

                            SkillManager.Instance.ActiveSkill_Elec();
                        }
                    }
                }
            }
        }

        if(Input.GetKeyDown("2"))
        {
            if(Energy - EnergyCostForSkillElec >= 0)
            {
                Energy -= EnergyCostForSkillElec;

                SkillManager.Instance.ActiveSkill_Elec();
            }
        }

        if(Input.GetKeyDown("1"))
        {
            if(Energy - EnergyCostForSkillSmoke >= 0)
            {
                Energy -= EnergyCostForSkillSmoke;
                            
                SkillManager.Instance.ActiveSkill_Smoke(PlayerMove.Get_LookDirection());
            }
        }
    }


    public bool Get_IsDead()
    {
        return IsDead;
    }

    public void BeHit()
    {
        if(Is_DelayBeHit == false)
        {
            SoundManager.Instance.GetEnemySound("EnemyAttack").AudioPlay();

            StartCoroutine(DelayBeHit());

            if(IsGodHp == false)
                Hp--;

            if(Hp == 0)
            {
                StartCoroutine(PlayerDead());
            }

            GameObject AF = GetPoolAttackEffect();

            AF.transform.position = this.transform.position + new UnityEngine.Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.2f, 0.5f), 0f);

            AF.SetActive(true);
        }
    }

    IEnumerator DelayBeHit()
    {
        Is_DelayBeHit = true;

        yield return new WaitForSeconds(0.2f);
        
        Is_DelayBeHit = false;
    }

    IEnumerator PlayerDead()
    {
        // Activate GameOver
        GameOver.Instance.Activate_GameOver();

        // Set Dead Animation
        IsDead = true;
        PlayerMove.Set_IsDead(true);
        PlayerMove.SkinOfPlayer.SetBool("IsDead", IsDead);
        PlayerMove.SkinOfPlayer.SetBool("IsCrouch", false);

        // Set Shadow When Player Dead
        yield return new WaitForSeconds(0.2f);

        PlayerMove.Set_Shadow_Scale(2f);
    }

    // Pooling Attack Effect
    private List<GameObject> PoolAttackEffect = new List<GameObject>();

    private int AmountPoolAttackEffect = 2; // 2 normal attack range, 1 circle attack

    private void Start_PoolingAttackEffect()
    {
        for(int i = 0; i < AmountPoolAttackEffect; i++)
        {
            GameObject AF = Instantiate(AttackEffect);
            AF.transform.parent = this.transform;
            AF.gameObject.SetActive(false);
            PoolAttackEffect.Add(AF);
        }
    }

    private GameObject GetPoolAttackEffect()
    {
        for(int i = 0; i < PoolAttackEffect.Count; i++)
        {
            if(!PoolAttackEffect[i].gameObject.activeInHierarchy)
            {
                return PoolAttackEffect[i];
            }
        }

        GameObject AF = Instantiate(AttackEffect);
        AF.transform.parent = this.transform;
        AF.gameObject.SetActive(false);
        PoolAttackEffect.Add(AF);

        return AF;
    }
}
