﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Logging;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShipCannon : MonoBehaviour
{
    public LineRenderer Line;
    public GameOptions GameOptions;

    public LayerMask RaycastMask;

    public static Action<bool> OnShipToolSwitch;
    public static Action<bool> OnShipCannonActive;
    public static Action<bool> OnShipCannonDisabled;

    private void OnValidate()
    {
        Debug.Assert(GameOptions != null, "Missing game options!");
        Debug.Assert(GameOptions.ShipCannon_Distance > 0, "GameOptions.ShipCannon_Distance should be > 0");
    }

    void Awake()
    {
        Line = GetComponent<LineRenderer>();
        Debug.Assert(Line != null, "Missing line renderer!");
    }

    private Vector2 hitPosition;
    private bool success;
    private bool doHeal;
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            doHeal = !doHeal;
            OnShipToolSwitch(doHeal);
        }
        Line.enabled = false;
        if (Input.GetKeyDown(KeyCode.Space) && OnShipCannonActive != null)
        {
            OnShipCannonActive(doHeal);
        }
        if (Input.GetKeyUp(KeyCode.Space) && OnShipCannonDisabled != null)
        {
            OnShipCannonDisabled(doHeal);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            success = false;
            Line.enabled = true;
            (hitPosition, success) = ShootRay(doHeal);
            if (success)
            {
                Line.SetPositions(new Vector3[]
                {
                    new Vector3(transform.position.x, transform.position.y),
                    hitPosition,
                });
            }
            else
            {
                Line.SetPositions(new Vector3[]
                {
                    transform.position,
                    transform.position + transform.up * GameOptions.ShipCannon_Distance,
                });
            }
        }
    }

    private RaycastHit2D hit;
    public (Vector2, bool) ShootRay(bool doHeal)
    {
        hit = Physics2D.Raycast(transform.position, transform.up, GameOptions.ShipCannon_Distance, RaycastMask);
        if (hit.collider == null) return (Vector2.zero, false);
        var calc = hit.collider.transform.GetComponent<CalcExplosion>();
        if (calc != null) calc.DoDamage(100f, hit.point, doHeal);
        return (hit.point, true);
    }
}
