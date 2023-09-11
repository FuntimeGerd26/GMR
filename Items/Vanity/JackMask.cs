using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class JackMask : ModItem
	{
		public override void Load()
		{
			// The code below runs only if we're not loading on a server
			if (Main.netMode == NetmodeID.Server)
				return;
		}

		// Called in SetStaticDefaults
		private void SetupDrawing()
		{
			// Since the equipment textures weren't loaded on the server, we can't have this code running server-side
			if (Main.netMode == NetmodeID.Server)
				return;

			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acheron Mask");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			SetupDrawing();
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 75);
			Item.vanity = true;
		}
	}
}
