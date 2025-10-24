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
    public CarStats carStats;
    public Wheel[] wheels; //ToDo: Know how many wheels are for steering, and how many for hand break


    public float maxSteer = 30f;
    public float wheelbase = 2.5f; // Distance between front and rear axles
    public float trackwidth = 1.5f; // Distance between left and right
    public bool SpacebarPressed;


    #region common 
    private Vector2 moveInput;
    public float wheelTurnLerpSpeed = 1f;

    #endregion

    void Start()
    {
        carStats = GetComponent<CarStats>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue input)
    {
        SpacebarPressed = input.Get<float>() == 1; //Set to true if spacebar is pressed
    }

    void FixedUpdate()
    {

        SpacebarPressed = Input.GetKey(KeyCode.Space); //Set to true if spacebar is pressed


        if (!carStats) return;

        //ToDo: Check if wheels are present

        for (int i = 0; i < wheels.Length; i++)
        {
            if (i > 1)
            {
                //Rear Wheels
                wheels[i].collider.motorTorque = SpacebarPressed ? 0 : moveInput.y * carStats.MaxPowerNM;
                wheels[i].collider.brakeTorque = !SpacebarPressed ? 0 : 1000;

            }
            else
            {
                //Front Wheels
                wheels[i].collider.motorTorque = moveInput.y * carStats.MaxPowerNM;

            }

        }



        float steer = moveInput.x * maxSteer;

        //ToDo: Implement Math Lerp
        if (moveInput.x > 0) //If turning right
        {
            wheels[0].collider.steerAngle = Mathf.Lerp(wheels[0].collider.steerAngle, Mathf.Rad2Deg * Mathf.Atan(wheelbase / (trackwidth / 2 + Mathf.Tan(Mathf.Deg2Rad * steer) * wheelbase)), Time.deltaTime * wheelTurnLerpSpeed);
            wheels[1].collider.steerAngle = Mathf.Lerp(wheels[1].collider.steerAngle, steer, Time.deltaTime * wheelTurnLerpSpeed);
        }
        else if (moveInput.x < 0) //If turning left
        {
            wheels[0].collider.steerAngle = Mathf.Lerp(wheels[0].collider.steerAngle, steer, Time.deltaTime * wheelTurnLerpSpeed);
            wheels[1].collider.steerAngle = Mathf.Lerp(wheels[1].collider.steerAngle, Mathf.Rad2Deg * Mathf.Atan(wheelbase / (-trackwidth / 2 + Mathf.Tan(Mathf.Deg2Rad * steer) * wheelbase)), Time.deltaTime * wheelTurnLerpSpeed);
        }
        else //If not turning
        {
            wheels[0].collider.steerAngle = wheels[1].collider.steerAngle = Mathf.Lerp(wheels[0].collider.steerAngle = wheels[1].collider.steerAngle, 0, Time.deltaTime * wheelTurnLerpSpeed);
        }
        for (int i = 0; i < wheels.Length; i++)
        {
            Quaternion Rot;
            Vector3 Pos;
            wheels[i].collider.GetWorldPose(out Pos, out Rot);
            //wheels[i].collider.transform.rotation = Rot;

            Transform[] ChildTransforms = new Transform[wheels[i].collider.transform.childCount];
            int index = 0;
            foreach (var item in ChildTransforms)
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
