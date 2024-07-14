using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PlayerGameObjectGetter : MonoBehaviour
{
   [SerializeField] private StudioListener m_StudioListener;
   [SerializeField] private GameObject m_Player;
   private void Awake()
   {
      if (!m_StudioListener)
      {
         m_StudioListener = GetComponent<StudioListener>();
      }

      if (!m_Player)
      {
         m_Player = GameObject.FindWithTag("Player");
      }
      
      m_StudioListener.AttenuationObject = m_Player;
   }
}
