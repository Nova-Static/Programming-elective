using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechGetter : MonoBehaviour
{

    public GameObject[] MechsAlive;
    [SerializeField]
    public GameObject Flag;
    public Cinemachine.CinemachineTargetGroup CM_TargetGroup;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LateStart");
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);

        MechsAlive = GameObject.FindGameObjectsWithTag("Mech");
        CM_TargetGroup = gameObject.GetComponent<Cinemachine.CinemachineTargetGroup>();

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject mech in MechsAlive)
        {
            CM_TargetGroup.AddMember(mech.transform, 1f, 0f);
        }

        // Also Add Flag with a higher Radius to the target group so it can be seen at all times
        if (Flag != null) {
            CM_TargetGroup.AddMember(Flag.transform, 1f, 9f);
        } else
        {
            Debug.LogError("Add the Flag to the mech Getter script!");
        }
    }
}
