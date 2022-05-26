using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    [SerializeField] private float swerveSpeed;
    
    private void Update()
    {
        float swerveAmount = Time.deltaTime * swerveSpeed * SwerveController.MoveFactorX;

        transform.Translate(swerveAmount, 0 , 0);
    }
}
