using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgLibrary.EntityClasses
{
    internal class EnemyData : EntityData
    {

        public EnemyData()
        {

        }

        public EnemyData(EnemyData data) : base(data)
        {

        }

        public EnemyData(EntityData entityData) : base(entityData)
        {

        }


    }
}
