using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FxDamage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtDamage;
    public void ParseDamager(float damage)
    {
        this.txtDamage.text = damage.ToString();
        float y = this.transform.localPosition.y;
        Sequence seq = DOTween.Sequence();
        seq.Join(this.transform.DOLocalMoveY(y + 100, 1f));
        seq.Join(this.transform.DOPunchScale(new Vector3(1,1), 0.5f, 2));
        seq.OnComplete(()=>{
            Destroy(this.gameObject);
        });
    }
}
