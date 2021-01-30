using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public class PositionLooper : MonoBehaviour
{
    public GameOptions GameOptions;
    private float minX, maxX, minY, maxY;

    private void OnValidate()
    {
        Debug.Assert(GameOptions != null, "Missing game options!");
    }

    private void Start()
    {
        minX = -GameOptions.Map_SizeX / 2f;
        maxX = GameOptions.Map_SizeX / 2f;
        minY = -GameOptions.Map_SizeY / 2f;
        maxY = GameOptions.Map_SizeY / 2f;

        L.og(L.Contexts.POSITION_LOOPER,
            $"Resolved: minX {minX} maxX {maxX} minY {minY} maxY {maxY}");
    }

    void FixedUpdate()
    {
        if (transform.position.x > maxX) SetX(minX);
        if (transform.position.x < minX) SetX(maxX);
        if (transform.position.y < minY) SetY(maxY);
        if (transform.position.y > maxY) SetY(minY);
    }

    void SetX(float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        L.og(L.Contexts.POSITION_LOOPER, "Teleported x!");
    }

    void SetY(float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        L.og(L.Contexts.POSITION_LOOPER, "Teleported y!");
    }
}
