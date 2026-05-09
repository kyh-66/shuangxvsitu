using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // ... 原有变量保持不变 ...
    public static AnimationController Instance; // 添加单例方便 WorldManager 访问

    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    public float animationSpeed = 1f;
    public GameObject panel;

    // 将 isShown 改为 public，让外部可以读取
    public bool isShown = false;
    private bool canClickToHide = false;
    private Coroutine currentCoroutine;
    private bool hasTriggeredOnce = false;

    private void Awake()
    {
        Instance = this; // 初始化单例
    }

    private void Start()
    {
        if (panel != null)
        {
            panel.transform.localScale = Vector3.zero;
            panel.SetActive(false);
        }
    }

    private void Update()
    {
        // 如果窗口开着，按任何键（包括E）都会先关掉它
        if (canClickToHide && Input.anyKeyDown)
        {
            HideCurrentPanel();
        }
    }

    // ... OnTriggerEnter2D 等其余部分完全不动 ...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isShown && !hasTriggeredOnce)
        {
            ShowCurrentPanel();
            hasTriggeredOnce = true;
        }
    }

    public void ShowCurrentPanel()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        panel.SetActive(true);
        currentCoroutine = StartCoroutine(ShowPanelCoroutine());
        isShown = true;
    }

    public void HideCurrentPanel()
    {
        if (!isShown) return;
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(HidePanelCoroutine());
        isShown = false;
        canClickToHide = false;
    }

    IEnumerator ShowPanelCoroutine()
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            panel.transform.localScale = Vector3.one * showCurve.Evaluate(timer);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }
        panel.transform.localScale = Vector3.one * showCurve.Evaluate(1f);
        canClickToHide = true;
    }

    IEnumerator HidePanelCoroutine()
    {
        // 不再进行计时和曲线计算，直接一步到位
        panel.transform.localScale = Vector3.zero;
        panel.SetActive(false);

        yield return null;
    }

    private void OnDisable()
    {
        if (isShown)
        {
            isShown = false;
            canClickToHide = false;
            if (panel != null)
            {
                panel.transform.localScale = Vector3.zero;
                panel.SetActive(false);
            }
        }
    }
}