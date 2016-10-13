using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AlienWarpDrivePlugin
{

    public class AlienWarpDrivePlugin : PartModule
    {

        [KSPField(guiActive = true)]
        public string orbital_vel = "unknown";

        [KSPField(guiActive = true)]
        public string vessel_heading = "unknown";

        [KSPField(guiActive = true)]
        public string reference_body = "unknown";

        [KSPField(guiActive = true)]
        public string new_heading = "unknown";

        [KSPField(guiActive = true)]
        public string new_heading_speed = "unknown";

        [KSPField(guiActive = true)]
        public string new_speed = "unknown";

        private bool _engaged = false;
        /*
         * This event is active when controlling the vessel with the part.
         */
        [KSPEvent(guiActive = true, guiName = "Activate")]
        public void ActivateEvent()
        {
            ScreenMessages.PostScreenMessage("Clicked Activate", 5.0f, ScreenMessageStyle.UPPER_CENTER);

            // This will hide the Activate event, and show the Deactivate event.
            Events["ActivateEvent"].active = false;
            Events["DeactivateEvent"].active = true;
            ScreenMessages.PostScreenMessage("going on rails");
            _engaged = true;
        }

        /*
         * This event is also active when controlling the vessel with the part. It starts disabled.
         */
        [KSPEvent(guiActive = true, guiName = "Deactivate", active = false)]
        public void DeactivateEvent()
        {
            ScreenMessages.PostScreenMessage("Clicked Deactivate", 5.0f, ScreenMessageStyle.UPPER_CENTER);

            // This will hide the Deactivate event, and show the Activate event.
            Events["ActivateEvent"].active = true;
            Events["DeactivateEvent"].active = false;
            ScreenMessages.PostScreenMessage("Going off rails");
            _engaged = false;
        }

        public override void OnUpdate()
        {
            if (_engaged)
            {
                Vector3d heading = part.transform.up;
                vessel_heading = heading.xzy.ToString();
                double temp1 = heading.y;
                heading.y = heading.z;
                heading.z = temp1;
                Vector3d position = vessel.orbit.pos;

                heading = heading * 1000;
                new_heading = heading.ToString();

                vessel.GoOnRails();
                orbital_vel = vessel.orbit.vel.magnitude.ToString();
                reference_body = vessel.orbit.referenceBody.name.ToString();
                new_heading_speed = heading.magnitude.ToString();
                new_speed = (vessel.orbit.vel + heading).magnitude.ToString();
                vessel.orbit.UpdateFromStateVectors(position, vessel.orbit.vel + heading, vessel.orbit.referenceBody, Planetarium.GetUniversalTime());

                vessel.GoOffRails();
            }
        }

    }
}
