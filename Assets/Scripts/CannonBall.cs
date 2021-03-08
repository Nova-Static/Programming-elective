using UnityEngine;

/// <summary>
/// The projectile that is fired.
/// Currently never leaves the scene once added
/// </summary>
public class CannonBall : MonoBehaviour
{
    private float speed = 10f;
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
        transform.Translate(new Vector3(0f, 0f, speed * Time.fixedDeltaTime), Space.Self);
    }
}
