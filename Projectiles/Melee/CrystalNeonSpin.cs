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
	public class CrystalNeonSpin : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Neon Blade");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 284;
			Projectile.height = 284;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.owner == Main.myPlayer && !player.controlUseItem)
			{
				Projectile.Kill();
				return;
			}
			if (Projectile.owner == Main.myPlayer && player.controlUseItem)
			{
				Projectile.timeLeft = 2;
			}

			if (player.dead)
			{
				Projectile.Kill();
				return;
			}

			if (++Projectile.localAI[0] == 120)
			{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center, new Vector2(Projectile.spriteDirection * 35f, 0f), ModContent.ProjectileType<Projectiles.Melee.CrystalNeonThrow>(), Projectile.damage / 2, Projectile.knockBack, Main.myPlayer);
					Projectile.localAI[0] = 0;
			}
			Projectile.position.X = Main.player[Projectile.owner].Center.X - Projectile.width / 2;
			Projectile.position.Y = Main.player[Projectile.owner].Center.Y - Projectile.height / 2;
			Projectile.direction = player.direction;
			Projectile.spriteDirection = Projectile.direction;
			player.heldProj = Projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.reuseDelay = 10;
			Projectile.rotation += Projectile.spriteDirection * 0.45f;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId3].noGravity = true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 playerArmPosition = Main.GetPlayerArmPosition(Projectile);
			playerArmPosition.Y -= Main.player[Projectile.owner].gfxOffY;
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;

			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				int max0 = (int)i - 1;//Math.Max((int)i - 1, 0);
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				float num165 = MathHelper.Lerp(Projectile.oldRot[(int)i], Projectile.oldRot[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), Color.HotPink, num165, origin2, Projectile.scale * 2, spriteEffects, 0);
			}
			return false;
		}
	}
}
