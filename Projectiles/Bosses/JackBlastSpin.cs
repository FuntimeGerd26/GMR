using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.NPCs.Bosses.Jack;

namespace GMR.Projectiles.Bosses
{
	public class JackBlastSpin : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Bosses/AcheronSaw";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack Blast");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.aiStyle = 0;
			Projectile.hostile = true;
			Projectile.timeLeft = 120000;
			Projectile.penetrate = -1;
			Projectile.alpha = 25;
			Projectile.light = 0.5f;
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

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 55, 25);

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

			if (++Projectile.ai[0] == 120)
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
			else if (Projectile.ai[0] > 120)
            {
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(2f));
			}


			if (!NPC.AnyNPCs(ModContent.NPCType<AcheronArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<AcheronArmClaw>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmClaw>()))
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