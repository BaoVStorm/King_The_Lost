using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ani_Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyAttackRange EnemyAttackRange;

    [SerializeField]
    private Weapon weapon;

    private int DirectionMove;

    Animator Ani;

    [SerializeField]
    private Player Player;

    public void Set_Player(Player Player_) 
    {
        Player = Player_;
    }

    void Start()
    {
        EnemyAttackRange = Resources.Load<EnemyAttackRange>("Refabs/Enemy/Attack/EnemyAttackRange");
        Start_PoolingEnemyAttackRange();

        Ani = this.GetComponent<Animator>();

        SetParameter_IsIdle(false);
    }

    public void SetParameter_IsBeHit(bool Is)
    {
        if(Ani != null)
            Ani.SetBool("IsBeHit", Is);
    }
    
    public void SetParameter_IsStop(bool Is)
    {
        if(Ani != null)
            Ani.SetBool("IsStop", Is);
    }

    public void SetParameter_IsIdle(bool Is)
    {
        if(Ani != null)
            Ani.SetBool("IsIdle", Is);
    }

    public void SetParameter_IsFloor(bool Is)
    {
        if(Ani != null)
            Ani.SetBool("IsFloor", Is);
    }

    // Attack

    public void Attack()
    {
        if(EnemyAttackRange != null)
        {
            StartCoroutine(Attacking());
        }
    }

    private IEnumerator Attacking()
    {
        EnemyAttackRange EAR = GetPoolEnemyAttackRange();

        EAR.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);

        EAR.gameObject.SetActive(false);
    }

    // throw weapon

    public void SetWeaponAndDirect(Weapon Wp, int Direc)
    {
        DirectionMove = Direc;

        if(PoolWeapon.Count != 0)
            return;

        weapon = Wp;

        Start_PoolingWeapon();
    }

    public void SetParameter_IsStopForThrow(bool Is)
    {
        if(Ani != null)
            Ani.SetBool("IsStopForThrow", Is);
    }

    public void ThrowWeapon()
    {
        Weapon Wp = GetPoolWeapon();

        Wp.Set_IsEnemyThrow(true);

        Wp.Set_Player(Player);

        Wp.transform.position = this.transform.position + new UnityEngine.Vector3(0, 1.075f, 0);

        Wp.Set_DirectionMove(DirectionMove);

        Wp.gameObject.SetActive(true);
    }

    // -- Pooling Weapon
    private List<Weapon> PoolWeapon = new List<Weapon>();

    private int AmountPoolWeapon = 1; 

    private void Start_PoolingWeapon()
    {
        for(int i = 0; i < AmountPoolWeapon; i++)
        {
            Weapon Wp = Instantiate(weapon);

            Wp.gameObject.SetActive(false);
            PoolWeapon.Add(Wp);
        }
    }

    private Weapon GetPoolWeapon()
    {
        for(int i = 0; i < PoolWeapon.Count; i++)
        {
            if(!PoolWeapon[i].gameObject.activeInHierarchy)
            {
                return PoolWeapon[i];
            }
        }

        Weapon Wp = Instantiate(weapon);
        Wp.gameObject.SetActive(false);
        PoolWeapon.Add(Wp);

        return Wp;
    }

    // Pooling Enemy Attack Range
    private List<EnemyAttackRange> PoolEnemyAttackRange = new List<EnemyAttackRange>();

    private int AmountPoolEnemyAttackRange = 2; 

    private void Start_PoolingEnemyAttackRange()
    {
        if(EnemyAttackRange == null)
            EnemyAttackRange = Resources.Load<EnemyAttackRange>("Refabs/Enemy/Attack/EnemyAttackRange");

        for(int i = 0; i < AmountPoolEnemyAttackRange; i++)
        {
            EnemyAttackRange AR = Instantiate(EnemyAttackRange);

            AR.transform.parent = this.transform;
            AR.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
            AR.gameObject.SetActive(false);
            PoolEnemyAttackRange.Add(AR);
        }
    }

    private EnemyAttackRange GetPoolEnemyAttackRange()
    {
        for(int i = 0; i < PoolEnemyAttackRange.Count; i++)
        {
            if(!PoolEnemyAttackRange[i].gameObject.activeInHierarchy)
            {
                return PoolEnemyAttackRange[i];
            }
        }

        EnemyAttackRange AR = Instantiate(EnemyAttackRange);
        AR.transform.parent = this.transform;
        AR.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
        AR.gameObject.SetActive(false);
        PoolEnemyAttackRange.Add(AR);

        return AR;
    }
}
