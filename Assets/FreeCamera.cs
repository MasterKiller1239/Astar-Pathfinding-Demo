using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 10.0f;
    [SerializeField]
    private float _lookSpeed = 2.0f;
    [SerializeField]
    private float _zoomSpeed = 4.0f;
    [SerializeField]
    private float _minZoom = 5.0f;
    [SerializeField]
    private float _maxZoom = 50.0f;       

    private float _yaw = 0.0f;
    private float _pitch = 0.0f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        transform.Translate(movement * _movementSpeed * Time.deltaTime, Space.Self);

        if (Input.GetMouseButton(1)) 
        {
            _yaw += _lookSpeed * Input.GetAxis("Mouse X");
            _pitch -= _lookSpeed * Input.GetAxis("Mouse Y");

            _pitch = Mathf.Clamp(_pitch, -90f, 90f);

            transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float distance = Vector3.Distance(transform.position, transform.position + transform.forward * scroll * _zoomSpeed);
        if (distance >= _minZoom && distance <= _maxZoom)
        {
            transform.Translate(0, 0, scroll * _zoomSpeed, Space.Self);
        }
    }
}
