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
			Projectile.width = 400;
			Projectile.height = 6000;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y + (float)Projectile.height) - Vector2.UnitY * 3000, new Vector3(0.75f, 0.15f, 0.5f));
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Projectile.timeLeft < 240)
				Projectile.alpha += 8;

			if (Projectile.alpha >= 255)
				Projectile.Kill();
		}

		DrawLayer IDrawable.DrawLayer => DrawLayer.BeforeProjectiles;
		public void Draw(Color lightColor)
		{
			Texture2D Beam = Request<Texture2D>("GMR/Assets/Images/Beam01", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 BeamOrigin = Beam.Size() * 0.5f;

			Main.EntitySpriteDraw(Beam, Projectile.Center - Main.screenPosition, null, new Color(255, 105, 135) * Projectile.Opacity, Projectile.rotation,
				BeamOrigin, new Vector2(Projectile.scale, Projectile.scale * 20f), SpriteEffects.None, 0);
		}
	}
}