using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class XShotVice : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultra-blue Vice");
			Tooltip.SetDefault("'Technology has lead to strange discoveries'");

			ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // This skips use animation-tied sound playback, so that we're able to make it be tied to use time instead in the UseItem() hook.
			ItemID.Sets.Spears[Item.type] = true; // This allows the game to recognize our new item as a spear.
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; //Items required for research on journey mode
		}

		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Pink; // Assign this item a rarity level of Pink
			Item.value = Item.sellPrice(silver: 110); // The number and type of coins item can be sold for to an NPC
			Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
			Item.useAnimation = 18; // The length of the item's use animation in ticks (60 ticks == 1 second.)
			Item.useTime = 18; // The length of the item's use time in ticks (60 ticks == 1 second.)
			Item.UseSound = SoundID.Item71; // The sound that this item plays when used.
			Item.autoReuse = true; // Allows the player to hold click to automatically use the item again. Most spears don't autoReuse, but it's possible when used in conjunction with CanUseItem()
			Item.damage = 60; // Damage of the weapon
			Item.knockBack = 6f; // Knockback of the weapon
			Item.noUseGraphic = true; // When true, the item's sprite will not be visible while the item is in use. This is true because the spear projectile is what's shown so we do not want to show the spear sprite as well.
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true; // Allows the item's animation to do damage. This is important because the spear is actually a projectile instead of an item. This prevents the melee hitbox of this item.
			Item.shootSpeed = 6f; // The speed of the projectile measured in pixels per frame.
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.XShotVice>(); // The projectile that is fired from this weapon
		}

		public override bool CanUseItem(Player player)
		{
			// Ensures no more than one spear can be thrown out, use this when using autoReuse
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool? UseItem(Player player)
		{
			// Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}

			return null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MythrilBar, 14);
			recipe.AddIngredient(ItemID.SoulofNight, 18);
			recipe.AddIngredient(ItemID.AdamantiteBar, 28);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.OrichalcumBar, 14);
			recipe2.AddIngredient(ItemID.SoulofNight, 18);
			recipe2.AddIngredient(ItemID.TitaniumBar, 28);
			recipe2.AddTile(TileID.MythrilAnvil);
			recipe2.Register();

			Recipe recipe3 = CreateRecipe();
			recipe3.AddIngredient(ItemID.OrichalcumBar, 14);
			recipe3.AddIngredient(ItemID.SoulofNight, 18);
			recipe3.AddIngredient(ItemID.AdamantiteBar, 28);
			recipe3.AddTile(TileID.MythrilAnvil);
			recipe3.Register();

			Recipe recipe4 = CreateRecipe();
			recipe4.AddIngredient(ItemID.MythrilBar, 14);
			recipe4.AddIngredient(ItemID.SoulofNight, 18);
			recipe4.AddIngredient(ItemID.TitaniumBar, 28);
			recipe4.AddTile(TileID.MythrilAnvil);
			recipe4.Register();
		}
	}
}