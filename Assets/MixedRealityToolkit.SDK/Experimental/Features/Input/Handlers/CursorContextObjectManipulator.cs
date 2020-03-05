// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.Experimental.UI
{
    /// <summary>
    /// This script provides cursor context for the object manipulator
    /// </summary>
    [AddComponentMenu("Scripts/MRTK/SDK/CursorContextObjectManipulator")]
    [RequireComponent(typeof(ObjectManipulator))]
    public class CursorContextObjectManipulator : MonoBehaviour
    {
        private ObjectManipulator objectManipulator;
        private CursorContextInfo contextInfo;
        private int hoverCount = 0;

        private void Start()
        {
            objectManipulator = GetComponent<ObjectManipulator>();

            contextInfo = gameObject.EnsureComponent<CursorContextInfo>();
            contextInfo.ObjectCenter = transform;

            objectManipulator.OnHoverEntered.AddListener(ManipulatorHoverEntered);
            objectManipulator.OnHoverExited.AddListener(ManipulatorHoverExited);
            objectManipulator.OnManipulationStarted.AddListener(ManipulationStarted);
            objectManipulator.OnManipulationEnded.AddListener(ManipulationEnded);
        }

        private void ManipulatorHoverEntered(ManipulationEventData manipEvent)
        {
            hoverCount++;
            if (hoverCount >= 2 &&
                objectManipulator.ManipulationType != ManipulationHandFlags.OneHanded)
            {
                contextInfo.CurrentCursorAction = CursorContextInfo.CursorAction.Move;
            }
        }

        private void ManipulatorHoverExited(ManipulationEventData manipEvent)
        {
            hoverCount--;
            if (hoverCount < 2)
            {
                contextInfo.CurrentCursorAction = CursorContextInfo.CursorAction.None;
            }
        }

        private void ManipulationStarted(ManipulationEventData manipEvent)
        {
            contextInfo.CurrentCursorAction = CursorContextInfo.CursorAction.Move;
        }

        private void ManipulationEnded(ManipulationEventData manipEvent)
        {
            contextInfo.CurrentCursorAction = hoverCount < 2 ?
                CursorContextInfo.CursorAction.None :
                CursorContextInfo.CursorAction.Move;
        }
    }
}
