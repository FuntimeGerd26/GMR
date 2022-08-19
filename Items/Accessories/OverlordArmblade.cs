using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using GMR;

namespace GMR.Items.Accessories
{
	public class OverlordArmblade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mech-Overlord Armblade");
			Tooltip.SetDefault("Increases melee damage and attack speed by 7%\nMakes enemies take 25% damage when hitting you\nIncreased damage reduction by 5%");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 62;
			Item.value = Item.sellPrice(silver: 125);
			Item.rare = 6;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Melee) += 0.7f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.7f;
			player.thorns = 0.25f;
			player.endurance = 1f - (0.05f * (1f - player.endurance));
			player.GPlayer().OverlordBlade = Item;
		}
	}
}