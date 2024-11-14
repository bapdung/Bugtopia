using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MeshVisualizerManager : MonoBehaviour
{
    private ARPlaneMeshVisualizer planeMeshVisualizer;

    void Start()
    {
        planeMeshVisualizer = GetComponent<ARPlaneMeshVisualizer>();

        // 초기 상태에서 planeMeshVisualizer 비활성화
        if (planeMeshVisualizer != null)
        {
            planeMeshVisualizer.enabled = false;
        }
    }

    public void ShowMeshVisualizer()
    {
        if (planeMeshVisualizer != null)
        {
            planeMeshVisualizer.enabled = true;
        }
    }

    public void HideMeshVisualizer()
    {
        if (planeMeshVisualizer != null)
        {
            planeMeshVisualizer.enabled = false;
        }
    }
}
