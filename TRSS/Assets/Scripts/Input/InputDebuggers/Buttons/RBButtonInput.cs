﻿/*************************************************
 * RBButtonInput.cs
 * 
 * This file contains:
 * - The RBButtonInput class.
 * ***********************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TRSS.Input
{

    /// <summary>
    /// An XInput button input, based off of the controller input component.
    /// </summary>
    public class RBButtonInput : ControllerInput
    {

        #region Trigger Keys.

        const string RB = "Right Bumper Button";

        #endregion

        #region Trigger Initialization.

        /// <summary>
        /// Create input scheme triggers here.
        /// </summary>
        public override void InitializeTriggers()
        {
            this.Scheme.AddTrigger(RB, Controls.RB, ControlResponse.HELD);
        }

        #endregion

        #region Handle Input.

        /// <summary>
        /// Handle input scheme triggers here.
        /// </summary>
        public override void HandleInput()
        {
            Trigger button = this.Scheme.GetTrigger(RB);
            float value = 0.0f;

            if (button.GetState())
            {
                value = 1.0f;
            }

            this.Display.UpdateDisplay(value);

            if (value != 0.0f)
            {
                this.AddMessage("The Right bumper is being held down.");
            }
        }

        #endregion

    }
}