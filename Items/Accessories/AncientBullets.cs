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
	public class AncientBullets : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault($"If the weapon uses bullets as ammo, it has a chance to turn them into Infra-Red Bullets that inflict 'Partially Crystalized'" +
				"\nIncreases ranged damage by 3% and ranged attack speed by 5%" +
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
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetDamage(DamageClass.Ranged) += 0.03f;
			player.GetAttackSpeed(DamageClass.Ranged) += 0.05f;
			player.GPlayer().AncientBullets = Item;
		}
	}
}