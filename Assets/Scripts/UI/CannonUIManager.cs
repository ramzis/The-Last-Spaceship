using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonUIManager : MonoBehaviour
{
    public GameObject Dial;
    public float DegreeRotationPerSecond = 45;

    private RectTransform _dialSprite;
    private bool _activated = false;

    private void OnEnable()
    {
        ShipCannon.OnShipCannonActive += ActivateTool;
        ShipCannon.OnShipCannonDisabled += DeactivateTool;
    }

    void Start()
    {
        _dialSprite = Dial.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_activated)
        {
            _dialSprite.Rotate(0, 0, Time.deltaTime * DegreeRotationPerSecond);
        }
    }

    void ActivateTool(bool Heal)
    {
        _activated = true;
    }

    void DeactivateTool(bool Heal)
    {
        _activated = false;
    }
}
