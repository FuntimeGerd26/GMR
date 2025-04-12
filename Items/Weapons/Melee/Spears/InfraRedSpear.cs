using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace GMR.Items.Weapons.Melee.Spears
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
			Item.AddElement(0);
			Item.AddElement(2);
		}

		public override void SetDefaults()
		{
			Item.rare = 4;
			Item.width = 112;
			Item.height = 112;
			Item.value = Item.sellPrice(silver: 180); 
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.UseSound = GMR.GetSounds("Items/Melee/swordSwoosh", 7, 0.66f, 0f, 0.2f).WithPitchOffset(Main.rand.NextFloat(0.5f));
			Item.autoReuse = true; 
			Item.damage = 55;
			Item.knockBack = 8f;
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

		float combo;
		public override bool CanUseItem(Player player)
		{
			if (combo == 2f)
			{
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.InfraRedSpearSpin>();
			}
			else if (combo < 2f)
			{
				Item.useTime = 15;
				Item.useAnimation = 15;
				Item.shoot = ModContent.ProjectileType<Projectiles.Melee.InfraRedSpear>();
			}

			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (combo >= 2f)
				combo = -1f;
			combo += 1f;

			velocity = velocity.RotatedByRandom(MathHelper.ToRadians(6));
			damage *= player.statLife < player.statLifeMax / 2 ? 2 : 1;
		}
	}
}