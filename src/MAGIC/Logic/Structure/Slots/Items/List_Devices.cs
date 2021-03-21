﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Servers.CoC.Core;

namespace BL.Servers.CoC.Logic.Structure.Slots.Items
{
    internal class List_Devices
    {
        internal Devices Devices = new Devices();

        public List_Devices(Device Device)
        {
            this.Devices.Add(Device.Socket.Handle, Device);
        }

        internal void Remove(Device Device)
        {
            if (Device != null)
            {
                if (this.Devices.ContainsKey(Device.Socket.Handle))
                {
                    this.Devices.Remove(Device.Socket.Handle);
                }
                //else
                    //this.Devices.Remove(Device.Socket.Handle);
            }
        }
    }
}
