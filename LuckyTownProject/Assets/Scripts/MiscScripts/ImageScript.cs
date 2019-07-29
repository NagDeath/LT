using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour
{
    [SerializeField]
    private SpriteAtlas atlas;
    [SerializeField]
    private string spriteName;

    private void OnValidate()
    {  
        GetComponent<Image>().sprite = atlas.GetSprite(spriteName);
    }

    private void OnEnable()
    {
        GetComponent<Image>().sprite = atlas.GetSprite(spriteName);
    }
}
