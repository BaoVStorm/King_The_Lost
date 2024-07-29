using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public float Hp;

    public float Point_Skill;
    
    public float Speed;

    public bool IsBeHit;

    public int DirectionMove = 1; // 1: Right, 2: Left

    public bool IsStop;

    public bool IsDelay;

    public bool StopForComBo;

    public float DelayForMoveAfterFloor = -1;

    public Rigidbody2D rg;

    public Ani_Enemy Skin;

    public Animator ThisRefab;

    public GameObject Shadow;

    public UnityEngine.Vector2 LocationOPlayer;

    private UnityEngine.Vector3 Shadow_Scale;

    private GameObject AttackEffect; // Spawn when Enemy be Hit

    public Player Player;

    [SerializeField]
    protected bool IsDead;

    protected bool IsDisableSound;

    public void Set_Player(Player Player_) 
    {
        Player = Player_;
    }

    public void Set_DirectionMoveAndSkin(int Direct, string Skin_Path) // 1: Right, 2: Left
    {
        Create_Skin(Skin_Path);

        ThisRefab.SetInteger("Direct", Direct);

        Set_DirectionMove(Direct);        
    }

    protected void Set_DirectionMove(int Direct)
    {
        DirectionMove = Direct;

        if(DirectionMove == 1) // 1: Right, 2: Left
        {
            this.transform.Find("Skin").transform.rotation = Quaternion.Euler(0, 180, 0);
            ThisRefab.SetInteger("Direct", 1);
        }
        else
        {
            this.transform.Find("Skin").transform.rotation = Quaternion.Euler(0, 0, 0);
            ThisRefab.SetInteger("Direct", 2);
        }
    }

    public int Get_DirectionMove() // 1: Right, 2: Left
    {
        return DirectionMove;
    }

    protected virtual void Start()
    {
        this.GetComponent<Collider2D>().isTrigger = false;

        if(AttackEffect == null)
            AttackEffect = Resources.Load<GameObject>("Refabs/AttackEffect/HitEffect/AE");
        
        if(PoolAttackEffect.Count == 0)
            Start_PoolingAttackEffect();

        CheckShadow();
        
        IsDead = false;

        if(rg == null)
            rg = this.GetComponent<Rigidbody2D>();

        StopForComBo = false;
        
        IsDelay = false;
        
        IsBeHit = false;
        if(Skin != null)
            Skin.SetParameter_IsBeHit(IsBeHit);
        
        IsStop = false;
        
        DelayForMoveAfterFloor = -1f;

        LocationOPlayer = new UnityEngine.Vector3(0, -1.597555f, 0);

        IsDisableSound = false;

        // Create_Skin(); do in Set_Direction
    }

    protected virtual void Update()
    {
        if(Player.Get_IsDead())
            LocationOPlayer = new UnityEngine.Vector3(0, 50, 0);
    
        CheckOutRange();

        // Shadow.transform.position = new UnityEngine.Vector3(this.transform.position.x, -1.4f, 0);

        EditShadow(this.transform.position);

        // Delay Move
        if(DelayForMoveAfterFloor > 0)
        {
            DelayForMoveAfterFloor -= 1f * Time.deltaTime;
        }
        else
        {
            if(DelayForMoveAfterFloor != -1f)
            {
                IsBeHit = false;
                if(Skin != null)
                    Skin.SetParameter_IsBeHit(IsBeHit);
            }
        }   

    }

    protected bool IsDelayAttackWhenBeHit = false;

    public IEnumerator BeHitByPlayer(UnityEngine.Vector2 Vec, float Damage) // 1: normal hit, 2: knockback, 3: thrown away hit
    {
        //
        SoundManager.Instance.GetPlayerSound("Combo1").AudioPlay();

        IsDelayAttackWhenBeHit = true;

        Hp -= Damage;

        if(IsDead == false)
            Spawn_AttackEffect(); 

        if(Vec != Vector2.zero && IsDead == false)
        {
            IsBeHit = true;

            if(Skin != null)
                Skin.SetParameter_IsBeHit(IsBeHit);

            IsDelay = true;
            
            rg.velocity = Vector2.zero;

            if(Vec.y > 0)
            {
                Skin.SetParameter_IsFloor(false);

                if(Vec.y >= Mathf.Abs(Vec.x))
                {    
                    ThisRefab.SetBool("IsSpin", true);
                    // Debug.Log("isspin");
                }
                else
                {
                    ThisRefab.SetBool("IsSpin", false);
                }

                // Debug.Log("Hight");
                DelayForMoveAfterFloor = -1f;
            }
            else
            {
                ThisRefab.SetBool("IsSpin", false);

                IsBeHit = false;
                if(Skin != null)
                    Skin.SetParameter_IsBeHit(IsBeHit);
            }

            rg.velocity = Vec;
            
            yield return new WaitForSeconds(0.2f);

            IsDelay = false;
        }
    }
 
    public void StartBeHit(UnityEngine.Vector2 Vec)
    {
        StartCoroutine(BeHitByPlayer(Vec, 1));
    }

    public void StartBeHitWithDamage(UnityEngine.Vector2 Vec, float Damage)
    {
        StartCoroutine(BeHitByPlayer(Vec, Damage));
    }


    void OnCollisionEnter2D(Collision2D Col)
    {
        if(Col.transform.tag == "Floor" && IsDead == false)
        {
            ThisRefab.SetBool("IsSpin", false);
            Skin.SetParameter_IsFloor(true);

            // Debug.Log("Enemy Floor");

            rg.velocity = Vector2.zero;

            DelayForMoveAfterFloor = 0.8f;
        }
    }

    private void Create_Skin(string Skin_Path)
    {
        if(Skin != null)
            return;

        // temp skin
        Skin = Resources.Load<Ani_Enemy>(Skin_Path);

        if(Skin != null)
        {
            Skin = Instantiate(Skin);

            if(this.transform.Find("Skin") == null)
            {
                GameObject s = new GameObject();
                s.transform.name = "Skin";
                s.transform.parent = this.transform;
                s.transform.position = this.transform.position;
                s.transform.localScale = new UnityEngine.Vector3(1, 1, 1);
            }

            ThisRefab = this.GetComponent<Animator>();

            Skin.transform.parent = this.transform.Find("Skin").transform;

            Skin.transform.localPosition = Vector3.zero;    

            Skin.transform.localScale = new UnityEngine.Vector3(1, 1, 1);
        }
        else
        {
            // Debug.Log("Skin false");

        }
    }

//
     private void CheckShadow()
    {
        if(Shadow == null)
        {
            Shadow = Resources.Load<GameObject>("Refabs/Environment/Shadow");
            Shadow = Instantiate(Shadow);

            Shadow.gameObject.SetActive(false);

            Shadow.transform.localScale *= 2.2f;
            Shadow.transform.position = new UnityEngine.Vector3(20, -2.55f, 0);


            // Shadow.transform.parent =  this.transform.parent.transform.Find("Shadow_List").transform;
            Shadow_Scale = Shadow.transform.localScale;
        }
    }

    private void EditShadow(UnityEngine.Vector3 pos_)
    {   
        if(Shadow != null)
        {
            Shadow.transform.position = new UnityEngine.Vector3(this.transform.position.x, -2.55f, 0);

            float Distance = UnityEngine.Vector3.Distance(pos_, Shadow.transform.position) - 1.5f;
            float MaxDistance = 4.5f;

            Distance = Mathf.Abs(Distance / MaxDistance);  

            // Debug.Log("Distance" + Distance);

            if(Distance < 1)
            {   
                
                Shadow.transform.localScale = Shadow_Scale -  Shadow_Scale * Distance;
            }
            else
            {
                Shadow.transform.localScale = UnityEngine.Vector3.zero;        
            }
        }
    }

    // Set AttackEffect
    private void Spawn_AttackEffect()
    {
        int dir;

        if(DirectionMove == 1)
            dir = 1;
        else
            dir = -1;

        var pos = this.transform.position + new UnityEngine.Vector3(dir * Random.Range(0f, 0.4f), Random.Range(-0.2f, 0.5f), 0f);

        GameObject AF = GetPoolAttackEffect();
        
        AF.transform.position = pos;

        AF.gameObject.SetActive(true);

        // FxManager.Instance.FxDamage(pos, Random.Range(10, 100));
    }

    // Enemy Death
    public IEnumerator EnemyDeath(int Dir)
    {
        if(IsDisableSound == false)
        {
            SoundManager.Instance.GetEnemySound("EnemyDead").AudioPlay();
            EnemyManager.Instance.EnemyKILL();
        }

        if(Skin != null)
        {
            Skin.SetParameter_IsBeHit(true);
            Skin.SetParameter_IsIdle(true);
            Skin.SetParameter_IsStop(false);
        }

        IsDelay = true;

        if(Dir == 1)
            rg.velocity = new UnityEngine.Vector3(-3f, 8f, 0f);
        else
            rg.velocity = new UnityEngine.Vector3(3f, 8f, 0f);

        ThisRefab.SetBool("IsSpin", true);
        
        this.GetComponent<Collider2D>().isTrigger = true;

        if(Shadow != null)
            Shadow.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        SetActiveObj(false);
    }

    private void CheckOutRange()
    {
        if(IsDead)
            return;

        if(this.transform.position.x <= -20 || this.transform.position.x >= 20)
        {
            IsDisableSound = true;
            Hp = 0;
        }
    }

    // Pooling Attack Effect
    private List<GameObject> PoolAttackEffect = new List<GameObject>();

    private int AmountPoolAttackEffect = 3; // 2 normal attack range, 1 circle attack

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

    // SetActiveFalse

    public void SetActiveObj(bool Bl)
    {
        if(Bl == true)
        {
            IsDead = false;
        }

        this.gameObject.SetActive(Bl);
        
        CheckShadow();

        if(Shadow != null)
            Shadow.gameObject.SetActive(Bl);
    }
}
