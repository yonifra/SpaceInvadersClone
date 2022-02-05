using UnityEngine;

namespace Assets.RW.Scripts
{
    public class TorchkaManager : MonoBehaviour
    {
        [SerializeField] 
        Texture2D damageTexture;

        [SerializeField] 
        private Sprite torchkaSprite;

        [Space]
        [Header("Torchka Spawn Settings")]
        [SerializeField] 
        private Transform spawnMidPoint;

        [SerializeField] 
        private Torchka torchkaPrefab;
        
        [SerializeField] 
        private int totalCount = 4;
        
        [SerializeField] 
        private float spacing = 150f;

        private float torchkaPPU;
        private Vector2 torchkaPivot;
        private Color[] damagePixelArray;

        private void Awake()
        {
            if (damageTexture == null) 
            {
                return;
            }

            damagePixelArray = damageTexture.GetPixels();

            torchkaPPU = torchkaSprite.pixelsPerUnit;
            torchkaPivot = torchkaSprite.pivot;

            var halfCount = totalCount / 2;
            var startShift = totalCount % 2 == 0 ? halfCount - 0.5f : halfCount;

            var currentPos =
                (Vector2)spawnMidPoint.position + spacing * startShift * Vector2.left;
            for (var i = 0; i < totalCount; i++)
            {
                var torchka = Instantiate(torchkaPrefab, currentPos, Quaternion.identity);
                torchka.manager = this;
                currentPos += spacing * Vector2.right;
            }
        }

        public bool CheckForDamage(Texture2D tex, Vector2 contactPosition)
        {
            var coordX = Mathf.RoundToInt(contactPosition.x * torchkaPPU + torchkaPivot.x);
            var coordY = Mathf.RoundToInt(contactPosition.y * torchkaPPU + torchkaPivot.y);

            if (tex.GetPixel(coordX, coordY).a == 0) 
            {
                return false;
            }
            
            var dir = (Random.value > 0.5) ? -1 : 1;
            var startX = coordX + damageTexture.width / 2 * -dir;
            coordY += damageTexture.height / 2 * -dir;
            for (var y = 0; y < damageTexture.height; y++)
            {
                coordX = startX;
                for (var x = 0; x < damageTexture.width; x++)
                {
                    var thisPix = tex.GetPixel(coordX, coordY);
                    thisPix.a *= damagePixelArray[x + y * damageTexture.width].a;
                    tex.SetPixel(coordX, coordY, thisPix);
                    coordX += dir;
                }

                coordY += dir;
            }

            tex.Apply();
            return true;
        }
    }
}