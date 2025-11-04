using Destructible2D;
using UnityEngine;

public class Test : MonoBehaviour
{
    public LayerMask Mask = -1;

    public D2dDestructible.PaintType StampPaint;

    public Texture2D StampShape;

    public Color StampColor = Color.white;

    public Vector2 StampSize = new Vector2(1.0f, 1.0f);

    float camSpeed = 5;
    float camZoomSpeed = 500;

    public Camera cam;

    Vector3 startInput;

    Vector2 startSize;

    public Transform shape;

    Material matShape;

    private void Awake()
    {
        matShape = shape.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        Vector3 currentInput = cam.ScreenToWorldPoint(Input.mousePosition);

        currentInput.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            startInput = cam.ScreenToWorldPoint(Input.mousePosition);

            startSize = StampSize;
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            float deltaFOV = scrollInput * camZoomSpeed * Time.deltaTime;

            cam.orthographicSize -= deltaFOV;

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 5, 15);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt)) matShape.color = Color.green;
        if (Input.GetKeyUp(KeyCode.LeftAlt)) matShape.color = Color.white;

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            float x = Mathf.Clamp(startSize.x + (currentInput.x - startInput.x), 1, 5);

            StampSize = Vector2.one * x;
            shape.localScale = Vector3.one * x;
        }
        else
        {
            shape.position = currentInput;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            matShape.color = Color.yellow;

            var stampPosition = shape.position;

            D2dStamp.All(StampPaint, stampPosition, StampSize, 0, StampShape, StampColor, Mask);
        }

        if (Input.GetKey(KeyCode.W))
        {
            cam.transform.Translate(Vector3.up * camSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cam.transform.Translate(Vector3.down * camSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            cam.transform.Translate(Vector3.left * camSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            cam.transform.Translate(Vector3.right * camSpeed * Time.deltaTime);
        }
    }
}
