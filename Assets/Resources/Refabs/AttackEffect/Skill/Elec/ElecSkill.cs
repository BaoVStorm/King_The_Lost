using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecSkill : MonoBehaviour
{
    // Animation End
    public void DestroyObj()
    {
        gameObject.SetActive(false);
    }

    // Skill
    void OnTriggerEnter2D(Collider2D Col)
    {
        if(Col.tag == "Enemy")
        {
            Enemy_Base Enemy_ = Col.GetComponent<Enemy_Base>();

            Enemy_.StartBeHitWithDamage(new UnityEngine.Vector2(0, 0), 10); // more damage
        }
    }
}
