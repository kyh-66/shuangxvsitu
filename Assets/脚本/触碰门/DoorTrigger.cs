using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Animator anim;
    private bool hasOpened = false; // 确保只触发一次

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 只有主角能触发，且之前没开过门
        if (other.CompareTag("Player") && !hasOpened)
        {
            // 2. 标记已开门，防止重复播放
            hasOpened = true;

            // 3. 设置动画参数 (记得在 Animator 里要把参数名设为 "Open")
            if (anim != null)
            {
                anim.SetTrigger("Open");
            }
        }
    }
}