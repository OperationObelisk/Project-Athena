using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [HideInInspector]
    public Image buttonImageComponent = null;

    [HideInInspector]
    public Button buttonComponent = null;

    public int skillID = 0;

    private void Start()
    {
        buttonComponent = GetComponent<Button>();
        buttonImageComponent = GetComponent<Image>();
    }
}