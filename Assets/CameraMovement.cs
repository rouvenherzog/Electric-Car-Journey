using UnityEngine;
using UnityEngine.Analytics;

public class CameraMovement : MonoBehaviour
{
    public float MoveSpeed = 10;
    public float ZoomSpeed = 25;

    public float TargetRotation = 60f;
    public float RotationSpeed = 60f;

    // Update is called once per frame
    void Update()
    {
        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");

        Vector3 translationDelta =
            Vector3.back * zDelta + Vector3.left * xDelta;
        transform.position +=
            MoveSpeed * Time.unscaledDeltaTime * translationDelta;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        transform.position += scrollInput * ZoomSpeed * transform.forward;

        if (Input.GetKeyDown(KeyCode.Space)) {
            TargetRotation = TargetRotation == 60 ? 90 : 60;
            transform.rotation = Quaternion.Euler(TargetRotation, 180f, 0f);
        }
    }
}
