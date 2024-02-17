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
	public class AluminiumBolt : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/PlaceholderProjectile";

		public override void SetStaticDefaults()
		{
			Projectile.AddElement(0);
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1200;
			Projectile.light = 0.5f; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
			var target = Projectile.FindTargetWithinRange(400f);
			if (target != null)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(target.Center - Projectile.Center) * 18f, 0.09f);
			}

			for (float i = 0; i < 10; i++)
			{
				Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width - 4, Projectile.height - 4, 63, 1f, 1f, 120, default(Color), 0.6f);
				dustId.velocity *= -0.5f;
				dustId.noGravity = true;
			}
			Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 63, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 60, default(Color), 0.5f);
			dustId3.noGravity = false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 63, -Projectile.velocity.X * 0.5f, -Projectile.velocity.Y * 0.5f, 60, default(Color), 1.5f);
			dustId.noGravity = false;
			Dust dustId3 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 63, -Projectile.velocity.X * 0.5f, -Projectile.velocity.Y * 0.5f, 60, default(Color), 1.5f);
			dustId3.noGravity = false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;
			Main.EntitySpriteDraw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.White, Projectile.rotation, origin2, Projectile.scale / 2f, SpriteEffects.None, 0);
			return false;
		}
	}
}