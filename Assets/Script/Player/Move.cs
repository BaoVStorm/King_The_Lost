using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Move : MonoBehaviour
{
    //Skin
    public Animator SkinOfPlayer;
    public Animator ThisRefab;
    //shadow
    private GameObject Shadow;
    private UnityEngine.Vector3 Shadow_Scale;
    //normal 
    private Rigidbody2D rg;

    private float PowerJump;

    [SerializeField]
    private int Combo = 4;

    [SerializeField]
    private float TimeResetCombo = 0f;

    public int DoubleJump = 2;

    [SerializeField]
    private int LookDirection = 1;

    public bool IsCrouch = false;

    [SerializeField]
    private bool IsJumpDown = false;

    [SerializeField]
    private bool IsDelayCrouchAttack = false;

    [SerializeField]
    public bool IsDelayStopDown = false;

    [SerializeField]
    private bool IsDead = false;

    // AttackRange
    public AttackRange AttackRange;
    public AttackRange CircleAttack;
    public AttackRange RadarAttack;
    // Radar

    public Radar RadarRight;
    public Radar RadarLeft;

    // Touch Infor
    private UnityEngine.Vector3 FirstTouch_Pos;

    private bool IsMoveTouching;
    [SerializeField]
    
    private UnityEngine.Vector2 Pos_DisableTouch;

    // Crouch
    float TimeDelayCrouchAttack =0;

    // Throw Weapon
    private bool IsThrowWeapon = false;
    private Weapon WpPlayer;

    public GameObject PlayerHandPickUpWeapon;

    // ---- Functions ----
    public int Get_LookDirection()
    {
        return LookDirection;
    }

    void Start()
    {
        Start_PoolingAttackRange();

        CheckShadow();

        Pos_DisableTouch = new UnityEngine.Vector2(0f, 4.034f);

        IsMoveTouching = false;

        ThisRefab = this.GetComponent<Animator>();
        SkinOfPlayer = this.GetComponentInChildren<Animator>();

        SkinOfPlayer.GetComponent<Animation_temp>().Set_Player(this);

        Combo = 4;

        // SkinOfPlayer.SetInteger("Combo", Combo);
        SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);
        SkinOfPlayer.SetBool("IsJump", false);
        SkinOfPlayer.SetBool("IsCrouch", false);

        rg = this.GetComponent<Rigidbody2D>();

        PowerJump = 11f;
        
        // --- Find Left Hand ---
        Start_SettingPlayerHand();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDead)
            return;

            if(TimeDelayCrouchAttack > 0)
            {
                IsDelayCrouchAttack = true;
                TimeDelayCrouchAttack += -1 * Time.deltaTime;
            }
            else
            {
                IsDelayCrouchAttack = false;
                TimeDelayCrouchAttack = 0;
            }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if(!Check_DisableTouch(Camera.main.ScreenToWorldPoint(touch.position)))
            {     
                //touch
                if(touch.phase == TouchPhase.Began)
                {
                    FirstTouch_Pos = Camera.main.ScreenToWorldPoint(touch.position);

                    //Right
                    if(FirstTouch_Pos.x > 0)
                    {
                        // Debug.Log("Right touch");
                        Right();
                    }

                    //Left
                    if(FirstTouch_Pos.x <= 0)
                    {
                        // Debug.Log("Left touch");
                        Left();
                    }
                }

                //move
                if(touch.phase == TouchPhase.Moved && IsMoveTouching == false)
                {
                    UnityEngine.Vector3 Touch_Pos = Camera.main.ScreenToWorldPoint(touch.position);

                    if(UnityEngine.Vector3.Distance(Touch_Pos, FirstTouch_Pos) >= 0.75f)
                    {
                        IsMoveTouching = true;

                        if(Touch_Pos.y > FirstTouch_Pos.y)
                        {
                            // Debug.Log("Up move");
                            Up();
                        }
                        else
                        {
                            // Debug.Log("Down move");
                            Down();
                        }
                    }
                }

                if(touch.phase == TouchPhase.Ended)
                {
                    // FirstTouch_Pos = ;
                    IsMoveTouching = false;
                }
            }
        }


        EditShadow(this.transform.position);

        // position = vector3.MoveTowards(this.position, that.position, speed)

        // Jump
        if(Input.GetKeyDown("w"))
        {
            Up();
        }
 
        // Crouch
        if(Input.GetKeyDown("s"))
        {
            Down();
        }

        // Attack Right
        if(Input.GetKeyDown("d"))
        {   
            Right();
        }

        // Attack Left
        if(Input.GetKeyDown("a"))
        {
            Left();
        }

        // Reset Combo
        if(TimeResetCombo > 0)
        {
            TimeResetCombo -= 1 * Time.deltaTime;
            SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);
        }
        else
        {
            TimeResetCombo = 0;
            SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);

            Combo = 4;
            // SkinOfPlayer.SetInteger("Combo", Combo);
        }
    }

    public void Set_IsDead(bool BOOL)
    {
        IsDead = BOOL;
    }

    public bool Get_IsDead()
    {
        return IsDead;
    }

    public IEnumerator Attack(UnityEngine.Vector3 Pos, AttackRange AR, bool IsCirle, UnityEngine.Vector2 Vec)
    {
        if(RadarAttack != null)
            RadarAttack.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.05f);

        AttackRange AttackRange_;

        if(IsCirle)
        {
            AttackRange_ = PoolAttackRangeCircle;
            AttackRange_.gameObject.SetActive(false); // cho chac chan rang active = false;
        }
        else
        {
            AttackRange_ = GetPoolAttackRange();
        }

        RadarAttack = AttackRange_;

        AttackRange_.Set_Circle(IsCirle);

        AttackRange_.Set_Vec(Vec);
        
        AttackRange_.transform.position = this.transform.position + Pos;
        AttackRange_.Set_Pos(Pos);

        AttackRange_.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        AttackRange_.gameObject.SetActive(false);
    }


    void OnCollisionEnter2D(Collision2D Col)
    {
        if(Col.transform.tag == "Floor")
        {
            SkinOfPlayer.SetBool("IsJump", false);

            DoubleJump = 2;

            if(IsJumpDown == true)
            {
                IsJumpDown = false;
                // Background.Instance.Shake_Background();
            }
        }
    }
    

    //shadow
    private void CheckShadow()
    {
        if(this.transform.Find("Shadow") == null)
        {
            Shadow = Resources.Load<GameObject>("Refabs/Environment/Shadow");
            Shadow = Instantiate(Shadow);

            Shadow.transform.position = new UnityEngine.Vector3(0, -2.55f, 0);

            Shadow.transform.parent = this.transform.Find("Skin").transform;
            Shadow.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
            Shadow_Scale = Shadow.transform.localScale;
        }
    }

    private void EditShadow(UnityEngine.Vector3 pos_)
    {   
        if(Shadow != null)
        {
            float Distance = UnityEngine.Vector3.Distance(pos_, Shadow.transform.position);
            float MaxDistance = 6f;

            Distance = Mathf.Abs(Distance / MaxDistance);  

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

    public void Set_Shadow_Scale(float x)
    {
        Shadow_Scale *= x;
    }

    // Be Hit By weapon
    public bool Get_IsCrouch()
    {
        return IsCrouch;
    }

    // Move
    private void Up()
    {
        if(DoubleJump > 0)
        {
            SkinOfPlayer.SetBool("IsCrouchAttack", false);
            IsDelayStopDown = false;

            IsCrouch = false;
            SkinOfPlayer.SetBool("IsCrouch", IsCrouch);

            SkinOfPlayer.SetBool("IsJump", true);

            if(DoubleJump != 1) // First Jump
            {
                SoundManager.Instance.GetPlayerSound("Jump").AudioPlay();

                rg.velocity = new UnityEngine.Vector2(0, PowerJump);
                
                // if(LookDirection == 2)
                //     StartCoroutine(Attack(new UnityEngine.Vector3(1.2f, 0, 0), AttackRange, false, new UnityEngine.Vector2(0, 0)));
                // else
                //     StartCoroutine(Attack(new UnityEngine.Vector3(-1.2f, 0, 0), AttackRange, false, new UnityEngine.Vector2(0, 0)));
            }
            else            // Second Jump
            {
                SoundManager.Instance.GetPlayerSound("CircleAttack").AudioPlay();

                rg.velocity = new UnityEngine.Vector2(0, 6f);

                SkinOfPlayer.SetBool("IsCircleAttack", true);

                StartCoroutine(Attack(new UnityEngine.Vector3(0, 0, 0), CircleAttack, true, new UnityEngine.Vector2(8f, 3f)));
            }

            DoubleJump--;
        }
    }

    private void Down()
    {
        if(IsDelayStopDown == true)
            return;

        SkinOfPlayer.SetBool("Attack", false);

        TimeResetCombo = 0f;
        SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);

        IsCrouch = true;
        SkinOfPlayer.SetBool("IsCrouch", IsCrouch);

        // TimeDelayCrouchAttack = 0.4f;

        if(DoubleJump != 2) // Player is Jumping
        {
            IsJumpDown = true;

            rg.velocity = new UnityEngine.Vector2(0, -20f);

            if(LookDirection == 2)
                StartCoroutine(Attack(new UnityEngine.Vector3(0.9f, -0.2f, 0), AttackRange, false, new UnityEngine.Vector2(9f, 2.5f)));
            else
                StartCoroutine(Attack(new UnityEngine.Vector3(-0.9f, -0.2f, 0), AttackRange, false, new UnityEngine.Vector2(-9f, 2.5f)));
        }
        else    // Player is in Floor
        {
            if(LookDirection == 2)
                StartCoroutine(Attack(new UnityEngine.Vector3(1.3f, -0.2f, 0), AttackRange, false, new UnityEngine.Vector2(0, 0)));
            else
                StartCoroutine(Attack(new UnityEngine.Vector3(-1.3f, -0.2f, 0), AttackRange, false, new UnityEngine.Vector2(0, 0)));
        
        }
    }

    private void Right()
    {
        if(IsThrowWeapon)
        {
            LookDirection = 2;
            this.transform.rotation = UnityEngine.Quaternion.Euler(0, 180, 0);

            ThrowWeapon();
        }
        else
        if(!SkinOfPlayer.GetBool("StopCombo"))
        {
            SoundManager.Instance.GetPlayerSound("Attack").AudioPlay();

            LookDirection = 2;
            this.transform.rotation = UnityEngine.Quaternion.Euler(0, 180, 0);

            if(DoubleJump == 0 || DoubleJump == 1) // đang nhảy
            {
                TimeResetCombo = 0;
                SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);
                StartCoroutine(Attack(new UnityEngine.Vector3(1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(10f, 2f)));

                SkinOfPlayer.SetBool("IsJumpAttack", true);
            }
            else
            if(IsCrouch == false)   // đang đứng
            {
                if(TimeResetCombo <= 0)
                {
                    Combo = 4;
                    // SkinOfPlayer.SetInteger("Combo", Combo);
                }

                TimeResetCombo = 1f;
                SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);
                // SkinOfPlayer.SetBool("Attack", true);

                if(Combo == 4)  
                {
                    SkinOfPlayer.SetBool("Attack", true);
                    SkinOfPlayer.SetInteger("Combo", Combo);
                    StartCoroutine(Attack(new UnityEngine.Vector3(1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(0f, 0f)));
                }
                else
                if(Combo == 3)
                {
                    SkinOfPlayer.SetBool("Attack", true);
                    SkinOfPlayer.SetInteger("Combo", Combo);
                    StartCoroutine(Attack(new UnityEngine.Vector3(1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(0f, 0f)));
                } 
                else
                if(Combo == 2)
                {
                    SkinOfPlayer.SetBool("Attack", true);
                    SkinOfPlayer.SetInteger("Combo", Combo);
                    StartCoroutine(Attack(new UnityEngine.Vector3(1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(2f, 9f)));
                }
                else
                {
                    SkinOfPlayer.SetBool("Attack", true);
                    SkinOfPlayer.SetInteger("Combo", Combo);
                    Combo = 5;
                    SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);

                    StartCoroutine(Attack(new UnityEngine.Vector3(2f, 0.5f, 0), AttackRange, false, new UnityEngine.Vector2(9f, 4.5f)));
                }

                Combo--;
                // SkinOfPlayer.SetInteger("Combo", Combo);

                // SkinOfPlayer.SetBool("Attack", false);
            }
            else                    // đang ngồi
            {
                if(IsDelayCrouchAttack == false)
                {
                    TimeDelayCrouchAttack = 0.6f;

                    IsCrouch = false;
                    SkinOfPlayer.SetBool("IsCrouch", IsCrouch);

                    TimeResetCombo = 0;
                    SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);

                    SkinOfPlayer.SetBool("IsThrowAwayAttack", true);

                    StartCoroutine(Attack(new UnityEngine.Vector3(1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(0.2f, 12f)));
                }
                else
                {
                    StartCoroutine(Attack(new UnityEngine.Vector3(1.5f, -0.2f, 0), AttackRange, false, new UnityEngine.Vector2(0, 0)));
                    StartCoroutine( CrouchAttack());
                }
            }
        }
    }

    private void Left()
    {
        if(IsThrowWeapon)
        {
            LookDirection = 1;
            this.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            ThrowWeapon();
        }
        else
        if(!SkinOfPlayer.GetBool("StopCombo"))
        {
            SoundManager.Instance.GetPlayerSound("Attack").AudioPlay();

            LookDirection = 1;
            this.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 0);

            if(DoubleJump == 0 || DoubleJump == 1) // đang nhảy
            {
                TimeResetCombo = 0;
                SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);

                StartCoroutine(Attack(new UnityEngine.Vector3(-1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(-10f, 2f)));

                SkinOfPlayer.SetBool("IsJumpAttack", true);
            }
            else
            if(IsCrouch == false)   // đang đứng
            {
                if(TimeResetCombo <= 0)
                {
                    Combo = 4;
                    // SkinOfPlayer.SetInteger("Combo", Combo);
                }

                TimeResetCombo = 1f;
                SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);

                if(Combo == 4)  
                {
                    SkinOfPlayer.SetBool("Attack", true);
                    SkinOfPlayer.SetInteger("Combo", Combo);
                    StartCoroutine(Attack(new UnityEngine.Vector3(-1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(0f, 0f)));
                }
                else
                if(Combo == 3)
                {
                    SkinOfPlayer.SetBool("Attack", true);
                    SkinOfPlayer.SetInteger("Combo", Combo);
                    StartCoroutine(Attack(new UnityEngine.Vector3(-1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(0f, 0f)));
                } 
                else
                if(Combo == 2)
                {
                    SkinOfPlayer.SetBool("Attack", true);
                    SkinOfPlayer.SetInteger("Combo", Combo);
                    StartCoroutine(Attack(new UnityEngine.Vector3(-1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(-2f, 9f)));
                }
                else
                {
                    SkinOfPlayer.SetBool("Attack", true);
                    SkinOfPlayer.SetInteger("Combo", Combo);
                    Combo = 5;
                    SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);

                    StartCoroutine(Attack(new UnityEngine.Vector3(-2f, 0.5f, 0), AttackRange, false, new UnityEngine.Vector2(-9f, 4.5f)));
                }

                Combo--;
                // SkinOfPlayer.SetInteger("Combo", Combo);
            }
            else                    // đang ngồi
            {
                if(IsDelayCrouchAttack == false)
                {
                    TimeDelayCrouchAttack = 0.6f;

                    IsCrouch = false;
                    SkinOfPlayer.SetBool("IsCrouch", IsCrouch);

                    TimeResetCombo = 0;
                    SkinOfPlayer.SetFloat("TResetC", TimeResetCombo);

                    SkinOfPlayer.SetBool("IsThrowAwayAttack", true);

                    StartCoroutine(Attack(new UnityEngine.Vector3(-1.8f, 0, 0), AttackRange, false, new UnityEngine.Vector2(0.2f, 12f)));
                }
                else
                {
                    StartCoroutine(Attack(new UnityEngine.Vector3(-1.5f, -0.2f, 0), AttackRange, false, new UnityEngine.Vector2(0, 0)));
                    StartCoroutine( CrouchAttack());
                }
            }
        }
    }

    IEnumerator CrouchAttack()
    {
        IsDelayStopDown = true;

        IsCrouch = false;
        SkinOfPlayer.SetBool("IsCrouch", false); //xoa crouch
        // attack crouc true
        SkinOfPlayer.SetBool("IsCrouchAttack", true);

        yield return new WaitForSeconds(0f);
    }

    // Check Disable Touch

    private bool Check_DisableTouch(UnityEngine.Vector3 Pos_Touch)
    {
        // float Xt = Pos_Touch.x, Yt = Pos_Touch.y, X1 = Pos_DisableTouch.x - 0.5f, X2 = Pos_DisableTouch.x + 0.5f, Y1 = Pos_DisableTouch.y - 0.5f, Y2 = Pos_DisableTouch.y + 0.5f;

        // if(X1 <= Xt && Xt <= X2 && Y1 <= Yt && Yt <= Y2)
        //     return true;
        // else
        //     return false;

        return false;
    }

    // Set StopComBo
    public void Set_StopComBo(bool check)
    {
        SkinOfPlayer.SetBool("StopCombo", check);
    }
    

    // ---- Pooling AttackRange ----

    private List<AttackRange> PoolAttackRange = new List<AttackRange>();
    private AttackRange PoolAttackRangeCircle;

    private int AmountPoolAttackRange = 2; // 2 normal attack range, 1 circle attack

    private void Start_PoolingAttackRange()
    {
        for(int i = 0; i < AmountPoolAttackRange; i++)
        {
            AttackRange AR = Instantiate(AttackRange);
            AR.transform.parent = this.transform;
            AR.gameObject.SetActive(false);
            PoolAttackRange.Add(AR);
        }

        PoolAttackRangeCircle = Instantiate(CircleAttack);
        PoolAttackRangeCircle.transform.parent = this.transform;
        PoolAttackRangeCircle.gameObject.SetActive(false);
    }

    private AttackRange GetPoolAttackRange()
    {
        for(int i = 0; i < PoolAttackRange.Count; i++)
        {
            if(!PoolAttackRange[i].gameObject.activeInHierarchy)
            {
                return PoolAttackRange[i];
            }
        }

        AttackRange AR = Instantiate(AttackRange);
        AR.transform.parent = this.transform;
        AR.gameObject.SetActive(false);
        PoolAttackRange.Add(AR);

        return AR;
    }

    // Pick Up And Throw Weapon
    private void Start_SettingPlayerHand()
    {
        PlayerHandPickUpWeapon = this.transform.Find("Skin").gameObject;

        PlayerHandPickUpWeapon = PlayerHandPickUpWeapon.transform.GetChild(0).transform.Find("LeftHand").gameObject;
    }

    public void PickUpWeapon(Weapon Wp)
    {
        if(Wp == null || PlayerHandPickUpWeapon == null || IsThrowWeapon == true)
            return;

        WpPlayer = Instantiate(Wp);

        IsThrowWeapon = true;

        WpPlayer.SetPlayerWeapon();

        WpPlayer.transform.parent = PlayerHandPickUpWeapon.transform;

        WpPlayer.transform.localPosition = UnityEngine.Vector3.zero;
    }

    public void ThrowWeapon()
    {
        float dir;

        if(WpPlayer == null)
            return;

        IsThrowWeapon = false;

        WpPlayer.Set_DirectionMove(LookDirectToMoveDirect());

        WpPlayer.transform.SetParent(null);

        if(LookDirection == 1)
            dir = -1;
        else
            dir = 1;

        WpPlayer.transform.position = this.transform.position + new UnityEngine.Vector3(dir * 1.45f, 1, 0);

        WpPlayer.PlayerThrow();

        WpPlayer = null;
    }

    public int LookDirectToMoveDirect()
    {
        if(LookDirection == 1)
            return 2;
        else
            return 1;
    }
}
