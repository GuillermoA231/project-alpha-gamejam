using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    
    public void PlayClickSound()
    {
        AudioManager.instance?.PlaySFX2D(AudioManager.instance.clickClip);
    }
    
    public void HoverSound()
    {
        AudioManager.instance?.PlaySFX2D(AudioManager.instance.hoverClip);
    }
}
