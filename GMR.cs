using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace GMR
{
	public class GMR : Mod
	{
		public const string ModName = nameof(GMR);

		public static GMR Instance => ModContent.GetInstance<GMR>();

		public GMR()
		{
			if (ModLoader.TryGetMod("ROR2HealthBars", out var ror2HBs))
			{
				ror2HBs.Call("HPPool", new List<int>()
				{
					ModContent.NPCType<NPCs.Bosses.Jack.Jack>(),
					ModContent.NPCType<NPCs.Bosses.Jack.JackArmGun>(),
					ModContent.NPCType<NPCs.Bosses.Jack.JackArmGunFlip>(),
					ModContent.NPCType<NPCs.Bosses.Jack.JackArmClaw>(),
					ModContent.NPCType<NPCs.Bosses.Jack.JackArmClawFlip>()
				});
				ror2HBs.Call("CustomName", ModContent.NPCType<NPCs.Bosses.Jack.Jack>(), "Jack");
				ror2HBs.Call("BossDesc", ModContent.NPCType<NPCs.Bosses.Jack.Jack>(), "The Wandering Machine");
			}
		}

		public static GMR GetInstance()
		{
			return ModContent.GetInstance<GMR>();
		}

		public override void AddRecipes()
		{
			Mod GMR = ModLoader.GetMod("GMR");
			RecipeGroup group = new RecipeGroup(() => Lang.misc[37] + " Gem type", ItemID.Diamond, ItemID.Ruby, ItemID.Emerald, ItemID.Sapphire, ItemID.Topaz, ItemID.Amethyst);
			RecipeGroup.RegisterGroup("GMR:AnyGem", group);

			/*recipe.AddRecipeGroup("GMR:AnyX");
			group = new RecipeGroup(() => Lang.misc[37] + " EXAMPLE RECIPE GROUP", Gerdsmod.Find<ModItem>("ITEM1").Type, Gerdsmod.Find<ModItem>("ITEM2").Type);
			RecipeGroup.RegisterGroup("GerdBoss:AnyX", group);*/
		}
	}
}