using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    bool isDrag;

    public float angularVelocity;

    public float maxForce;
    public float maxTime;

    public float minArrow;
    public float maxArrow;

    public RectTransform arrow;

    Tween delay;
    Rigidbody2D rb;

    bool isRight;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        rb.simulated = false;

        arrow.gameObject.SetActive(false);

        arrow.anchoredPosition = new Vector2(0, 75);
        arrow.localRotation = Quaternion.Euler(0, 0, 90);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;

            ActiveArrow(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;

            ActiveArrow(false);
        }
    }

    void ActiveArrow(bool isActive)
    {
        arrow.gameObject.SetActive(isActive);

        arrow.DOKill();

        delay.Kill();

        //animator.SetBool("Rush", isActive);

        if (isActive)
        {
            Time.timeScale = 0.3f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            if (rb.simulated)
            {
                AfterFly();
            }

            arrow.sizeDelta = new Vector2(minArrow, arrow.sizeDelta.y);

            float time = 1f;

            arrow.DOSizeDelta(new Vector2(maxArrow, arrow.sizeDelta.y), time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(delegate
            {
                arrow.DOSizeDelta(new Vector2(minArrow, arrow.sizeDelta.y), time * 2).SetEase(Ease.Linear).SetUpdate(true);
            });
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            rb.gravityScale = 0;
            rb.angularVelocity = 0;

            if (!rb.simulated)
            {
                isRight = true;

                arrow.anchoredPosition = new Vector2(50, 0);
                arrow.localRotation = Quaternion.identity;

                rb.simulated = true;

                rb.velocity = transform.up * GetForce();

                delay = DOVirtual.DelayedCall(GetTime(), delegate
                {
                    AfterFly();
                });
            }
            else
            {
                isRight = IsRight();

                Flip();

                rb.velocity = (isRight ? transform.right : -transform.right) * GetForce();

                delay = DOVirtual.DelayedCall(GetTime(), delegate
                {
                    AfterFly();
                });
            }
        }
    }

    void AfterFly()
    {
        rb.gravityScale = 1;
        rb.velocity *= 0.5f;
        rb.angularVelocity = isRight ? -angularVelocity : angularVelocity;
    }

    bool IsRight()
    {
        float angle = ConvertAngle(arrow.eulerAngles.z);

        return angle >= -90f && angle <= 90f;
    }

    float ConvertAngle(float angle)
    {
        if (angle > 180) return angle - 360;

        return angle;
    }

    void Flip()
    {
        if (IsRight())
        {
            if (arrow.anchoredPosition.x > 0) return;

            arrow.anchoredPosition = new Vector2(50, 0);
            arrow.localRotation = Quaternion.Euler(0, 0, 0);

            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + 180);
        }
        else
        {
            if (arrow.anchoredPosition.x < 0) return;

            arrow.anchoredPosition = new Vector2(-50, 0);
            arrow.localRotation = Quaternion.Euler(0, 0, 180);

            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z - 180);
        }
    }

    float GetPercent()
    {
        float total = maxArrow - minArrow;

        float value = arrow.sizeDelta.x - minArrow;

        return value * 100 / total;
    }

    float GetForce()
    {
        return GetPercent() / 100 * maxForce;
    }

    float GetTime()
    {
        return GetPercent() / 100 * maxTime;
    }
}
