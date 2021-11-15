using System;
using System.Collections.Generic;
using MenuUI;
using UnityEngine;

namespace View.UI
{
    public class AmuletsMediator : MonoBehaviour
    {
        [SerializeField] private List<Amulets> amuletsList;

        private void Awake()
        {
            foreach (var amulet in amuletsList)
            {
                amulet.Configure(this);
            }
        }

        public void AnyScriptCanMove()
        {
            foreach (var amulet in amuletsList)
            {
                amulet.amuletIsMoving = false;
            }
        }
        
    }
}