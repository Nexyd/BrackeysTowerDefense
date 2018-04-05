using UnityEngine;
using UnityEngine.UI;

public class ObjectSizeManager : MonoBehaviour
{
    [Header("User Interface")]
    public Canvas shopUI;
    public Canvas playerStatsUI;

    [Header("Turrets")]
    public GameObject standardTurretPrefab;
    public GameObject missileLauncherPrefab;

    [Header("Projectiles")]
    //public GameObject bulletPrefab;
    public GameObject missilePrefab;

    #if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    private float panelSize;
    private float panelY;
    private Vector3 newPos;
    private RectTransform rect;
    private HorizontalLayoutGroup layout;

    private float textHeight;
    private float textWidth;
    private float playerUIPosY;

    private float turretHeight;
    private float turretWidth;
    private Vector3 newScale;

    private void Start()
    {
        textHeight = 20f;
        textWidth  = 60f;
        playerUIPosY = 10f;
        turretHeight = 100f;
        turretWidth  = 100f;
        panelSize = 130f;
        panelY = 65f;
        newScale = new Vector3(2, 2, 2);

        ResizeShop();
        MovePlayerStatsUI();
        ScaleTurrets(newScale);
        ScaleProjectiles(newScale);
    }

    private void ScaleTurrets(Vector3 newScale)
    {
        newScale = new Vector3(2, 2, 2);
        Transform standardTurretTransform = 
            standardTurretPrefab.GetComponent<Transform>();

        standardTurretTransform.localScale = newScale;
        Transform missileLauncherTransform =
            missileLauncherPrefab.GetComponent<Transform>();

        missileLauncherTransform.localScale = newScale;
    }

    private void ScaleProjectiles(Vector3 newScale)
    {
        Transform missileTransform =
            missilePrefab.GetComponent<Transform>();

        missileTransform.localScale = newScale;
    }

    private void ResizeShop()
    {
        rect = shopUI.GetComponent<RectTransform>();
        layout = shopUI.GetComponent<HorizontalLayoutGroup>();

        newPos = rect.position;
        newPos.y = panelY;
        rect.position = newPos;

        rect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical, panelSize);

        foreach (LayoutElement element in layout
            .GetComponentsInChildren<LayoutElement>())
        {
            element.preferredWidth = turretWidth;
            element.preferredHeight = turretHeight;

            ResizeCostText(element);
        }
    }

    private void ResizeCostText(LayoutElement element)
    {
        RectTransform costTransform = element
            .GetComponentInChildren<RectTransform>();

        costTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical, textHeight);

        costTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal, textWidth);
    }

    private void MovePlayerStatsUI()
    {
        playerUIPosY += shopUI.GetComponent
            <RectTransform>().rect.height;

        rect = playerStatsUI.GetComponent<RectTransform>();
        newPos = rect.position;
        newPos.y = playerUIPosY;
        rect.position = newPos;
    }
    #endif
}