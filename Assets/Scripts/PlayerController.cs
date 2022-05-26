using UnityEngine;
using PathCreation;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{    
    public static Action OnFinish;
    [SerializeField] private float swerveSpeed;

    [SerializeField] private GameObject player;

    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private PathCreator rotatePathCreator;
    
    [SerializeField] private Animator animator;

    [SerializeField] private float swerveSmooth;
    [SerializeField] private float speed;

    [SerializeField] private Camera camera;
    
    [Header("UI")]

    [SerializeField] private Slider slider;

    [SerializeField] private GameObject startPanel;

    public static float DistanceTravelled = 0;

    private bool _play;
    
    [Space]

    public CameraController CameraController;

    private Vector3 swerveAmount;

    private float _distance;

    private void Awake()
    {
        DistanceTravelled = 0;
    }
    private void Start()
    {
        
        float startPosZ = transform.position.z;
        float finishZ = GameObject.FindGameObjectWithTag("Finish").transform.position.z;

        _distance = finishZ - startPosZ;

        slider.maxValue = _distance;

        slider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerController.OnFinish += Finish;
    }

    private void OnDisable()
    {
        PlayerController.OnFinish -= Finish;
    }

    private void Update()
    {
        if(transform.childCount == 0)
        {
            SceneManager.LoadScene(0);
        }
    }
    private void FixedUpdate()
    {
        if(!_play) 
            return;
        
        swerveAmount += Vector3.right * Time.deltaTime * swerveSpeed * SwerveController.MoveFactorX;

        Vector3 pos = new Vector3(swerveAmount.x, swerveAmount.y + 0.41f, swerveAmount.z);
        
        DistanceTravelled += -speed * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, pathCreator.path.GetPointAtDistance(DistanceTravelled) + pos, swerveSmooth * Time.deltaTime);
        transform.rotation = rotatePathCreator.path.GetRotationAtDistance(PlayerController.DistanceTravelled);

        slider.value += Time.deltaTime * speed;
    }

    private void Finish()
    {
        _play = false;
    }
    public void Play()
    {
        _play = true;

        transform.eulerAngles = new Vector2(0, 0);

        animator.SetBool("isRun", true);

        startPanel.SetActive(false);
        slider.gameObject.SetActive(true);
        
        float speedAnim = 0.7f;
        
        camera.GetComponent<Transform>().DOMoveY(3.15f, speedAnim);
        Vector3 pos = new Vector3(22, 0, 0);
        camera.GetComponent<Transform>().DORotate(pos, speedAnim).OnComplete(() => CameraController.Play = true);
    }
}
