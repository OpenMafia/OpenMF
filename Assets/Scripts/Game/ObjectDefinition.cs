﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MafiaUnity
{
    public class ObjectDefinition : MonoBehaviour
    {

        [SerializeField] public MafiaFormats.Scene2BINLoader.Object data;

        public void Init()
        {
            bool tempDestroySelf = false;

            switch (data.type)
            {
                case MafiaFormats.Scene2BINLoader.ObjectType.Model:
                {
                    
                }
                break;

                case MafiaFormats.Scene2BINLoader.ObjectType.Lightmap:
                {
                    // TODO
                    GameObject.DestroyImmediate(gameObject, true);
                }
                break;

                case MafiaFormats.Scene2BINLoader.ObjectType.Sector:
                {
                    
                }
                break;

                case MafiaFormats.Scene2BINLoader.ObjectType.Light:
                {
                    //NOTE(zaklaus): Re-parent the light
                    var parent = GameObject.Find(data.lightSectors);

                    if (parent != null)
                    {
                        transform.parent = parent.transform;

                        transform.localPosition = data.pos;
                        transform.localRotation = data.rot;
                        transform.localScale = data.scale;
                    }

                    if (data.lightType != MafiaFormats.Scene2BINLoader.LightType.Directional && data.lightType != MafiaFormats.Scene2BINLoader.LightType.Point)
                        break;

                    var light = gameObject.AddComponent<Light>();

                    light.type = LightType.Point;
                    light.shadows = LightShadows.Soft;

                    if (data.lightType == MafiaFormats.Scene2BINLoader.LightType.Directional)
                    {
                        light.type = LightType.Spot;
                        light.spotAngle = Mathf.Rad2Deg * data.lightAngle;
                    }

                    light.intensity = data.lightPower;
                    light.range = data.lightFar;
                    light.color = new Color(data.lightColour.x, data.lightColour.y, data.lightColour.z);
                }
                break;

                default:
                {
                    tempDestroySelf = true;
                }
                break;
            }

            if (tempDestroySelf && data.specialType == MafiaFormats.Scene2BINLoader.SpecialObjectType.None)
            {
                GameObject.DestroyImmediate(gameObject, true);
            }

            switch (data.specialType)
            {
                case MafiaFormats.Scene2BINLoader.SpecialObjectType.Physical:
                {

                }
                break;
                
                case MafiaFormats.Scene2BINLoader.SpecialObjectType.Door:
                {
                    var meshFilter = GetComponent<MeshFilter>();

                    if (meshFilter != null)
                    {
                        gameObject.AddComponent<MeshCollider>().sharedMesh = meshFilter.sharedMesh;

                        var door = gameObject.AddComponent<Door>();
                        door.door = data.doorObject;
                    }
                }
                break;
            }
        }
    }
}