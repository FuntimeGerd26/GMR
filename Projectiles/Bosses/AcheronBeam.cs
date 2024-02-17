using System;
using System.IO;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;
using GMR;

namespace GMR.Projectiles.Bosses
{
	public class AcheronBeam : ModProjectile, IDrawable
	{
		public override string Texture => "GMR/Empty";

		public override void SetStaticDefaults()
		{
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 380;
			Projectile.height = 5680;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.timeLeft = 1200;
			Projectile.light = 1.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Projectile.timeLeft < 240)
				Projectile.alpha += 4;

			if (Projectile.alpha >= 255)
				Projectile.Kill();
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			var offset = Projectile.Size / 2f - Main.screenPosition;
			Color color26 = new Color(255, 105, 135);

			Main.EntitySpriteDraw(texture, Projectile.position + offset, null, color26 * Projectile.Opacity, Projectile.rotation, drawOrigin, new Vector2(Projectile.scale, Projectile.scale * 2f), SpriteEffects.None, 0);
			return false;
		}

		DrawLayer IDrawable.DrawLayer => DrawLayer.BeforeProjectiles;
		public void Draw(Color lightColor)
		{
			Texture2D Beam = Request<Texture2D>("GMR/Assets/Images/Beam01", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 BeamOrigin = Beam.Size() * 0.5f;

			Main.EntitySpriteDraw(
				Beam,
				Projectile.Center - Main.screenPosition,
				null,
				new Color(255, 105, 135) * Projectile.Opacity,
				Projectile.rotation,
				BeamOrigin,
				new Vector2(Projectile.scale, Projectile.scale * 16f),
				SpriteEffects.None,
				0
				);
		}
	}
}