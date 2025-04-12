using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using GMR.NPCs.Bosses.Acheron;
using GMR.NPCs.Bosses.Jack;
using GMR.NPCs.Bosses.Jack.Eternity;
using GMR.NPCs.Bosses.Superboss;

namespace GMR.Projectiles.Bosses
{
	// Not much of an orb now but whatever
	public class AcheronOrb : ModProjectile
	{
		public override string Texture => "GMR/Projectiles/GlowSprite";

		public Vector2? ForcedTargetPosition { get; set; }

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.AddElement(0);
			Projectile.AddElement(2);
		}

		public override void SetDefaults()
		{
			Projectile.width = 5;
			Projectile.height = 5;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.timeLeft = 900;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (--Projectile.timeLeft < 840)
			{
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Rectangle hitbox = Projectile.Hitbox;

					int maxDistance = 2400;

					Rectangle areaCheck;

					Player player = Main.player[i];

					if (ForcedTargetPosition is Vector2 target)
						areaCheck = new Rectangle((int)target.X - maxDistance, (int)target.Y - maxDistance, maxDistance * 2, maxDistance * 2);
					else if (player.active && !player.dead && !player.ghost)
						areaCheck = new Rectangle((int)player.position.X - maxDistance, (int)player.position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
					else
						continue;  // Not a valid player


					Vector2 targetPlayer = player.Center;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(targetPlayer - Projectile.Center) * 7.5f, 0.09f);
				}
			}
			else
				Projectile.velocity = Projectile.velocity;

			if (Projectile.timeLeft < 120)
			{
				Projectile.alpha += 16;
				Projectile.velocity *= 0.95f;
			}

			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}

			Projectile.rotation = Projectile.velocity.ToRotation();

			Lighting.AddLight(Projectile.Center, new Vector3(0.75f, 0.15f, 0.5f));

			if (!NPC.AnyNPCs(ModContent.NPCType<Jack>()) && !NPC.AnyNPCs(ModContent.NPCType<Acheron>()) && !NPC.AnyNPCs(ModContent.NPCType<TheAmalgamation>()) && !NPC.AnyNPCs(ModContent.NPCType<JackE>()))
			{
				Projectile.Kill();
				return;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin2 = texture2D13.Size() / 2f;

			Color color26 = new Color(255, 55, 125) * Projectile.Opacity;

			SpriteEffects spriteEffects = SpriteEffects.None;

			// Main Projectile
			for (float i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i += 0.5f)
			{
				Color color27 = color26;
				color27.A = 0;
				color27 *= (float)(ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / ProjectileID.Sets.TrailCacheLength[Projectile.type];
				int max0 = (int)i - 1;
				if (max0 < 0)
					continue;
				Vector2 value4 = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[max0], 1 - i % 1);
				Main.EntitySpriteDraw(texture2D13, value4 + Projectile.Size / 2f - Main.screenPosition + new Vector2(0, Projectile.gfxOffY),
					null, color27, Projectile.rotation, origin2, Projectile.scale * 0.1f, spriteEffects, 0);
			}
			return false;
		}
	}
}