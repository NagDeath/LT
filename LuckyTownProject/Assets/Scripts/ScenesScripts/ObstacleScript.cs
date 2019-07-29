using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour, IDamagable
{
    [SerializeField]
    private int hP;
    [SerializeField]
    private List<GameObject> details;
    [SerializeField]
    private Animation anim;

    private bool isDestroyed;
    public bool IsDestroyed { get => isDestroyed; set => isDestroyed = value; }

    public IEnumerator GetDamage(int dmg, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(AnimationCor());
        hP -= dmg;
        switch (hP)
        {
            case 0:
                if (gameObject.name.Contains("Car"))
                {
                    var meshs = GetComponentsInChildren<MeshRenderer>();
                    foreach (var mesh in meshs)
                    {
                        if (!mesh.name.Contains("Explosion"))
                            mesh.enabled = false;
                    }
                }
                else
                    GetComponent<MeshRenderer>().enabled = false;

                GetComponent<BoxCollider>().enabled = false;
                isDestroyed = true;
                break;
            case 1:
                details[0].SetActive(false);
                break;
            case 2:
                details[0].SetActive(true);
                details[1].SetActive(false);
                break;
        }
    }
    private IEnumerator AnimationCor()
    {
        anim.gameObject.SetActive(true);
        anim.Play();
        yield return new WaitForSeconds(1.2f);
        anim.gameObject.SetActive(false);
        if (hP == 0)
            Destroy(gameObject);
    }
}
