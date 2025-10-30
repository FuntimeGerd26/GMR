using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Magic
{
	public class AncientInfraRedCrate : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 360;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
		}

		float vScale;
		Vector2 cursorPos;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (++Projectile.localAI[0] == 1)
            {
				cursorPos = Main.MouseWorld;
			}
			if (++Projectile.localAI[0] >= 30)
			{
				Projectile.tileCollide = true;
			}

			Projectile.velocity = (cursorPos - Projectile.Center) * 0.25f;
			Projectile.rotation = (Projectile.velocity.X * 0.0314f);
			Projectile.scale = 1f + vScale;

			if (++Projectile.ai[0] % 60 == 0)
			{
				for (int y = 0; y < 4; y++)
				{
					Vector2 projDirection = (1f * Vector2.UnitX).RotatedBy(MathHelper.ToRadians(90f * y)) * 9f;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projDirection, ModContent.ProjectileType<JackBlast>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);

					int dustType = 60;
					for (int i = 0; i < 6; i++)
					{
						Dust dust = Dust.NewDustPerfect(Projectile.Center, dustType, (projDirection.RotatedByRandom(MathHelper.ToRadians(32f)) * 0.5f) * Main.rand.NextFloat(0.9f, 1.25f), 120, Color.White, Main.rand.NextFloat(1.2f, 3.8f));

						dust.noLight = false;
						dust.noGravity = true;
						dust.fadeIn = Main.rand.NextFloat(0.1f, 0.6f);
					}
				}
				vScale = 0.35f;
			}
			if (vScale > 0f)
				vScale -= 0.025f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<Projectiles.InfraRedExplosion>(), damageDone / 4, Projectile.knockBack, Main.myPlayer);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * vScale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

			int dustType = 60;
			for (int i = 0; i < 5; i++)
			{
				Vector2 velocity = Projectile.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
				Dust dust = Dust.NewDustPerfect(Projectile.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

				dust.noLight = false;
				dust.noGravity = true;
				dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
			}
		}
	}

	public class JackBlast : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Bosses/JackBlastBad";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 4;
			AIType = ProjectileID.Bullet;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 85, 5);

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));

			Projectile.velocity += 0.25f / Projectile.MaxUpdates * Vector2.Normalize(Projectile.velocity);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Vector2 origin = texture.Size() / 2f;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.direction == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k]) - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(255, 55, 85, 5)) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			}

			return false;
		}
	}
}