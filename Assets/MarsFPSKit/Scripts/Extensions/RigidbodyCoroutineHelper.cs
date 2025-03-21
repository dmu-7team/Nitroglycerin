﻿using System.Collections;
using UnityEngine;

namespace MarsFPSKit
{
    public class RigidbodyCoroutineHelper : MonoBehaviour
    {
        public IEnumerator AddForceNextFrame(Vector3 force)
        {
            yield return new WaitForEndOfFrame();
            GetComponent<Rigidbody>().AddForce(force);
            Destroy(this);
        }
    }
}