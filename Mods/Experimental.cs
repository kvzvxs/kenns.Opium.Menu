﻿using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using ExitGames.Client.Photon;
using GorillaNetworking;
using System.Text;
using System.IO;
using HarmonyLib;
using Fusion;
using CjLib;

namespace OpiumWare.Mods
{
    internal class Experimental
    {
        public static void EnableFPSBoost()
        {
            QualitySettings.globalTextureMipmapLimit = 99999;
        }

        public static void DisableFPSBoost()
        {
            QualitySettings.globalTextureMipmapLimit = 1;
        }

        private static float setLevelDelay = 0f;
        public static void SetMaxLevel()
        {
            if (Time.time > setLevelDelay)
            {
                setLevelDelay = Time.time + 1f;
                GorillaTagger.Instance.offlineVRRig.SetQuestScore(99999);
            }
        }

        public static void Draw()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                draworb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                UnityEngine.Object.Destroy(draworb.GetComponent<SphereCollider>());
                draworb.GetComponent<Renderer>().material.color = Color.magenta;
                draworb.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                draworb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                draworb.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                draworb.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                PhotonNetwork.RaiseEvent(69, new object[2] { draworb.transform.position, draworb.transform.rotation }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
            }
        }

        private static GameObject draworb;

        private static bool autoclickstate = false;
        public static void AutoClicker()
        {
            autoclickstate = !autoclickstate;
            if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f)
            {
                ControllerInputPoller.instance.leftControllerIndexFloat = autoclickstate ? 1f : 0f;

                GorillaTagger.Instance.offlineVRRig.leftHand.calcT = autoclickstate ? 1f : 0f;
                GorillaTagger.Instance.offlineVRRig.leftHand.LerpFinger(1f, false);
            }
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
            {
                ControllerInputPoller.instance.rightControllerIndexFloat = autoclickstate ? 1f : 0f;

                GorillaTagger.Instance.offlineVRRig.rightHand.calcT = autoclickstate ? 1f : 0f;
                GorillaTagger.Instance.offlineVRRig.rightHand.LerpFinger(1f, false);
            }
        }

        public static void UnlockAll()
        {
            // Made By @L_nar_yt

            {
                foreach (CosmeticsController.CosmeticItem cosmeticItem in CosmeticsController.instance.allCosmetics)
                {
                    CosmeticsController.instance.ProcessExternalUnlock(cosmeticItem.itemName, false, false);
                }
                CosmeticsController.instance.UpdateMyCosmetics();
                CosmeticsController.instance.UpdateWardrobeModelsAndButtons();
                Action onCosmeticsUpdated = CosmeticsController.instance.OnCosmeticsUpdated;
                bool flag2 = onCosmeticsUpdated != null;
                if (flag2)
                {
                    onCosmeticsUpdated();
                }

            }
        }
    }
}