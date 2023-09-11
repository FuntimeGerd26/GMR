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
	public class PrismaticBlade : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Magic/PrismaticBlade";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Blade");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 600; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 9;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 25);

		public override void AI()
		{
			var target = Projectile.FindTargetWithinRange(1000f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 18f, 0.09f);
			}

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 1;
			Color disco = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 91, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 30, disco, 1f);
			Main.dust[dustId].noGravity = true;
		}

		public override void Kill(int timeleft)
		{
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
				Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}