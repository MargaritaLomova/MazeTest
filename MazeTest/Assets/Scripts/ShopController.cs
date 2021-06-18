using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [Header("Components"), SerializeField]
    private Transform content;

    [Header("Objects From Scene"), SerializeField]
    private PlayerController player;
    [SerializeField]
    private GameObject menuInterface;

    [Header("Prefabs"), SerializeField]
    private CustomObject customObjectPrefb;

    [Header("Variables To Control"), SerializeField]
    private List<Color> customColors = new List<Color>
    {
        Color.white,
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

    private void Start()
    {
        foreach (var color in customColors)
        {
            var newColor = Instantiate(customObjectPrefb, content);
            newColor.Set("", color);
        }
    }

    #region Clicked

    public void OnBackButtonClicked()
    {
        gameObject.SetActive(false);
        menuInterface.SetActive(true);
    }

    public void OnColorClicked(Color clickedColor)
    {
        player.SetColor(clickedColor);
        gameObject.SetActive(false);
        menuInterface.SetActive(true);
    }

    #endregion
}