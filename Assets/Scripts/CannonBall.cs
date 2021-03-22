using UnityEngine;

/// <summary>
/// The projectile that is fired.
/// Currently never leaves the scene once added
/// </summary>
public class CannonBall : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    private float speed = 15f;
    MechController MechController;
    private Vector3 target;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Makes the bullet fly
    /// </summary>
    /// 

    public void Seek2(Transform _direction)
    {
        transform.LookAt(_direction);
    }

    void FixedUpdate()
    {
        speed += Time.deltaTime*20;
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        if (collision.collider.gameObject.tag.Equals("Mech"))
        {
            MechController = collision.gameObject.GetComponent<MechController>();
            MechController.hitByBullet();
            
        }
        Destroy(gameObject);
    }
}
