using UnityEngine;

public class PlayerDirectionLogger : MonoBehaviour
{
    // 何秒ごとにログを取得するか
    [SerializeField] private float logIntervalSeconds = 1.0f;

    // Rayの長さ
    [SerializeField] private float rayDistance = 10.0f;

    private float nextLogTime;

    void Start()
    {
        nextLogTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= nextLogTime)
        {
            RecordDirection();

            nextLogTime = Time.time + logIntervalSeconds;
        }
    }

    void RecordDirection()
    {
        // カメラの向いている方向
        Vector3 direction = transform.forward;

        // 左右・上下の角度
        float yaw = Mathf.Atan2(direction.x, direction.z)
                    * Mathf.Rad2Deg;

        float pitch = Mathf.Asin(direction.y)
                      * Mathf.Rad2Deg;

        // Rayを作成
        Ray ray = new Ray(transform.position, direction);

        string hitObject = "None";

        // Rayが物体に当たったか確認
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            hitObject = hit.collider.gameObject.name;
        }

        // Consoleに出力
        Debug.Log(
            "Time: " + Time.time.ToString("F2") +
            " Direction: " + direction +
            " Yaw: " + yaw.ToString("F2") +
            " Pitch: " + pitch.ToString("F2") +
            " HitObject: " + hitObject
        );
    }
}