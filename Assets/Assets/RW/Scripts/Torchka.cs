using UnityEngine;

namespace Assets.RW.Scripts
{
    public class Torchka : MonoBehaviour
    {
        internal TorchkaManager manager;

        [SerializeField] 
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            var sprite = spriteRenderer.sprite;
            var normalizedPivot = sprite.pivot / sprite.rect.size;
            spriteRenderer.sprite = Sprite.Create(Instantiate(sprite.texture),
                                        sprite.rect, normalizedPivot, sprite.pixelsPerUnit);
        }

        //anyone in layer 'Hero' or 'Enemy' will trigger this
        private void OnTriggerStay2D(Collider2D other)
        {
            var damage = manager.CheckForDamage(spriteRenderer.sprite.texture,
                 spriteRenderer.transform.InverseTransformPoint(other.transform.position));

            //Uncomment the following after adding the 'Bullet' code:
            // if (other.GetComponent<Bullet>() && damage)
            //     other.GetComponent<Bullet>().DestroySelf();
        }
    }
}