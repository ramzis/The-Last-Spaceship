using System;
using System.Collections;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public class SectorResolver : MonoBehaviour
{
    public GameOptions GameOptions;
    private int sectorX, sectorY;

    public Action<(int, int)> OnNewSector;

    private void OnValidate()
    {
        Debug.Assert(GameOptions != null, "Missing game options!");
    }

    private void Start()
    {
        sectorX = -1;
        sectorY = -1;
    }

    private int newSectorX, newSectorY;
    private bool newSector;
    void FixedUpdate()
    {
        newSector = false;
        newSectorX = Mathf.FloorToInt((transform.position.x + GameOptions.Map_SizeX / 2) / (GameOptions.Map_SizeX / GameOptions.Map_SectorCountX));
        newSectorY = Mathf.FloorToInt((transform.position.y + GameOptions.Map_SizeY / 2) / (GameOptions.Map_SizeY / GameOptions.Map_SectorCountY));
        if (newSectorX != sectorX)
        {
            newSector = true;
            sectorX = newSectorX;
        }
        if (newSectorY != sectorY)
        {
            newSector = true;
            sectorY = newSectorY;
        }
        if (newSector)
        {
            if (OnNewSector != null) OnNewSector((sectorX, sectorY));
            L.og(L.Contexts.SECTOR_RESOLVER, $"Entered new sector ({sectorX},{sectorY})");
        }
    }
}
