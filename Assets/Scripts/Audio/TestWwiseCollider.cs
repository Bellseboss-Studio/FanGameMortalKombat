using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWwiseCollider : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Switch m_SetToGravel;
    [SerializeField] private AK.Wwise.Switch m_SetToGrass;
    private void OnTriggerEnter(Collider other)
    {
     m_SetToGravel.SetValue(gameObject);   
    }

    private void OnTriggerExit(Collider other)
    {
        m_SetToGrass.SetValue(gameObject);
    }
}
