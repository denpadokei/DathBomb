using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DathBomb.Models
{
    public class Vector3Wrapper
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public Vector3 ConvertToVector3()
        {
            return new Vector3(this.x, this.y, this.z);
        }
        public Vector3Wrapper(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }
    }
}
