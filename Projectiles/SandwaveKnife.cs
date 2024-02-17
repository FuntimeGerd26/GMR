using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles
{
	public class SandwaveKnife : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Placeholder";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandwave Knife");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 14;
			AIType = ProjectileID.Bullet;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(125, 225, 0, 0);

		public override void AI()
		{
			if (--Projectile.ai[1] < 0)
			{
				if (Projectile.ai[0] == -1) //no target atm
				{
					if (Projectile.ai[1] % 6 == 0)
					{
						Projectile.rotation += 0.475f;
						Projectile.ai[0] = GerdHelper.FindClosestHostileNPC(Projectile.Center, 6000, true);
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

						float modifier = Math.Min(Projectile.velocity.Length() / 50f, 1f);
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
		}

		public override void Kill(int timeleft)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 64, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = new Color(125, 225, 0, 0);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}