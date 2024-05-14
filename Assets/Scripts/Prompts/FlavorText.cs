using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;

public class FlavorText : ProximityPrompt
{
    [SerializeField] string[] text;
    public override void OnInteract() {
        active = false;
        GUIManager.Instance.textbox.CreateTextSequence(text, TextFinished);
        void TextFinished()
        {
            active = true;
        }
    }
}
