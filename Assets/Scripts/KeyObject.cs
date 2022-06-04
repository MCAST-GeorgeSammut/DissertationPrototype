using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject :MonoBehaviour
{
    //this allows setting of key type from editor
    [SerializeField] private KeyType keyType;
    public enum KeyType {
        Yellow, 
        Green, 
        Red
    };

    public KeyType GetKeyType()
    {
        return keyType;
    }
}