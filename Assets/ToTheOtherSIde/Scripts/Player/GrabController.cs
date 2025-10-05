using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class GrabController : PausedBehaviour
{
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private GameObject eIcon;
    [SerializeField] private Transform grabPoint;
    [SerializeField] private GameObject handsDefault;
    [SerializeField] private GameObject handsGrab;
    [SerializeField] private Collider playerCollider;
    //[SerializeField, Range(5f, 20f)] private float followSpeed = 10f;

    private bool isGrabbing = false;
    [SerializeField] private GameObject targetObject = null;
    [SerializeField] private GameObject grabbedObject = null;

    private void Start()
    {
        eIcon.SetActive(true);
        eIcon.SetActive(false);
    }

    protected override void GameUpdate()
    {
        HandleRaycast();
        HandleGrabInput();
        FollowGrabbedObject();
    }

    private void HandleRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("PuzzleObject") && !isGrabbing)
            {
                eIcon.SetActive(true);
                targetObject = hit.collider.gameObject;
            }
            else if (!isGrabbing)
            {
                eIcon.SetActive(false);
                targetObject = null;
            }
        }
        else if (!isGrabbing)
        {
            eIcon.SetActive(false);
            targetObject = null;
        }
    }

    private void HandleGrabInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isGrabbing && targetObject != null)
            {
                GrabObject(targetObject);
                handsDefault.SetActive(false);
                handsGrab.SetActive(true);
            }
            else if (isGrabbing && grabbedObject != null)
            {
                ReleaseObject();
                handsGrab.SetActive(false);
                handsDefault.SetActive(true);
            }
        }
    }

    private void GrabObject(GameObject obj)
    {
        grabbedObject = obj;

        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        Collider objCol = grabbedObject.GetComponent<Collider>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (objCol != null)
        {
            objCol.isTrigger = true;
            Physics.IgnoreCollision(playerCollider, objCol, true);
        }

        // Присваиваем parent и позицию только после захвата
        grabbedObject.transform.parent = grabPoint;
        grabbedObject.transform.position = grabPoint.position;

        isGrabbing = true;
        eIcon.SetActive(false);
    }

    private void ReleaseObject()
    {
        if (grabbedObject == null) return;

        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        Collider objCol = grabbedObject.GetComponent<Collider>();

        if (rb != null)
            rb.useGravity = true;

        if (objCol != null)
        {
            objCol.isTrigger = false;
            StartCoroutine(ReenableCollision(objCol));
        }

        // Сдвигаем объект немного вперед, чтобы не пересекался с игроком
        grabbedObject.transform.parent = null;
        grabbedObject.transform.position += grabPoint.forward * 0.5f;

        grabbedObject = null;
        isGrabbing = false;
    }

    private IEnumerator ReenableCollision(Collider objCol)
    {
        yield return null; // один кадр
        Physics.IgnoreCollision(playerCollider, objCol, false);
    }

    private void FollowGrabbedObject()
    {
        if (isGrabbing && grabbedObject != null)
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            Vector3 targetPos = grabPoint.position;

            if (rb != null)
            {
                // Плавное движение через Lerp
                rb.MovePosition(targetPos);
            }
            else
            {
                grabbedObject.transform.position = targetPos;
            }
        }
    }
}
