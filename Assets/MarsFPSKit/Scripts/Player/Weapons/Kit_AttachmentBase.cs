using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace MarsFPSKit
{
    namespace Weapons
    {
        public abstract class Kit_AttachmentBase : ScriptableObject
        {
            public LocalizedString displayName;
        }
    }
}