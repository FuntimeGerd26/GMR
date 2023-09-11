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
	public class JackShuriken : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Bosses/AcheronShuriken";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloy Crate");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.aiStyle = 0;
			Projectile.hostile = true;
			Projectile.timeLeft = 120;
			Projectile.alpha = 0;
			Projectile.light = 0.45f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 55, 25);

		public override bool? CanDamage()
		{
			return false; // Set to false since the projectile will most likely hit the player otherwise
		}

		public override void AI()
		{
			Projectile.rotation += 0.25f;

			if (++Projectile.localAI[0] > 60)
			{
				if (Projectile.velocity.Y > 0f || Projectile.velocity.Y < 0f)
				{
					Projectile.velocity.Y = 0f;
				}
				if (Projectile.velocity.X > 0f || Projectile.velocity.X < 0f)
				{
                    Projectile.velocity.X = 0f;
                }
            }
		}

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

		public override void Kill(int timeleft)
		{
			float numberProjectiles = 4;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = Vector2.UnitX.RotatedBy(2 * Math.PI / numberProjectiles * i) * 2f;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
			}
		}
	}
}