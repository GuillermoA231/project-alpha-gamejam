using UnityEngine;
 
[System.Serializable]
public class IntroUI
{
    [Tooltip("Full-screen background image for this slide")]
    public Sprite background;
 
    [TextArea]
    [Tooltip("Text shown at the bottom via TextMeshPro")]
    public string caption;
 
    [Tooltip("Set Seconds before automatically advancing; 0 to always wait")]
    public float duration = 3f;
}