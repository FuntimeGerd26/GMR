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
	// No longer a boss btw
	internal class Trerios : WormHead
	{
		public override int BodyType => ModContent.NPCType<TreriosBody>();

		public override int TailType => ModContent.NPCType<TreriosTail>();

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
			NPC.AddElement(0);
		}

		public override void SetDefaults()
		{
			// Head is 10 defence, body 20, tail 30.
			NPC.CloneDefaults(NPCID.DiggerHead);
			NPC.width = 92;
			NPC.height = 92;
			NPC.lifeMax = 400;
			NPC.damage = 40;
			NPC.aiStyle = -1;
			NPC.npcSlots = 1f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Born from the hatred of a mad scientist, it fuels itself eating any organic material it comes across. It can also eat inorganic things to repair itself, but only on cases of extreme danger.")
			});
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneDirtLayerHeight)
			{
				return 0.0125f; //1.25% chance of spawning on the underground, not the caverns
			}
			return 0f;
		}

		public override void Init()
		{
			// Set the segment variance
			// If you want the segment length to be constant, set these two properties to the same value
			MinSegmentLength = 28;
			MaxSegmentLength = 34;

			CommonWormInit(this);
		}

		// This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
		internal static void CommonWormInit(Worm worm)
		{
			// These two properties handle the movement of the worm
			worm.MoveSpeed = 18f;
			worm.Acceleration = 0.075f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			int[] drops = { ModContent.ItemType<Items.Weapons.Melee.HatefulBlade>(), ModContent.ItemType<Items.Weapons.Ranged.HatredBow>(), ModContent.ItemType<Items.Weapons.Ranged.HatredGun>() };
			npcLoot.Add(ItemDropRule.OneFromOptions(1, drops));
		}

		public override void OnKill()
		{
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
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPC.AddElement(0);
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.aiStyle = -1;
			NPC.width = 44;
			NPC.height = 44;
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
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			NPC.AddElement(0);
		}

		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.aiStyle = -1;
			NPC.width = 60;
			NPC.height = 60;
		}

		public override void Init()
		{
			Trerios.CommonWormInit(this);
		}
	}
}