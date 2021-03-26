using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

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
    public GameObject flag = null;
    private FlagManager flagManager;
    [SerializeField]
    float MaxSpeed = 5;

    [SerializeField]
    GameObject deadPrefab;
    [SerializeField]
    float AngularSpeed = 50;

    public bool damaging;
    public float healthHealth;
    public float Maxhealth = 100f;
    public float currentHealth;
    public float Damage = 20f; 
    public ShowHealthBar healthbar;
    
    public GameObject eyes;
    GameObject eyes2;
    LayerMask mask;
    // the bullets and the locations on the prefab where they spawn from
    public GameObject BulletPrefab = null;
   
    public Transform ShootOrigin = null;

    public CinemachineImpulseSource CISource;

    private AudioSource sfx;
    public AudioSource audio;

    public AudioClip moveAudio;
    public AudioClip shootAudio;
    public AudioClip explodeAudio;
    public AudioClip deathAudio;
    private Animator animator;
    public Renderer rendMesh;
    private TextMeshProUGUI mechNameUI;
    public GameObject wallDetection;
    private detecting detector;
    //Get the Renderer component from the new cube


    //Call SetColor using the shader property name "_Color" and setting the color to red

    private Canvas canvas;
    float timePerShot = 2f;

    private GameObject[] teleporters;
    void Start()
    {
        //rendMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        sfx = GetComponent<AudioSource>();
        healthbar.SetMaxHealth(Mathf.RoundToInt(Maxhealth));
        flag = GameObject.Find("Flag");

        // Add Flag info to AI function
        FlagInfo fInfo = new FlagInfo();
        fInfo.position = flag.transform.position;
        AI.OnFlagInfo(fInfo);

        flagManager = flag.GetComponent<FlagManager>();
        mask = LayerMask.GetMask("Mech");
        mask = ~mask;
        animator = gameObject.GetComponentInChildren<Animator>();
        canvas = gameObject.GetComponentInChildren<Canvas>();
        
        CISource = this.gameObject.GetComponent<CinemachineImpulseSource>();
        teleporters = GameObject.FindGameObjectsWithTag("Teleport");
        mechNameUI = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        detector = transform.GetChild(4).gameObject.GetComponent<detecting>();
        mechNameUI.text = AI.name;
        AI.Start();
    }

    void Awake()
    {
        audio.PlayOneShot(moveAudio, PlayerPrefs.GetFloat("Music", 0) / 10);
        // Make sure all Mechs have the same speed (independant from inspector values)
        MaxSpeed = 5;
        AngularSpeed = 50;

        currentHealth = Maxhealth;
        // look at the center of the arena
        //transform.LookAt(Vector3.zero);
        rigidbody = GetComponent<Rigidbody>();
    }


    public void CapturingFlag()
    {
        if (flagManager.name != null)
        {
            FlagBeingCaptured flagBeingCaptured = new FlagBeingCaptured();
            flagBeingCaptured.Name = flagManager.name;
            flagBeingCaptured.capturing = flagManager.robot;
            AI.OnFlagBeingCaptured(flagBeingCaptured);
        }
    }

    public bool getCapturingState()
    {
        return flagManager.robot;
    }

    /// <summary>
    /// Move forward in the direction the bot is facing
    /// </summary>
    public void MoveForward()
    {
        if (!sfx.isPlaying)
        {
            sfx.PlayOneShot(moveAudio, PlayerPrefs.GetFloat("SFX", 0)/10);
        }
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

    public void Fire(Transform _direction)
    {
        if (timePerShot <= 0f)
        {
            timePerShot = 2f;
            GetComponent<AudioSource>().PlayOneShot(shootAudio, PlayerPrefs.GetFloat("SFX", 0)/20);
            GameObject newInstance =(GameObject) Instantiate(BulletPrefab, ShootOrigin.position, ShootOrigin.rotation);
            newInstance.GetComponent<CannonBall>().Seek2(_direction);
            CISource.GenerateImpulse();
        }
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
       // transform.LookAt(targetDirection);

        ////Debug.Log("Rotating");
        //Debug.DrawRay(transform.position, targetDirection, Color.magenta, 1);
    }

    /// <summary>
    /// When (and as long as) another bot is on the radar, relay that info to the AI
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<MechController>() != null||other.gameObject.tag=="flag")
        {
            eyes2 = other.gameObject.transform.GetChild(2).gameObject;
            if (Physics.Linecast(eyes.transform.position, eyes2.transform.position, mask))
            {
               // Debug.Log("blocked");
            }
            else
            {
             //   Debug.Log("Shoot");
                //Debug.Log($"{name} sees: {other.gameObject.GetComponent<BotController>().name}");
                RadarBlibInfo info = new RadarBlibInfo();
                info.visible = true;
                info.name = other.gameObject.name;
                info.health = Mathf.RoundToInt(other.gameObject.GetComponent<MechController>().currentHealth);
                info.position = other.gameObject.transform.position;
                info.transform = other.transform;
                AI.OnRecordRadarBlib(info);
            }
        }
    }
    public void hitByBullet()
    {
       
            
          
            sfx.PlayOneShot(explodeAudio, PlayerPrefs.GetFloat("SFX", 0) / 10);
            
        
    }

    // Below this is stuff to make the 'architecture' work.

    void Update()
    {
        if (IsActive)
        {

            if (currentHealth <= 0)
            {
                Die();
            }
            if (rigidbody.velocity.magnitude > 0)
            {
                animator.SetBool("walking", true);
            }
            else
            {
                animator.SetBool("walking", false);
            }
            Accelleration.Set(0, 0, 0);
            if (mechNameUI != null)
            {
                mechNameUI.gameObject.transform.LookAt(GameObject.Find("CM_Camera1").transform);
                mechNameUI.rectTransform.rotation = new Quaternion(0, 180, 0, 0);
            }
            AI.Update();
            CapturingFlag();
            if (detector.detected)
            {

                WallDetection detected = new WallDetection();
                detected.detecting = true;
                detected.objectDetected = detector.detectedGameObject;
                AI.StartDetection(detected);
            }
            else
            {
                WallDetection detected = new WallDetection();
                detected.detecting = false;
                detected.objectDetected = null;
                AI.StartDetection(detected);
            }
        }
    }
    private void Die()
    {
        sfx.PlayOneShot(deathAudio, PlayerPrefs.GetFloat("SFX", 0)/10);
        Instantiate(deadPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if (damaging)
        {
            if (healthHealth < currentHealth)
            {
                currentHealth--;
                healthbar.SetHealth(Mathf.RoundToInt(currentHealth));
                rendMesh.material.color = Color.red;
            }
            else
            {
                currentHealth = healthHealth;
                damaging = false;
                rendMesh.material.color = Color.white;
            }
        }
        if (IsActive)
        {
           // Debug.Log($"Accelleration of {name} is {Accelleration}");
            foreach(GameObject teleporter in teleporters)
            {
                TeleportersInfo tpInfo = new TeleportersInfo();
                tpInfo.position = teleporter.transform.position;
                AI.OnTeleportersInfo(tpInfo);
            }
            
            rigidbody.velocity += Accelleration;
            if (timePerShot >= 0)
            {
                timePerShot -= Time.deltaTime;
            }
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
