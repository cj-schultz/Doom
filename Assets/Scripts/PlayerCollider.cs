using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CapsuleCollider))]
public class PlayerCollider : MonoBehaviour
{
    public Transform headCamera;

    public Text eatenText;

    private CapsuleCollider capsuleCollider;

    private int bulletsEaten;

    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        bulletsEaten = 0;
        eatenText.text = "BULLETS EATEN: " + bulletsEaten;
    }
 
    void FixedUpdate()
    {
        float distanceFromFloor = Vector3.Dot(headCamera.localPosition, Vector3.up);
        capsuleCollider.height = Mathf.Max(capsuleCollider.radius, distanceFromFloor);
        transform.localPosition = headCamera.localPosition - 0.5f * distanceFromFloor * Vector3.up;
    }

    void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();

        if(bullet)
        {
            eatenText.text = "BULLETS EATEN: " + ++bulletsEaten;
            bullet.Despawn();
        }
    }
}