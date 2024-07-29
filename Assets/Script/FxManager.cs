using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    private static FxManager _instance;
    public static FxManager Instance => _instance;
    private void Awake() {
        _instance = this;
    }
    [SerializeField]
    private FxDamage damagePrefab;
    public void FxDamage(Vector3 pos, float FxDamage)
    {
        pos.z = 0;
        FxDamage fx = Instantiate(this.damagePrefab, this.transform);
        fx.gameObject.SetActive(true);
        fx.transform.position = new Vector3(pos.x, pos.y);
        fx.ParseDamager(FxDamage);
    }
}
