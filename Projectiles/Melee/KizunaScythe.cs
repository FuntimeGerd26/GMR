using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Projectiles.Melee
{
	public class KizunaScythe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kizuna Scythe");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 88;
			Projectile.height = 90;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 4;
			Projectile.timeLeft = 600; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
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
							Projectile.timeLeft = 600;
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

						float modifier = Math.Min(Projectile.velocity.Length() / 100f, 1f);
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
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MathHelper.Lerp(Projectile.velocity.Length(), 24f, 0.02f);

			Projectile.rotation += 0.475f;

			if (Projectile.localAI[0] < ProjectileID.Sets.TrailCacheLength[Projectile.type])
				Projectile.localAI[0] += 0.1f;
			else
				Projectile.localAI[0] = ProjectileID.Sets.TrailCacheLength[Projectile.type];

			Projectile.localAI[1] += 0.25f;

			int dustId = Dust.NewDust(Projectile.position, Projectile.width,Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId3].noGravity = true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.damage = Projectile.damage + (Projectile.damage / 20);
			target.AddBuff(BuffID.Electrified, 600);
			target.immune[Projectile.owner] = 1;
		}

		public override void Kill(int timeleft)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath14, Projectile.Center);
			Projectile.position = Projectile.Center;
			Projectile.width = Projectile.height = 144;
			Projectile.position.X -= (float)(Projectile.width / 2);
			Projectile.position.Y -= (float)(Projectile.height / 2);
			for (int index = 0; index < 2; ++index)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, 0.0f, 0.0f, 100, new Color(), 1.5f);
			for (int index1 = 0; index1 < 20; ++index1)
			{
				int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, 0.0f, 0.0f, 0, new Color(), 2.5f);
				Main.dust[index2].noGravity = true;
				Dust dust1 = Main.dust[index2];
				dust1.velocity = dust1.velocity * 3f;
				int index3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, 0.0f, 0.0f, 100, new Color(), 1.5f);
				Dust dust2 = Main.dust[index3];
				dust2.velocity = dust2.velocity * 2f;
				Main.dust[index3].noGravity = true;
			}

			if (Projectile.penetrate >= 0)
			{
				Projectile.penetrate = -1;
				Projectile.Damage();
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
	}
}