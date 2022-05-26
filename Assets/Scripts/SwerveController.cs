using UnityEngine;

public class SwerveController : MonoBehaviour
{
    private float _lastFrameFingerPosX;
    
    private float _moveFactorX;

    public static float MoveFactorX;

    private void Update()
    {
        SwerveController.MoveFactorX = _moveFactorX;
        
        if (Input.GetMouseButtonDown(0))
        {
            _lastFrameFingerPosX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            _moveFactorX = Input.mousePosition.x - _lastFrameFingerPosX;
            _lastFrameFingerPosX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moveFactorX = 0f;
        }
    }
}
