using UnityEngine;
using PathCreation;

public class SoloPlayerController : MonoBehaviour
{
    
    [SerializeField] private GameObject deadParticles;
    [SerializeField] private Material yellowMat;
    [SerializeField] private float speedFollow;
    [SerializeField] private Transform parentCollector;
    [SerializeField] private PathCreator rotatePathCreator;


    private float rotY;
    private bool isDead;
    public Transform currentTarget;
    public Animator anim;
    public bool isAdded;
    public GameObject bodyStick;

    private void Start()
    {
        ChangeRagdoll(true, false);
        GetComponent<Rigidbody>().isKinematic = false;

        GetComponent<BoxCollider>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;

    }
    private void Update()
    {
        if(currentTarget && !isDead)
        {
            transform.LookAt(currentTarget);

            if(Vector2.Distance(transform.position, currentTarget.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speedFollow * Time.deltaTime);
            }
            else 
            {
                anim.SetBool("isRun", false);
                this.Dead();
            }
        }
        if(transform.position.y < -5 )
        {
            transform.SetParent(null);
        }
    }

    public void Rotator()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(SwerveController.MoveFactorX > 0) rotY = transform.eulerAngles.y - 45f;
            if(SwerveController.MoveFactorX < 0) rotY = transform.eulerAngles.y + 45f;
        }
        if(Input.GetMouseButton(0))
        {
            if(SwerveController.MoveFactorX > 0)
            {    
                transform.eulerAngles = new Vector2(0f, rotY);
            }
            if(SwerveController.MoveFactorX < 0)
            {
                transform.eulerAngles = new Vector2(0f, rotY);
            }            
        }        
    }


    private void OnEnable()
    {
        PlayerController.OnFinish += OnFinish;
    }

    private void OnDisable()
    {
        PlayerController.OnFinish -= OnFinish;
    }

    private void CollectHumans(GameObject currentHuman)
    {
        currentHuman.transform.SetParent(parentCollector);
    }

    private void OnFinish()
    {
        anim.SetBool("isDance", true);        
    }

    public void Dead()
    {
        isDead = true;
        GetComponent<Rigidbody>().AddForce(Vector3.back * 15, ForceMode.Impulse);
        GetComponent<BoxCollider>().enabled = false;

        transform.SetParent(null);

        anim.enabled = false;

        ChangeRagdoll(false, true);
    }

    private void ChangeRagdoll(bool isActive, bool collidersActive)
    {
        Rigidbody[] rbAll = GetComponentsInChildren<Rigidbody>();
        Collider[] colliders = GetComponentsInChildren<Collider>();


        foreach (Rigidbody rb in rbAll)
        {
            rb.isKinematic = isActive;
        }
        foreach (Collider coll in colliders)
        {
            coll.enabled = collidersActive;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AddPlayer"))
        {
            SoloPlayerController player = other.GetComponent<SoloPlayerController>();

            if(isAdded && !player.currentTarget && !player.isAdded)
            {
                player.isAdded = true;

                player.anim.SetBool("isRun", true);
                player.bodyStick.GetComponent<SkinnedMeshRenderer>().material = yellowMat;

                CollectHumans(other.gameObject);                    
            }
        }

        if(other.CompareTag("Finish"))
        {
            PlayerController.OnFinish?.Invoke();
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Box"))
        {
            Dead();

        }
    }
}
