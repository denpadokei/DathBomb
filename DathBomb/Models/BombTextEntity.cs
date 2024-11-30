using UnityEngine;

namespace DathBomb.Models
{
    public class BombTextEntity
    {
        public int Index { get; set; } = -1;
        public int GroupingID { get; set; } = 0;
        public string Text { get; set; } = "";
        public float FontSize { get; set; } = 5f;
        public string ImageName { get; set; } = "";
        public float ImageScale { get; set; } = 0.005f;
        public float ViewTime { get; set; } = 7f;
        public float Interval { get; set; } = 5f;
        public Vector3Wrapper TargetPos { get; set; }
        public BombTextEntity()
        {
            this.TargetPos = new Vector3Wrapper(new Vector3(0f, 1.8f, 4f));
        }
    }
}
