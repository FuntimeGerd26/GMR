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

namespace GMR.NPCs.Special
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class ShapeShifter : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from localization files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 50; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
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
				.SetNPCAffection(NPCID.Angler, AffectionLevel.Dislike) // Dislikes living near the angler.
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like) // Likes living near the party girl.
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
			NPC.HitSound = SoundID.NPCHit4;
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
				new FlavorTextBestiaryInfoElement("A passive machine that knows little of this world, it dosen't need an excuse to ask anything in it's mind"),
			});
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, 60);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{ // Requirements for the town NPC to spawn.
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (!player.active)
				{
					continue;
				}

				// If the player has above 200 HP and it's Hardmode
				if (player.statLifeMax2 > 200 && Main.hardMode)
				{
					return true;
				}
			}

			return false;
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
				"Jacky",
				"Jack",
				"Barbara",
				"J-26-07"
			};
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && Main.rand.NextBool(4))
			{
				chat.Add(Language.GetTextValue("Isn't it fun hanging out with ", Main.npc[partyGirl].GivenName));
			}
			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("Could you look at that, my coding seems unfinished, i better call snith to help me get some supplies before trying anything"), 0.7);
			chat.Add(Language.GetTextValue("I can shape shift most materials like the one's i make my body with, i just don't wanna do that all the time"));
			chat.Add(Language.GetTextValue("Fun fact: not even my creator knew i could shape shift"));
			chat.Add(Language.GetTextValue("Do not question why i choose this form, it's comftable"));
			chat.Add(Language.GetTextValue("Maybe don't fall into a well, and you'd get some real W's"));
			chat.Add(Language.GetTextValue("What do you mean im useless, im more useful than that kid is"));
			chat.Add(Language.GetTextValue("No i won't call you 'my pog champ', what does that even mean?"), 0.25);
			chat.Add(Language.GetTextValue("This world lacks something, i don't know what tho"));
			chat.Add(Language.GetTextValue("I can help you change some items into another one, only if you have items that i know about tho, so don't give me dirt blocks expecting anything"), 0.01); //"Message, Chance of message popping up"
			chat.Add(Language.GetTextValue("Do not call emergency on a broken elevator if you don't work on the company"), 0.00910923);
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Accessories.JackExpert>())) //If the player has the item, make this chat possible to appear
			{
				chat.Add(Language.GetTextValue($"I feel i can change one of your items, specifically your {Lang.GetItemNameValue(ModContent.ItemType<Items.Weapons.Ranged.JackRifle>())}"), 2.0);
			}
			if (Main.LocalPlayer.HasItem(ItemID.WaterBolt))
			{
				chat.Add(Language.GetTextValue($"I feel i can change one of your items, specifically your {Lang.GetItemNameValue(ItemID.WaterBolt)}"), 2.0);
			}
			if (Main.hardMode && Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Weapons.Melee.AmethystSword>()))
			{
				chat.Add(Language.GetTextValue($"I feel i can change one of your items, specifically your {Lang.GetItemNameValue(ModContent.ItemType<Items.Weapons.Melee.AmethystSword>())}"), 2.0);
			}

			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28");
			button2 = "Change";
			if (Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Accessories.JackExpert>()) || Main.LocalPlayer.HasItem(ItemID.WaterBolt) || Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Weapons.Melee.AmethystSword>()))
			{
				button2 = "Change Item";
			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (!firstButton)
			{
				if (Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Accessories.JackExpert>())) //If the player has the items, change the option on the second button
				{
					SoundEngine.PlaySound(SoundID.Item37); // Reforge/Anvil sound

					Main.npcChatText = $"Have fun with your new {Lang.GetItemNameValue(ModContent.ItemType<Items.Weapons.Ranged.JackRifle>())}, get ready for...";

					int itemIndex = Main.LocalPlayer.FindItem(ModContent.ItemType<Items.Accessories.JackExpert>());
					var entitySource = NPC.GetSource_GiftOrReward();

					Main.LocalPlayer.inventory[itemIndex].TurnToAir();
					Main.LocalPlayer.QuickSpawnItem(entitySource, ModContent.ItemType<Items.Weapons.Ranged.JackRifle>());

					return;
				}
				else if (Main.LocalPlayer.HasItem(ItemID.WaterBolt))
				{
					SoundEngine.PlaySound(SoundID.Item37);

					Main.npcChatText = $"Have fun with your new {Lang.GetItemNameValue(ModContent.ItemType<Items.Accessories.BLBook>())}, shameless";

					int itemIndex = Main.LocalPlayer.FindItem(ItemID.WaterBolt);
					var entitySource = NPC.GetSource_GiftOrReward();

					Main.LocalPlayer.inventory[itemIndex].TurnToAir();
					Main.LocalPlayer.QuickSpawnItem(entitySource, ModContent.ItemType<Items.Accessories.BLBook>());

					return;
				}
				else if (Main.hardMode && Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Weapons.Melee.AmethystSword>()))
				{
					SoundEngine.PlaySound(SoundID.Item37);

					Main.npcChatText = $"Have fun with your new {Lang.GetItemNameValue(ModContent.ItemType<Items.Weapons.Melee.AmethystGreatSlasher>())}, I remember someone that used this";

					int itemIndex = Main.LocalPlayer.FindItem(ModContent.ItemType<Items.Weapons.Melee.AmethystSword>());
					var entitySource = NPC.GetSource_GiftOrReward();

					Main.LocalPlayer.inventory[itemIndex].TurnToAir();
					Main.LocalPlayer.QuickSpawnItem(entitySource, ModContent.ItemType<Items.Weapons.Melee.AmethystGreatSlasher>());

					return;
				}
				else //Give a fail message, or a response to not items
				{
					Main.npcChatText = "Sorry, i don't think you have anything i know i can modify";

					return;
				}
			}
			else if (firstButton) //open the shop of the NPC
			{
			    shop = true; 
			}
		}

		// Not completely finished, but below is what the NPC will sell

		public override void SetupShop(Chest shop, ref int nextSlot) 
		{
			shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Misc.Materials.CrystalNeonChip>());
			// 	shop.item[nextSlot++].SetDefaults(ItemType<ExampleItem>());
			// 	// shop.item[nextSlot].SetDefaults(ItemType<EquipMaterial>());
			// 	// nextSlot++;
			// 	// shop.item[nextSlot].SetDefaults(ItemType<BossItem>());
			// 	// nextSlot++;
			// 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleWorkbench>());
			// 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleChair>());
			// 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleDoor>());
			// 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleBed>());
			// 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleChest>());
			// 	shop.item[nextSlot++].SetDefaults(ItemType<ExamplePickaxe>());
			// 	shop.item[nextSlot++].SetDefaults(ItemType<ExampleHamaxe>());
			//
			// 	if (Main.LocalPlayer.HasBuff(BuffID.Lifeforce)) {
			// 		shop.item[nextSlot++].SetDefaults(ItemType<ExampleHealingPotion>());
			// 	}
			//
			// 	// if (Main.LocalPlayer.GetModPlayer<ExamplePlayer>().ZoneExample && !GetInstance<ExampleConfigServer>().DisableExampleWings) {
			// 	// 	shop.item[nextSlot].SetDefaults(ItemType<ExampleWings>());
			// 	// 	nextSlot++;
			// 	// }
			//
			// 	if (Main.moonPhase < 2) {
			// 		shop.item[nextSlot++].SetDefaults(ItemType<ExampleSword>());
			// 	}
			// 	else if (Main.moonPhase < 4) {
			// 		// shop.item[nextSlot++].SetDefaults(ItemType<ExampleGun>());
			// 		shop.item[nextSlot].SetDefaults(ItemType<ExampleBullet>());
			// 	}
			// 	else if (Main.moonPhase < 6) {
			// 		// shop.item[nextSlot++].SetDefaults(ItemType<ExampleStaff>());
			// 	}
			//
			// 	// todo: Here is an example of how your npc can sell items from other mods.
			// 	// var modSummonersAssociation = ModLoader.TryGetMod("SummonersAssociation");
			// 	// if (ModLoader.TryGetMod("SummonersAssociation", out Mod modSummonersAssociation)) {
			// 	// 	shop.item[nextSlot].SetDefaults(modSummonersAssociation.ItemType("BloodTalisman"));
			// 	// 	nextSlot++;
			// 	// }
			//
			// 	// if (!Main.LocalPlayer.GetModPlayer<ExamplePlayer>().examplePersonGiftReceived && GetInstance<ExampleConfigServer>().ExamplePersonFreeGiftList != null) {
			// 	// 	foreach (var item in GetInstance<ExampleConfigServer>().ExamplePersonFreeGiftList) {
			// 	// 		if (Item.IsUnloaded) continue;
			// 	// 		shop.item[nextSlot].SetDefaults(Item.Type);
			// 	// 		shop.item[nextSlot].shopCustomPrice = 0;
			// 	// 		shop.item[nextSlot].GetGlobalItem<ExampleInstancedGlobalItem>().examplePersonFreeGift = true;
			// 	// 		nextSlot++;
			// 	// 		//TODO: Have tModLoader handle index issues.
			// 	// 	}
			// 	// }
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered.
		public override bool CanGoToStatue(bool toKingStatue) => false;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}
	}
}