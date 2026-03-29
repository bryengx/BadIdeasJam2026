using UnityEngine;

public class CleanSponge : SpriteDrag
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dirt dirt = collision.gameObject.GetComponent<Dirt>();
        if (dirt != null)
        {
            if (dirt.type == Dirt.DirtType.Dishes)
            {
                dirt.stain--;
                if (dirt.stain <= 0)
                {
                    Destroy(collision.gameObject);
                }
            }

        }
    }
}
