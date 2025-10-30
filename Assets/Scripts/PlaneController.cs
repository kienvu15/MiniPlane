using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float FlySpeed = 5;
    public float YawAmount = 120;
    private float Yaw;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * FlySpeed * Time.deltaTime;

        float horizontalInput = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Yaw += horizontalInput * YawAmount * Time.deltaTime;
        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(vertical) * Mathf.Sign(vertical));
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput) * -Mathf.Sign(horizontalInput));

        transform.localRotation = Quaternion.Euler(Vector3.up * Yaw);
    }
}
