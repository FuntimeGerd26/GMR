using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Misc.Modules
{
	public class NeonModule : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Piercing beam");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.damage = 120; // The damage for projectiles isn't actually 100, it actually is the damage combined with the projectile and the item together
			Item.DamageType = DamageClass.Ranged; // What type of damage does this ammo affect?
			Item.width = 18;
			Item.height = 30;
			Item.rare = 4;
			Item.knockBack = 2f; // Sets the item's knockback. Ammunition's knockback added together with weapon and projectiles.
			Item.value = Item.sellPrice(silver: 45);
			Item.shoot = ModContent.ProjectileType<Projectiles.Ranged.Railgun.RailcannonRay>(); // The projectile that weapons fire when using this item as ammunition.
			Item.ammo = ModContent.ItemType<Items.Misc.Modules.NeonModule>(); // Important. The first item in an ammo class sets the AmmoID to its type
		}
	}
}