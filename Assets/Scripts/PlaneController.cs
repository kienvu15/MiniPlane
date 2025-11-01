using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float FlySpeed = 20f;
    public float BoostMultiplier = 2f;
    public float YawSpeed = 120f;
    public float PitchSpeed = 60f;
    public float RollSpeed = 90f;

    [Header("Balance Settings")]
    public float MaxPitchForBalance = 45f;   // góc pitch được coi là "đang bay thẳng lên"
    public float TimeToAutoBalance = 5f;     // thời gian bay thẳng trước khi cân bằng
    public float BalanceSpeed = 2f;          // tốc độ cân bằng lại
    public float ReleaseBalanceSpeed = 1.5f; // tốc độ lerp khi thả phím lên/xuống

    private float yaw;
    private float pitch;
    private float roll;

    private float balanceTimer = 0f;
    private bool isBalancing = false;

    void Update()
    {
        // --- 1️⃣ Di chuyển cơ bản ---
        float currentSpeed = FlySpeed;

        // Nếu ấn Space → tăng tốc
        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed *= BoostMultiplier;
        }

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // --- 2️⃣ Nhập điều khiển ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Nếu không đang auto balance thì điều khiển thủ công
        if (!isBalancing)
        {
            yaw += horizontal * YawSpeed * Time.deltaTime;

            // Nếu có input dọc → thay đổi pitch
            if (Mathf.Abs(vertical) > 0.1f)
            {
                pitch -= vertical * PitchSpeed * Time.deltaTime;
            }
            else
            {
                // Nếu thả phím → dần dần quay về thăng bằng
                pitch = Mathf.Lerp(pitch, 0f, Time.deltaTime * ReleaseBalanceSpeed);
            }

            // Roll nghiêng theo hướng rẽ
            roll = Mathf.Lerp(roll, -horizontal * 45f, Time.deltaTime * 3f);
        }

        // --- 3️⃣ Kiểm tra nếu máy bay đang bay thẳng lên quá lâu ---
        if (pitch < -MaxPitchForBalance) // pitch âm = mũi chúc lên
        {
            balanceTimer += Time.deltaTime;
            if (balanceTimer >= TimeToAutoBalance)
            {
                isBalancing = true;
            }
        }
        else
        {
            balanceTimer = 0f; // reset nếu không còn chúc lên
        }

        // --- 4️⃣ Auto balance sau 5s bay thẳng ---
        if (isBalancing)
        {
            pitch = Mathf.Lerp(pitch, 0f, Time.deltaTime * BalanceSpeed);
            roll = Mathf.Lerp(roll, 0f, Time.deltaTime * BalanceSpeed);

            if (Mathf.Abs(pitch) < 1f && Mathf.Abs(roll) < 1f)
            {
                pitch = 0f;
                roll = 0f;
                isBalancing = false;
                balanceTimer = 0f;
            }
        }

        // --- 5️⃣ Áp dụng xoay ---
        Quaternion rotation = Quaternion.Euler(pitch, yaw, roll);
        transform.rotation = rotation;
    }
}
