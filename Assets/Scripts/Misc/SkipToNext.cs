using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipToNext : MonoBehaviour
{
    [SerializeField] float skipTime;
    [SerializeField] string sceneToSkipTo;
    public enum Transitions { InTransDown, InTransUp, OutTransDown, OutTransUp, FadeInTrans, FadeOutTrans, None }
    [SerializeField] Transitions transitionType;

    [SerializeField] Animator anims;

    // Start is called before the first frame update
    void Start()
    {
        TransitionManager.transAnim = anims;

        if ((int)transitionType != 6)
            TransitionManager.Transition(((int)transitionType));
        Debug.Log((int)transitionType);
        Invoke("SkipScene", skipTime);
    }

    void SkipScene()
    {
        TransitionManager.SceneSkip(sceneToSkipTo);
    }
}
