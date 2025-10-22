using UnityEngine;
using UnityEngine.InputSystem;

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
    public Vector2 moveInput;
    public float powerMultiplier = 1f;

    public float maxSteer = 30f;
    public float wheelbase = 2.5f; // Distance between front and rear axles
    public float trackwidth = 1.5f; // Distance between left and right

    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    void FixedUpdate()
    {
        //ToDo: Check if wheels are present

        foreach (var wheel in wheels)
        {
            wheel.collider.motorTorque = moveInput.y * powerMultiplier;
        }
        float steer = moveInput.x * maxSteer;

        //ToDo: Implement Math Lerp
        if (moveInput.x > 0) //If turning right
        {
            wheels[0].collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (trackwidth / 2 + Mathf.Tan(Mathf.Deg2Rad * steer) * wheelbase));
            wheels[1].collider.steerAngle = steer;
        }
        else if (moveInput.x < 0) //If turning left
        {
            wheels[0].collider.steerAngle = steer;
            wheels[1].collider.steerAngle = Mathf.Rad2Deg * Mathf.Atan(wheelbase / (-trackwidth / 2 + Mathf.Tan(Mathf.Deg2Rad * steer) * wheelbase));
        }
        else //If not turning
        {
            wheels[0].collider.steerAngle = wheels[1].collider.steerAngle = 0;
        }
        for (int i = 0; i < wheels.Length; i++)
        {
            Quaternion Rot;
            Vector3 Pos;
            wheels[i].collider.GetWorldPose(out Pos, out Rot);
            //wheels[i].collider.transform.rotation = Rot;

            Transform[] ChildTransforms = new Transform[wheels[i].collider.transform.childCount];
            int index = 0;
            foreach(var item in ChildTransforms)
            {
                wheels[i].collider.transform.GetChild(index).rotation = Rot;
                wheels[i].collider.transform.GetChild(index).position = Pos;
                index++;
            }

            //wheels[i].collider.transform.position = Pos;
            //ToDo: Add break Calipers Formula Below
            //wheels[i].collider.transform.localRotation = Quaternion.Euler(0, wheels[i].collider.steerAngle, 0);
        }
    }
}
