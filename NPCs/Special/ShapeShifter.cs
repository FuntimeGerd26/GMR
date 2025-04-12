using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.DataStructures;
using System.Collections.Generic;
using ReLogic.Content;
using Terraria.ModLoader.IO;
using GMR.Items.Accessories;
using GMR.Items.Misc.Materials;
using GMR.Items.Misc.Modules;
using GMR.Items.Vanity;
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
using GMR.NPCs.Bosses.Acheron;

namespace GMR.NPCs.Special
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class ShapeShifter : ModNPC
	{
		public const string ShopName = "Shop";

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from localization files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 300; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 60; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 75;
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
							  // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
							  // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Do NOT ask me where these are, I hate this coding part

			// Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
			// NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
			NPC.Happiness
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike) // Dislikes the desert.
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Love) // Loves the snow.
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like) // Likes living near the dryad.
				.SetNPCAffection(NPCID.Stylist, AffectionLevel.Love) // Loves living near the stylist.
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like) // Likes living near the party girl.
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Hate) // Hates living near the angler.
			; // < Mind the semicolon!
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 42;
			NPC.height = 56;
			NPC.aiStyle = 7;
			NPC.damage = 25;
			NPC.defense = 20;
			NPC.lifeMax = 350;
			NPC.HitSound = SoundID.NPCHit42;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Guide;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("A less threatening form of the Acheron Design, this is a form Acheron took in order to gather whatever information it could have about rebuilding it's home. It still holds a grudge against Jack"),
			});
		}

		public override bool CanTownNPCSpawn(int numTownNPCs)
		{ // Requirements for the town NPC to spawn.
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (!player.active)
				{
					continue;
				}

				// If the player has above 300 HP and defeated EoC
				if (player.statLifeMax2 > 300 && NPC.downedBoss1)
				{
					return true;
				}
			}

			return false;
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
				"Jake",
				"Acheron",
				"Mike",
				"Unit-07",
				"Jacky",
				"Barbara",
				"Diana",
				"Gabriela",
				"Sophia",
				"Neon"
			};
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("Could you look at that, my coding seems unfinished, i better call snith to help me get some supplies before trying anything."), 0.7); // "Message, Chance of message popping up"
			chat.Add(Language.GetTextValue("I can shape shift most materials like the one's i make my body with, i just don't wanna do that all the time."));
			chat.Add(Language.GetTextValue("Fun fact: not even my creator knew i could shape shift. Crazy right?."));
			chat.Add(Language.GetTextValue("Do not question why i choose this form, it's comftable."));
			chat.Add(Language.GetTextValue("Maybe don't fall into a well, and you'd get some real loot."), 0.0125);
			chat.Add(Language.GetTextValue("From where the heck did a kid get technology from? and you are telling me he gives it for fishes???."));
			chat.Add(Language.GetTextValue("No i won't call you 'my pog champ', what does that even mean?"), 0.25);
			chat.Add(Language.GetTextValue("This world lacks something, i don't know what tho."));
			chat.Add(Language.GetTextValue("Don't call random numbers, it won't lead you to getting mine."));
			chat.Add(Language.GetTextValue("Don't bother asking where i'm from, i forgot the name of my home."));
            chat.Add(Language.GetTextValue("Do you think you're safe?"), 0.125);
			
			if(GerdWorld.downedJack)
			chat.Add(Language.GetTextValue("You wanna fight again? I'd say that i don't want to, but if you insist."));
			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28");
            if (Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Accessories.JackExpert>()) && Main.expertMode)
            {
                button2 = $"Trade Infra-Red Emblem";
            }
			else if (Main.hardMode)
            {
				button2 = "[c/FF55AA:Challenge!]";
			}
        }

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			if (!firstButton)
			{
				if (Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Accessories.JackExpert>())) //If the player has the items, change the option on the second button
				{
					SoundEngine.PlaySound(SoundID.Item37); // Reforge/Anvil sound

					Main.npcChatText = $"Oh, you got your hands on it? Well here's a trade for yours";

					int itemIndex = Main.LocalPlayer.FindItem(ModContent.ItemType<Items.Accessories.JackExpert>());
					var entitySource = NPC.GetSource_GiftOrReward();

					Main.LocalPlayer.inventory[itemIndex].TurnToAir();
					Main.LocalPlayer.QuickSpawnItem(entitySource, ModContent.ItemType<JackRifle>());

					return;
				}
				else if (!NPC.AnyNPCs(ModContent.NPCType<Acheron>()))
				{
					Main.npcChatText = $"Want a challenge? Let's go";
					NPC.NewNPC(new EntitySource_Parent(NPC), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<NPCs.Bosses.Acheron.Acheron>());
					NPC.life -= 2600;

					return;
				}
				else
				{
					Main.npcChatText = $"Bro look behind you, IS THAT ME!??";

					return;
				}
			}
			else if (firstButton) //open the shop of the NPC
			{
				shop = ShopName; // Name of the shop tab we want to open.
			}
		}

		// Fuck it we ball
		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, ShopName)
				.Add<TomeOfDreams>()
				.Add<YinGun>()
				.Add<YangGun>()
				.Add<GunslayerPistol>(new Condition("Mods.GMR.Conditions.DefeatEvil", () => NPC.downedBoss2))

				.Add<AluminiumCharm>(new Condition("Mods.GMR.Conditions.DefeatEvil", () => NPC.downedBoss2))
				.Add<Coffin>(new Condition("Mods.GMR.Conditions.DefeatEvil", () => NPC.downedBoss2))

				.Add<MagmaticShard>(new Condition("Mods.GMR.Conditions.DefeatMagmaEye", () => GerdWorld.downedMagmaEye))
				.Add<HardmodeUpgradeCrystal>(new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))
				.Add<InfraRedCrystalShard>(new Condition("Mods.GMR.Conditions.DefeatAcheron", () => GerdWorld.downedAcheron))
				.Add<AncientInfraRedPlating>(new Condition("Mods.GMR.Conditions.DefeatAcheron", () => GerdWorld.downedAcheron))
				.Add<InfraRedBar>(new Condition("Mods.GMR.Conditions.DefeatAcheron", () => GerdWorld.downedAcheron))

				.Add<NeonModule>(new Condition("Mods.GMR.Conditions.DefeatJack", () => GerdWorld.downedJack))
				.Add<MaskedPlagueModule>(new Condition("Mods.GMR.Conditions.DefeatJack", () => GerdWorld.downedJack))
				.Add<AmethystModule>(new Condition("Mods.GMR.Conditions.DefeatJack", () => GerdWorld.downedJack))

				.Add<JackyMask>(new Condition("Mods.GMR.Conditions.Hardmode", () => Main.hardMode))
				.Add<GerdHead>(new Condition("Mods.GMR.Conditions.DefeatJack", () => GerdWorld.downedJack))
				.Add<GerdBody>(new Condition("Mods.GMR.Conditions.DefeatJack", () => GerdWorld.downedJack))
				.Add<GerdLegs>(new Condition("Mods.GMR.Conditions.DefeatJack", () => GerdWorld.downedJack))
				.Add<InfraRedWings>(new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))

				.Add(ItemID.FrostCore, new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))
				.Add(ItemID.AncientBattleArmorMaterial, new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))
				.Add(ItemID.BlackInk, new Condition("Mods.GMR.Conditions.GetFixedBoi", () => Main.getGoodWorld && NPC.downedBoss2))
				.Add(ItemID.SharkFin, new Condition("Mods.GMR.Conditions.GetFixedBoi", () => Main.getGoodWorld && NPC.downedBoss2))
				.Add(ItemID.DivingHelmet, new Condition("Mods.GMR.Conditions.GetFixedBoi", () => Main.getGoodWorld && NPC.downedBoss2));

			npcShop.Register(); // Name of this shop tab
		}

		public override void ModifyActiveShop(string shopName, Item[] items)
		{
			foreach (Item item in items)
			{
				// Skip 'air' items and null items.
				if (item == null || item.type == ItemID.None)
				{
					continue;
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.SpecialUpgradeCrystal>(), 3)); // npcLoot.Add(ItemDropRule.Common(Item, Chance of drop, Min amount of item, Max amount of item));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Vanity.JackyMask>(), 10));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered.
		public override bool CanGoToStatue(bool toKingStatue) => false; // false = Queen Statue 

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = NPC.downedMoonlord ? 80 : 25;
			knockback = NPC.downedMoonlord ? 4f : 2f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 20;
			randExtraCooldown = 10;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ModContent.ProjectileType<Projectiles.Melee.JackSwordThrow>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 1f;
		}
	}
}