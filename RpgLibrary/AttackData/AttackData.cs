using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.AttackData
{
    public class AttackData
    {
        public int AttackLength { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Vector2 AttackOffset { get; set; }

        public AttackData() { }


        public override string ToString()
        {
            return $"Attack Length: {AttackLength}, " +
                $"Start Time: {StartTime}, " +
                $"Duration: {Duration}, " + 
                $"Attack Offset: {AttackOffset}, ";
        }
    }
}
