using UnityEngine;

public enum RotateDirection
{
    Left,
    Right
}

public class MechController : MonoBehaviour
{
    private bool IsActive = false;
    private BaseAI AI = null;

    private Rigidbody rigidbody = null;

    private Vector3 Accelleration = Vector3.zero;
    // The flag trigger
    private GameObject flag = null;
    private FlagManager flagManager;
    [SerializeField]
    float MaxSpeed = 5;

    [SerializeField]
    float AngularSpeed = 10;

    int Health = 100;

    void Start()
    {
        flag = GameObject.Find("Flag");
        flagManager = flag.GetComponent<FlagManager>();
    }

    void Awake()
    {
        // look at the center of the arena
        //transform.LookAt(Vector3.zero);
        rigidbody = GetComponent<Rigidbody>();
    }


    public void CapturingFlag()
    {
        Debug.Log(flagManager.name);
        if (flagManager.name != null)
        {
            FlagBeingCaptured flagBeingCaptured = new FlagBeingCaptured();
            flagBeingCaptured.Name = flagManager.name;
            flagBeingCaptured.capturing = flagManager.robot;
            AI.OnFlagBeingCaptured(flagBeingCaptured);
        }
    }

    /// <summary>
    /// Move forward in the direction the bot is facing
    /// </summary>
    public void MoveForward()
    {
        //Vector3 newPosition = rigidbody.position + transform.forward * MaxSpeed * Time.deltaTime;
        Vector3 clampedPostion = Vector3.Max(Vector3.Min(transform.position, new Vector3(25, 0, 25)), new Vector3(-25, 0, -25));
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        //Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(SeaSize, 0, SeaSize)), new Vector3(-SeaSize, 0, -SeaSize));
        rigidbody.MovePosition(transform.position + (transform.forward * MaxSpeed * Time.deltaTime));
        //rigidbody.MovePosition(clampedPostion);
    }

    /// <summary>
    /// Followint Craig Reynolds' Seek behaviour .See also https://natureofcode.com/book/chapter-6-autonomous-agents/ (6.3)
    /// </summary>
    /// <param name="position"></param>
    public void Seek(Vector3 position)
    {
        //
        Vector3 desiredVelocity = position - transform.position;
        desiredVelocity.Normalize();
        desiredVelocity *= MaxSpeed;

        Vector3 steeringForce = desiredVelocity - rigidbody.velocity;

        Accelleration += (steeringForce / rigidbody.mass);
    }

    /// <summary>
    /// Rotates in the indicated direction
    /// rotation is done with 
    /// </summary>
    /// <param name="direction"></param>
    public void Rotate(RotateDirection direction)
    {
        float angle = (direction == RotateDirection.Left ? -1 : 1) * AngularSpeed;
        Quaternion deltaRotation = Quaternion.Euler(0, angle * Time.deltaTime, 0);
        rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
    }

    /// <summary>
    /// Rotates towards the indicated direction/position in space
    /// Rotation happens at the AngularSpeed as set in the inspector
    /// </summary>
    /// <param name="targetDirection"></param>
    public void RotateTo(Vector3 targetDirection)
    {
        float singleStep = Mathf.Deg2Rad * AngularSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);

        Debug.Log("Rotating");
        Debug.DrawRay(transform.position, targetDirection, Color.magenta, 1);
    }

    /// <summary>
    /// When (and as long as) another bot is on the radar, relay that info to the AI
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<MechController>() != null)
        {
            //Debug.Log($"{name} sees: {other.gameObject.GetComponent<BotController>().name}");
            RadarBlibInfo info = new RadarBlibInfo();
            info.visible = true;
            info.name = other.gameObject.name;
            info.health = other.gameObject.GetComponent<MechController>().Health;
            info.position = other.gameObject.transform.position;
            AI.OnRecordRadarBlib(info);
        }
    }

    // Below this is stuff to make the 'architecture' work.

    void Update()
    {
        if (IsActive)
        {
            Accelleration.Set(0, 0, 0);
            AI.Update();
        }
    }

    private void FixedUpdate()
    {
        if (IsActive)
        {
            Debug.Log($"Accelleration of {name} is {Accelleration}");

            rigidbody.velocity += Accelleration;

            //Vector3 newDirection = Vector3.RotateTowards(transform.forward, rigidbody.velocity, 10f, 0.0f);
            //transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    /// <summary>
    /// Assign the AI to this Bot
    /// The AI is told which game object it controls
    /// </summary>
    /// <param name="ai"></param>
    public void setAI(BaseAI ai)
    {
        AI = ai;
        AI.SetController(this);
    }

    public void SetActive(bool active)
    {
        IsActive = active;
      //  rigidbody.velocity = transform.forward * MaxSpeed;
    }

    public void StartBattle()
    {
        SetActive(true);
    }
}
