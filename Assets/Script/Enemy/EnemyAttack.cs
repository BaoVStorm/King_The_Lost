using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Enemy_Base Enemy_Base;

    [SerializeField]
    private EnemyAttackRange EnemyAttackRange;

    private bool IsfirstAttack;

    private bool IsDelayForAttack;

    // Start is called before the first frame update
    void Start()
    {
        Enemy_Base = this.GetComponent<Enemy_Base>();

        EnemyAttackRange = Resources.Load<EnemyAttackRange>("Refabs/Enemy/Attack/EnemyAttackRange");

        IsDelayForAttack = false;

        IsfirstAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Enemy_Base.IsStop)
        {
            if(IsfirstAttack)
            {
                IsDelayForAttack = true;
                StartCoroutine(DelayFirstAttack());
                
                IsfirstAttack = false;
            }
        }
        else
        {
            IsfirstAttack = true;
        }

        if(Enemy_Base.IsStop && IsDelayForAttack == false)
        {   
            IsDelayForAttack = true;
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        EnemyAttackRange EAR = Instantiate(EnemyAttackRange);

        EAR.transform.parent = this.transform;

        EAR.transform.localPosition = new UnityEngine.Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.2f);

        EAR.DestroyObj();

        yield return new WaitForSeconds(1.3f);

        IsDelayForAttack = false;
    }

    IEnumerator DelayFirstAttack()
    {
        yield return new WaitForSeconds(1f);
        
        IsDelayForAttack = false;
    }
}
