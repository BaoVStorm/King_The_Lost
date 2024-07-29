using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarWeapon : MonoBehaviour
{
    [SerializeField]
    private Weapon Weapon;

    public Collider2D NearestEnemy;

    public int Direction; // 1 = Right, 2 = Left

    private float ChangeZ = 0.1f;

    public void Set_Direction(int Dr)
    { 
        Direction = Dr;
    }

    public void Set_Parent(Weapon Wp)
    { 
        Weapon = Wp;
    }

    void Start()
    {
        ChangeZ = 0.1f;
        NearestEnemy = null;
    }

    void Update()
    {
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

        // Follow Enemy
        this.transform.localPosition = new UnityEngine.Vector3(0f, 0f, 0f);
        this.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
    }

    void OnTriggerStay2D(Collider2D Col)
    {
        if(Col.tag == "Enemy" && NearestEnemy != Col)
        {
            if(NearestEnemy == null)
            {
                NearestEnemy = Col;
            }
            else
            {
                if(Weapon != null)
                    if(UnityEngine.Vector2.Distance(Weapon.transform.position, NearestEnemy.transform.position) > UnityEngine.Vector2.Distance(Weapon.transform.position, Col.transform.position))
                    {
                        NearestEnemy = Col;
                    }
            }
        }
    }

    void OnTriggerExit2D(Collider2D Col)
    {
        if(Col == NearestEnemy)
        {
            NearestEnemy = null;
        }
    }
}
