using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinApply : MonoBehaviour
{
    [SerializeField] private float deltaTimeLocal, forward, deltaTimeLocalAux;
    [SerializeField] private float distance;
    [SerializeField] private GameObject target;

    private Vector3 positionLocal;
    private Vector3 positionLocalOriginal;
    private Vector3 positionLocalAux;
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
        positionLocalAux = positionLocal;
        positionLocal.y = Mathf.Sin((deltaTimeLocal*Mathf.PI) / Vector3.Distance(target.transform.position, transform.position)) + positionLocalOriginal.y;
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

        positionLocal.z = Vector3.MoveTowards(transform.position, target.transform.position, forward).z;
        transform.position = positionLocal;
        Debug.Log(deltaTimeLocal);
    }
}
