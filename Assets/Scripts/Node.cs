using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Vector3 positionOffset;
    private BuildManager manager;
    private GameObject turret;
    private UIData data;

    #if UNITY_EDITOR || UNITY_STANDALONE
    public Color hoverColor;
    public Color errorColor;
    private Color originalColor;
    private Renderer rend;
    #endif

    #if UNITY_ANDROID || UNITY_IOS
    private Touch touch;
    private float touchTimer;
    private float buildingWait = 0.3f;
    private bool actionDone = false;
    #endif

    #region GeneralMethods
    private void Start()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        #endif

        data = UIData.GetInstance();
        manager = BuildManager.instance;
    }

    void CreateTurret()
    {
        if (!manager.CanBuild)
            return;

        if (turret == null)
            manager.BuildTurretOn(this);
        else {
            data.GameLoggerText = "Occupied";
        }
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    public void SetTurret(GameObject turret)
    {
        this.turret = turret;
    }
    #endregion

    #region DesktopMethods
    #if UNITY_EDITOR || UNITY_STANDALONE
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject()
            && manager.CanBuild && manager.PlayerHasMoney)
            CreateTurret();
    }

    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            if (manager.CanBuild && manager.PlayerHasMoney)
                rend.material.color = hoverColor;
            else if (manager.CanBuild)
                rend.material.color = errorColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = originalColor;
    }
    #endif
    #endregion

    #region MobileMethods
    #if UNITY_ANDROID || UNITY_IOS
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began ||
                touch.phase == TouchPhase.Stationary
                && !actionDone)
            {
                touchTimer += Time.deltaTime;
                if (touch.phase == TouchPhase.Stationary
                    && touchTimer >= buildingWait) {
                    OnTouch(touch);
                    actionDone = true;
                }

            } else if (touch.phase == TouchPhase.Ended) {
                touchTimer = 0f;
                actionDone = false;

            } else {
                return;
            }
        }
    }

    void OnTouch(Touch touch)
    {
        RaycastHit hitInfo;
        float maxDistance = 100f;
        Collider collider = GetComponent<Collider>();
        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            if (collider.Raycast(ray, out hitInfo, maxDistance))
                CreateTurret();
    }
    #endif
    #endregion
}