using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 30f;
    public float minY = 20f;
    public float maxY = 80f;

    #if UNITY_EDITOR || UNITY_STANDALONE
    [Header("Desktop Variables")]
    public bool doMovement = true;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 5f;
    private float shopPanelHeight = 60f;
    #endif

    #if UNITY_ANDROID || UNITY_IOS
    [Header("Android Variables")]
    // The rate of change of the field of view.
    public float perspectiveZoomSpeed = 0.5f;
    public float orthoZoomSpeed = 0.5f;
    public float dragSpeed = 2f;

    private Vector3 dragOrigin;
    private new Camera camera;
    private Touch touch;

    private void Start()
    {
        camera = GetComponent<Camera>();
    }
    #endif

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
        DoDesktopCameraMovement();
        DoDesktopCameraZoom();
     
        #elif UNITY_ANDROID || UNITY_IOS
        DoMobileCameraMovement();
        DoMobileCameraZoom();
        #endif
    }

    #region DesktopMethods
    #if UNITY_EDITOR || UNITY_STANDALONE
    void DoDesktopCameraMovement()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            doMovement = !doMovement;

        if (doMovement)
        {
            if (Input.GetKey("w") || Input.mousePosition.y >=
                (Screen.height - panBorderThickness))
                transform.Translate(Vector3.forward * panSpeed
                    * Time.deltaTime, Space.World);

            if (Input.GetKey("s") || Input.mousePosition.y <=
                (panBorderThickness + shopPanelHeight))
                transform.Translate(Vector3.back * panSpeed
                    * Time.deltaTime, Space.World);

            if (Input.GetKey("a") || 
                Input.mousePosition.x <= panBorderThickness)
                transform.Translate(Vector3.left * panSpeed
                    * Time.deltaTime, Space.World);

            if (Input.GetKey("d") || Input.mousePosition.x >=
                (Screen.width - panBorderThickness))
                transform.Translate(Vector3.right * panSpeed
                    * Time.deltaTime, Space.World);
        }
    }

    void DoDesktopCameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;

        pos.y -= (scroll * 1000) * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
    #endif
    #endregion

    #region MobileMethods
    #if UNITY_ANDROID || UNITY_IOS
    void DoMobileCameraMovement()
    {
        float threshold = 50f;
        if (Input.touchCount == 1)
        {
            touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
                dragOrigin = Input.touches[0].position;

            threshold = (touch.position.x > 
                touch.deltaPosition.x) ? 50f : 0f;

            if (touch.phase == TouchPhase.Moved &&
                touch.position.x > (dragOrigin.x + threshold))
                transform.Translate(Vector3.right * panSpeed
                    * Time.deltaTime, Space.World);

            else if (touch.phase == TouchPhase.Moved &&
                touch.position.x < (dragOrigin.x - threshold))
                transform.Translate(Vector3.left * panSpeed
                    * Time.deltaTime, Space.World);

            threshold = (touch.position.y > 
                touch.deltaPosition.y) ? 50f : 0f;

            if (touch.phase == TouchPhase.Moved &&
                touch.position.y > (dragOrigin.y + threshold))
                transform.Translate(Vector3.forward * panSpeed
                    * Time.deltaTime, Space.World);

            else if (touch.phase == TouchPhase.Moved &&
                touch.position.y < (dragOrigin.y - threshold))
                transform.Translate(Vector3.back * panSpeed
                    * Time.deltaTime, Space.World);
        }
    }

    void DoMobileCameraZoom()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (camera.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                camera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                camera.orthographicSize = Mathf.Max(camera.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                camera.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 20 and 80.
                camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minY, maxY);
            }
        }
    }
    #endif
    #endregion
}