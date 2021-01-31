using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDestroy : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
