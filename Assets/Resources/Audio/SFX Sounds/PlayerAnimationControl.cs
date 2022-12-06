using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    public void AnimPlaySmallWoosh()
    {
        SoundManager.Instance.PlaySfx("LightWooshSFX");
    }

    public void AnimPlayBigWoosh()
    {
        SoundManager.Instance.PlaySfx("HeavyWooshSFX");
    }

    public void AnimStep()
    {
        SoundManager.Instance.PlaySfx("PlayerStepSFX");
    }
}
