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
	public class GerdHeroSwordSpin : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Melee/GerdHeroSword";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(3);
		}

		public override void SetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 300;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
		}

		float Angle;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.25f, 1f, 0.45f));

			Player player = Main.player[Projectile.owner];

			Angle += 4f;

			Projectile.Center = player.MountedCenter + Vector2.One.RotatedBy(MathHelper.ToRadians(player.direction * Angle) + (Math.PI / 4) * (Projectile.ai[0] + 1)) * 64f;

			Vector2 toPlayer = player.MountedCenter - Projectile.Center;
			Projectile.rotation = toPlayer.ToRotation() + MathHelper.ToRadians(225f);

			if (++Projectile.ai[2] == 300)
            {
				Projectile.NewProjectileDirect(player.GetSource_FromThis(), Projectile.Center, (Vector2.UnitX * -6f).RotatedBy(Projectile.rotation - MathHelper.ToRadians(255f)),
					ModContent.ProjectileType<GerdHeroSwordHoming>(), Projectile.damage * 2, Projectile.knockBack, Main.myPlayer);
				Projectile.Kill();
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			player.AddBuff(ModContent.BuffType<Buffs.Buff.Empowered>(), 180);

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 1f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 1.5f);
			Main.dust[dustId3].noGravity = true;
		}

		public override void Kill(int timeLeft)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 1f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 1.5f);
			Main.dust[dustId3].noGravity = true;

			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			SoundEngine.PlaySound(SoundID.Item28, Projectile.position);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;

			Color color26 = new Color(125, 205, 125);

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
	}
}