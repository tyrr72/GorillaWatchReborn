using BepInEx;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilla;
using static NetworkSystem;
using Steamworks;
using Valve.VR;
using UnityEngine.XR.Interaction.Toolkit;
using GorillaWatch.Models;

namespace GorillaWatch
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Main : BaseUnityPlugin
    {
        public static Main instance;
        public bool isDevMode = true;

        // ---------------------------

        public int CurrentMod;

        // ---------------------------

        private GorillaHuntComputer watch;

        // ---------------------------

        private bool lDebounce = false;
        private bool rDebounce = false;
        private bool clickDebounce = false;
        bool IsSteamVR;


        // ---------------------------

        float leftStickX;

        private bool inRoom;

        List<Mod> activeMods = new List<Mod>();
        public void LastPage()
        {
            if (Main.instance.CurrentMod > 0)
            {
                Main.instance.CurrentMod--;
            }
            else
            {
                Main.instance.CurrentMod = Mods.Count - 1;
            }
        }

        public void NextPage()
        {
            if (Main.instance.CurrentMod < Mods.Count - 1)
            {
                Main.instance.CurrentMod++;
            }
            else
            {
                Main.instance.CurrentMod = 0;
            }
        }

        public void Start()
        {
            Utilla.Events.GameInitialized += Init;
            instance = this;
        }

        public List<Mod> Mods = new List<Mod>
        {
            new Mod(false, null, "Welcome to the GorillaWatch Reborn", false, null, null),
            new Mod(true, ModsMethods.Platforms, "Platforms", false, null,null),
            new Mod(true, ModsMethods.Fly, "Fly", false, null, null),
            new Mod(false, null, "Placeholder 3", false, null, null),
            new Mod(false, null, "Placeholder 4", false, null, null),
            new Mod(false, null, "Placeholder 5", false, null, null),
        };

        public void OnGUI()
        {
            if (Keyboard.current.qKey.isPressed)
            {
                GUILayout.Label(watch.text.text);
                if (GUILayout.Button("Last"))
                {
                    LastPage();
                }
                if (GUILayout.Button("Next"))
                {
                    NextPage();
                }
            }
        }


        public void Init(object sender, EventArgs e)
        {
            watch.text.text = "Initializing";
            IsSteamVR = Traverse.Create(PlayFabAuthenticator.instance).Field("platform").GetValue().ToString().ToLower() == "steam";
        }

        public void Update()
        {
            if (inRoom || isDevMode)
            {

                foreach (var v in activeMods)
                {
                    v.method.Invoke();
                }
                if (ControllerInputPoller.instance.leftControllerSecondaryButton)
                {
                    if (!lDebounce)
                    {
                        lDebounce = true;
                        LastPage();
                    }
                }
                else
                {
                    lDebounce = false;
                }

                if (ControllerInputPoller.instance.leftControllerPrimaryButton)
                {
                    if (!rDebounce)
                    {
                        rDebounce = true;
                        NextPage();
                    }
                }
                else
                {
                    rDebounce = false;
                }

                if (ControllerInputPoller.instance.rightControllerPrimaryButton)
                {
                    if (!clickDebounce)
                    {
                        clickDebounce = true;
                        if (Mods[CurrentMod].isToggleable)
                        {
                            Mods[CurrentMod].enabled = !Mods[CurrentMod].enabled;
                            if (Mods[CurrentMod].enabled)
                            {
                                activeMods.Add(Mods[CurrentMod]);
                            }
                            else
                            {
                                activeMods.Remove(Mods[CurrentMod]);
                            }
                        }
                        else
                        {
                            Mods[CurrentMod].enabled = true;
                        }
                    }
                }
                else
                {
                    clickDebounce = false;
                }

                watch = GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>();
                GorillaTagger.Instance.offlineVRRig.EnableHuntWatch(true);
                GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().enabled = false;
                GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().badge.gameObject.SetActive(false);
                GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().leftHand.gameObject.SetActive(false);
                GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().rightHand.gameObject.SetActive(false);
                GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().hat.gameObject.SetActive(false);
                GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().face.gameObject.SetActive(false);

                if (watch != null && Mods != null && Mods.Count > CurrentMod)
                {
                    watch.text.text = Mods[CurrentMod].name;

                    if (Mods[CurrentMod].isToggleable)
                    {
                        watch.material.color = Mods[CurrentMod].enabled ? Color.green : Color.red;
                        if (Mods[CurrentMod].enabled)
                        {
                            Mods[CurrentMod].method.Invoke();
                        }
                    }
                    else
                    {
                        watch.material.color = Color.grey;
                        if (Mods[CurrentMod].enabled)
                        {
                            Mods[CurrentMod].method.Invoke();
                            Mods[CurrentMod].enabled = false;
                        }
                    }
                }

            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.EnableHuntWatch(false);
                GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>().enabled = false;
            }
        }

        [ModdedGamemodeJoin]
        public void Join()
        {
            inRoom = true;
        }
        [ModdedGamemodeLeave]
        public void Leave()
        {
            inRoom = false;

        }
    }
}
