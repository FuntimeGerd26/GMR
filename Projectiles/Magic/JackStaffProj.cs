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
	public class JackStaffProj : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/Bosses/AcheronSaw";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.aiStyle = 0;
			Projectile.penetrate = 10;
			Projectile.friendly = true;
			Projectile.timeLeft = 2400;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 19;
			AIType = ProjectileID.Bullet;
			Projectile.localNPCHitCooldown = 400;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 55, 55, 55);

		public override void AI()
		{
			Projectile.rotation += 0.045f;

			if (++Projectile.ai[1] == 120)
			{
				Projectile.velocity = -Projectile.velocity;
			}
			else if (++Projectile.ai[1] > 120)
			{
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(0.5f));
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.PartiallyCrystallized>(), 120);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * Projectile.frame; //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture2D13.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			float opacity = Projectile.Opacity / 2;
			var drawOffset = new Vector2(Projectile.width / 2f, Projectile.height / 2f) - Main.screenPosition;

			SpriteEffects spriteEffects = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++)
			{
				Color color26 = new Color(255, 55, 55, 55) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);

				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				Vector2 value4 = Projectile.oldPos[i];
				float num165 = Projectile.oldRot[i];
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(rectangle), color27, num165, origin2, Projectile.scale * 1.5f, spriteEffects, 0);
			}
			return false;
		}
	}
}