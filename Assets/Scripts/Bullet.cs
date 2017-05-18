using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 7f;

    private Vector3 target;
    private Vector3 direction;

    void FixedUpdate()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
        //transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);        
    }

    public void SetTarget(Vector3 _target)
    {
        target = _target;
        direction = (target - transform.position).normalized;
    }

    public void Despawn()
    {
        gameObject.SetActive(false);
        enabled = false;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.layer == LayerMask.NameToLayer("EnvironmentPlatform"))
        {
            Despawn();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnvironmentPlatform"))
        {
            Despawn();
        }
    }
}
