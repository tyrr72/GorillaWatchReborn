using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaWatch
{
    public class ModsMethods : MonoBehaviour
    {
        public static GameObject leftplat;
        public static GameObject rightplat;
        public static void Platforms()
        {
            Vector3 leftOffset = new Vector3(0f, -0.06f, 0f);
            Vector3 rightOffset = new Vector3(0f, -0.06f, 0f);

            Color playerColor = GorillaTagger.Instance.offlineVRRig.mainSkin.material.color;

            if (ControllerInputPoller.instance.leftGrab)
            {
                if (leftplat == null)
                {
                    leftplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftplat.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                    leftplat.transform.position = GorillaTagger.Instance.leftHandTransform.position + leftOffset;
                    leftplat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    leftplat.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                    leftplat.GetComponent<Renderer>().material.color = playerColor;
                }
            }
            else
            {
                if (leftplat != null)
                {
                    GameObject.Destroy(leftplat);
                    leftplat = null;
                }
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (rightplat == null)
                {
                    rightplat = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightplat.transform.localScale = new Vector3(0.02f, 0.270f, 0.353f);
                    rightplat.transform.position = GorillaTagger.Instance.rightHandTransform.position + rightOffset;
                    rightplat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    rightplat.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                    rightplat.GetComponent<Renderer>().material.color = playerColor;
                }
            }
            else
            {
                if (rightplat != null)
                {
                    GameObject.Destroy(rightplat);
                    rightplat = null;
                }
            }
        }
        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat >= 0.3)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.transform.forward * 1.2f;
            }
        }
        
    }
}
