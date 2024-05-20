using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;

public class CSStarter : ProximityPrompt
{
    [SerializeField] string cutscene;
    public override void OnInteract() {
        active = false;
        CutsceneManager.Instance.StartCutscne(cutscene);
    }
}
