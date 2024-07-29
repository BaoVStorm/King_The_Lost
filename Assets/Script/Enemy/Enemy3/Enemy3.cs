using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy3 : Enemy_Base
{
    [SerializeField]
    private bool IsAttackSkill;
    
    private bool IsFirstStop;

    public float SpeedSkill;

    private bool IsFirstAttackSkill;

    private bool IsResetAttackSkill;

    private bool IsFirstResetAttackSkill;

    private bool IsStopAttackSkill;

    // RangeAttackSkill
    public RangeAttackSkillEnemy3 RangeAttackSkill;
    private bool IsRangeAttackSkillSpawn = false;

    protected override void Start()
    {
        base.Start();

        SpeedSkill = 21f;

        // ThrowWeapon = false;
    
        IsFirstStop = true;

        IsResetAttackSkill = false;

        IsFirstAttackSkill = true;

        Hp = 8f;

        Speed = 4f;

        LocationOPlayer = new UnityEngine.Vector3(0, -1.597555f, 0);

        IsAttackSkill = true;

        IsFirstResetAttackSkill = true;

        IsStopAttackSkill = false;
        
        Start_RangeAttackSkill();
    }

    protected override void Update()
    {
        base.Update();

        // Move Enemy
        if(Vector2.Distance(this.transform.position, LocationOPlayer) <= 5.25f && IsDead == false)
        {
            if(IsAttackSkill)
            {
                // this.gameObject.layer = 8; // EcanpassP layer
                if(IsResetAttackSkill == false)
                {
                    if(IsFirstAttackSkill)
                    {
                        StartCoroutine(DelayFirstStop());

                        IsFirstAttackSkill = false;
                    }

                    float dir = 1;
                    if(DirectionMove == 2)
                        dir = -1;

                    if(IsStopAttackSkill == false)
                    {
                        IsStop = true;
                        Skin.SetParameter_IsIdle(false);
                    }
                    else
                    {
                        Skin.SetParameter_IsIdle(true);
                        IsStop = false;
                    }

                    if(Skin != null)
                        Skin.SetParameter_IsStop(IsStop);
                    
                    if(IsStopAttackSkill == false)
                    {
                        // this.transform.position = Vector2.MoveTowards(transform.position, dir * new UnityEngine.Vector3(11f, -1.5f, 0f), SpeedSkill * Time.deltaTime);
                        rg.velocity = new UnityEngine.Vector3((SpeedSkill) * dir ,rg.velocity.y, 0f);
                    }

                    if(DirectionMove == 2)
                    {
                        if(transform.position.x < -3.3f)
                        {
                            StartCoroutine(DoAttackSkill());
                        }
                    }
                    else
                    {
                        if(transform.position.x > 3.3f)
                        {
                            StartCoroutine(DoAttackSkill());
                        }
                    }
                }
                else
                {
                    // this.transform.position = Vector2.MoveTowards(transform.position, dir * new UnityEngine.Vector3(5.2f, transform.position.y + 0.5f, 0f), (SpeedSkill / 2) * Time.deltaTime);
                    if(DirectionMove == 2)
                    {
                        if(transform.position.x > 4.5f)
                        {
                            if(IsFirstResetAttackSkill == true)
                                rg.velocity = Vector3.zero;
                            
                            IsResetAttackSkill = false;
                            IsFirstResetAttackSkill = false;

                            StartCoroutine(DelayFirstStop());
                        }
                        else
                        {
                            if(transform.position.x > 0)
                                rg.velocity = new UnityEngine.Vector3(SpeedSkill / 2, rg.velocity.y, 0f);
                        }
                    }
                    else
                    {
                        if(transform.position.x < -4.5f)
                        {
                            if(IsFirstResetAttackSkill == true)
                                rg.velocity = Vector3.zero;
                            
                            IsResetAttackSkill = false;
                            IsFirstResetAttackSkill = false;

                            StartCoroutine(DelayFirstStop());
                        }
                        else
                        {
                            if(transform.position.x < 0)
                                rg.velocity = new UnityEngine.Vector3(-SpeedSkill / 2, rg.velocity.y, 0f);
                        }
                    }
                }
            }
            else
            {
                if(Vector2.Distance(this.transform.position, LocationOPlayer) <= 3.1f && IsDelay == false)
                {
                    if(IsFirstStop)
                    {
                        if(DirectionMove == 1)
                            this.transform.position = new UnityEngine.Vector3(-3f, this.transform.position.y, this.transform.position.z);
                        else
                            this.transform.position = new UnityEngine.Vector3(3f, this.transform.position.y, this.transform.position.z);

                        IsFirstStop = false;

                        IsStop = true;

                        if(Skin != null)
                        {    
                            Skin.SetParameter_IsIdle(true);
                            Skin.SetParameter_IsStop(IsStop);
                        }  
                        
                        StartCoroutine(ResetDoAttackSkill());

                        rg.velocity = new UnityEngine.Vector2(0f, rg.velocity.y);
                    }
                }
                else
                {
                    IsFirstStop = true;

                    IsStop = false;
                    if(Skin != null)
                    {    
                        Skin.SetParameter_IsStop(IsStop);
                        Skin.SetParameter_IsIdle(false);
                    }  
                }
            }
        }
        else
        {
            RangeAttackSkill.SetActiveObj(false);

            if(this.gameObject.tag == "Default")
            {
                IsAttackSkill = false;
            }

            this.gameObject.tag = "Enemy";
            
            IsFirstStop = true;

            IsStop = false;
            if(Skin != null)
            {    
                Skin.SetParameter_IsStop(IsStop);
                Skin.SetParameter_IsIdle(false);
            }  
        }

        // Move in floor
        if(StopForComBo)
        {
            rg.velocity = new UnityEngine.Vector2(0, rg.velocity.y);
        }
        else
        if(IsBeHit == false && IsStop == false && IsDead == false && IsStopAttackSkill == false)
        {
            if(DirectionMove == 1)
                rg.velocity = new UnityEngine.Vector2(Speed, rg.velocity.y);
            else
                rg.velocity = new UnityEngine.Vector2(-Speed, rg.velocity.y);
        }

        // destroy enemy
        if(Hp <= 0 && IsDead == false)
        {
            IsDead = true;

            StartCoroutine(EnemyDeath(DirectionMove));

            StartCoroutine(ResetEnemy_());
        }
    }

    // Delay for first stop
    IEnumerator DelayFirstStop()
    {
        IsStopAttackSkill = true;

        yield return new WaitForSeconds(0.3f);

        RangeAttackSkill.SetActiveObj(true);

        RangeAttackSkill.Set_Direction(DirectionMove);

        yield return new WaitForSeconds(1.7f);

        this.gameObject.tag = "Default";

        RangeAttackSkill.SetActiveObj(false);

        yield return new WaitForSeconds(0.2f);

        IsStopAttackSkill = false;        
    }
    
    // AttackSkill Enemy3
    IEnumerator DoAttackSkill()
    {
        Debug.Log("Change Dir");

        // this.gameObject.layer = 6; // enemy layer

        if(DirectionMove == 2)  // is left direction
        {
            Set_DirectionMove(1);
            
              // set right
        }
        else
        {
            Set_DirectionMove(2);
        }  // set left

        IsStop = false;
        if(Skin != null)
        {    
            Skin.SetParameter_IsIdle(true);
            Skin.SetParameter_IsStop(IsStop);
        }  

        StopForComBo = true;

        yield return new WaitForSeconds(0.7f);

        StopForComBo = false;

        this.gameObject.tag = "Enemy";

        IsAttackSkill = false;
    }

    IEnumerator ResetDoAttackSkill()
    {
        float DelayTime = Random.Range(4f, 7.1f);

        yield return new WaitForSeconds(DelayTime);

        IsAttackSkill = true;

        IsResetAttackSkill = true;

        IsFirstResetAttackSkill = true;
    }

    IEnumerator ResetEnemy_()
    {
        yield return new WaitForSeconds(1.6f);
        Start();
    }

    // RangeAttackSkill
    private void Start_RangeAttackSkill()
    {
        if(RangeAttackSkill == null || IsRangeAttackSkillSpawn == true)
            return;

        IsRangeAttackSkillSpawn = true;

        RangeAttackSkill = Instantiate(RangeAttackSkill);

        RangeAttackSkill.transform.parent = this.transform.Find("Skin").transform;
   
        RangeAttackSkill.transform.localPosition = Vector3.zero;

        RangeAttackSkill.SetActiveObj(false);
    }
}
