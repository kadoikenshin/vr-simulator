using UnityEngine;

public class PlayerDirectionLogger : MonoBehaviour
{
    [Header("観測判定設定")]
    [SerializeField] private string targetTag = "GazeTarget";

    // 連続して観測する必要がある時間
    [SerializeField] private float requiredGazeTime = 3.0f;

    [Header("視界・Ray設定")]
    [SerializeField] private float rayDistance = 10.0f;

    // 画面中央の検出範囲
    [SerializeField] private float detectionRadius = 0.15f;

    private Transform currentTarget;

    private float gazeStartTime;

    [Header("内部状態")]
    private bool hasLogged = false;

    void Update()
    {
        CheckGazeTarget();
    }

    // 画面中央付近に対象物があるか確認
    void CheckGazeTarget()
    {
        Transform hitTarget = null;

        // 画面中央から少し範囲を持たせてRayを飛ばす
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit[] hits = Physics.SphereCastAll(
            ray,
            detectionRadius,
            rayDistance
        );

        // 最も近い対象Tagのオブジェクトを取得
        float nearestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            Transform target = hit.collider.transform;

            while (target != null)
            {
                if (target.CompareTag(targetTag))
                {
                    if (hit.distance < nearestDistance)
                    {
                        nearestDistance = hit.distance;
                        hitTarget = target;
                    }

                    break;
                }

                target = target.parent;
            }
        }

        // 観測対象が切り替わった場合
        if (hitTarget != currentTarget)
        {
            // 前の対象の観測を終了
            if (currentTarget != null)
            {
                EndObservation();
            }

            currentTarget = hitTarget;

            // 新しい対象の観測開始
            if (currentTarget != null)
            {
                gazeStartTime = Time.time;
                hasLogged = false;

                Debug.Log(
                    "観測開始: " + currentTarget.name
                );
            }
        }

        // 対象が視界から外れた場合
        if (currentTarget == null)
        {
            return;
        }

        // 観測時間を計算
        float gazeDuration = Time.time - gazeStartTime;

        // 3秒以上連続して観測した場合
        if (gazeDuration >= requiredGazeTime && !hasLogged)
        {
            RecordObservation(gazeDuration);
            hasLogged = true;
        }
    }

    // 観測終了時の処理
    void EndObservation()
    {
        float duration = Time.time - gazeStartTime;

        if (duration < requiredGazeTime)
        {
            Debug.Log(
                "観測終了（記録条件未達）: " +
                currentTarget.name +
                " / " +
                duration.ToString("F2") + "秒"
            );
        }
        else
        {
            Debug.Log(
                "観測終了: " + currentTarget.name +
                " / " + duration.ToString("F2") + "秒"
            );
        }
    }

    // ログ出力
    void RecordObservation(float duration)
    {
        Vector3 objectPosition = currentTarget.position;

        Debug.Log(
            "========== Observation Log ==========\n" +
            "Object: " + currentTarget.name + "\n" +
            "Tag: " + currentTarget.tag + "\n" +
            "Position: " + objectPosition + "\n" +
            "Start Time: " + gazeStartTime.ToString("F2") + "\n" +
            "Observation Time: " + duration.ToString("F2") + " seconds\n" +
            "======================================"
        );
    }
}