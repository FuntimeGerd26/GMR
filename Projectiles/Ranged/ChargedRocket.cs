using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
	public class ChargedRocket : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Rocket");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 60;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.RocketI;
		}

		public override void AI()
		{
			if (--Projectile.ai[1] < 0)
			{
				if (Projectile.ai[0] == -1) //no target atm
				{
					if (Projectile.ai[1] % 6 == 0)
					{
						Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
						Projectile.ai[0] = GerdHelper.FindClosestHostileNPC(Projectile.Center, 3000, true);
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

						float modifier = Math.Min(Projectile.velocity.Length() / 20f, 10f);
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
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MathHelper.Lerp(Projectile.velocity.Length(), 18f, 0.02f);

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

			if (Projectile.localAI[0] < ProjectileID.Sets.TrailCacheLength[Projectile.type])
				Projectile.localAI[0] += 0.1f;
			else
				Projectile.localAI[0] = ProjectileID.Sets.TrailCacheLength[Projectile.type];

			Projectile.localAI[1] += 0.25f;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.1f, ModContent.ProjectileType<Projectiles.Explotion>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
			target.AddBuff(BuffID.OnFire3, 60);
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
			int dustId2 = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 186, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * -0.7f, 15, Color.White, 2f);
			Main.dust[dustId2].noGravity = true;
			int dustId4 = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 186, Projectile.velocity.X * -0.7f,
				Projectile.velocity.Y * -0.7f, 15, Color.White, 2f);
			Main.dust[dustId4].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = lightColor;
			color26 = Projectile.GetAlpha(color26);

			SpriteEffects effects = SpriteEffects.None;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 2f)
			{
				Color color27 = color26;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale, effects, 0);
			}

			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Projectile.GetAlpha(lightColor), Projectile.rotation, origin2, Projectile.scale, effects, 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 0.1f, ModContent.ProjectileType<Projectiles.Explotion>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);
			Projectile.width = Projectile.height = 22;
			int dustId = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 186, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 186, Projectile.velocity.X * -0.7f,
				Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
			Main.dust[dustId3].noGravity = true;
		}
	}
}