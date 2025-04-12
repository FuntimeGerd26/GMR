using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;
using GMR;

#region Mod Items

using GMR.Items.Misc;
using GMR.Items.Misc.Materials;
using GMR.Items.Misc.Consumable;
using GMR.Items.Weapons.Melee;
using GMR.Items.Weapons.Melee.Swords;
using GMR.Items.Weapons.Melee.Spears;
using GMR.Items.Weapons.Melee.Others;
using GMR.Items.Weapons.Ranged;
using GMR.Items.Weapons.Ranged.Bows;
using GMR.Items.Weapons.Ranged.Guns;
using GMR.Items.Weapons.Ranged.Others;
using GMR.Items.Weapons.Ranged.Railcannons;
using GMR.Items.Weapons.Magic;
using GMR.Items.Weapons.Magic.Books;
using GMR.Items.Weapons.Magic.Staffs;
using GMR.Items.Weapons.Magic.Others;

#endregion

namespace GMR.NPCs.Bosses.Jack
{
	[AutoloadBossHead()]
	public class Jack : ModNPC
	{
		public bool PlayAnimation;
		public bool PlayerDead;
		public bool HalfHealthSummon;
		public bool Alive;
		public int AI4;
		public int AI5;
		public int AI6;
		public int AI7;
		public int AI8;
		public bool IncrAtkTime;
		public bool Alivent;
		public int ExplodeTimer;
		public int ShouldDie;

		private const float DEATHTIME = MathHelper.PiOver4 * 134;

		public static int MinionType()
		{
			return ModContent.NPCType<JackArmGun>();
			return ModContent.NPCType<JackArmClaw>();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jack");
			NPCID.Sets.DontDoHardmodeScaling[Type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			NPCID.Sets.BossBestiaryPriority.Add(Type);
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				PortraitScale = 1f,
				PortraitPositionYOverride = 1.2f,
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			Main.npcFrameCount[Type] = 13;
			NPC.AddElement(0);
			NPC.AddElement(2);
		}

		public override void SetDefaults()
		{
			NPC.width = 110;
			NPC.height = 122;
			NPC.lifeMax = 3850;
			NPC.defense = 20;
			NPC.HitSound = SoundID.NPCHit42;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.knockBackResist = 0f;
			NPC.damage = 25;
			NPC.aiStyle = -1;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(gold: 7);
			NPC.boss = true;
			NPC.npcSlots = 10f;
			if (!Main.dedServ)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Bosses/Jack");
			}
			NPC.ElementMultipliers([1f, 0.5f, 0.8f, 1.5f]);
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return false; // Set to false because fuck contact damage
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
			BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
			new FlavorTextBestiaryInfoElement("An ancient machine that was never finished, it has a really bad temper for this reason." +
			"It launched an attack on it's creators for never bothring finishing it."),
			});
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			int dustType = 60;
			if (!Main.dayTime)
				dustType = 64;
			for (int i = 0; i < 10; i++)
			{
				Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f));
				Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

				dust.noLight = false;
				dust.noGravity = true;
				dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
			}

			if (NPC.life <= 0)
			{
				for (int i = 0; i < 40; i++)
				{
					Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-10f, 10f));
					Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 120, Color.White, Main.rand.NextFloat(1.6f, 3.8f));

					dust.noLight = false;
					dust.noGravity = true;
					dust.fadeIn = Main.rand.NextFloat(0.1f, 0.5f);
				}
			}
		}

		float deadcheck;
		public override bool CheckDead()
		{
			if (deadcheck == -3)
			{
				NPC.lifeMax = -33333;
				return true;
			}
			deadcheck = -3;
			NPC.velocity = new Vector2(0f, 0f);
			NPC.dontTakeDamage = true;
			NPC.life = NPC.lifeMax;
			Alivent = true;
			return false;
		}
		
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * (0.5f * (3 - balance) + (numPlayers * 0.1f)));
			NPC.damage = (int)(NPC.damage * (0.5f * (3 - balance)));
		}

		public override void AI()
		{
			if (Main.dayTime)
				Lighting.AddLight(NPC.Center, new Vector3(0.8f, 0.15f, 0.5f));
			else
				Lighting.AddLight(NPC.Center, new Vector3(0.8f, 0.8f, 0.15f));

			Player player = Main.player[NPC.target];

			if (Alivent)
			{
				NPC.alpha++;

				if (++ShouldDie == 180)
					player.ApplyDamageToNPC(NPC, NPC.lifeMax * 2, 0, 0, false);

				if (++ExplodeTimer % 15 == 0)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X + Main.rand.Next(-NPC.width / 2, NPC.width / 2), NPC.Center.Y + Main.rand.Next(-NPC.height / 2, NPC.height / 2)),
						Vector2.Zero, ModContent.ProjectileType<Projectiles.SmallExplosion>(), 0, 0f, Main.myPlayer, NPC.whoAmI);
					SoundEngine.PlaySound(SoundID.Item62, NPC.Center);
				}
			}

			// Make sure to target a player
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
				NPC.netUpdate = true;
			}

			#region Arm & More Spawn Managing

			if (!Alivent && NPC.life <= NPC.lifeMax / 2 && !HalfHealthSummon && Main.masterMode) // When under 50% HP, Spawn this NPC
			{
				int spawnX = (int)NPC.position.X + NPC.width / 2;
				int spawnY = (int)NPC.position.Y + NPC.height / 2 - 300;
				for (int i = 0; i < 1; i++)
				{
					NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX, spawnY, ModContent.NPCType<JackDrone>(), NPC.whoAmI, 0f, 0f);
				}
				HalfHealthSummon = true; // Keep false to Spawn an NPC every frame
				NPC.netUpdate = true;
			}

			// Become vulnerable if no arms are alive or Jack is under 10% of max HP
			if (!Alivent && !NPC.AnyNPCs(ModContent.NPCType<JackArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmClaw>()) || NPC.life < (int)(NPC.lifeMax * 0.1))
			{
				NPC.dontTakeDamage = false;
				NPC.netUpdate = true;
			}
			else
			{
				NPC.dontTakeDamage = true;
				NPC.netUpdate = true;
			}

			if (NPC.dontTakeDamage == false) // Arms alive or not under 10% of max HP
			{
				// After 15 seconds, Spawn arms
				if (++NPC.ai[3] == 900)
				{
					NPC.ai[2] = 0; // Spawn Arms, Look below
				}
				// Become less aggressive when arms are live
				if (!Main.dayTime || NPC.life < (int)(NPC.lifeMax * 0.1))
					IncrAtkTime = false;
				else
					IncrAtkTime = true;
				NPC.netUpdate = true;
			}
			else // No arms alive or under 10% of max HP
			{
				NPC.ai[3] = 0; // Reset countdown for a delay in spawning arms
				NPC.ai[2] = 2; // Any number higher than 0 works
				IncrAtkTime = true;
				NPC.netUpdate = true;
			}

			// Spawn Arms
			if (!Alivent && NPC.ai[2] == 0)
			{
				NPC.ai[2]++;
				int count = 1;
				int spawnX = (int)player.position.X + player.width / 2;
				int spawnY = (int)player.position.Y + player.height / 2;
				for (int i = 0; i < count; i++)
				{
					NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX + 1000 * 2, spawnY, ModContent.NPCType<JackArmGun>(), NPC.whoAmI, 0f, 0f, 5f);
					NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX + 1000 * 2, spawnY, ModContent.NPCType<JackArmClaw>(), NPC.whoAmI, 0f, 0f, 5f);
					NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX - 1000 * 2, spawnY, ModContent.NPCType<JackArmGun>(), NPC.whoAmI, 0f, 0f, -5f);
					NPC.NewNPC(NPC.GetSource_FromThis("GMR/NPCs/Jack"), spawnX - 1000 * 2, spawnY, ModContent.NPCType<JackArmClaw>(), NPC.whoAmI, 0f, 0f, -5f);
				}
				NPC.netUpdate = true;
			}

			#endregion

			// Fall into the void if all players that it can target are dead
			if (Main.player[NPC.target].dead && !Main.player[NPC.target].active)
			{
				NPC.netUpdate = true;
				NPC.velocity.Y += 7f;
				NPC.EncourageDespawn(300);
				if (!PlayerDead)
				{
					Main.NewText("Objective Clear. " + NPC.life + " shield integrity remaining. Required maintenance.", Color.Red);
					PlayerDead = true;
				}
				return;
			}
			else
			{
				Movement();
				NPC.netUpdate = true;
			}

			if (!Alivent && NPC.dontTakeDamage == false && ++AI7 >= 60)
			{
				SideAttack();
				NPC.netUpdate = true;
			}

			if (!Alivent && NPC.life < (int)(NPC.lifeMax * 0.45) && ++AI4 % (IncrAtkTime ? 420 : 300) == 0)
			{
				WallAttack();
				NPC.netUpdate = true;
			}

			// Making it so it does not trigger while the arms are alive, it's chaotic enough already
			if (!Alivent && !NPC.AnyNPCs(ModContent.NPCType<JackArmGun>()) && !NPC.AnyNPCs(ModContent.NPCType<JackArmClaw>()) && NPC.life < (int)(NPC.lifeMax * 0.75) && Main.expertMode && ++AI6 % 540 == 0)
			{
				RainAttack();
				NPC.netUpdate = true;
			}

			if (!Alivent && NPC.dontTakeDamage == false && NPC.life > (int)(NPC.lifeMax * 0.1) && ++AI5 % (IncrAtkTime ? 420 : 300) == 0)
			{
				int max = 10;
				for (int i = 0; i < max; i++)
				{
					Projectile.NewProjectile(NPC.GetSource_FromThis(), player.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max * (i + 0.5)) * 12f,
					ModContent.ProjectileType<Projectiles.Bosses.JackBlastRotate>(), NPC.damage, 1f, Main.myPlayer);
					SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
					NPC.netUpdate = true;
				}
			}
		}

		private void Movement()
		{
			Player player = Main.player[NPC.target];
			Vector2 bossShouldMoveTo = player.Center - 150 * Vector2.UnitY;

			if (NPC.ai[3] <= 0)
			{
				bossShouldMoveTo = player.Center - 240 * Vector2.UnitY;
			}

			NPC.velocity = (bossShouldMoveTo - NPC.Center) * 0.055f;
		}

		private void SideAttack()
		{
			Player player = Main.player[NPC.target];

			if (++AI8 % 60 == 0 && Main.expertMode)
			{
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(35f);
				for (int y = 0; y < numberProjectiles; y++)
				{
					Vector2 projDirection = (NPC.DirectionTo(player.Center)).RotatedBy(MathHelper.Lerp(-rotation, rotation, y / (numberProjectiles - 1))) * 8f;
					Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, projDirection, ModContent.ProjectileType<Projectiles.Bosses.JackShuriken>(), NPC.damage, 1f, Main.myPlayer);
					SoundEngine.PlaySound(SoundID.Item74, NPC.Center);
					if (AI8 >= 240)
						AI7 = 0;
					NPC.netUpdate = true;
				}
			}
		}

		private void WallAttack()
		{
			Player player = Main.player[NPC.target];
			player.GetModPlayer<GerdPlayer>().ShakeScreen(2, 0.50f);

			SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream"), NPC.Center);
			float x = 1000f;
			float y = 2000f;
			int amount = 24;
			var posX = player.position.X + player.width / 2f;
			var posY = player.position.Y + player.height / 2f - y / 2f;
			float yAdd = y / (amount / 2);
			int type = ModContent.ProjectileType<Projectiles.Bosses.JackBlastBad>();
			for (int i = 0; i < amount; i++)
			{
				Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX + x, posY + yAdd * i), new Vector2(-3f, 0f), type, NPC.damage, 1f, Main.myPlayer);
				Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX - x, posY + yAdd * i), new Vector2(3f, 0f), type, NPC.damage, 1f, Main.myPlayer);
				SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
				NPC.netUpdate = true;
			}
		}

		private void RainAttack()
		{
			Player player = Main.player[NPC.target];

			if (Main.masterMode)
			{
				player.GetModPlayer<GerdPlayer>().ShakeScreen(3, 0.5f);
				int max = 8;
				for (int i = 0; i < max; i++)
				{
					Projectile.NewProjectile(NPC.GetSource_FromThis(), player.Center, Vector2.UnitX.RotatedBy(2 * Math.PI / max * (i + 0.5)) * 4f,
					ModContent.ProjectileType<Projectiles.Bosses.JackBlastFlip>(), NPC.damage, 1f, Main.myPlayer);
					SoundEngine.PlaySound(new SoundStyle($"{nameof(GMR)}/Sounds/NPCs/acheronscream"), NPC.Center);
					NPC.netUpdate = true;
				}
			}

			//Projectile.NewProjectile(NPC.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Bosses.JackRain>(), NPC.damage, 0f, Main.myPlayer);
			//SoundEngine.PlaySound(SoundID.Item61, NPC.Center);
			NPC.netUpdate = true;
		}


		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());

			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientInfraRedPlating>(), 1, 9, 25));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<InfraRedCrystalShard>(), 2, 4, 12));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DGPCrate>(), 4));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<JackRailcannon>(), 50));

			int[] drops = { ModContent.ItemType<JackSword>(), ModContent.ItemType<AncientHarpoon>(), ModContent.ItemType<AncientRifle>(), };

			notExpertRule.OnSuccess(ItemDropRule.OneFromOptions(1, drops));

			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Tiles.JuiceBox>(), 5, 1, 3));

			if (ModLoader.TryGetMod("MagicStorage", out Mod magicStorage) && !GerdWorld.downedJack)
			{
				npcLoot.Add(ItemDropRule.Common(magicStorage.Find<ModItem>("ShadowDiamond").Type));
			}

			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<JackTreasureBag>()));
			npcLoot.Add(notExpertRule);
		}

		public override void OnKill()
		{
			GerdWorld.downedJack = true;
			NPC.netUpdate = true;

			int dustType = 60;
			for (int i = 0; i < 40; i++)
			{
				Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
				Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 60, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

				dust.noLight = false;
				dust.noGravity = true;
				dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
			}
		}

		public override void FindFrame(int FrameHeight)
		{
			int startFrame = 0;
			int finalFrame = Main.npcFrameCount[NPC.type] - 1;

			int frameSpeed = 10; // Used to delay in frames an animation
			NPC.frameCounter += 1f; // How fast the frames are going

			if (Main.rand.NextBool(50) && NPC.frame.Y == 7 * FrameHeight) // Enable the animation randomly as long as the 8th frame is reached
				PlayAnimation = true;

			// Randomly if the current frame is the 8th or higher and the counter per tick is over the speed, next frame
			if (Alivent)
			{
				NPC.frameCounter = 0;
				NPC.frame.Y = 8 * FrameHeight;
				frameSpeed = 0;
				NPC.frameCounter += 0f;
			}
			else if (PlayAnimation && (NPC.frame.Y >= 7 * FrameHeight) && NPC.frameCounter > frameSpeed)
			{
				NPC.frameCounter = 0; // Reset counter
				NPC.frame.Y += FrameHeight; // Next frame
				frameSpeed = 15;

				if (NPC.frame.Y > finalFrame * FrameHeight) // If the current frame is past the final frame
				{
					NPC.frame.Y = startFrame + 7 * FrameHeight; // Reset to the 8th frame
					PlayAnimation = false; // Distable animation
				}
			}
			else if (NPC.frame.Y < 7 * FrameHeight && NPC.frameCounter > frameSpeed) // As long as there's no random animation playing
			{
				NPC.frameCounter = 0;
				NPC.frame.Y += FrameHeight;

				if (NPC.frame.Y == 7 * FrameHeight) // If the current frame is the 8th frame
				{
					NPC.frame.Y = startFrame + 7 * FrameHeight; // Reset to the 8th frame
				}

				/*if (NPC.frame.Y > finalFrame * FrameHeight) // If the current frame is past all frames
				{
					NPC.frame.Y = startFrame * FrameHeight; // Reset to the first frame
				}*/
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var texture = Terraria.GameContent.TextureAssets.Npc[Type].Value;
			int num156 = Terraria.GameContent.TextureAssets.Npc[Type].Value.Height / Main.npcFrameCount[Type]; //ypos of lower right corner of sprite to draw
			int y3 = num156 * (int)(NPC.frame.Y / 122); //ypos of upper left corner of sprite to draw
			Rectangle rectangle = new Rectangle(0, y3, texture.Width, num156);
			Vector2 origin2 = rectangle.Size() / 2f;
			Color flareColor = !Main.dayTime ? new Color(195, 195, 95, 5) * NPC.Opacity : new Color(194, 91, 112, 5) * NPC.Opacity;

			Main.EntitySpriteDraw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY),
				new Microsoft.Xna.Framework.Rectangle?(rectangle), drawColor * NPC.Opacity, NPC.rotation, origin2, NPC.scale, SpriteEffects.None, 0);

			texture = GMR.Instance.Assets.Request<Texture2D>($"NPCs/Bosses/Jack/Jack_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			Main.EntitySpriteDraw(texture, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY),
				new Microsoft.Xna.Framework.Rectangle?(rectangle), flareColor, NPC.rotation, origin2, NPC.scale, SpriteEffects.None, 0);

			Main.instance.LoadProjectile(ProjectileID.RainbowCrystalExplosion);
			var flare = TextureAssets.Projectile[ProjectileID.RainbowCrystalExplosion].Value;
			Vector2 flareOrigin = flare.Size() / 2f;
			Main.EntitySpriteDraw(flare, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY) + (30 * Vector2.UnitY),
				null, flareColor, NPC.rotation, flareOrigin, new Vector2(NPC.scale * 1.5f, NPC.scale * 0.2f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(flare, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY) + (30 * Vector2.UnitY),
				null, flareColor, NPC.rotation + MathHelper.ToRadians(90f), flareOrigin, new Vector2(NPC.scale * 1.25f, NPC.scale * 0.125f), SpriteEffects.None, 0);

			Main.EntitySpriteDraw(flare, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY) - (8 * Vector2.UnitY),
				null, flareColor, NPC.rotation, flareOrigin, new Vector2(NPC.scale * 1.25f, NPC.scale * 0.125f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(flare, NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY) - (8 * Vector2.UnitY),
				null, flareColor, NPC.rotation + MathHelper.ToRadians(90f), flareOrigin, new Vector2(NPC.scale, NPC.scale * 0.05f), SpriteEffects.None, 0);
			return false;
		}
	}
}
