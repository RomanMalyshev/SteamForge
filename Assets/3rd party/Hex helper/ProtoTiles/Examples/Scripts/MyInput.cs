﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RedBjorn.ProtoTiles.Example
{
    public class MyInput
    {
        public class FrameInfo
        {
            public int Frame;
            public GameObject OverObject;
            public Vector3 CameraGroundPosition;
        }

        static FrameInfo LastFrame = new FrameInfo();

        static void Validate(Plane plane)
        {
            if (LastFrame.Frame != Time.frameCount)
            {
                LastFrame.Frame = Time.frameCount;
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
                {
                    LastFrame.OverObject = hit.collider.gameObject;
                }
                else
                {
                    LastFrame.OverObject = null;
                }
                var screemCenterRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                float enter = 0f;
                if (plane.Raycast(screemCenterRay, out enter))
                {
                    LastFrame.CameraGroundPosition = screemCenterRay.GetPoint(enter);
                }
                else
                {
                    LastFrame.CameraGroundPosition = Vector3.zero;
                }
            }
        }

        public static bool GetOnWorldDownFree(Plane plane)
        {
            Validate(plane);
            return Input.GetMouseButtonDown(0);
        }

        public static bool GetOnWorldUpFree(Plane plane)
        {
            Validate(plane);
            return Input.GetMouseButtonUp(0);
        }

        public static bool GetOnWorldUp(Plane plane)
        {
            Validate(plane);
            return GetOnWorldUpFree(plane) && !CameraController.IsMovingByPlayer;
        }

        public static bool GetOnWorldFree(Plane plane)
        {
            Validate(plane);
            return Input.GetMouseButton(0);
        }

        public static Vector3 CameraGroundPosition(Plane plane)
        {
            Validate(plane);
            return LastFrame.CameraGroundPosition;
        }

        public static Vector3? GroundPosition(Plane plane)
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0f;
            if (plane.Raycast(mouseRay, out enter) && !IsPointerOverUIObject())
            {
                return mouseRay.GetPoint(enter);
            }
            
         
            return null;
        }
        private static bool IsPointerOverUIObject() {
            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
 
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        public static Vector3 GroundPositionCameraOffset(Plane plane)
        {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0f;
            if (plane.Raycast(mouseRay, out enter))
            {
                return mouseRay.GetPoint(enter) - Camera.main.transform.position;
            }
            return Vector3.zero;
        }
    }
}

