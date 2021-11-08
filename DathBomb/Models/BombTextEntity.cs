using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DathBomb.Models
{
    public class BombTextEntity
    {
        public int Index { get; set; } = -1;
        public int GroupingID { get; set; } = 0;
        public string Text { get; set; } = "";
        public float FontSize { get; set; } = 30f;
        public string ImageName { get; set; } = "";
        public float ViewTime { get; set; } = 7f;
        public Vector3 TargetPos { get; set; }
        public BombTextEntity()
        {
            TargetPos = new Vector3(0f, 1.8f, 4f);
        }
    }
}
