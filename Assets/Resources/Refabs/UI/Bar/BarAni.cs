using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarAni : MonoBehaviour
{
    Animator Ani;

    void Start() 
    {
        Ani = this.GetComponent<Animator>();
    }

    public void Set_IsAct_True()
    {
        Ani.SetBool("IsAct", true);
    }

    public void Set_IsAct_False()
    {
        Ani.SetBool("IsAct", false);
    }
}
