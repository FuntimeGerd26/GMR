using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Melee
{
	public class GerdHeroSwordHoming : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/GerdHeroSword";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hero Training Sword");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 70;
			Projectile.height = 70;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 1200;
			Projectile.light = 0.25f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 4;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			if (--Projectile.ai[1] < 0)
			{
				if (Projectile.ai[0] == -1) //no target atm
				{
					if (Projectile.ai[1] % 6 == 0)
					{
						Projectile.rotation += 0.475f;
						Projectile.ai[0] = GerdHelper.FindClosestHostileNPC(Projectile.Center, 2000, true);
						Projectile.netUpdate = true;
						if (Projectile.ai[0] == -1)
						{
							Projectile.timeLeft = 1200;
						}
					}
				}
				else //currently have target
				{
					NPC npc = Main.npc[(int)Projectile.ai[0]];

					if (npc.active && npc.CanBeChasedBy()) //target is still valid
					{
						Vector2 distance = npc.Center - Projectile.Center;
						double angle = distance.ToRotation() - Projectile.velocity.ToRotation();
						if (angle > Math.PI)
							angle -= 2.0 * Math.PI;
						if (angle < -Math.PI)
							angle += 2.0 * Math.PI;

						float modifier = Math.Min(Projectile.velocity.Length() / 25f, 5f);
						Projectile.velocity = Projectile.velocity.RotatedBy(angle * modifier);
					}
					else //target lost, reset
					{
						Projectile.ai[0] = -1;
						Projectile.netUpdate = true;
					}
				}
			}

			if (Projectile.ai[1] < 0)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MathHelper.Lerp(Projectile.velocity.Length(), 25f, 0.02f);

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

			if (Projectile.localAI[0] < ProjectileID.Sets.TrailCacheLength[Projectile.type])
				Projectile.localAI[0] += 0.1f;
			else
				Projectile.localAI[0] = ProjectileID.Sets.TrailCacheLength[Projectile.type];

			Projectile.localAI[1] += 0.25f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 1;
			Player player = Main.player[Projectile.owner];
			player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 300);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId3].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = Color.Green;
			color26.A = 55;

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, spriteEffects, 0);
			}
			return false;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}
}