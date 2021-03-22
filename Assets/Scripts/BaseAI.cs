using UnityEngine;

public class RadarBlibInfo
{
    public bool visible = true;
    public string name;
    public int health;
    public Vector3 position;
    public Transform transform;
}
public class TeleportersInfo
{
    public Vector3 position;
}
public class FlagInfo
{
    public Vector3 position;
}
public class FlagBeingCaptured
{
    public string Name;
    public bool capturing;
}

/// <summary>
/// The BaseAI
/// Should be extended by your AI
/// In a sense acts as a proxy between the AI and the Botcontroller
/// </summary>
public class BaseAI
{
    private MechController controller;

    public string name = "BaseIA";

    // bit of a hack violating some OOP principles. But now the AI cannot access the game object
    public MechController Controller { private get => controller; set => controller = value; }

    public virtual void Update() { }

    public virtual void OnRecordRadarBlib(RadarBlibInfo info)
    {
    }
    public virtual void OnTeleportersInfo(TeleportersInfo info)
    {
    }
    
    public virtual void OnFlagInfo(FlagInfo info)
    {
    }
    public virtual void OnFlagBeingCaptured(FlagBeingCaptured e)
    {        
    }

    protected bool getCapturingState()
    {
        return Controller.getCapturingState();
    }

    protected void MoveForward()
    {
        Controller.MoveForward();
    }
    protected void Fire(Transform _direction)
    {
        Controller.Fire(_direction);
    }
    protected void Seek(Vector3 position)
    {
        Controller.Seek(position);
    }

    protected Vector3 GetPosition()
    {
        return Controller.transform.position;
    }

    protected Vector3 GetForwardDirection()
    {
        return Controller.transform.forward;
    }

    protected float GetHealth()
    {
        return Controller.currentHealth;
    }
    protected Vector3 GetForwardPos()
    {
        return Controller.transform.forward;
    }

    protected Vector3 GetFlagPos()
    {
        return Controller.flag.transform.position;
    }
    protected void Rotate(RotateDirection direction)
    {
        Controller.Rotate(direction);
    }

    protected void RotateTo(Vector3 direction)
    {
        Controller.RotateTo(direction);
    }

    protected void SetKinematic(bool isKinematic)
    {
        Controller.GetComponent<Rigidbody>().isKinematic = isKinematic;
    }

    public void SetController(MechController controller)
    {
        Controller = controller;
    }
}