using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    public float animationSpeed;
    public GameObject panel;

    IEnumerator ShowPanel(GameObject gameObject)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            gameObject.transform.localScale = Vector3.one * showCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
    }

    IEnumerator HidePanel(GameObject gameObject)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            gameObject.transform.localScale = Vector3.one * hideCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
        gameObject.transform.localScale = Vector3.zero;
    }

    private bool isShown = false; // 记录当前状态：true表示已弹出，false表示已隐藏
    private Coroutine currentCoroutine;

    private void Update()
    {
        // 只检测鼠标左键 (0)
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<AudioSource>().Play(); // 播放点击音效
            // 如果之前有动画在跑，先停掉防止冲突
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);

            if (!isShown)
            {
                // 如果当前是隐藏的 -> 弹出
                currentCoroutine = StartCoroutine(ShowPanel(panel));
                isShown = true; // 切换状态
            }
            else
            {
                // 如果当前是弹出的 -> 隐藏
                currentCoroutine = StartCoroutine(HidePanel(panel));
                isShown = false; // 切换状态
            }
        }
    }
}

