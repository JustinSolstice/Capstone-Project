using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;

public class Test : ProximityPrompt
{
    public override void OnInteract() {
        Active = false;
        GUIManager.Instance.textbox.CreateTextSequence(new string[] { "prompt pressed", "yippie and such" }, TextFinished);
        void TextFinished()
        {
            Active = true;
        }
    }
}
