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
using GMR.Items.Misc;
using GMR.Items.Misc.Materials;
using GMR.Items.Vanity;
using GMR.Items.Weapons.Melee;
using GMR.Items.Weapons.Summoner;

namespace GMR.NPCs.Special
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class Memer : ModNPC
	{
		public const string ShopName = "Shop";

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from localization files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 4;
			NPCID.Sets.DangerDetectRange[Type] = 400; // The amount of pixels away from the center of the npc that it tries to attack enemies.
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
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Hate) // Dislikes the desert.
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Love) // Loves the snow.
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Like) // Likes living near the demolitionist.
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) // Loves living near the dryad.
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like) // Likes living near the party girl.
				.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Like) // Likes living near the arms dealer.
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
			NPC.damage = 30;
			NPC.defense = 30;
			NPC.lifeMax = 200;
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
				new FlavorTextBestiaryInfoElement("A kind machine that came by to share some puns, it has no other motives to come here"),
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

				// If the player has defeated any evil boss
				if (NPC.downedBoss2)
				{
					return true;
				}
			}

			return false;
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
				"Spazmatanium",
				"Spaz",
				"Spazmatism"
			};
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();
			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("Terraria is a nice game, to bad I almost never play it..."), 0.7);
			chat.Add(Language.GetTextValue("I have a handmade costume of me, i don't know why tho."));
			chat.Add(Language.GetTextValue("Crabulon best waifu, change my mind."));
			chat.Add(Language.GetTextValue("Subscribing to me is a mistake"));
			chat.Add(Language.GetTextValue("Thank you Gerd for modding me in, oh no wrong chat box"));
			chat.Add(Language.GetTextValue("You might be wondering where retinazer is, hes dead."));
			chat.Add(Language.GetTextValue("I'm happy tmodloader isn't on 1.4 yet... Wait what do you mean it already is?"));
			chat.Add(Language.GetTextValue("Just buy something and go."));
			chat.Add(Language.GetTextValue("Before you ask, that spaz fumo is mine."));
			chat.Add(Language.GetTextValue("Oh I'm sold out on some things, I better call snith."));
			chat.Add(Language.GetTextValue("You wanna buy something?"));
			chat.Add(Language.GetTextValue("Never gonna give you up, never gonna let you down."));
			chat.Add(Language.GetTextValue("You can make rules, but who's gonna follow them?"));
			chat.Add(Language.GetTextValue("It was never orange"));
			if (GerdWorld.downedJack)
				chat.Add(Language.GetTextValue("Yo, defeated the big robot yet? Sick."));
			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (!player.active)
				{
					continue;
				}
				// What the chat buttons are when you open up the chat UI
				button = Language.GetTextValue("LegacyInterface.28");
				button2 = player.ZoneUnderworldHeight ? "[c/FF6622:Yo]" : "[c/55FFAA:Yo]";
			}
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shop)
		{
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (!player.active)
				{
					continue;
				}

				if (!firstButton && player.ZoneUnderworldHeight)
				{
					Main.npcChatText = $"Welp, there it goes";
					NPC.NewNPC(new EntitySource_Parent(NPC), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height / 2 - 1200, ModContent.NPCType<NPCs.Bosses.MagmaEye.MagmaEye>());
					SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
					NPC.life *= -1;
					return;
				}
				else if (!firstButton)
				{
					Rectangle displayPoint = new Rectangle(NPC.Hitbox.Center.X, NPC.Hitbox.Center.Y - NPC.height / 4, 2, 2);
					CombatText.NewText(displayPoint, Color.Cyan, "IT'S ORANGE");
					Main.npcChatText = "The button is orange";
					return;
				}
				else if (firstButton) //open the shop of the NPC
				{
					shop = ShopName; // Name of the shop tab we want to open.
				}
			}
		}

		// Fuck it we ball
		public override void AddShops()
		{
			var npcShop = new NPCShop(Type, ShopName)
				.Add<PsycopathAxe>()
				.Add<DesertAxe>()
				.Add<BookOfVirtues>()
				.Add<Hatred>()
				.Add<UpgradeCrystal>()
				.Add<BossUpgradeCrystal>(new Condition("Mods.GMR.Conditions.Hardmode", () => Main.hardMode))

				.Add<SandwaveHat>()
				.Add<SandwaveShirt>()
				.Add<SandwavePants>()

				.Add<Items.Tiles.MagmaAltar>(new Condition("Mods.GMR.Conditions.DefeatMagmaEye", () => GerdWorld.downedMagmaEye))

				.Add<ChaosAngelHalo>(new Condition("Mods.GMR.Conditions.DefeatSkeletron", () => NPC.downedBoss3))
				.Add<ChaosAngelShirt>(new Condition("Mods.GMR.Conditions.DefeatSkeletron", () => NPC.downedBoss3))
				.Add<ChaosAngelPants>(new Condition("Mods.GMR.Conditions.DefeatSkeletron", () => NPC.downedBoss3))
				.Add<ChaosAngelWings>(new Condition("Mods.GMR.Conditions.DefeatSkeletron", () => NPC.downedBoss3 && Main.hardMode))
				.Add<PhoenixSword>(new Condition("Mods.GMR.Conditions.DefeatSkeletron", () => NPC.downedBoss3))


				.Add<IceyMask>(new Condition("Mods.GMR.Conditions.Hardmode", () => Main.hardMode))
				.Add<IceyBody>(new Condition("Mods.GMR.Conditions.Hardmode", () => Main.hardMode))
				.Add<IceyLegs>(new Condition("Mods.GMR.Conditions.Hardmode", () => Main.hardMode))
				.Add<IceyWings>(new Condition("Mods.GMR.Conditions.Hardmode", () => Main.hardMode))

				.Add<KizunaScythe>(new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))

				.Add<SpazHatMask>(new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))
				.Add<SpazMask>(new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))
				.Add<SpazDress>(new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))
				.Add<SpazThighs>(new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))
				.Add<SpazCape>(new Condition("Mods.GMR.Conditions.AnyMech", () => NPC.downedMechBossAny))


				.Add<ChargedArm>(new Condition("Mods.GMR.Conditions.DefeatAcheron", () => GerdWorld.downedAcheron))
				.Add<ShadowSword>(new Condition("Mods.GMR.Conditions.DefeatAcheron", () => GerdWorld.downedAcheron));
			npcShop.Register(); // Name of this shop tab
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Misc.Materials.SpecialUpgradeCrystal>(), 3)); // npcLoot.Add(ItemDropRule.Common(Item, Chance of drop, Min amount of item, Max amount of item));
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
			projType = ModContent.ProjectileType<Projectiles.Ranged.SpazArrow>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 0.5f;
		}
	}
}