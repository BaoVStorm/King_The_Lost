using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackSkillEnemy3 : MonoBehaviour
{
    public float DistanceSkill = 4f;

    public float TimeDistanceSkill = 3f;

    private float ScaleX = 0f;

    private int Dir = 1;

    // Start is called before the first frame update
    void Start()
    {
        DistanceSkill = 3.5f;
        TimeDistanceSkill = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeInHierarchy)
        {
            if(ScaleX <= DistanceSkill - 0.1f)
            {
                ScaleX += Time.deltaTime * TimeDistanceSkill;
            }
            else
            {
                ScaleX = DistanceSkill;
            }

            this.transform.localScale = new UnityEngine.Vector3(ScaleX * Dir, transform.localScale.y, transform.localScale.z);
        }
    }

    public void SetActiveObj(bool Bl)
    {       
        ScaleX = 0f;
        this.transform.localScale = new UnityEngine.Vector3(ScaleX, transform.localScale.y, transform.localScale.z);

        this.transform.rotation = Quaternion.Euler(0,0,0);

        gameObject.SetActive(Bl);
    }

    public void Set_Direction(int Direct) // 1 right, 2 left
    {
        if(Direct == 2)
            Dir = 1;
        else
            Dir = -1;
    }
}
