using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    Animator animator;

    bool isMouseDown;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDown = true;
            
        animator.SetTrigger("MouseDown");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isMouseDown) return;

        isMouseDown = false;

        animator.SetTrigger("MouseUp");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isMouseDown) return;

        isMouseDown = false;

        animator.SetTrigger("MouseUp");
    }
}
