using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace GMR
{
    // Disclaimer: I have no idea how this works, All code using this is possible thanks to Pellucid Mod


    /// <summary>Allows a <see cref="ModProjectile"/> to draw stuff with <see cref="BlendState.Additive"/>.</summary>
    public interface IDrawable
    {
        /// <summary>
        /// Allows you to return a custom snapshot to overwrite the spritebatch state.
        /// </summary>
        /// <param name="snapshot">A snapshot containing the current <see cref="SpriteBatch"/> state.</param>
        /// <returns>A snapshot to set the spritebatch state before calling <see cref="Draw"/>.</returns>
        /// <remarks>The default interface implementation of this method returns the same snapshot with <see cref="BlendState.Additive"/>.</remarks>
        /// The <paramref name="snapshot"/> provided is the spritebatch's current state without changes.<br />
        SpriteBatchSnapshot ModifySpritebatchState(SpriteBatchSnapshot snapshot) => snapshot with { BlendState = BlendState.Additive };

        /// <summary>
        /// 
        /// </summary>
        public DrawLayer DrawLayer { get; }

        /// <summary>
        /// Called in <see cref="DrawAdditiveGlobalProjectile.PostDraw(Projectile, Color)"/>.<br />
        /// Before calling this, the spritebatch is ended and begun with <see cref="BlendState.Additive"/> (unless <br/> 
        /// a custom <see cref="SpriteBatchSnapshot"/> is provided). <br />
        /// After the call to this method the spritebatch ends and begins again.
        /// </summary>
        void Draw(Color lightColor);

        public static void Draw(IDrawable drawAdditive, Color lightColor)
        {
            var snapshit = Main.spriteBatch.CaptureSnapshot();
            Main.spriteBatch.End();
            var newSnapshit = drawAdditive.ModifySpritebatchState(snapshit);
            Main.spriteBatch.Begin(in newSnapshit);

            drawAdditive.Draw(lightColor);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(snapshit); // restore the spritebatch to how it was before
        }
    }

    public enum DrawLayer
    {
        BeforeProjectiles,
        AfterProjectiles,
        BeforeNPCs,
        AfterNPCs,
        BeforeTiles
    }

    public class DrawableLoader : ILoadable
    {
        public void Load(Mod mod)
        {
            
        }

        public void Unload()
        {
            
        }
    }

    public class DrawAdditiveGlobalProjectile : GlobalProjectile
    {
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            if (projectile.ModProjectile is IDrawable drawAdditive)
            {
                IDrawable.Draw(drawAdditive, lightColor);
            }
        }
    }

    public class DrawAdditiveGlobalNPC : GlobalNPC
    {
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.ModNPC is IDrawable drawAdditive)
            {
                IDrawable.Draw(drawAdditive, drawColor);
            }
        }
    }
}
