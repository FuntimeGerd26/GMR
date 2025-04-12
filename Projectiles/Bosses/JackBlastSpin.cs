using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.NPCs.Bosses.Acheron;
using GMR.NPCs.Bosses.Jack;

namespace GMR.Projectiles.Bosses
{
	public class JackBlastSpin : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Bosses/AcheronSaw";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack Blast");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 4;
			AIType = ProjectileID.Bullet;
		}

		public override bool? CanDamage()
		{
			if (Projectile.ai[0] >= 120)
				return true;
			else
				return false;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.8f, 0.15f, 0.5f));

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

			if (Projectile.timeLeft == 1080)
			{
				Projectile.velocity = -Projectile.velocity;

				int dustType = 60;
				for (int i = 0; i < 40; i++)
				{
					Vector2 velocityDust = Projectile.velocity + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f));
					Dust dust = Dust.NewDustPerfect(Projectile.Center, dustType, velocityDust, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

					dust.noLight = false;
					dust.noGravity = true;
					dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
				}
			}
			else if (Projectile.timeLeft < 1080)
            {
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(2f));
			}


			if (!NPC.AnyNPCs(ModContent.NPCType<AcheronArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<AcheronArmClaw>()))
				Projectile.timeLeft += -20000;

			if (!NPC.AnyNPCs(ModContent.NPCType<Jack>()) && !NPC.AnyNPCs(ModContent.NPCType<Acheron>()))
			{
				Projectile.Kill();
				return;
			}
		}

        public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = texture.Size() / 2;
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(255, 55, 85, 5)) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}