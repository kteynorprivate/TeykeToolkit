﻿using UnityEngine;
using System.Collections;

namespace Teyke
{
    public class AutoAttacker : MonoBehaviour
    {
        private Attack attack;

        void Start()
        {
            attack = gameObject.GetComponent<Attack>() as Attack;
        }

        void Update()
        {

        }
    }
}