using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SkeletonsAdventure.GameWorld;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.GameObjects
{
    public class TeleporterManager()
    {
        private List<Teleporter> Teleporters { get; set; } = [];

        public void Update()
        {
            foreach (var teleporter in Teleporters)
            {
                if (teleporter is null)
                    continue;

                teleporter.Update(World.Player.GetRectangle);

                // Check if the player is within the teleporter's rectangle
                if (teleporter.Intersects(World.Player.GetRectangle))
                {
                    // Logic to handle player interaction with the teleporter
                    if (teleporter.CheckRequirements())
                    {
                        // Logic to handle teleportation, e.g., moving the player to the destination
                        //World.Player.Position = teleporter.Destination; //TODO?


                        System.Diagnostics.Debug.WriteLine(teleporter.ToString());
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var teleporter in Teleporters)
            {
                if (teleporter.IsActive)//TODO
                {
                    // Draw the teleporter
                    teleporter.Draw(spriteBatch);
                }
                else
                {
                    // Optionally draw inactive teleporters differently
                    spriteBatch.DrawRectangle(teleporter.Rectangle, Color.Red, 1, 1);
                }
            }
        }

        public void AddTeleporter(Teleporter teleporter)
        {
            if (teleporter is null)
                throw new ArgumentNullException(nameof(teleporter), "Teleporter cannot be null.");

            Teleporters.Add(teleporter);
        }

        public void RemoveTeleporter(Teleporter teleporter)
        {
            if (teleporter is null)
                throw new ArgumentNullException(nameof(teleporter), "Teleporter cannot be null.");

            Teleporters.Remove(teleporter);
        }

        public void ClearTeleporters()
        {
            Teleporters.Clear();
        }

        public List<Teleporter> GetTeleporters()
        {
            return Teleporters;
        }

        public Teleporter GetTeleporterByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Teleporter name cannot be null or empty.", nameof(name));

            return Teleporters.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<Teleporter> GetActiveTeleporters()
        {
            return [.. Teleporters.Where(t => t.IsActive)];
        }

        public void SetDestinationForAllTeleporters()
        {
            List<string> names = [.. Teleporters.Select(t => t.Name).Distinct()];

            foreach (var teleporter in Teleporters)
            {
                if (teleporter is null)
                    continue;

                //foreach(string name in names)
                //{
                //    if (teleporter.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(teleporter.ToName))
                //    {
                //        var destinationTeleporter = GetTeleporterByName(teleporter.ToName);
                //        if (destinationTeleporter != null)
                //        {
                //            teleporter.Destination = destinationTeleporter.Position;
                //        }
                //    }
                //}

                //TODO add logic to set destination based on ToName
            }
        }
    }
}
