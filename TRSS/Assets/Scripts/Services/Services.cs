/*************************************************
 * Services.cs
 * 
 * This file contains:
 * - The Services class.
 * - The Debugger class.
 * ***********************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TRSS
{

    #region Class: Services

    /// <summary>
    /// Static class that provides certain helper functions and services.
    /// </summary>
    public static class Services
    {

        #region GameObject Services.

        /// <summary>
        /// Create an empty game object (with optional name).
        /// </summary>
        /// <param name="title">Name of the game object after creation.</param>
        /// <returns>Returns reference to created game object.</returns>
        public static GameObject CreateEmptyObject(string title = "")
        {
            string objectName = "New Empty GameObject";

            if (title.Length > 0)
            {
                objectName = title;
            }

            return new GameObject() { name = objectName };
        }

        /// <summary>
        /// Adds a child to the parent GameObject. Returns reference to the child GameObject.
        /// </summary>
        /// <param name="_parent">Parent GameObject.</param>
        /// <param name="_child">Child GameObject.</param>
        /// <returns>Returns reference to the child GameObject.</returns>
        public static GameObject AddChild(GameObject _parent, GameObject _child)
        {
            // If child is null, return null.
            if (_child != null)
            {
                GameObject parent = _parent;

                // If the parent is null, create an empty game object.
                if (parent == null)
                {
                    parent = CreateEmptyObject(_child.name + "(Parent)");
                }

                // Make the child object a child of the parent object, and, also retain current world space values.
                _child.transform.SetParent(_parent.transform, true);
                return _child;
            }

            // Return null if the child is null.
            return null;
        }

        /// <summary>
        /// Adds a parent to the child GameObject. Returns reference to the parent GameObject.
        /// </summary>
        /// <param name="_parent">Parent GameObject.</param>
        /// <param name="_child">Child GameObject.</param>
        /// <returns>Returns reference to the parent GameObject.</returns>
        public static GameObject AddParent(GameObject _parent, GameObject _child)
        {
            // If parent is null, return null.
            if (_parent != null)
            {
                GameObject child = _child;

                // If the child is null, create an empty game object.
                if (child == null)
                {
                    child = CreateEmptyObject(_parent.name + "(Child)");
                }

                // Make the child object a child of the parent object, and, also retain current world space values.
                child.transform.SetParent(_parent.transform, true);
                return _parent;
            }

            // Return null if the parent is null.
            return null;
        }

        /// <summary>
        /// Create a component as a part of the supplied GameObject, or, on a new, empty GameObject, if none are supplied.
        /// </summary>
        /// <typeparam name="T">Component type to add.</typeparam>
        /// <param name="_parent">Parent GameObject to add component to.</param>
        /// <returns>Returns reference to the component that was created.</returns>
        public static T CreateComponent<T>(GameObject _parent = null) where T : MonoBehaviour
        {
            GameObject parent = _parent;

            if (parent == null)
            {
                // Create empty object with the name of the component (for reference).
                parent = CreateEmptyObject(typeof(T).Name);
            }

            T component = parent.GetComponent<T>();

            if (component == default(T))
            {
                // Add the component to the parent object and return its reference.
                component = parent.AddComponent<T>();
            }

            // Return the component reference.
            return component;
        }

        #endregion

    }

    #endregion

    #region Class: Debugger

    /// <summary>
    /// Static class that prints debug messages to the console.
    /// </summary>
    public static class Debugger
    {
        /// <summary>
        /// Flag used to print to console.
        /// </summary>
        public static bool DEBUG_MODE = true;

        /// <summary>
        /// Print a message (with an optional title) to the debug window if debug mode is on and a true flag is passed.
        /// </summary>
        /// <param name="_message">Message body.</param>
        /// <param name="_title">Title of the message.</param>
        /// <param name="_debug">Debug flag to pass in.</param>
        public static void Print(string _message, string _title = "", bool _debug = true)
        {
            if (_debug && DEBUG_MODE)
            {
                string message = _message;
                if (_title.Length > 0)
                {
                    message = _title + ": " + message;
                }
                Debug.Log(message);
            }
        }
    }

    #endregion

}
