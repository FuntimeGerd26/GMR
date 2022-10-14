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
	public class VoidScythe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Scythe");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 1200; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.rotation += 0.25f;
			Projectile.velocity -= 0.1f / Projectile.MaxUpdates * Vector2.Normalize(Projectile.velocity);
			Projectile.localAI[1] = 1f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			// 60 frames = 1 second
			target.AddBuff(BuffID.Frostburn, 900);
			target.AddBuff(BuffID.Weak, 600);
		}

		public override void Kill(int timeleft)
		{
			Projectile.position = Projectile.Center;
			Projectile.width = Projectile.height = 28;
			Projectile.position.X -= (float)(Projectile.width / 2);
			Projectile.position.Y -= (float)(Projectile.height / 2);
			for (int index = 0; index < 2; ++index)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, 0.0f, 0.0f, 100, new Color(), 1.5f);
			for (int index1 = 0; index1 < 20; ++index1)
			{
				int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, 0.0f, 0.0f, 0, new Color(), 2.5f);
				Main.dust[index2].noGravity = true;
				Dust dust1 = Main.dust[index2];
				dust1.velocity = dust1.velocity * 3f;
				int index3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 68, 0.0f, 0.0f, 100, new Color(), 1.5f);
				Dust dust2 = Main.dust[index3];
				dust2.velocity = dust2.velocity * 2f;
				Main.dust[index3].noGravity = true;
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