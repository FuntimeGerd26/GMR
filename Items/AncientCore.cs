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
	public class AncientCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"Increases summon and melee damage by 5%" +
				$"\nIncreases magic crit rate by 5%\nIf the weapon uses bullets as ammo, it has a chance to turn them into Explosive Infra-Red Bullets that inflict 'Partially Crystalized'" +
				"\n'Holds just a bit of data'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 30;
			Item.value = Item.sellPrice(silver: 120);
			Item.rare = 3;
			Item.accessory = true;
			Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.GetDamage(DamageClass.Melee) += 0.05f;
			player.GetCritChance(DamageClass.Magic) += 5f;
			player.GPlayer().AncientCore = Item;
		}
	}
}