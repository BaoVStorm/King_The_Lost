using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private float Speed;

    [SerializeField]
    private bool IsEnemyThrow; // true: Player Throw Weapon, false: Enemy throw weapon
    private Rigidbody2D rg;

    [SerializeField]
    private int DirectionMove = 1; // 1: Right, 2: Left

    [SerializeField]
    private RadarWeapon RadarWeapon;

    [SerializeField]
    private Player Player;

    private bool IsWeaponPlayer = false;

    private bool IsPlayerHoldWeapon = false;

    [SerializeField]
    private GameObject SkinWeapon;

    private float ChangeZ = 0.1f;

    private Animator AniWeapon;

    public WeaponStatus StatusWeapon;

    private bool IsSafe;

    public void Set_Player(Player Player_) 
    {
        Player = Player_;
    }

    public void Set_DirectionMove(int Direct) // 1: Right, 2: Left
    {
        if(AniWeapon == null)
            AniWeapon = this.GetComponent<Animator>();

        if(Direct == 1)
        {
            AniWeapon.SetBool("IsDirRight", true);
        }
        else
        {
            AniWeapon.SetBool("IsDirRight", false);
        }

        if(RadarWeapon == null)
        {
            RadarWeapon = Resources.Load<RadarWeapon>("Refabs/Weapon/Radar/WeaponRadar");
            RadarWeapon = Instantiate(RadarWeapon);

            SetSkinWeapon();

            RadarWeapon.transform.parent = this.transform;
        }

        if(Direct == 1)
            RadarWeapon.transform.localPosition = new UnityEngine.Vector3(1f, 0f, 0f);
        else
            RadarWeapon.transform.localPosition = new UnityEngine.Vector3(-1f, 0f, 0f);

        DirectionMove = Direct;

        // Debug.Log("Direct" + Direct);

        RadarWeapon.Set_Direction(Direct);
        RadarWeapon.Set_Parent(this);
    }

    private void SetSkinWeapon()
    {
        SkinWeapon = Resources.Load<GameObject>("Refabs/Weapon/Skin/Shield");
        SkinWeapon = Instantiate(SkinWeapon);

        SkinWeapon.transform.parent = this.transform.Find("Skin").transform;

        SkinWeapon.transform.localPosition = UnityEngine.Vector3.zero;

        // Status Weapon
        if(StatusWeapon == null)
            return;

        StatusWeapon = Instantiate(StatusWeapon);

        StatusWeapon.transform.parent = SkinWeapon.transform;

        StatusWeapon.transform.localPosition = UnityEngine.Vector3.zero;
    }

    public void Set_IsEnemyThrow(bool Pt) 
    {
        IsEnemyThrow = Pt;
    }

    void Start()
    {
        // Player = Resources.Load<Player>("Refabs/Player/Player");

        ChangeZ = 0.1f;

        Speed = 5f;

        rg = this.GetComponent<Rigidbody2D>();

        AniWeapon = this.GetComponent<Animator>();

        IsSafe = false;
    }

    void Update()
    {
        if(IsPlayerHoldWeapon == true)
            return;

        if(DirectionMove == 1)  // 1: right, 2: left
        {

            rg.velocity = new UnityEngine.Vector2(Speed, rg.velocity.y);

            if(this.transform.position.x > 1.1f)  
            {
                StatusWeapon.SetStatusSafe();

                IsSafe = true;
            }   
            else
            {
                StatusWeapon.SetStatusUnsafe();

                IsSafe = false;
            }
        }
        else
        {
            rg.velocity = new UnityEngine.Vector2(-Speed, rg.velocity.y);

            if(this.transform.position.x < -1.1f)  
            {
                StatusWeapon.SetStatusSafe();

                IsSafe = true;
            }   
            else
            {
                StatusWeapon.SetStatusUnsafe();

                IsSafe = false;
            }
        }

        // change position Of z
        if(ChangeZ == 0.1f)
        {
            this.transform.position += new UnityEngine.Vector3(0, 0, ChangeZ);
            ChangeZ = -0.1f;
        }
        else
        {
            this.transform.position += new UnityEngine.Vector3(0, 0, ChangeZ);
            ChangeZ = 0.1f;
        }

        if(this.transform.position.x <= -15f || this.transform.position.x >= 15f)
        {
            this.gameObject.SetActive(false);
        }

    }

    void OnTriggerStay2D(Collider2D Col)
    {
        if(Col.tag == "Enemy")
        {
            if(RadarWeapon != null && Col == RadarWeapon.NearestEnemy)
            {
                Enemy_Base En = Col.GetComponent<Enemy_Base>();

                if(En.Get_DirectionMove() != DirectionMove)
                {
                    if(DirectionMove == 1)
                        En.StartBeHit(new UnityEngine.Vector2(4f, 3.5f));
                    else
                        En.StartBeHit(new UnityEngine.Vector2(-4f, 3.5f));
                    
                    this.gameObject.SetActive(false);
                }
            }
        }
        else
        if(Col.tag == "Player" && IsEnemyThrow && IsWeaponPlayer == false)
        {
            if(Player != null )
                if(!Player.PlayerMove.Get_IsCrouch())
                {
                    Player.BeHit();

                    this.gameObject.SetActive(false);
                }
        }
    }

    public bool GetIsSafe()
    {
        return IsSafe;
    }

    // Player Pick Up and Throw

    public void PlayerPickUp()
    {
        this.gameObject.SetActive(false);
    }

    public void SetPlayerWeapon()   // Weapon belong to Player
    {
        StatusWeapon.gameObject.SetActive(false);

        IsWeaponPlayer = true;
        
        IsPlayerHoldWeapon = true;
    }

    public void PlayerThrow()
    {
        Speed += 1.2f;

        IsPlayerHoldWeapon = false;
    }
}
