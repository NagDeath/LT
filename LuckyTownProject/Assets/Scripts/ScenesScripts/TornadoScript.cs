using UnityEngine;

public class TornadoScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector3 target;
    public Vector3 Target { get => target; set => target = value; }

    private void Update()
    {
        this.transform.Rotate(0, 10f, 0);
        Move();
    }

    private void Move()
    {
        float step = speed * Time.deltaTime;

        if (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
        else
            gameObject.SetActive(false);
    }
}
