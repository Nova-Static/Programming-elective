using System.Collections;
using UnityEngine;

/// <summary>
/// The vehicle that will do battle. This is the same for every participant in the arena.
/// Its 'brains' (the AI you'll write) will be assigned by the <seealso cref="CompetitionManager"/>
/// </summary>
public class MechController : MonoBehaviour
{
    // the bullets and the locations on the prefab where they spawn from
    public GameObject BulletPrefab = null;
    public Transform ShootOrigin = null;

    // the 'scanner' that allows the ship to 'see' its surroundings
    public GameObject Lookout = null;
    public GameObject point = null;

    // sails can be used to indicate the state of the ship (attacking, fleeing, searching etc.)
    public GameObject[] sails = null;

    /// <summary>
    /// the AI that will control this ship. Is set by <seealso cref="CompetitionManager"/>.
    /// </summary>
    private BaseAI ai = null;

    // create a level playing field. Every ship has the same basic abilities
    private float MechSpeed = 2f;
    private float SeaSize = 25.0f;
    private float RotationSpeed = 250.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Assigns the AI that steers this instance
    /// </summary>
    /// <param name="_ai"></param>
    public void SetAI(BaseAI _ai) {
        ai = _ai;
        ai.Mech = this;
    }

    /// <summary>
    /// Tell this ship to start battling
    /// Should be called only once
    /// </summary>
    public void StartBattle() {
        Debug.Log("test");
        StartCoroutine(ai.RunAI());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    /// <summary>
    /// If a ship is inside the 'scanner', its information (distance and name) will be sent to the AI
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other) {
        if (other.tag == "Mech") {
            ScannedRobotEvent scannedRobotEvent = new ScannedRobotEvent();
            scannedRobotEvent.Distance = Vector3.Distance(transform.position, other.transform.position);
            scannedRobotEvent.Name = other.name;
            ai.OnScannedRobot(scannedRobotEvent);
        }
    }

    //public void SlopeDetected()
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(point.transform.position, transform.forward, out hit, 1.0f))
    //    {
    //        if (Vector3.Dot(Vector3.up, hit.normal) > 0.7)
    //        {
    //            SlopeDetectedEvent slopeDetectedEvent = new SlopeDetectedEvent();
    //            slopeDetectedEvent.isStuck = true;
    //            ai.OnSlopeDetected(slopeDetectedEvent);
    //        }
    //    }
    //}


    /// <summary>
    /// Move this ship ahead by the given distance
    /// </summary>
    /// <param name="distance">The distance to move</param>
    /// <returns></returns>
    public IEnumerator __Ahead(float distance) {
        int numFrames = (int)(distance / (MechSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++) {
            transform.Translate(new Vector3(0f, 0f, MechSpeed * Time.fixedDeltaTime), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(SeaSize, 0, SeaSize)), new Vector3(-SeaSize, 0, -SeaSize));
            // transform.position = clampedPosition;

            //CharacterController controller = gameObject.GetComponent<CharacterController>();
            //controller.Move(clampedPosition);

            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.MovePosition(transform.position + (Vector3.forward * 3));

            yield return new WaitForFixedUpdate();            
        }
    }

    /// <summary>
    /// Move the ship backwards by the given distance
    /// </summary>
    /// <param name="distance">The distance to move</param>
    /// <returns></returns>
    public IEnumerator __Back(float distance) {
        int numFrames = (int)(distance / (MechSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++) {
            transform.Translate(new Vector3(0f, 0f, -MechSpeed * Time.fixedDeltaTime), Space.Self);
            Vector3 clampedPosition = Vector3.Max(Vector3.Min(transform.position, new Vector3(SeaSize, 0, SeaSize)), new Vector3(-SeaSize, 0, -SeaSize));
            transform.position = clampedPosition;

            yield return new WaitForFixedUpdate();            
        }
    }

    /// <summary>
    /// Turns the ship left by the given angle
    /// </summary>
    /// <param name="angle">The angle to rotate</param>
    /// <returns></returns>
    public IEnumerator __TurnLeft(float angle) {
        int numFrames = (int)(angle / (RotationSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++) {
            transform.Rotate(0f, -RotationSpeed * Time.fixedDeltaTime, 0f);

            yield return new WaitForFixedUpdate();            
        }
    }

    /// <summary>
    /// Turns the ship right by the given angle
    /// </summary>
    /// <param name="angle">The angle to rotate</param>
    /// <returns></returns>
    public IEnumerator __TurnRight(float angle) {
        int numFrames = (int)(angle / (RotationSpeed * Time.fixedDeltaTime));
        for (int f = 0; f < numFrames; f++) {
            transform.Rotate(0f, RotationSpeed * Time.fixedDeltaTime, 0f);

            yield return new WaitForFixedUpdate();            
        }
    }

    /// <summary>
    /// Sit and hold still for one (fixed!) update
    /// </summary>
    /// <returns></returns>
    public IEnumerator __DoNothing() {
        yield return new WaitForFixedUpdate();
    }

    /// <summary>
    /// Fire from the forward pointing cannon
    /// </summary>
    /// <param name="power">???</param>
    /// <returns></returns>
    public IEnumerator __FireFront(float power) {
        GameObject newInstance = Instantiate(BulletPrefab, ShootOrigin.position, ShootOrigin.rotation);
        yield return new WaitForFixedUpdate();
    }

    /// <summary>
    /// Change the color of the sails (for vanity or visualising state)
    /// </summary>
    /// <param name="color"></param>
    public void __SetColor(Color color) {
        foreach (GameObject sail in sails) {
            sail.GetComponent<MeshRenderer>().material.color = color;
        }
    }


}
