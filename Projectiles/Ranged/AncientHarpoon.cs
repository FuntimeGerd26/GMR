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
	public class AncientHarpoon : ModProjectile
	{
		public override string Texture => "GMR/Items/Weapons/Ranged/Others/AncientHarpoon";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 6;
			Projectile.timeLeft = 600; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			Projectile.velocity.X *= 0.97f;
			if (Projectile.velocity.Y < 0f)
            {
				Projectile.velocity.Y *= 0.97f;
			}

			int size = 40;
			bool collding = Collision.SolidCollision(Projectile.position + new Vector2(size / 2f, size / 2f), Projectile.width - size, Projectile.height - size);
			if (collding)
			{
				Projectile.Kill();
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.PartiallyCrystallized>(), 600);
			target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Rupture>(), 600);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = (Projectile.position - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(lightColor);
			for (int k = 0; k < Projectile.oldPos.Length; k += 2)
			{
				Vector2 drawPos2 = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color2 = new Color(255, 55, 55, 55) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos2, null, color2, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

			for (int i = 0; i < 10; i++)
			{
				Dust dustId = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 60, Main.rand.NextFloat(-12f, 12f), Main.rand.NextFloat(-12f, 12f), 60, default(Color), 1f);
				dustId.noGravity = true;
			}
		}
	}
}