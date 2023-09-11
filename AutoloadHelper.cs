using Terraria.ModLoader;

namespace GMR
{
    internal class AutoloadSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            foreach (var t in GMR.Instance.GetContent<IPostSetupContent>())
            {
                t.PostSetupContent(GMR.Instance);
            }
        }

        public override void AddRecipeGroups()
        {
            foreach (var t in GMR.Instance.GetContent<IAddRecipeGroups>())
            {
                t.AddRecipeGroups(GMR.Instance);
            }
        }

        public override void AddRecipes()
        {
            foreach (var t in GMR.Instance.GetContent<IAddRecipes>())
            {
                t.AddRecipes(GMR.Instance);
            }
        }

        public override void PostAddRecipes()
        {
            foreach (var t in GMR.Instance.GetContent<IPostAddRecipes>())
            {
                t.PostAddRecipes(GMR.Instance);
            }
        }
    }

    internal interface IOnModLoad : ILoadable
    {
        void OnModLoad(GMR gmod);
    }

    internal interface IPostSetupContent : ILoadable
    {
        void PostSetupContent(GMR gmod);
    }

    internal interface IAddRecipeGroups : ILoadable
    {
        void AddRecipeGroups(GMR gmod);
    }

    internal interface IAddRecipes : ILoadable
    {
        void AddRecipes(GMR gmod);
    }

    internal interface IPostAddRecipes : ILoadable
    {
        void PostAddRecipes(GMR gmod);
    }
}