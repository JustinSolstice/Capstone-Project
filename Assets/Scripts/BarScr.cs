using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class BarScr : MonoBehaviour
{
    public enum BarFillDirection
    {
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom,
    }

    private BarFillDirection fillDirection = BarFillDirection.LeftToRight;
    public BarFillDirection FillDirection { 
        get => fillDirection; 
        set {
            fillDirection = value;
            onFillDirChanged();
        }
    }
    private void onFillDirChanged()
    {
        RectTransform fgTransform = barFG.GetComponent<RectTransform>();
        if (fgTransform == null) return;
        fgTransform.offsetMin = Vector2.zero;
        fgTransform.offsetMax = Vector2.zero;
        switch (FillDirection)
        {
            case BarFillDirection.LeftToRight:
                fgTransform.pivot = new Vector2(0, 1 / 2);
                fgTransform.anchorMin = new Vector2(0, 0);
                fgTransform.anchorMax = new Vector2(0, 1);
                break;
            case BarFillDirection.RightToLeft:
                fgTransform.pivot = new Vector2(1, 1 / 2);
                fgTransform.anchorMin = new Vector2(1, 0);
                fgTransform.anchorMax = new Vector2(1, 1);
                break;
            case BarFillDirection.BottomToTop:
                fgTransform.pivot = new Vector2(1 / 2, 0);
                fgTransform.anchorMin = new Vector2(0, 0);
                fgTransform.anchorMax = new Vector2(1, 0);
                break;
            case BarFillDirection.TopToBottom:
                fgTransform.pivot = new Vector2(1 / 2, 1);
                fgTransform.anchorMin = new Vector2(0, 1);
                fgTransform.anchorMax = new Vector2(1, 1);
                break;
            default:
                break;
        }
    }

    [SerializeField] private GameObject barBG;
    [SerializeField] private GameObject barFG;

    private void Awake() {
        if (barBG == null) barBG = gameObject;
        if (barFG == null) barFG = barBG.transform.Find("Fill").gameObject;
    }

    private void Start() {
        onFillDirChanged();
    }

    public void SetBarPercent(float frac) {
        RectTransform bgTransform = barBG.GetComponent<RectTransform>();
        RectTransform fgTransform = barFG.GetComponent<RectTransform>();

        fgTransform.offsetMin = Vector2.zero;
        fgTransform.offsetMax = Vector2.zero;

        bool vertical = fillDirection == BarFillDirection.BottomToTop || fillDirection == BarFillDirection.TopToBottom;
        if (vertical) fgTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, frac * bgTransform.sizeDelta.y);
        else fgTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, frac * bgTransform.sizeDelta.x);
    } 
}
