using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.U2D.Common;

public class Animation_temp : MonoBehaviour
{
    public Animator animator_;
    
    private Move Player;

    public bool a;

    void Start()
    {
        animator_ = this.GetComponent<Animator>();
    }

    public void Set_Player(Move Player_)
    {
        Player = Player_;
    }

    public void Set_Attack_InAnimation()
    {
        animator_.SetBool("Attack", false);
    }

    public void Set_Attack_True()
    {
        animator_.SetBool("Attack", true);
    }

    public void Set_StopCombo_False()
    {
        animator_.SetBool("StopCombo", false);
        // Debug.Log("StopCombo false");
    }

    public void Set_StopCombo_True()
    {
        animator_.SetBool("StopCombo", true);
        // Debug.Log("StopCombo true");
    }

    public void Set_IsJumpAttack_False()
    {
        animator_.SetBool("IsJumpAttack", false);
    }

    public void Set_IsCircleAttack_False()
    {
        animator_.SetBool("IsCircleAttack", false);
    }

    public void Set_IsThrowAwayAttack_False()
    {
        animator_.SetBool("IsThrowAwayAttack", false);
    }

    public void Set_IsCrouchAttack_False()
    {

        animator_.SetBool("IsCrouchAttack", false);

        Player.IsDelayStopDown = false;
        Player.IsCrouch = true;
        animator_.SetBool("IsCrouch", true);
    }

    public void Set_IsDead_False()
    {
        animator_.SetBool("IsDead", false);
    }
}
