using UnityEngine;

/// <summary>
/// The projectile that is fired.
/// Currently never leaves the scene once added
/// </summary>
public class CannonBall : MonoBehaviour
{
    public GameObject ExplosionPrefab;
    private float speed = 10f;
    MechController MechController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Makes the bullet fly
    /// </summary>
    void FixedUpdate()
    {
        speed += Time.deltaTime*20;
        transform.Translate(Vector3.forward* speed * Time.fixedDeltaTime);
       
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
