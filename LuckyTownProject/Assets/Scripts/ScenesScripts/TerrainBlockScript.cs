using UnityEngine;

public class TerrainBlockScript : MonoBehaviour
{
    public void Animation()
    {
        GetComponentInChildren<Animation>().Play();
    }
}
