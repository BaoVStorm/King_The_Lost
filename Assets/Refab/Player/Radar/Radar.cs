using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField]
    private Move Player;

    // public Enemy NearestEnemy;

    public Collider2D NearestEnemy;

    public bool Direction; // true = Right, false = Left

    private float ChangeZ = 0.1f;

    void Start()
    {
        ChangeZ = 0.1f;
        NearestEnemy = null;
        Player = this.GetComponentInParent<Move>();
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

        // follow Player
        if(Direction)
            this.transform.position = Player.transform.position + new UnityEngine.Vector3(1.3f, 0f, 0f) + new UnityEngine.Vector3(0f, 0.5f, 0f);
        else
            this.transform.position = Player.transform.position - new UnityEngine.Vector3(1.3f, 0f, 0f) + new UnityEngine.Vector3(0f, 0.5f, 0f);
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
                if(UnityEngine.Vector2.Distance(Player.transform.position, NearestEnemy.transform.position) > UnityEngine.Vector2.Distance(Player.transform.position, Col.transform.position))
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
