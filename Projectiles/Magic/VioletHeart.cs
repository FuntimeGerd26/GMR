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
	public class VioletHeart : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Violet Heart");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 600;
			Projectile.light = 0.25f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
		{
			var target = Projectile.FindTargetWithinRange(625f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 12f, 0.09f);
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 21, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 21, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Glimmering>(), 1200);
		}

		public override void Kill(int timeleft)
		{
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 21, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 21, default(Color), 2f);
			Main.dust[dustId].noGravity = true;

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
				Color color = Color.White;
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}