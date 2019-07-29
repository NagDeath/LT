using UnityEngine;

public class LightningScript : MonoBehaviour
{
    public void AnimationEnd()
    {
        GameManagerScript.animEndDelegate?.Invoke(1.2f);
    }
}
