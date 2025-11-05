using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageIndicator : MonoBehaviour
{
    public TMP_Text damageText;

    public float moveSpeed;

    public float lifetime = 3f;

    private RectTransform myRect;

    private void Start()
    {
        Destroy(gameObject, lifetime);

        myRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        myRect.anchoredPosition += new Vector2(0f, -moveSpeed * Time.deltaTime);
    }
}
