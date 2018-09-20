using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class InputAbstraction
{
    public enum Handedness
    {
        LEFT,
        RIGHT
    }

    public enum ButtonAlias
    {
        AXIS_CLICK,
        AXIS_TOUCH
    }

    public enum AxisAlias
    {
        X,
        Y
    }

    private static float m_FireTriggerThreshold = 0.75f;

    public static bool FireControlActive(Handedness hand)
    {
        if (XRSettings.loadedDeviceName == "daydream")
        {
            if (hand == Handedness.LEFT)
                return Input.GetButton("Button2");
            else // right hand
                return Input.GetButton("Button0");
        }
        else
        {
            if (hand == Handedness.LEFT)
                return Input.GetAxis("Axis9") > m_FireTriggerThreshold;
            else // right hand
                return Input.GetAxis("Axis10") > m_FireTriggerThreshold;
        }
    }

    public static string AliasToControlString(ButtonAlias Alias, Handedness hand)
    {
        if (hand == Handedness.LEFT)
        {
            switch (Alias)
            {
                case ButtonAlias.AXIS_CLICK:
                    return "Button8";
                case ButtonAlias.AXIS_TOUCH:
                    return "Button16";
                default:
                    return "";
            }
        }
        else // RIGHT HAND
        {
            switch (Alias)
            {
                case ButtonAlias.AXIS_CLICK:
                    return "Button9";
                case ButtonAlias.AXIS_TOUCH:
                    return "Button17";
                default:
                    return "";
            }
        }
    }

    public static string AliasToControlStringSDKSpecific(AxisAlias Alias, Handedness Hand)
    {
        // For daydream, you hold the controller sideways.
        if (XRSettings.loadedDeviceName == "daydream")
        {
            if (Hand == Handedness.LEFT)
            {
                switch (Alias)
                {
                    case AxisAlias.X:
                        return "Axis2Inv";
                    case AxisAlias.Y:
                        return "Axis1";
                    default:
                        return "";
                }
            }
            else // RIGHT HAND
            {
                switch (Alias)
                {
                    case AxisAlias.X:
                        return "Axis5";
                    case AxisAlias.Y:
                        return "Axis4Inv";
                    default:
                        return "";
                }
            }
        }
        else   // Non-daydream platforms
        {
            return AliasToControlString(Alias, Hand);
        }
    }

    public static string AliasToControlString(AxisAlias Alias, Handedness Hand)
    {
        if (Hand == Handedness.LEFT)
        {
            switch (Alias)
            {
                case AxisAlias.X:
                    return "Axis1";
                case AxisAlias.Y:
                    return "Axis2";
                default:
                    return "";
            }
        }
        else     // RIGHT HAND
        {
            switch (Alias)
            {
                case AxisAlias.X:
                    return "Axis4";
                case AxisAlias.Y:
                    return "Axis5";
                default:
                    return "";
            }
        }
    }

    public static bool GetButton(ButtonAlias alias, Handedness hand)
    {
        return Input.GetButton(AliasToControlString(alias, hand));
    }

    public static bool GetButtonDown(ButtonAlias alias, Handedness hand)
    {
        return Input.GetButtonDown(AliasToControlString(alias, hand));
    }

    public static bool GetButtonUp(ButtonAlias alias, Handedness hand)
    {
        return Input.GetButtonUp(AliasToControlString(alias, hand));
    }

    public static float GetAxis(AxisAlias alias, Handedness hand, bool ignoreSDKSpecific = false)
    {
        if (ignoreSDKSpecific)
            return Input.GetAxis(AliasToControlString(alias, hand));
        else
            return Input.GetAxis(AliasToControlStringSDKSpecific(alias, hand));
    }

    public static Handedness PreferedHand()
    {
        List<XRNodeState> nodeStates = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodeStates);

        bool hasLeft = false;
        bool hasRight = false;
        foreach (XRNodeState nodeState in nodeStates)
        {
            if (nodeState.nodeType == XRNode.LeftHand)
                hasLeft = true;
            if (nodeState.nodeType == XRNode.RightHand)
                hasRight = true;
        }

        if (hasLeft && !hasRight)
            return Handedness.LEFT;
        else if (!hasLeft && hasRight)
            return Handedness.RIGHT;
        else
            return Handedness.RIGHT;
    }
}