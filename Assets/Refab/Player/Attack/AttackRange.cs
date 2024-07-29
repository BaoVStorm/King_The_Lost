using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private Move PlayerMove;

    private UnityEngine.Vector2 Vec = Vector2.zero;

    [SerializeField]
    private bool IsCircle;

    private UnityEngine.Vector3 Pos;

    public void Set_Circle(bool Cr)
    {
        IsCircle = Cr;
    }

    public bool Get_Circle()
    {
        return IsCircle;
    }

    public void Set_Pos(UnityEngine.Vector3 Pos_)
    {
        Pos = Pos_;
    }

    public void Set_Vec(UnityEngine.Vector2 Vec_)
    {
        Vec = Vec_;
    }

    public UnityEngine.Vector2 Get_Vec()
    {
        return Vec;
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerMove = this.GetComponentInParent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.parent != null)
        {
            this.transform.position = this.transform.parent.position + Pos;
        }
    }

     void OnTriggerEnter2D(Collider2D Col)
     {
        // Enemy 
        if(Col.tag == "Enemy" && (Get_Circle() == true || Col == PlayerMove.RadarLeft.NearestEnemy || Col == PlayerMove.RadarRight.NearestEnemy))
        {
            SoundManager.Instance.GetPlayerSound("Combo1");

            // rg.velocity = Vector2.zero;

            // Debug.Log("Hit Enemy");

            Enemy_Base En = Col.GetComponent<Enemy_Base>();

            if(Get_Circle() == true)
            {
                // Debug.Log("iscirle");

                UnityEngine.Vector2 VecDir = En.transform.position - this.transform.position;
                
                if(VecDir.y <= 1f)
                {   
                    if(VecDir.x > 0)
                        VecDir.x = Mathf.Abs(Get_Vec().x);
                    else
                        VecDir.x = -1 * Mathf.Abs(Get_Vec().x);

                    VecDir.y = Get_Vec().y;
                }
                else
                    VecDir = VecDir.normalized * 6.5f;

                En.StartBeHit(VecDir);

                gameObject.SetActive(false);
            }
            else
            {
                // Debug.Log("Not cirle" + Get_Vec());
                En.StartBeHit(Get_Vec());
            }
        }

        // Weapon
        if(Col.tag == "Weapon" && Get_Circle() == false)
        {
            Weapon Wp = Col.GetComponent<Weapon>();

            if(Wp.GetIsSafe())
            {
                PlayerMove.PickUpWeapon(Wp);

                Wp.PlayerPickUp();
            }
        }
    }
}
