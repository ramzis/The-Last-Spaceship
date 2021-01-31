using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectorUIManager : MonoBehaviour
{
    private Image Image;
    private TextMeshProUGUI Text;

    private void Start()
    {
        Image = GetComponent<Image>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ToggleSectorView(bool enabled)
    {
        if(Image != null) Image.enabled = enabled;
        if (Text != null) Text.enabled = enabled;
    }

    private String[] sectorA = new[] {"A", "B", "C", "D", "E", "F", "G"};
    public void UpdateSector((int a, int b) sector)
    {
        Text.text = $"{sectorA[sector.a % sectorA.Length]}{sector.b+1}";
    }
}
