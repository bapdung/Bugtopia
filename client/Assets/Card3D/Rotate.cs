using UnityEngine;

public class Rotate : MonoBehaviour
{

  [Tooltip("Material Shader Mask Number")]
  public int maskNumber = 1;

  [Tooltip("Sensitivity of Horizontal Rotation")]
  public float rotationSensitivityH = 0.5f;

  [Tooltip("Sensitivity of Vertical Rotation")]
  public float rotationSensitivityV = 0.3f;

  [Tooltip("Maximum Horizontal Angle (side-to-side)")]
  [Range(0, 60)]
  public float maxAngleH = 20;

  [Tooltip("Maximum Vertical Angle (up-and-down)")]
  [Range(0, 60)]
  public float maxAngleV = 8;

  [Tooltip("Smooth Damping for Rotation")]
  public float rotationDamping = 5.0f;

  private Transform windowTransform;
  private Transform worldTransform;
  private Vector2 lastTouchPosition;
  private bool isTouching;

  private float targetRotationX = -6; // Target X rotation angle
  private float targetRotationY = 0; // Target Y rotation angle

  private void Awake()
  {
    windowTransform = transform.GetChild(1);
    worldTransform = transform.GetChild(2);

    SetStencilMask(maskNumber);
  }

  void Update()
  {
    HandleTouchInput();

    // Smoothly interpolate to target rotation for smooth motion
    Quaternion targetRotation = Quaternion.Euler(targetRotationX, targetRotationY, 0);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationDamping);
  }

  private void HandleTouchInput()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);

      if (touch.phase == TouchPhase.Began)
      {
        isTouching = true;
        lastTouchPosition = touch.position;
      }
      else if (touch.phase == TouchPhase.Moved && isTouching)
      {
        Vector2 touchDelta = touch.position - lastTouchPosition;
        lastTouchPosition = touch.position;

        // Calculate target rotation based on swipe
        targetRotationX = Mathf.Clamp(targetRotationX + touchDelta.y * rotationSensitivityV, -maxAngleV, maxAngleV);
        targetRotationY = Mathf.Clamp(targetRotationY - touchDelta.x * rotationSensitivityH, -maxAngleH, maxAngleH); // Invert to align with swipe direction
      }
      else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
      {
        // Reset touch tracking and set target rotation back to the original view
        isTouching = false;
        targetRotationX = -6;
        targetRotationY = 0;
      }
    }
  }

  public void SetStencilMask(int maskNumber)
  {
    this.maskNumber = maskNumber;

    windowTransform.GetComponent<Renderer>().material.SetFloat("_StencilMask", maskNumber);
    foreach (Transform worldObject in worldTransform.GetComponentInChildren<Transform>())
    {
      foreach (Material material in worldObject.GetComponent<Renderer>().materials)
      {
        material.SetFloat("_StencilMask", maskNumber);
      }
    }
  }
}
