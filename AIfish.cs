using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIfastfish : MonoBehaviour {

    public float moveSpeed = 4f;
    public float runSpeed = 10f;
    public float rotSpeed = 100f;
    public float runDistance = 25f;
    public Transform Player;
    private Animator anim;
    private bool close = true;
    public Transform Reff;
    private float ComingBackDistance;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isMoveingUp = false;
    private bool isMoveingDown = false;
    private bool isWalking = false;
    private Quaternion _lookRotation;
    private Vector3 _dirction;
    private Quaternion originalRotsion;



    // Use this for initialization
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Playita")
        {
            ComingBackDistance = 40f;
        }
        if(sceneName == "MetalGraveyard")
        {
            ComingBackDistance = 75f;
        }
        if(sceneName == "StoneTwon")
        {
            ComingBackDistance = 100f;
        }

            anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        originalRotsion = transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y > 1)
        {
            Debug.Log("im above water");
        }

        AvoidEnvirment();

        RunAwaya();

        ComeBack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            close = false;
        }
    }

    IEnumerator Wander()
    {
        _dirction = (Reff.position - transform.position).normalized;

        _lookRotation = Quaternion.LookRotation(_dirction);

        if (close == true)
        {
            int rotTime = Random.Range(1, 3);
            int rotateWait = Random.Range(1, 4);
            int rotateLorRUorD = Random.Range(0, 6);
            int walkWait = Random.Range(1, 2);
            int walkTime = Random.Range(5, 10);



            isWandering = true;

            yield return new WaitForSeconds(walkWait);
            isWalking = true;
            yield return new WaitForSeconds(walkTime);
            isWalking = false;
            yield return new WaitForSeconds(rotateWait);
            if (rotateLorRUorD < 3)
            {
                isRotatingRight = true;
                yield return new WaitForSeconds(rotTime);
                isRotatingRight = false;
            }
            if (rotateLorRUorD > 3)
            {
                isRotatingLeft = true;
                yield return new WaitForSeconds(rotTime);
                isRotatingLeft = false;

            }
            if (rotateLorRUorD == 3)
            {
                isMoveingUp = true;
                yield return new WaitForSeconds(rotTime);
                isMoveingUp = false;

            }
            if (rotateLorRUorD == 4)
            {
                isMoveingDown = true;
                yield return new WaitForSeconds(rotTime);
                isMoveingDown = false;

            }
            isWandering = false;
        }
    }

    public void movment()
    {
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (isRotatingLeft == true)
        {
            transform.Rotate(0, -rotSpeed * Time.deltaTime, 0);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (isMoveingUp == true && transform.position.y < -2)
        {
            transform.Translate(0, moveSpeed * Time.deltaTime, 0);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (isMoveingDown == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
        }
        if (isWalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    void AvoidEnvirment()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 dwd = transform.TransformDirection(Vector3.down);

        if (Physics.Raycast(transform.position, fwd, 25f))
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
        }
        if (Physics.Raycast(transform.position, dwd, 5f))
        {
            transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        }

    }

    void RunAwaya()
    {
        Vector3 dwd = transform.TransformDirection(Vector3.down);
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        _dirction = (Player.position - transform.position).normalized;

        _lookRotation = Quaternion.LookRotation(-_dirction);

        if (!Physics.Raycast(transform.position, dwd, 5f) && !Physics.Raycast(transform.position, fwd, 5f))
        {
            if (Vector3.Distance(transform.position, Player.position) < runDistance && transform.position.y < -2)
            {
                anim.SetBool("isRuning", true);
                Debug.Log("run");
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, runSpeed * Time.deltaTime);
                transform.position += transform.forward * runSpeed * Time.deltaTime;
            }

            else
            {
                movment();
                anim.SetBool("isRuning", false);
            }

            if (transform.position.y > -2)
            {
                transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, Player.position) > 30f && transform.rotation.eulerAngles.x < 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, originalRotsion, moveSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, Player.position) > 30f && transform.rotation.eulerAngles.x > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, originalRotsion, moveSpeed * Time.deltaTime);
            }
        }
    }

    public void ComeBack()
    {
        _dirction = (Reff.position - transform.position).normalized;

        _lookRotation = Quaternion.LookRotation(_dirction);

        if (Vector3.Distance(transform.position, Reff.position) > ComingBackDistance)
        {
            close = false;
        }
        if (close == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, runSpeed * Time.deltaTime);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Vector3.Distance(transform.position, Reff.position) < 20f)
        {
            close = true;
        }
    }

}
