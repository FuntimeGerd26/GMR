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

namespace GMR.NPCs.Bosses.Worms
{
	[AutoloadBossHead()]
	internal class Trerios : WormHead
	{
		public override int BodyType => ModContent.NPCType<TreriosBody>();

		public override int TailType => ModContent.NPCType<TreriosTail>();

		public int AI4;
		public int AI5;
		public int AI6;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trerios");
			NPCID.Sets.DontDoHardmodeScaling[Type] = true;
			NPCID.Sets.MPAllowedEnemies[Type] = true;

			NPCID.Sets.BossBestiaryPriority.Add(Type);
			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{ // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "GMR/NPCs/Bosses/Trerios_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				Position = new Vector2(-1f, -12f),
				PortraitPositionXOverride = -1f,
				PortraitPositionYOverride = -12f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
			Main.npcFrameCount[Type] = 3;
		}

		public override void SetDefaults()
		{
			// Head is 10 defence, body 20, tail 30.
			NPC.CloneDefaults(NPCID.DiggerHead);
			NPC.width = 92;
			NPC.height = 92;
			NPC.lifeMax = 3850;
			NPC.damage = 28;
			NPC.aiStyle = -1;
			NPC.boss = true;
			NPC.npcSlots = 10f;
			if (!Main.dedServ)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Bosses/Trerios");
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Born from the hatred and sprite of a higher entity, it feasts on souls that have no aim.")
			});
		}

		public override void Init()
		{
			// Set the segment variance
			// If you want the segment length to be constant, set these two properties to the same value
			MinSegmentLength = 26;
			MaxSegmentLength = 26;

			CommonWormInit(this);
		}

		// This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
		internal static void CommonWormInit(Worm worm)
		{
			// These two properties handle the movement of the worm
			worm.MoveSpeed = 18f;
			worm.Acceleration = 0.075f;
		}

		public override void AI()
		{
			if (!NPC.AnyNPCs(ModContent.NPCType<TreriosBody>()) && !NPC.AnyNPCs(ModContent.NPCType<TreriosTail>()))
			{
				NPC.life += -2000;
				NPC.netUpdate = true;
			}

				Player target = Main.player[NPC.target];

			switch ((int)NPC.ai[0])
			{
				case -1:
					NPC.ai[0]++;
					break;

				case 0:
					NPC.ai[0]++;
					break;

				case 1:
					if (++NPC.ai[1] == 1)
					{
						NPC.netUpdate = true;

						SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
					}
					if (++NPC.ai[2] % 200 == 0)
					{
						NPC.netUpdate = true;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
							int type = ModContent.ProjectileType<Projectiles.Bosses.Worms.HatredBlade>();
							for (int i = 0; i < 4; i++)
							{
								SoundEngine.PlaySound(SoundID.Item12, NPC.Center);

								Vector2 position = target.Center + 250 * Vector2.UnitX - 250 * Vector2.UnitY;
								Vector2 position1 = target.Center - 250 * Vector2.UnitX + 250 * Vector2.UnitY;
								Vector2 position2 = target.Center - 250 * Vector2.UnitY - 250 * Vector2.UnitX;
								Vector2 position3 = target.Center + 250 * Vector2.UnitY + 250 * Vector2.UnitX;
								Projectile.NewProjectile(NPC.GetSource_FromThis(), position, (target.Center - position) * 0.02f, type, NPC.damage, 1f, Main.myPlayer);
								Projectile.NewProjectile(NPC.GetSource_FromThis(), position1, (target.Center - position1) * 0.02f, type, NPC.damage, 1f, Main.myPlayer);
								Projectile.NewProjectile(NPC.GetSource_FromThis(), position2, (target.Center - position2) * 0.02f, type, NPC.damage, 1f, Main.myPlayer);
								Projectile.NewProjectile(NPC.GetSource_FromThis(), position3, (target.Center - position3) * 0.02f, type, NPC.damage, 1f, Main.myPlayer);
							}
						}
					}
					if (++NPC.ai[1] > 600)
					{
						NPC.TargetClosest();
						NPC.ai[0]++;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
						NPC.netUpdate = true;
					}
					break;

				case 2:
					if (++NPC.ai[1] == 1)
					{
						NPC.netUpdate = true;

						SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
					}
					if (++NPC.ai[2] % 300 == 0)
					{
						NPC.netUpdate = true;

						SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
						float x = 1500f;
						float y = 1300f;
						int amount = 20;
						var posX = target.position.X + target.width / 2f - x / 2f;
						var posY = target.position.Y + target.height / 2f;
						float xAdd = x / (amount / 2);
						int type = ModContent.ProjectileType<Projectiles.Bosses.Worms.HatredBlade>();
						for (int i = 0; i < amount; i++)
						{
							SoundEngine.PlaySound(SoundID.Item12, NPC.Center);
							Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX + xAdd * i, posY + y), new Vector2(0f, -18f), type, NPC.damage, 1f, Main.myPlayer);
							Projectile.NewProjectile(NPC.GetSource_FromThis(), new Vector2(posX + xAdd * i, posY - y), new Vector2(0f, 18f), type, NPC.damage, 1f, Main.myPlayer);
						}
					}
					if (++NPC.ai[1] > 601)
					{
						NPC.TargetClosest();
						NPC.ai[0] = 0;
						NPC.ai[1] = 0;
						NPC.ai[2] = 0;
						NPC.netUpdate = true;
					}
					break;
			}
		}

		public override void FindFrame(int FrameHeight)
		{
			int startFrame = 0;
			int finalFrame = Main.npcFrameCount[NPC.type] - 1;

			int frameSpeed = 10; // Used to delay in frames an animation
			NPC.frameCounter += 1f; // How fast the frames are going

			if (NPC.frameCounter > frameSpeed) // As long as there's no random animation playing
			{
				NPC.frameCounter = 0;
				NPC.frame.Y += FrameHeight;

				if (NPC.frame.Y > finalFrame * FrameHeight) // If the current frame is past all frames
                {
                    NPC.frame.Y = startFrame * FrameHeight; // Reset to the first frame
                }
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			int[] drops = { ModContent.ItemType<Items.Weapons.Melee.HatefulBlade>(), ModContent.ItemType<Items.Weapons.Ranged.HatredBow>(), ModContent.ItemType<Items.Weapons.Ranged.HatredGun>() };
			npcLoot.Add(ItemDropRule.OneFromOptions(1, drops));
		}

		public override void OnKill()
		{
			GerdWorld.downedTrerios = true;

			int dustType = 60;
			for (int i = 0; i < 20; i++)
			{
				Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
				Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 64, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

				dust.noLight = false;
				dust.noGravity = true;
				dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
			}
		}
	}

	internal class TreriosBody : WormBody
	{
		public int AI6;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trerios");

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.aiStyle = -1;
			NPC.width = 48;
			NPC.height = 48;
		}

		public override void Init()
		{
			Trerios.CommonWormInit(this);
		}
	}

	internal class TreriosTail : WormTail
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trerios");

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.aiStyle = -1;
			NPC.width = 64;
			NPC.height = 64;
		}

		public override void Init()
		{
			Trerios.CommonWormInit(this);
		}
	}
}