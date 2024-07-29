using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    private float ChangePosZ = 1f;

    // Start is called before the first frame update
    void Start()
    {   
        ChangePosZ = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(ChangePosZ > 0)
            ChangePosZ = -1f;
        else
            ChangePosZ = 1f;
        this.transform.position += new UnityEngine.Vector3(0, 0, 0.1f) * ChangePosZ;
    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        if(Col.tag == "Player")
        {
            Player player = Col.GetComponent<Player>();

            player.BeHit();

            Debug.Log("Hp Player -1 - IS a script, Not -hp");
        }
    }
}
