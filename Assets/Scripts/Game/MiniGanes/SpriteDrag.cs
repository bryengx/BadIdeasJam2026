using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SpriteDrag : MonoBehaviour
{
    [SerializeField] protected LayerMask dragLayer;
    [SerializeField] protected Rigidbody2D rb;

    protected bool endDrag = false;

    private TargetJoint2D joint;
    private Camera mainCam;

    private void OnEnable()
    {
        mainCam = Camera.main;
    }
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && endDrag == false)
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
        if ((Mouse.current.leftButton.wasReleasedThisFrame && joint != null) || endDrag)
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
        if(joint != null)
        {
            Destroy(joint);

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
    }
    

}
