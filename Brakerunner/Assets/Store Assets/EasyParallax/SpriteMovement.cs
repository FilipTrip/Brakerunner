using UnityEngine;

namespace EasyParallax
{
    /**
 * Moves a sprite along the X axes using a predefined speed
 */
    public class SpriteMovement : MonoBehaviour
    {
        public bool useCameraManager;
        public MovementSpeedType movementSpeedType;

        [Tooltip("Used only if no movement speed type is specified")]
        public float speed = 1f;

        private void Awake()
        {
            if (movementSpeedType)
                speed = movementSpeedType.speed;
        }

        private void Update()
        {
            //Save the current position, so we can edit it
            var newPosition = transform.position;
            //Move the position along the x axis by an amount that depends on the
            //defined speed and the deltaTime, so we can get a framerate independent movement

            if (useCameraManager)
                newPosition.x -= speed * CameraManager.Instance.DeltaPosition.x * 0.04f;
            else
                newPosition.x -= speed * Time.deltaTime * 0.2f;

            //Update our position
            transform.position = newPosition;
        }
    }
}