using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlendable
{
    /*
     * each blendable has:
     * - a skinned mesh renderer
     * - blend shapes (anywhere from 2 to 10)
     * - a slider for each blend shape
     * 
     * some blendables have:
     * - one or more "win conditions" (achieved if the player has positioned the sliders correctly)
     * 
     * each blendable can:
     * - allow its blend shapes to be set by the player via its sliders
     * 
     * when the player wants to interact with a blendable,
     * they can exchange mouse control of the camera
     * for the ability to manipulate its sliders via mouse
     * 
     * while manipulating a blendable's sliders:
     * - the player must be able to clearly see all parts of the blendable affected by its blend shapes
     * - the player must be able to clearly see all of the blendable's sliders
     * 
     */
}
