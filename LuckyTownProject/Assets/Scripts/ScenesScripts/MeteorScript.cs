using System.Collections;
using UnityEngine;

public class MeteorScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private GameObject explosionAnimGO;

    private Vector3 previousPos;

    private bool isMoving;

    private Transform target;
    public Transform Target { get => target; set => target = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }


    private void Start()
    {
        previousPos = transform.position;
    }

    private void Update()
    {
        if (isMoving)
            Move();
    }

    private void Move()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (transform.position == Target.position)
        {
            isMoving = false;
            explosionAnimGO.SetActive(true);
            StartCoroutine(RestartPosCor());
            GameManagerScript.animEndDelegate?.Invoke(1.3f);
        }
    }

    private IEnumerator RestartPosCor()
    {
        yield return new WaitForSeconds(1.3f);
        explosionAnimGO.SetActive(false);
        gameObject.SetActive(false);
        transform.position = previousPos;
        gameObject.SetActive(true);
    }
}
