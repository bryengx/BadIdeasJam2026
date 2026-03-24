using UnityEngine;
using UnityEngine.InputSystem;

public class EggBrake : MonoBehaviour
{
    [SerializeField] private GameObject crackedEgg;
    [SerializeField] private float breakSPeed = 5f;
    [SerializeField] private LayerMask dragLayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D eggCollide;

    [SerializeField] private DialogOnTrigger.SpeakerInfo failToBrakeText;

    private TargetJoint2D joint;
    private Camera mainCam;

    private void OnEnable()
    {
        mainCam = Camera.main;
    }
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.value);
            
            if (Physics2D.OverlapPoint(mousePos, dragLayer) == GetComponent<Collider2D>())
            {
                BeginDrag(mousePos);
            }
        }
        if (joint != null)
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.value);
            joint.target = mousePos;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame && joint != null)
        {
            EndDrag();
        }
    }
    private void BeginDrag(Vector2 pos)
    {
        joint = gameObject.AddComponent<TargetJoint2D>();
        joint.target = pos;
        joint.anchor = transform.InverseTransformPoint(pos);

        joint.frequency = 15;
        joint.dampingRatio = 1;
    }
    private void EndDrag()
    {
        Destroy(joint);

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (eggCollide != collision.collider) return;

        float impactSpeed = collision.relativeVelocity.magnitude;

        Debug.Log("Speed was: " + impactSpeed);

        if (impactSpeed >= breakSPeed)
        {
            BreakEgg();
        }
        else
        {
            DialogOnTrigger.SpeakerInfo[] info = new DialogOnTrigger.SpeakerInfo[] { failToBrakeText };
            DialogOnTrigger.OnTriggerDialog?.Invoke(null, info, true);
        }
    }
    private void BreakEgg()
    {
        Debug.Log("Egg broke!");
        crackedEgg.SetActive(true);

        GetComponent<CompleteTask>()?.TaskComplete();

        Destroy(gameObject);
    }
}
