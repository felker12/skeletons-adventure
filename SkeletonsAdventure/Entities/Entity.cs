using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Attacks;
using System;
using SkeletonsAdventure.ItemLoot;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.GameWorld;
using SkeletonsAdventure.Animations;
using System.Collections.Generic;

namespace SkeletonsAdventure.Entities
{
    public class Entity : AnimatedSprite
    {
        public int baseDefence, baseAttack, baseHealth, baseXP, weaponAttack, armourDefence;
        public Texture2D basicAttackTexture;
        public TimeSpan lastDeathTime;
        public Vector2 RespawnPosition = Vector2.Zero;
        public string Type { get; set; } = string.Empty;
        public StatusBar HealthBar = new();

        //TODO load the cooldown length from the entity data instead of being hard coded
        public int AttackCoolDownLength { get; protected set; } = 600; //length of the delay between attacks in milliseconds
        public AttackManager AttackManager { get; set; }
        public EntityAttack EntityAttack { get; set; }
        public int ID { get; protected set; } = 0;
        public int XP { get; set; } //Xp gained for killing the entity
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Defence { get; set; }
        public int Attack { get; set; }
        public int RespawnTime { get; set; } //time in seconds until the entity respawns
        public LootList LootList { get; set; }
        public int EntityLevel { get; protected set; }
        public Color BasicAttackColor { get; set; }
        public bool IsCollidingBoundary { get; set; } = false;
        public bool IsDead { get; set; } = false;
        public List<EntityAttack> AttacksHitBy { get; set; } = [];
        public bool HealthBarVisible { get; set; } = true;
        public TimeSpan LastTimeAttacked { get; set; }

        public Entity() : base()
        {
            baseHealth = 1;
            baseAttack = 1;
            baseDefence = 1;
            baseXP = 1;
            EntityLevel = 0;
            Health = baseHealth;
            Position = new();

            Initialize();
        }

        public Entity(EntityData entityData) : base()
        {
            UpdateEntityData(entityData);
            Initialize();
        }

        private void Initialize()
        {
            Type = this.GetType().Name;
            weaponAttack = 0;
            armourDefence = 0;
            AttackManager = new(this);
            Health = baseHealth;
            MaxHealth = baseHealth;
            Defence = baseDefence;
            Attack = baseAttack;

            RespawnTime = 3;
            lastDeathTime = new();
            XP = baseXP;
            LootList = new LootList();
            BasicAttackColor = Color.White;
            basicAttackTexture = GameManager.SkeletonAttackTexture;
            EntityAttack = new(GameManager.BasicAttackData, basicAttackTexture, this);
        }

        public override void Update(GameTime gameTime)
        {
            AttackManager.Update(gameTime);

            base.Update(gameTime);

            if(AttacksHitBy.Count > 0)
                SpriteColor = Color.Red;
            else
                SpriteColor = DefaultColor;

            Vector2 healthBarOffset = new(HealthBar.Width / 2 - Width / 2, HealthBar.Height + HealthBar.BorderWidth + 4);
            HealthBar.UpdateStatusBar(Health, MaxHealth, Position - healthBarOffset);

            //TODO
            Info.Text += "\nLevel = " + EntityLevel;
            Info.Text += "\nLast Attacked = " + LastTimeAttacked;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(HealthBarVisible)
                HealthBar.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        public EntityData GetEntityData()
        {
            return new EntityData()
            {
                id = ID,
                type = Type,
                baseHealth = baseHealth,
                baseDefence = baseDefence,
                baseAttack = baseAttack,
                currentHealth = Health,
                position = Position,
                respawnPosition = RespawnPosition,
                baseXP = baseXP,
                entityLevel = EntityLevel,
                isDead = IsDead,
                lastDeathTime = lastDeathTime,
                Items = LootList.GetLootListItemData()
            };
        }

        public void UpdateEntityData(EntityData entityData)
        {
            ID = entityData.id;
            Type = entityData.type;
            baseHealth = entityData.baseHealth;
            baseAttack = entityData.baseAttack;
            baseDefence = entityData.baseDefence;
            baseXP = entityData.baseXP;
            EntityLevel = entityData.entityLevel;
            Health = entityData.currentHealth;
            IsDead = entityData.isDead;

            if (entityData.position != null)
                Position = (Vector2)entityData.position;
            if (entityData.respawnPosition != null)
                RespawnPosition = (Vector2)entityData.respawnPosition;
            if (entityData.lastDeathTime != null)
                lastDeathTime = (TimeSpan)entityData.lastDeathTime;
        }

        public virtual void Respawn()
        {
            Health = MaxHealth;
            Position = RespawnPosition;
            Motion = Vector2.Zero;
            IsDead = false;
            AttackManager.ClearAttacks();
        }

        public virtual void EntityDied(GameTime gameTime) //TODO change how the timer for dead entities works
        {
            IsDead = true;
            lastDeathTime = gameTime.TotalGameTime;
            Motion = Vector2.Zero;
            AttackManager.ClearAttacks();
        }

        public virtual void PerformAttack(GameTime gameTime, EntityAttack entityAttack)
        {
            if(AttackingIsOnCoolDown(gameTime) is false && entityAttack.IsOnCooldown(gameTime) is false)
            {
                if(this is Player player)
                {
                    if(player.Mana < entityAttack.ManaCost)
                        return;
                    else
                        player.Mana -= entityAttack.ManaCost;
                }

                //if the attack has speed it will move. If not it will  be stationary
                if (entityAttack.Speed > 0)
                {
                    if (CurrentAnimation == AnimationKey.Up)
                        entityAttack.Motion = new(0, -1);
                    else if (CurrentAnimation == AnimationKey.Down)
                        entityAttack.Motion = new(0, 1);
                    if (CurrentAnimation == AnimationKey.Left)
                        entityAttack.Motion = new(-1, 0);
                    else if (CurrentAnimation == AnimationKey.Right)
                        entityAttack.Motion = new(1, 0);

                    entityAttack.Motion.Normalize();
                    entityAttack.Motion *= entityAttack.Speed;
                }
                entityAttack.SetUpAttack(gameTime, BasicAttackColor);

                AttackManager.AddAttack(entityAttack, gameTime);
            }
        }

        public bool AttackingIsOnCoolDown(GameTime gameTime)
        {
            return ((gameTime.TotalGameTime - AttackManager.LastAttackTime).TotalMilliseconds < AttackCoolDownLength);
        }

        public virtual void CheckIfAttackIsOffCooldown(GameTime gameTime, EntityAttack entityAttack)
        {
            int diffMilliseconds = (int)(gameTime.TotalGameTime - entityAttack.LastAttackTime).TotalMilliseconds;
            if (diffMilliseconds > entityAttack.AttackCoolDownLength)
            {

            }
        }

        public virtual Entity Clone()
        {
            Entity entity = new(GetEntityData())
            {
                Position = Position,
                EntityLevel = this.EntityLevel,
                SpriteColor = this.SpriteColor
            };
            return entity;
        }

        public virtual void PathToPoint(Entity target)
        {
            Vector2 movement = new(0,0);
            Vector2 targetLocation = target.Position;

            if ((int)targetLocation.Y > (int)Position.Y)
                movement.Y = 1;
            else if ((int)targetLocation.Y < (int)Position.Y)
                movement.Y = -1;
            else if ((int)targetLocation.Y == (int)Position.Y)
                movement.Y = 0;
            if ((int)targetLocation.X > (int)Position.X)
                movement.X = 1;
            else if ((int)targetLocation.X < (int)Position.X)
                movement.X = -1;
            else if ((int)targetLocation.X == (int)Position.X)
                movement.X = 0;

            if (movement != Vector2.Zero)
                movement.Normalize();

            Motion = movement;
        }

        public void FaceTarget(Entity target)
        {
            //find the center of target
            Vector2 targetLocation = target.Position;
            targetLocation.X += target.Width / 2;
            targetLocation.Y += target.Height / 2;


            //find the distance to the center of the target
            Vector2 distance = Position - targetLocation;
            distance.X = (int)distance.X;
            distance.Y = (int)distance.Y;

            if (distance.X > -Width/2 && distance.Y >= 0)
            {
                CurrentAnimation = Animations.AnimationKey.Left;
                if (distance.Y > Width * .75)
                    CurrentAnimation = Animations.AnimationKey.Up;
            }
            else if (distance.X <= -Width/2 && distance.Y >= 0)
            {
                CurrentAnimation = Animations.AnimationKey.Right;
                if (distance.Y > Width * .75)
                    CurrentAnimation = Animations.AnimationKey.Up;
            }
            else if (distance.X >= -Width / 4 && distance.Y < 0)
            {
                CurrentAnimation = Animations.AnimationKey.Left;
                if (distance.Y < -Height - Height / 4)
                    CurrentAnimation = Animations.AnimationKey.Down;
            }
            else if (distance.X <= -Width / 2 && distance.Y < 0)
            {
                CurrentAnimation = Animations.AnimationKey.Right;
                if (distance.Y < -Height  -Height / 4)
                    CurrentAnimation = Animations.AnimationKey.Down;
            }
        }
    }
}
