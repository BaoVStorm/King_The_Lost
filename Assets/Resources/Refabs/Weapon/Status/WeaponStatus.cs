using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    public GameObject Status;

    private Renderer Render;

    // Start is called before the first frame update
    void Start()
    {
        if(Status != null)
            Render = Status.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStatusSafe()
    {
        if(Render == null)
            return;

        Render.material.SetColor("_Color", Color.green);
    }

    public void SetStatusUnsafe()
    {
        if(Render == null)
            return;

        Render.material.SetColor("_Color", Color.red);
    }

    public void DestroyObj()
    {
        gameObject.SetActive(false);
    }
}
