using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee
{
	public class InfraRedSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infra-Red Spear");
			Tooltip.SetDefault($"'You won't even remember what you used it for'\n Having over [c/FF4444:50%] HP makes the spear deal double damage" +
				$"\n Hold up to spin the spear around you, hitting enemies causes them to explode and heals your by 1% of your max health");

			ItemID.Sets.SkipsInitialUseSound[Item.type] = true;
			ItemID.Sets.Spears[Item.type] = true;

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.rare = 4;
			Item.width = 124;
			Item.height = 124;
			Item.value = Item.sellPrice(silver: 180); 
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 10;
			Item.useAnimation = 20;
			Item.UseSound = SoundID.Item7;
			Item.autoReuse = true; 
			Item.damage = 64;
			Item.knockBack = 5f;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.shootSpeed = 6f;
			Item.shoot = ModContent.ProjectileType<Projectiles.Melee.InfraRedSpear>();
		}

		public override bool? UseItem(Player player)
		{
			if (!Main.dedServ && Item.UseSound.HasValue)
			{
				SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
			}
			return null;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.controlUp)
			{
				Item.useTime = 2;
				Item.useAnimation = 2;
				Item.reuseDelay = 10;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.InfraRedSpearSpin>();
			}
			else
			{
				Item.useTime = 24;
				Item.useAnimation = 24;
				Item.reuseDelay = 0;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.InfraRedSpear>();
			}
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));
			damage *= player.statLife > player.statLifeMax / 2 ? 2 : 1;
		}
	}
}