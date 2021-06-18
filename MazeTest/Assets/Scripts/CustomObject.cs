using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomObject : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private TMP_Text title;
    [SerializeField]
    private Image colorPreview;

    private ShopController parent;

    private void Start()
    {
        parent = GetComponentInParent<ShopController>();
    }

    public void Set(string text, Color color)
    {
        title.text = text;
        colorPreview.color = color;
    }

    public void OnClicked()
    {
        parent.OnColorClicked(colorPreview.color);
    }
}