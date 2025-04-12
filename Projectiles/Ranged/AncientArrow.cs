using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Projectiles.Ranged
{
	public class AncientArrow : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_41";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			int dustId = Dust.NewDust(Projectile.position, Projectile.width / 2, Projectile.height / 2, 60, Projectile.velocity.X * 0.7f,
				Projectile.velocity.Y * 0.7f, 15, Color.White, 2f);
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
				Color color = new Color(255, 185, 200) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity,
				ModContent.ProjectileType<Projectiles.InfraRedExplosion>(), Projectile.damage / 4, Projectile.knockBack / 2f, Main.myPlayer);

			Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width * 4, Projectile.height * 4, 60, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 60, default(Color), 2f);
			dustId.noGravity = true;
			Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width * 4, Projectile.height * 4, 60, Projectile.velocity.X * 0.4f, Projectile.velocity.Y * 0.4f, 60, default(Color), 2f);
			dustId3.noGravity = true;
		}
	}
}