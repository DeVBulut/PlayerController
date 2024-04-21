using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    int hitPoints = 5;
    public TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        if (textMeshProUGUI) {
            textMeshProUGUI.text = hitPoints.ToString();
        }
    }
    public void Damage()
    {
        hitPoints -= 1;
    }
}
