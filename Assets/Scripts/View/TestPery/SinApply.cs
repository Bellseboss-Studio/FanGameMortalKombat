using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinApply : MonoBehaviour
{
    [SerializeField] private float deltaTimeLocal, high;
    [SerializeField] private float velocity;
    [SerializeField] private GameObject target;
    [SerializeField] private float increment;

    private Vector3 positionLocal;
    private Vector3 positionLocalOriginal;
    public bool stay;

    // Start is called before the first frame update
    void Start()
    {
        positionLocalOriginal = positionLocal = transform.position;
        stay = true;
    }

    // Update is called once per frame
    void Update()
    { 
        velocity += increment;
        positionLocal.z = Vector3.MoveTowards(transform.position, target.transform.position, velocity).z;
        positionLocal.x = Vector3.MoveTowards(transform.position, target.transform.position, velocity).x;

        positionLocal.y = (Mathf.Sin((deltaTimeLocal*Mathf.PI) / Vector3.Distance(target.transform.position, positionLocalOriginal)) * high) + positionLocalOriginal.y;
        if (!stay)
        {
            deltaTimeLocal -= Time.deltaTime;
            if (deltaTimeLocal <= 0)
            {
                stay = true;
                deltaTimeLocal = 0;
            }
        }
        else
        {
            deltaTimeLocal += Time.deltaTime;
            if (deltaTimeLocal >= Mathf.PI)
            {
                stay = false;
                deltaTimeLocal = Mathf.PI;
            }
        }
        transform.localPosition = positionLocal;
        Debug.Log(deltaTimeLocal);
    }
}
