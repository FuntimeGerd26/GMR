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
	public class AlloyCrate : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alloy Crate");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = 1;
			Projectile.hostile = true;
			Projectile.timeLeft = 125;
			Projectile.alpha = 0;
			Projectile.light = 0.45f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

			if (Projectile.localAI[0] > 121)
			{

			}
			else if (++Projectile.localAI[0] > 120)
            {
				SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.position);
				float numberProjectiles = 4; //3 shots
				float rotation = MathHelper.ToRadians(180);
				Vector2 velocity = new Vector2(0f, 20f);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * 1f;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
				}
			}
			else if (++Projectile.localAI[0] > 20)
			{
				if (Projectile.velocity.Y > 0)
				{
					Projectile.velocity.Y += -1f;
				}
				if (Projectile.velocity.X > 0)
				{
					Projectile.velocity.X += -1f;
				}
				if (Projectile.velocity.Y < 0)
				{
					Projectile.velocity.Y += 1f;
				}
				if (Projectile.velocity.X < 0)
				{
					Projectile.velocity.X += 1f;
				}
				if (Projectile.velocity.Y == 0)
				{
					Projectile.velocity.Y = 0f;
				}
				if (Projectile.velocity.X == 0)
				{
					Projectile.velocity.X = 0f;
				}
			}
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = -Projectile.velocity + 0.5f / Projectile.MaxUpdates * Vector2.Normalize(Projectile.velocity);
			return false;
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
	}
}