using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Bosses
{
	public class JackRain : ModProjectile
	{
		public override string Texture => "GMR/Assets/Images/JackRitual";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack Rain");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.friendly = true;
			Projectile.timeLeft = 360;
			Projectile.light = 0.75f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.scale = 1.1f;
		}

		public override bool? CanDamage()
		{
			return false;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));

			if (++Projectile.ai[1] > 40 && ++Projectile.ai[0] % 15 == 0)
			{
				float numberProjectiles = 2;
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = new Vector2(0f, 2f);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - 1100 * Vector2.UnitY, perturbedSpeed.RotatedByRandom(MathHelper.ToRadians(1 + Main.rand.Next(3))), ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
				}
			}
			Projectile.velocity = Vector2.Zero;

			if (Projectile.scale > 0.5f)
				Projectile.scale = +0.01f;

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.5f, 15, Color.White, 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 85, 5);

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.5f, 15, Color.White, 2f);
			Main.dust[dustId].noGravity = true;
		}
	}
}