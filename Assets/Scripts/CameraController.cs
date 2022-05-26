using UnityEngine;
using DG.Tweening;
using PathCreation;
public class CameraController : MonoBehaviour
{

    [SerializeField] private float smoothCamera;
    [SerializeField] private PathCreator rotatePathCreator;
    private Transform _player;

    private Transform _camera;

    [HideInInspector] public bool Play;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _camera = GetComponent<Transform>();
    }
    
    private void Update()
    {
        if(!Play) 
            return;
        
        Vector3 pos = new Vector3(_player.position.x, _player.position.y + 4, _player.position.z - 5);
        
        transform.position = Vector3.Lerp(transform.position, pos, smoothCamera * Time.deltaTime);

    }

    public void Finish()
    {
        Play = false;
        Vector3 rot = new Vector3(29, 0, 0);
        _camera.DORotate(rot, 2);
    }
}
