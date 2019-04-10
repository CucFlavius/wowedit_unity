using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Assets.World.Models;
using UnityEditor;
using Assets.Data.WoW_Format_Parsers.WMO;

namespace Assets.World.Tests
{
    public class Test : MonoBehaviour
    {
        public WMOhandler wmoHandler;
        public M2handler m2Handler;

        // Use this for initialization
        void Start()
        {
            Invoke("Delayed", 1.0f);
        }

        void Delayed()
        {
            //wmoHandler.AddToQueue(@"world\wmo\azeroth\buildings\duskwood_barn\duskwood_barn.wmo", -1, Vector3.zero, Quaternion.identity, Vector3.one);
            //wmoHandler.AddToQueue(@"world\wmo\azeroth\buildings\stormwind\stormwind2_016.wmo", -1, Vector3.zero, Quaternion.identity, Vector3.one);
            try
            {
                //wmoHandler.AddToQueue(@"world\wmo\KulTiras\Human\8ara_warfronts_mine01.wmo", -1, Vector3.zero, Quaternion.identity, Vector3.one);
                //M2.Load(@"character\draenei\female\draeneifemale.m2", -1, Vector3.zero, Quaternion.identity, Vector3.one);
                //M2.Load(@"world\expansion07\doodads\riverzone\8riv_ivy_b01.m2", -1, new Vector3 (.5f,0,0), Quaternion.identity, new Vector3(.1f,.1f,.1f));
                //M2.Load(@"creature\anduin\anduin.m2", -1, Vector3.zero, Quaternion.identity, Vector3.one);
                //M2.Load(@"creature\\ammunae\\ammunae.m2", -1, Vector3.zero, Quaternion.identity, Vector3.one);
                // DB2.Read("battlepetabilityeffect.db2");
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }
}
