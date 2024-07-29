using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeSkill : MonoBehaviour
{
    private Animator ani;

    [SerializeField]
    private int DirectionMove = 0; // 1: right  2: left

    // Start is called before the first frame update
    void Start()
    {
        ani = this.GetComponent<Animator>();
        
    }

    public void Set_DirectionMove(int Direct)
    {
        DirectionMove = Direct;

        if(ani == null)
            ani = this.GetComponent<Animator>();

        ani.SetInteger("DirectionMove", Direct);
    }

    public void Reset_DirectionMove()
    {
        if(ani != null)
            ani.SetInteger("DirectionMove", 0);
    }

    // Animation End
    public void DestroyObj()
    {
        gameObject.SetActive(false);

        Reset_DirectionMove();
    }

    // Skill
    void OnTriggerEnter2D(Collider2D Col)
    {
        if(Col.tag == "Enemy")
        {
            Enemy_Base Enemy_ = Col.GetComponent<Enemy_Base>();

            float Direct = 1;
            if(DirectionMove == 1)
                Direct = -1;

            Enemy_.StartBeHitWithDamage(new UnityEngine.Vector2(Direct * 9f , 3f), 3);
        }
    }
}
