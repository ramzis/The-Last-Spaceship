using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Image sprite = toolIndicator.GetComponent<Image>();
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
            Image sprite = indicator.GetComponent<Image>();

            if (indicator == _toolList[(int)CurrentTool])
                sprite.color = new Color(1, 1, 1, 1);
            else
                sprite.color = new Color(1, 1, 1, 0.5f);
        }
    }
}
