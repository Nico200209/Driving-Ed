using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;
    public Quaternion rotationOffset;
    public float smoothSpeed = 0.125f;


    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.TransformPoint(offset), Time.deltaTime * smoothSpeed);
        //targetTransform.LookAt(transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation * rotationOffset, Time.deltaTime * smoothSpeed);
    }
}
