﻿using RpgLibrary.EntityClasses;

namespace SkeletonsAdventure.Entities
{
    internal class Skeleton : Enemy
    {
        public Skeleton(EntityData entityData) : base(entityData)
        {
            EnemyType = EnemyType.Skeleton;
        }

        public override Skeleton Clone()
        {
            Skeleton skeleton = new(GetEntityData())
            {
                Position = Position,
                GuaranteedDrops = GuaranteedDrops,
                SpriteColor = this.SpriteColor,
                DefaultColor = this.DefaultColor,
           };
            return skeleton;
        }
    }
}
