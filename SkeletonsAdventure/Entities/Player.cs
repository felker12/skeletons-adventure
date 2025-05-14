using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using RpgLibrary.EntityClasses;
using RpgLibrary.ItemClasses;
using SkeletonsAdventure.Attacks;
using SkeletonsAdventure.Engines;
using SkeletonsAdventure.GameUI;
using SkeletonsAdventure.GameWorld;
using SkeletonsAdventure.ItemClasses;
using Effect = RpgLibrary.ItemClasses.Effect;

namespace SkeletonsAdventure.Entities
{
    public class Player : Entity
    {
        private int bonusAttackFromLevel = 0, bonusDefenceFromLevel = 0, bonusHealthFromLevel = 0;

        public Backpack Backpack { get; set; }
        public EquippedItems EquippedItems { get; set; }
        public int TotalXP { get; set; } = 0;
        public int Mana { get; set; }
        public int BaseMana { get; set; }
        public int MaxMana { get; set; }
        public int StatusPoints { get; set; } = 0;
        public StatusBar ManaBar { get; set; } = new();
        public float XPModifier { get; set; } = 1.0f; //TODO
        public FireBall FireBall { get; set; }
        public FireBall FireBall2 { get; set; }
        public IcePillar IcePillar { get; set; }
        public bool ManaBarVisible { get; set; } = true;
        public bool AimVisible { get; set; } = false;

        public Player() : base()
        {
            baseAttack = 400; //TODO correct the values
            baseDefence = 6;
            baseHealth = 10000;
            TotalXP = 0;

            Initialize(); 
        }

        private void Initialize()
        {
            RespawnTime = 0;
            Health = baseHealth;
            MaxHealth = baseHealth;
            Defence = baseDefence;
            Attack = baseAttack;
            Speed = 6; //TODO

            BaseMana = 10;
            MaxMana = BaseMana;
            Mana = BaseMana;

            Backpack = new();
            EquippedItems = new();

            ManaBar.BarColor = Color.Blue;

            //TODO delete this line
            Info.Color = Color.Aqua;

            FireBall ??= new(GameManager.FireBallData, GameManager.FireBallTexture, this);
            FireBall2 ??= new(GameManager.FireBallData, GameManager.FireBallTexture2, this);
            IcePillar ??= new(GameManager.IcePillarData, GameManager.IcePillarTexture, this);

            //TODO
            FireBall.AnimatedAttack = false;

            //TODO
            HealthBarVisible = false;
            ManaBarVisible = false;
        }
        public void UpdatePlayerData(PlayerData playerData)
        {
            UpdateEntityData(playerData);

            TotalXP = playerData.totalXP;
            BaseMana = playerData.baseMana;
            Mana = playerData.mana;
            MaxMana = playerData.maxMana;
            StatusPoints = playerData.statusPoints;
        }

        public PlayerData GetPlayerData()
        {
            return new(GetEntityData())
            {
                totalXP = TotalXP,
                baseMana = BaseMana,
                mana = Mana,
                maxMana = MaxMana,
                statusPoints = StatusPoints,
                backpack = Backpack.GetBackpackData()
            };
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            if(ManaBarVisible)
                ManaBar.Draw(spriteBatch);

            spriteBatch.DrawRectangle(GetRectangle, SpriteColor, 1, 0); //TODO

            if (AimVisible)
            {
                spriteBatch.DrawLine(GetMousePosition(), GetPlayerCenter(), Color.White, 1);
            }
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePlayerMotion();
            CheckInput(gameTime);

            base.Update(gameTime);

            if(ManaBarVisible)
            {
                HealthBar.Position -= new Vector2(0, ManaBar.Height + ManaBar.BorderWidth + 2);
                ManaBar.UpdateStatusBar(Mana, MaxMana, HealthBar.Position + new Vector2(0, ManaBar.Height + ManaBar.BorderWidth + 2));
            }
            
            Backpack.Update();

            Attack = baseAttack + EquippedItems.EquippedItemsAttackBonus() + bonusAttackFromLevel;
            Defence = baseDefence + EquippedItems.EquippedItemsDefenceBonus() + bonusDefenceFromLevel;
            MaxHealth = baseHealth + bonusHealthFromLevel; //TODO maybe allow gear to provide a health bonus

            //TODO delete this
            Info.Text += $"\nXP = {TotalXP}";
            //Info.Text += $"\nAttack = {Attack}\nDefence = {Defence}";
            //Info.Text += $"\nMotion = {Motion}";
            //Info.Text += "\nFPS = " + (1 / gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void GainXp(int XpGained)
        {
            int currentLevel = GameManager.GetPlayerLevelAtXP(TotalXP);

            TotalXP += (int)(XpGained * XPModifier);

            EntityLevel = GameManager.GetPlayerLevelAtXP(TotalXP);

            if (EntityLevel > currentLevel)
            {
                while(currentLevel < EntityLevel) //perform the levelUp event for every level gained
                {
                    LevelUP();
                    currentLevel++;
                }

                PlayerStatAdjustmentForLevel();
            }
        }

        public void LevelUP() //TODO
        {
            StatusPoints += 2;
            Health = MaxHealth;
            Mana = MaxMana;
        }

        private void PlayerStatAdjustmentForLevel()
        {
            //TODO
            int levelModifier = EntityLevel * 2;

            bonusAttackFromLevel = levelModifier;
            bonusDefenceFromLevel = levelModifier;
            bonusHealthFromLevel = levelModifier * 10;
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

                            if(Health != MaxHealth)
                            {
                                if (Health + consumable.EffectBonus < MaxHealth)
                                    Health += consumable.EffectBonus;
                                else
                                    Health = MaxHealth;

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
            //TODO properly handle controller input
            if (InputHandler.KeyReleased(Keys.E) ||
            InputHandler.ButtonDown(Buttons.RightTrigger, PlayerIndex.One))
            {
                PerformAttack(gameTime, EntityAttack);
            }
            if (InputHandler.KeyReleased(Keys.D1) ||
            InputHandler.ButtonDown(Buttons.RightTrigger, PlayerIndex.One))
            {
                PerformAttack(gameTime, FireBall);
            }
            if (InputHandler.KeyReleased(Keys.D2) ||
            InputHandler.ButtonDown(Buttons.RightTrigger, PlayerIndex.One))
            {
                PerformAimedAttack(gameTime, FireBall2, GetMousePosition());
            }
            if (InputHandler.KeyReleased(Keys.D3) ||
            InputHandler.ButtonDown(Buttons.RightTrigger, PlayerIndex.One))
            {
                PerformPopUpAttack(gameTime, IcePillar, GetMousePosition());
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
            }

            Motion = motion;
        }

        public override void Respawn()
        {
            base.Respawn();
        }

        public virtual void PerformAimedAttack(GameTime gameTime, ShootingAttack entityAttack, Vector2 targetPosition)
        {
            if (AttackingIsOnCoolDown(gameTime) is false && entityAttack.IsOnCooldown(gameTime) is false)
            {
                if (AimVisible == true)
                { 
                    AimVisible = false; 
                    
                    if (Mana < entityAttack.ManaCost)
                        return;
                    else
                        Mana -= entityAttack.ManaCost;

                    entityAttack.SetUpAttack(gameTime, BasicAttackColor, Position);
                    entityAttack.MoveToPosition(targetPosition);

                    AttackManager.AddAttack(entityAttack, gameTime);
                }
                else
                    AimVisible = true;
            }
        }

        public virtual void PerformPopUpAttack(GameTime gameTime, ShootingAttack entityAttack, Vector2 targetPosition)
        {
            if (AttackingIsOnCoolDown(gameTime) is false && entityAttack.IsOnCooldown(gameTime) is false)
            {
                if (AimVisible == true)
                {
                    AimVisible = false;

                    if (Mana < entityAttack.ManaCost)
                        return;
                    else
                        Mana -= entityAttack.ManaCost;

                    entityAttack.SetUpAttack(gameTime, BasicAttackColor, targetPosition);

                    AttackManager.AddAttack(entityAttack, gameTime);
                }
                else
                    AimVisible = true;
            }
        }

        public Vector2 GetPlayerCenter()
        {
            return Position + new Vector2(Width / 2, Height / 2);
        }

        public static Vector2 GetMousePosition()
        {
            MouseState _mouseState = Mouse.GetState();
            return Vector2.Transform(new(_mouseState.X, _mouseState.Y), Matrix.Invert(GameManager.Game.GameScreen.Camera.Transformation));
        }
    }
}
