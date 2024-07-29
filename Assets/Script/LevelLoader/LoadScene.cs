using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    private Animator Ani;

    // Start is called before the first frame update
    void Start()
    {
        Ani = this.GetComponent<Animator>();
    }

    public void Set_IsEnd_false()
    {
        Ani.SetBool("IsEnd", false);
    }

    public void Set_IsStart_false()
    {
        Ani.SetBool("IsStart", false);
    }
}
