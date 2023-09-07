using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_ParallaxBackground : MonoBehaviour
{
   
   [SerializeField] private Vector2 backgroundOffset;
   [SerializeField] private Vector2 parallaxEffectOld;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX; 
    public GameObject Camera;

   private void Start() {
          cameraTransform = Camera.transform;
          // lastCameraPosition = cameraTransform.position;
          lastCameraPosition = Vector3.zero;
              // Consider using vector3.zero if camera moves
              // causing problems
          transform.position = new Vector3(transform.position.x + backgroundOffset.x, transform.position.y + backgroundOffset.y);
          Sprite sprite = GetComponent<SpriteRenderer>().sprite;
          Texture2D texture = sprite.texture;
          textureUnitSizeX = (texture.width / sprite.pixelsPerUnit);
           
   }

   private void LateUpdate() {
          Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
          transform.position += new Vector3(-(deltaMovement.x) * parallaxEffectOld.x, -(deltaMovement.y) * parallaxEffectOld.y);
          lastCameraPosition = cameraTransform.position;

          if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX) {
             float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
             transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y); 
          }
   }
}
