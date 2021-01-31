using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUIManager : MonoBehaviour
{
    public enum ShipTool
    {
        Mine,
        Fix
    }

    public GameObject MineTool;
    public GameObject FixTool;

    private List<GameObject> _toolList;
    public ShipTool CurrentTool;
    public bool Activated = false;
    public float ActiveDuration;

    private void OnEnable()
    {
        ShipCannon.OnShipToolSwitch += switchTool;
        ShipCannon.OnShipCannonActive += ActivateTool;
        ShipCannon.OnShipCannonDisabled += DeactivateTool;
    }

    void Start()
    {
        CurrentTool = ShipTool.Mine;

        _toolList = new List<GameObject>{ MineTool, FixTool };

        IdleReset();
    }

    void Update()
    {
        if (Activated)
        {
            ActiveDuration += Time.deltaTime;

            Pulse(_toolList[(int)CurrentTool], ActiveDuration);
        }
        else
        {
            ActiveDuration = 0;
            IdleReset();
        }
    }

    void switchTool(bool Heal)
    {
        if (Heal)
            CurrentTool = ShipTool.Fix;
        else
            CurrentTool = ShipTool.Mine;

        IdleReset();
    }

    void ActivateTool(bool Heal)
    {
        Activated = true;
    }

    void DeactivateTool(bool Heal)
    {
        Activated = false;
    }

    void Pulse(GameObject toolIndicator, float time)
    {
        SpriteRenderer sprite = toolIndicator.GetComponent<SpriteRenderer>();
        time = (time % 0.5f) / 0.5f * 2f;

        float additionalGreen = Mathf.Sin(time * Mathf.PI);
        sprite.color = new Color(1f - additionalGreen,
                                 1f,
                                 1f - additionalGreen,
                                 sprite.color.a);
    }

    void IdleReset()
    {
        foreach (GameObject indicator in _toolList)
        {
            SpriteRenderer sprite = indicator.GetComponent<SpriteRenderer>();

            if (indicator == _toolList[(int)CurrentTool])
                sprite.color = new Color(1, 1, 1, 1);
            else
                sprite.color = new Color(1, 1, 1, 0.5f);
        }
    }
}
