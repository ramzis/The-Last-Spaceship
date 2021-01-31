using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonUIManager : MonoBehaviour
{
    public GameObject Dial;

    private RectTransform _dialSprite;
    private bool _activated = false;
    private float _activeDuration;

    private void OnEnable()
    {
        ShipCannon.OnShipCannonActive += ActivateTool;
        ShipCannon.OnShipCannonDisabled += DeactivateTool;
    }

    void Start()
    {
        _dialSprite = Dial.GetComponent<RectTransform>();
        IdleReset();
    }

    void Update()
    {
        if (_activated)
        {
            _activeDuration += Time.deltaTime;

            Pulse(_activeDuration);
        }
        else
        {
            _activeDuration = 0;
            IdleReset();
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

    void Pulse(float time)
    {
        time = time % 360;

        _dialSprite.rotation = Quaternion.Euler(0, 0, time);
    }

    void IdleReset()
    {
        RectTransform sprite = Dial.GetComponent<RectTransform>();

        _dialSprite.rotation = Quaternion.Euler(0, 0, 0);
    }
}
