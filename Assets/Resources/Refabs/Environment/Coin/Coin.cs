using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public TextMeshProUGUI PointCoin;

    void Update()
    {
        if(PointCoin != null)
            PointCoin.text = EnemyManager.Instance.Point.ToString("0");
    }
}
