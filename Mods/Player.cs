﻿using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using GorillaLocomotion.Climbing;
using OpiumWare.Classes;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using static OpiumWare.Menu.Main;

namespace OpiumWare.Mods
{
    internal class Player
    {
        public static void Ghost()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton || Mouse.current.rightButton.isPressed)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void NoClip()
        {
            bool disableColliders2 = ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f;
            MeshCollider[] colliders = Resources.FindObjectsOfTypeAll<MeshCollider>();

            foreach (MeshCollider collider in colliders)
            {
                collider.enabled = !disableColliders2;
            }
        }

        public static void Invis()
        {
            if (ControllerInputPoller.instance.rightControllerSecondaryButton)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = new Vector3(0f, 0f, 0f);
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void FixHead()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.x = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 0f;
        }

        public static void UpsideDownHead()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 180f;
        }

        public static void BrokenNeck()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 90f;
        }

        public static void BackwardsHead()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 180f;
        }

        public static void SpinHeadX()
        {
            if (GorillaTagger.Instance.offlineVRRig.enabled)
            {
                GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.x += 10f;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(10f, 0f, 0f));
            }
        }

        public static void SpinHeadY()
        {
            if (GorillaTagger.Instance.offlineVRRig.enabled)
            {
                GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y += 10f;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
            }
        }

        public static void SpinHeadZ()
        {
            if (GorillaTagger.Instance.offlineVRRig.enabled)
            {
                GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z += 10f;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation.eulerAngles + new Vector3(0f, 0f, 10f));
            }
        }

        public static void GrabRig()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
                GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            }
            else
            {
                if (ControllerInputPoller.instance.leftGrab)
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                }
                else GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void RigGunMod()
        {
            if (ControllerInputPoller.instance.rightGrab || Mouse.current.rightButton.isPressed)
            {
                Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out var hitinfo);

                if (Mouse.current.rightButton.isPressed)
                {
                    Camera cam = GameObject.Find("Shoulder Camera").GetComponent<Camera>();
                    Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
                    Physics.Raycast(ray, out hitinfo, 100);
                }

                GunSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GunSphere.transform.position = hitinfo.point;
                GunSphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                GunSphere.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                GunSphere.GetComponent<Renderer>().material.color = Color.white;
                GameObject.Destroy(GunSphere.GetComponent<BoxCollider>());
                GameObject.Destroy(GunSphere.GetComponent<Rigidbody>());
                GameObject.Destroy(GunSphere.GetComponent<Collider>());

                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f || Mouse.current.leftButton.isPressed)
                {
                    GameObject.Destroy(GunSphere, Time.deltaTime);
                    GunSphere.GetComponent<Renderer>().material.color = GorillaTagger.Instance.offlineVRRig.playerColor;

                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                    GorillaTagger.Instance.offlineVRRig.transform.position = GunSphere.transform.position + new Vector3(0f, 1f, 0f);
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = true;
                }
            }
            if (GunSphere != null)
            {
                GameObject.Destroy(GunSphere, Time.deltaTime);
            }
        }
        private static GameObject GunSphere;

        public static void TPGun()
        {
            if (ControllerInputPoller.instance.rightGrab || UnityInput.Current.GetMouseButton(1))
            {
                Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out var hitinfo);

                if (Mouse.current.rightButton.isPressed)
                {
                    Camera cam = GameObject.Find("Shoulder Camera").GetComponent<Camera>();
                    Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
                    Physics.Raycast(ray, out hitinfo, 100);
                }

                GunSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GunSphere.transform.position = hitinfo.point;
                GunSphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                GunSphere.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                GunSphere.GetComponent<Renderer>().material.color = Color.white;
                GameObject.Destroy(GunSphere.GetComponent<BoxCollider>());
                GameObject.Destroy(GunSphere.GetComponent<Rigidbody>());
                GameObject.Destroy(GunSphere.GetComponent<Collider>());

                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f || UnityInput.Current.GetMouseButton(0))
                {
                    GameObject.Destroy(GunSphere, Time.deltaTime);
                    GunSphere.GetComponent<Renderer>().material.color = GorillaTagger.Instance.offlineVRRig.playerColor;

                    GorillaLocomotion.Player.Instance.transform.position = GunSphere.transform.position;
                }

            }
            if (GunSphere != null)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                GameObject.Destroy(GunSphere, Time.deltaTime);
            }
        }

        public static void StickLongArms()
        {
            GorillaLocomotion.Player.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.leftHandTransform.position + (GorillaTagger.Instance.leftHandTransform.forward * (armlength - 0.917f));
            GorillaLocomotion.Player.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.rightHandTransform.position + (GorillaTagger.Instance.rightHandTransform.forward * (armlength - 0.917f));
        }

        public static void EnableSteamLongArms()
        {
            GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(armlength, armlength, armlength);
        }

        public static void DisableSteamLongArms()
        {
            GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public static void MultipliedLongArms()
        {
            GorillaLocomotion.Player.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position) * armlength;
            GorillaLocomotion.Player.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - (GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position) * armlength;
        }

        public static void VerticalLongArms()
        {
            Vector3 lefty = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position;
            lefty.y *= armlength;
            Vector3 righty = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position;
            righty.y *= armlength;
            GorillaLocomotion.Player.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - lefty;
            GorillaLocomotion.Player.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - righty;
        }

        public static void HorizontalLongArms()
        {
            Vector3 lefty = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.leftHandTransform.position;
            lefty.x *= armlength;
            lefty.z *= armlength;
            Vector3 righty = GorillaTagger.Instance.headCollider.transform.position - GorillaTagger.Instance.rightHandTransform.position;
            righty.x *= armlength;
            righty.z *= armlength;
            GorillaLocomotion.Player.Instance.leftControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - lefty;
            GorillaLocomotion.Player.Instance.rightControllerTransform.transform.position = GorillaTagger.Instance.headCollider.transform.position - righty;
        }

        public static void FixHandTaps()
        {
            GorillaTagger.Instance.handTapVolume = 0.1f;
            Patches.HandTapPatch.doPatch = false;
            Patches.HandTapPatch.tapsEnabled = true;
            Patches.HandTapPatch.doOverride = false;
            Patches.HandTapPatch.overrideVolume = 0.1f;
            Patches.HandTapPatch.tapMultiplier = 1;
        }

        public static void LoudHandTaps()
        {
            Patches.HandTapPatch.doPatch = true;
            Patches.HandTapPatch.tapsEnabled = true;
            Patches.HandTapPatch.doOverride = true;
            Patches.HandTapPatch.overrideVolume = 99999f;
            Patches.HandTapPatch.tapMultiplier = 10;
            GorillaTagger.Instance.handTapVolume = 99999f;
        }

        public static void SilentHandTaps()
        {
            Patches.HandTapPatch.doPatch = true;
            Patches.HandTapPatch.tapsEnabled = false;
            Patches.HandTapPatch.doOverride = false;
            Patches.HandTapPatch.overrideVolume = 0f;
            Patches.HandTapPatch.tapMultiplier = 0;
            GorillaTagger.Instance.handTapVolume = 0;
        }

        public static void SilentHandTapsOnTag()
        {
            if (PlayerIsTagged(GorillaTagger.Instance.offlineVRRig))
            {
                SilentHandTaps();
            }
            else
            {
                FixHandTaps();
            }

        }

        public static void EnableSlipperyHands()
        {
            EverythingSlippery = true;
        }

        public static void DisableSlipperyHands()
        {
            EverythingSlippery = false;
        }

        public static void EnableGrippyHands()
        {
            EverythingGrippy = true;
        }

        public static void DisableGrippyHands()
        {
            EverythingGrippy = false;
        }

        public static GameObject stickpart = null;
        public static void StickyHands()
        {
            if (stickpart == null)
            {
                stickpart = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                FixStickyColliders(stickpart);
                stickpart.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                stickpart.GetComponent<Renderer>().enabled = false;
            }
            if (GorillaLocomotion.Player.Instance.IsHandTouching(true))
                stickpart.transform.position = TrueLeftHand().position;

            if (GorillaLocomotion.Player.Instance.IsHandTouching(false))
                stickpart.transform.position = TrueRightHand().position;

            if (GorillaLocomotion.Player.Instance.IsHandTouching(true) && GorillaLocomotion.Player.Instance.IsHandTouching(false))
                stickpart.transform.position = Vector3.zero;
        }

        public static void DisableStickyHands()
        {
            if (stickpart != null)
            {
                UnityEngine.Object.Destroy(stickpart);
                stickpart = null;
            }
        }

        private static bool leftisclimbing = false;
        private static bool rightisclimbing = false;
        private static GameObject climb = null;
        public static void ClimbyHands()
        {
            if (climb == null)
            {
                climb = new GameObject("GR");
                climb.AddComponent<GorillaClimbable>();
            }
            if (leftGrab)
            {
                if (GorillaLocomotion.Player.Instance.IsHandTouching(true) && !leftisclimbing)
                {
                    climb.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    leftisclimbing = true;
                    GorillaLocomotion.Player.Instance.BeginClimbing(climb.AddComponent<GorillaClimbable>(), GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/LeftHand Controller/GorillaHandClimber").GetComponent<GorillaHandClimber>());
                }
            }
            else
            {
                leftisclimbing = false;
            }
            if (rightGrab)
            {
                if (GorillaLocomotion.Player.Instance.IsHandTouching(false) && !rightisclimbing)
                {
                    climb.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    rightisclimbing = true;
                    GorillaLocomotion.Player.Instance.BeginClimbing(climb.AddComponent<GorillaClimbable>(), GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/RightHand Controller/GorillaHandClimber").GetComponent<GorillaHandClimber>());
                }
            }
            else
            {
                rightisclimbing = false;
            }
        }

        public static void DisableClimbyHands()
        {
            if (climb != null)
            {
                UnityEngine.Object.Destroy(climb);
                climb = null;
            }
        }

        public static void StrobeColor()
        {
            if (Time.time > colorChangerDelay)
            {
                colorChangerDelay = Time.time + 0.1f;
                strobeColor = !strobeColor;
                ChangeColor(new Color(strobeColor ? 1 : 0, strobeColor ? 1 : 0, strobeColor ? 1 : 0));
            }
        }

        public static void RainbowColor()
        {
            if (Time.time > colorChangerDelay)
            {
                colorChangerDelay = Time.time + 0.1f;
                float h = (Time.frameCount / 180f) % 1f;
                ChangeColor(UnityEngine.Color.HSVToRGB(h, 1f, 1f));
            }
        }
    }
} 