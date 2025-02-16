using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Attacks;
using System;
using SkeletonsAdventure.ItemLoot;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.Entities
{
    public class Entity : AnimatedSprite
    {
        public int baseDefence, baseAttack, baseHealth, baseXP;
        public int maxHealth, defence, attack, weaponAttack, armourDefence, respawnTime;
        public Texture2D basicAttackTexture;
        public TimeSpan lastDeathTime;
        public bool isDead = false;
        public Vector2 RespawnPosition = Vector2.Zero;
        public string type = string.Empty;
        public static int AttackDelay { get; } = 800;  //length of the delay between attacks in milliseconds
        public AttackManager AttackManager { get; set; }
        public EntityAttack EntityAttack { get; set; }
        public int XP { get; set; } //Xp gained for killing the entity
        public int Health { get; set; }
        public LootList LootList { get; set; }
        public int EntityLevel { get; protected set; }
        public Color BasicAttackColor { get; set; }

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
            type = this.GetType().Name;
            weaponAttack = 0;
            armourDefence = 0;
            AttackManager = new(this);
            Health = baseHealth;
            maxHealth = baseHealth;
            defence = baseDefence;
            attack = baseAttack;
            respawnTime = 3;
            lastDeathTime = new();
            XP = baseXP;
            LootList = new LootList();
            BasicAttackColor = Color.White;
            basicAttackTexture = GameManager.SkeletonAttackTexture;
            EntityAttack = new(basicAttackTexture, this);
        }

        public override void Update(GameTime gameTime)
        {
            AttackManager.Update(gameTime);

            base.Update(gameTime);

            if (CollisionCount > 0)
            {
                IsColliding = true;
                CollisionCount = 0;
            }
            else
                IsColliding = false;

            //TODO
            Info.Text = Health.ToString();
            Info.Text += "\nLevel = " + EntityLevel;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public EntityData GetEntityData()
        {
            return new EntityData()
            {
                type = type,
                baseHealth = baseHealth,
                baseDefence = baseDefence,
                baseAttack = baseAttack,
                currentHealth = Health,
                position = Position,
                respawnPosition = RespawnPosition,
                baseXP = baseXP,
                entityLevel = EntityLevel,
                isDead = isDead,
                LastDeathTime = lastDeathTime,
                Items = LootList.GetLootListItemData()
            };
        }

        public void UpdateEntityData(EntityData entityData)
        {
            type = entityData.type;
            baseHealth = entityData.baseHealth;
            baseAttack = entityData.baseAttack;
            baseDefence = entityData.baseDefence;
            baseXP = entityData.baseXP;
            EntityLevel = entityData.entityLevel;
            Health = entityData.currentHealth;
            isDead = entityData.isDead;
            if (entityData.position != null)
            {
                Position = (Vector2)entityData.position;
            }
            if (entityData.respawnPosition != null)
            {
                RespawnPosition = (Vector2)entityData.respawnPosition;
            }
            if (entityData.LastDeathTime != null)
            {
                lastDeathTime = (TimeSpan)entityData.LastDeathTime;
            }
        }

        public virtual void Respawn()
        {
            Health = maxHealth;
            Position = RespawnPosition;
            Motion = Vector2.Zero;
            isDead = false;
            AttackManager.ClearAttacks();
        }

        public virtual void EntityDied(GameTime gameTime) //TODO change how the timer for dead entities works
        {
            isDead = true;
            lastDeathTime = gameTime.TotalGameTime;
            Motion = Vector2.Zero;
            AttackManager.ClearAttacks();
        }

        public virtual void BasicAttack(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - AttackManager.lastAttackTime > new TimeSpan(0, 0, 0, 0, AttackDelay))
            {
                EntityAttack attack = EntityAttack.Clone();
                attack.StartTime = gameTime.TotalGameTime;
                attack.Offset();
                attack.Position = this.Position + attack.AttackOffset;
                attack.DefaultColor = BasicAttackColor;
                attack.SpriteColor = attack.DefaultColor;
                AttackManager.AddAttack(attack);
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
