using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Engines;
using SkeletonsAdventure.ItemClasses;
using RpgLibrary.ItemClasses;
using Effect = RpgLibrary.ItemClasses.Effect;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.Entities
{
    public class Player : Entity
    {
        public Backpack Backpack { get; set; }
        public EquippedItems EquippedItems { get; set; }
        public int TotalXP { get; set; } = 0;

        public Player() : base()
        {
            baseAttack = 400; //TODO;
            baseDefence = 6;
            baseHealth = 1000000;

            TotalXP = 0;

            Initialize(); 
        }

        public Player(PlayerData playerData) : base(playerData)
        {
            TotalXP = playerData.totalXP;

            Initialize();
        }

        public void UpdatePlayerData(PlayerData playerData)
        {
            UpdateEntityData(playerData);

            TotalXP = playerData.totalXP;

        }

        public PlayerData GetPlayerData()
        {
            return new PlayerData()
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
                Items = LootList.GetLootListItemData(),

                totalXP = TotalXP,
                backpack = Backpack.GetBackpackData()
            };
        }


        private void Initialize()
        {
            respawnTime = 0;
            Health = baseHealth;
            maxHealth = baseHealth;
            defence = baseDefence;
            attack = baseAttack;
            speed = 6;
            
            Backpack = new();
            EquippedItems = new();

            //TODO delete this line
            Info.Color = Color.Aqua;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePlayerMotion();
            base.Update(gameTime);
            Backpack.Update();
            CheckInput(gameTime);

            Info.Text += "\nXP = " + TotalXP;

            attack = baseAttack + EquippedItems.EquippedItemsAttackBonus();
            defence = baseDefence + EquippedItems.EquippedItemsDefenceBonus();
        }

        public void GainXp(int XpGained)
        {
            int currentLevel = GameManager.GetPlayerLevelAtXP(TotalXP);

            TotalXP += XpGained;
            EntityLevel = GameManager.GetPlayerLevelAtXP(TotalXP);

            if (EntityLevel > currentLevel)
                LevelUP();
        }

        public void LevelUP() //TODO
        {
            System.Diagnostics.Debug.WriteLine($"Leveled Up to Level {EntityLevel}!");
        }

        public void ConsumeItem(GameItem item)
        {
            if (Backpack.ContainsBaseItem(item))
            {
                //TODO
                if(item.BaseItem is Consumable consumable)
                {
                    switch(consumable.Effect)
                    {
                        case Effect.Heal:

                            if(Health != maxHealth)
                            {
                                if (Health + consumable.EffectBonus < maxHealth)
                                    Health += consumable.EffectBonus;
                                else
                                    Health = maxHealth;

                                item.Quantity -= 1;
                                if (item.Quantity == 0)
                                    Backpack.RemoveItem(item);
                            }
                            break;
                        case Effect.DefenceIncrease: //TODO
                            System.Diagnostics.Debug.WriteLine("increase def");
                            break;
                        case Effect.AttackIncrease: //TODO
                            System.Diagnostics.Debug.WriteLine("increase attack");
                            break;
                    }
                }
            }
        }

        public void CheckInput(GameTime gameTime)
        {
            if (InputHandler.KeyReleased(Keys.E) ||
            InputHandler.ButtonDown(Buttons.RightTrigger, PlayerIndex.One))
            {
                BasicAttack(gameTime);
            }
        }

        private void UpdatePlayerMotion()
        {
            Vector2 motion = new();

            if (InputHandler.KeyDown(Keys.W) ||
            InputHandler.ButtonDown(Buttons.LeftThumbstickUp, PlayerIndex.One))
            {
                motion.Y = -1;
            }
            else if (InputHandler.KeyDown(Keys.S) ||
            InputHandler.ButtonDown(Buttons.LeftThumbstickDown, PlayerIndex.One))
            {
                motion.Y = 1;
            }
            if (InputHandler.KeyDown(Keys.A) ||
            InputHandler.ButtonDown(Buttons.LeftThumbstickLeft, PlayerIndex.One))
            {
                motion.X = -1;
            }
            else if (InputHandler.KeyDown(Keys.D) ||
            InputHandler.ButtonDown(Buttons.LeftThumbstickRight, PlayerIndex.One))
            {
                motion.X = 1;
            }

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                motion *= speed;
            }

            Motion = motion;
        }

        public override void Respawn()
        {
            base.Respawn();
        }
    }
}
