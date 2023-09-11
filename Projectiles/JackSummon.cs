using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.NPCs.Bosses.Jack;

namespace GMR.Projectiles
{
	public class JackSummon : ModProjectile
	{
		public override string Texture => "GMR/Items/Misc/Consumable/JackDroneOld";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Drone");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 240;
			Projectile.light = 0.5f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
			if (++Projectile.ai[0] % 30 == 0)
				Projectile.velocity.Y += -0.5f;
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

		public override void Kill(int timeLeft)
		{
			Player player= Main.player[Projectile.owner];
			if (Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Jack>()))
			{
				SoundEngine.PlaySound(SoundID.Roar, player.position);

				int type = ModContent.NPCType<Jack>();
				int spawnX = (int)player.position.X + player.width / 2;
				int spawnY = (int)player.position.Y + player.height / 2 - 700;
				NPC.NewNPC(player.GetSource_FromThis(), spawnX, spawnY, type, player.whoAmI, -1f, -1f, -1f, -1f);
				Projectile.netUpdate = true;
			}
			int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * 0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId].noGravity = true;
			int dustId3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 60, Projectile.velocity.X * -0.5f,
				Projectile.velocity.Y * 0.2f, 60, default(Color), 2f);
			Main.dust[dustId3].noGravity = true;
		}
	}
}