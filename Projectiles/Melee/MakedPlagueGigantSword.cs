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
	public class MaskedPlagueGigantSword : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/Swords/MaskedPlagueSword";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masked Plague's Sword");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(2);
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 88;
			Projectile.height = 88;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.penetrate = 3;
			Projectile.light = 0.25f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 14;
			AIType = ProjectileID.Bullet;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(0, 255, 0, 0);

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 61, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * -0.7f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 61, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * -0.7f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeleft)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 61, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * -0.7f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
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
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale * 2, spriteEffects, 0);
			}
			return false;
		}
	}
}