using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour
{
    [SerializeField] private KeyObject.KeyType requiredKey;

   public KeyObject.KeyType GetKeyType()
    {
        return requiredKey;
    }
}
