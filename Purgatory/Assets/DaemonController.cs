using System.Collections;
using UnityEngine;

public class DaemonController : MonoBehaviour
{
    public Transform player;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;

    [Header("Behavior Timers")]
    public float actionInterval = 2f;
    public float teleportChance = 0.25f;

    [Header("Dash Indicator")]
    public GameObject dashIndicatorPrefab;
    private GameObject dashIndicatorInstance;
    private LineRenderer dashLine;

    [Header("Dash Windup")]
    public float dashWindupTime = 0.5f;

    private bool isWindingUp = false;
    private float windupTimer;

    private float actionTimer = 0f;
    private bool isDashing = false;
    private Vector2 dashDirection;
    private float dashTimer = 0f;


    private void Update()
    {
        if (isWindingUp)
        {
            windupTimer += Time.deltaTime;

            if (dashIndicatorInstance != null && dashLine != null)
            {
                Vector3 start = transform.position;
                Vector3 direction = (player.position - start).normalized;
                float overshootDistance = 0.5f;

                Vector3 end = player.position + (Vector3)(direction * overshootDistance);

                dashLine.SetPosition(0, start);
                dashLine.SetPosition(1, end);
            }

            if (windupTimer >= dashWindupTime)
            {
                StartDash();
                isWindingUp = false;
            }
            return;
        }


        if (player == null) return;

        if (isDashing)
        {
            DashMovement();
        }
        else
        {
            Move();

            actionTimer += Time.deltaTime;
            if (actionTimer >= actionInterval)
            {
                actionTimer = 0f;
                RandomizeAction();
            }
        }

    }

    void Move()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    void RandomizeAction()
    {
        if (Random.value < teleportChance)
        {
            Teleport();
        }
        else
        {
            StartDashWindup();
        }
    }

    void StartDashWindup()
    {
        dashDirection = (player.position - transform.position).normalized;
        windupTimer = 0f;
        isWindingUp = true;

        if (dashIndicatorInstance == null && dashIndicatorPrefab != null)
        {
            dashIndicatorInstance = Instantiate(dashIndicatorPrefab);
            dashLine = dashIndicatorInstance.GetComponent<LineRenderer>();
        }

        if (dashIndicatorInstance != null)
        {
            dashIndicatorInstance.SetActive(true);

            Vector3 start = transform.position;
            Vector3 end = start + (Vector3)(dashDirection * dashSpeed * dashDuration);

            dashLine.SetPosition(0, start);
            dashLine.SetPosition(1, end);

            Color c = Color.red;
            dashLine.startColor = c;
            dashLine.endColor = c;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = 0f;

        if (dashIndicatorInstance != null)
        {
            StartCoroutine(FadeOutDashIndicator(0.2f));
        }
    }

    void DashMovement()
    {
        dashTimer += Time.deltaTime;
        transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);

        if (dashTimer >= dashDuration)
        {
            isDashing = false;
        }
    }

    void Teleport()
    {
        Vector2 offset = Random.insideUnitCircle.normalized * 0.6f;
        transform.position = player.position + (Vector3)offset;
    }

    IEnumerator FadeOutDashIndicator(float duration)
    {
        if (dashLine == null) yield break;

        float elapsed = 0f;

        Color startColor = dashLine.startColor;
        Color endColor = dashLine.endColor;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

            dashLine.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            dashLine.endColor = new Color(endColor.r, endColor.g, endColor.b, alpha);

            yield return null;
        }

        dashLine.startColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        dashLine.endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        if (dashIndicatorInstance != null)
            dashIndicatorInstance.SetActive(false);
    }
}
