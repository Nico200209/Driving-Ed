using UnityEngine;

[System.Serializable]
public class Wheel
{
    public WheelCollider collider;
    public WheelType wheelType;
}

[System.Serializable]
public enum WheelType
{
    FrontWheel,
    RearWheel
}
public class CarController : MonoBehaviour
{
    public Wheel[] wheels;

    void Start()
    {

    }

    void Update()
    {

    }
}
