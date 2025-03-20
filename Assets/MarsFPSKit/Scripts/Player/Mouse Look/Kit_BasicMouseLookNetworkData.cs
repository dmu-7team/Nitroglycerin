using Mirror;
using UnityEngine;

namespace MarsFPSKit
{
    public class Kit_BasicMouseLookNetworkData : Kit_MouseLookNetworkBase
    {
        public float mouseX; //Rotation on Unity Y-Axis (Player Object)
        public float mouseY; //Rotation on Unity X-Axis (Camera/Weapons)

        public float recoilMouseX; //Recoil on x axis
        public float recoilMouseY; //Recoil on y axis

        public float finalMouseX; //Rotation on Unity Y-Axis with recoil applied
        [SyncVar]
        public float finalMouseY; //Rotation on Unity X-Axis with recoil applied

        [SyncVar]
        /// <summary>
        /// How are we currently leaning -1 (L) to 1 (R)
        /// </summary>
        public float leaningState;

        public Quaternion leaningSmoothState = Quaternion.identity;

        /// <summary>
        /// Last input of the third person button
        /// </summary>
        public bool lastThirdPersonButton;

        /// <summary>
        /// Were we aiming?
        /// </summary>
        public bool wasAimingLast;

        /// <summary>
        /// 0 = First person pos
        /// 1 = Third person pos
        /// </summary>
        public float firstPersonThirdPersonBlend;

        /// <summary>
        /// Hit for perspective
        /// </summary>
        public RaycastHit perspectiveClippingAvoidmentHit;

        /// <summary>
        /// Hit for crosshair
        /// </summary>
        public RaycastHit worldPositionCrosshair;

        /// <summary>
        /// The currently desired perspective
        /// </summary>
        public Kit_GameInformation.Perspective desiredPerspective = Kit_GameInformation.Perspective.FirstPerson;

        /// <summary>
        /// The currently active perspective
        /// </summary>
        public Kit_GameInformation.Perspective currentPerspective = Kit_GameInformation.Perspective.FirstPerson;
    }
}