using UnityEngine;

public class ChangeFovPlayer : MonoBehaviour
{
    public new Camera camera;
    public int FOV;
    // Update is called once per frame
    void Update()
    {
        camera.fieldOfView = FOV;
    }
}
