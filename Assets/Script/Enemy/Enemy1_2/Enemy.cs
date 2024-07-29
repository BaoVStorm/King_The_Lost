using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Enemy_Base
{
    [SerializeField]
    private bool ThrowWeapon = false;

    public bool IsEnemy2 = false;

    public Weapon weapon;
 
    private bool IsFirstStop;

    public void Set_ThrowWeapon(bool TW)
    {
        ThrowWeapon = TW;
    }

    protected override void Start()
    {
        base.Start();

        // ThrowWeapon = false;
    
        IsFirstStop = true;

        Hp = 7f;

        Speed = 4.6f;

        weapon = Resources.Load<Weapon>("Refabs/Weapon/Weapon");

        ThrowWeapon = IsEnemy2;
    }

    protected override void Update()
    {
        base.Update();

        // ThrowWeapon
        if(Vector2.Distance(this.transform.position, LocationOPlayer) <= 8f && ThrowWeapon)
        {
            ThrowWeapon = false;
            StartCoroutine(IsThrowingWeapon());
        }

        // Move Enemy
        if(Vector2.Distance(this.transform.position, LocationOPlayer) <= 3.1f && IsDelay == false && !IsDelayAttackWhenBeHit)
        {
            if(IsFirstStop && IsDead == false)
            {
                IsFirstStop = false;

                IsStop = true;

                if(Skin != null)
                {    
                    Skin.SetParameter_IsIdle(true);
                }  

                StartCoroutine(DelayFirstStop(IsStop));
                
                rg.velocity = new UnityEngine.Vector2(0f, rg.velocity.y);
                
                if(DirectionMove == 1)
                    this.transform.position = new UnityEngine.Vector3(-3f, this.transform.position.y, this.transform.position.z);
                else
                    this.transform.position = new UnityEngine.Vector3(3f, this.transform.position.y, this.transform.position.z);
            }
        }
        else
        {
            IsDelayAttackWhenBeHit = false;
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
            // rg.velocity = new UnityEngine.Vector2(0, rg.velocity.y);
            if(IsDelay) // When Enemy Be Hit
            {
                StopForComBo = false;
                Skin.SetParameter_IsStopForThrow(StopForComBo);
            }
        }
        else
        if(IsBeHit == false && IsStop == false && IsDead == false)
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

    public IEnumerator IsThrowingWeapon()
    {
        StopForComBo = true;
        Skin.SetParameter_IsStopForThrow(StopForComBo);

        Skin.SetWeaponAndDirect(weapon, DirectionMove);

        Skin.Set_Player(Player);

        yield return new WaitForSeconds(2.8f);

        StopForComBo = false;
        Skin.SetParameter_IsStopForThrow(StopForComBo);
    }

    IEnumerator DelayFirstStop(bool IsStop_)
    {
        yield return new WaitForSeconds(0.4f);

        if(Skin != null)
            Skin.SetParameter_IsStop(IsStop_);
    }


    IEnumerator ResetEnemy_()
    {
        yield return new WaitForSeconds(1.6f);
        Start();
    }

}
