using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace GMR
{
    // Disclaimer: I have no idea how this works, All code using this is possible thanks to Pellucid Mod


    /// <summary>Contains the data for a <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix)"/> call. Stole this one from LOLXD87 （￣︶￣)</summary>
    public struct SpriteBatchSnapshot
    {
        public SpriteSortMode SortMode;
        public BlendState BlendState;
        public SamplerState SamplerState;
        public DepthStencilState DepthStencilState;
        public RasterizerState RasterizerState;
        public Effect Effect;
        public Matrix TransformMatrix;

        public SpriteBatchSnapshot(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
        {
            SortMode = sortMode;
            BlendState = blendState;
            SamplerState = samplerState;
            DepthStencilState = depthStencilState;
            RasterizerState = rasterizerState;
            Effect = effect;
            TransformMatrix = transformMatrix;
        }

        /// <summary>Call <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix)"/> with the fields in this <see cref="SpriteBatchSnapshot"/> instance</summary>.
        /// <param name="spriteBatch">The spritebatch to begin.</param>
        public void Begin(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, TransformMatrix);
        }

        /// <summary>
        /// Obtains the data passed to <see cref="SpriteBatch.Begin"/> and saves it into a <see cref="SpriteBatchSnapshot"/>.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to capture it's data.</param>
        /// <returns></returns>
        /// <remarks>
        /// If <see cref="SpriteBatch.Begin"/> has not been called, the contents of the <see cref="SpriteBatchSnapshot"/> <br />
        /// are whatever is in the <paramref name="spriteBatch"/> instance.
        /// </remarks>
        public static SpriteBatchSnapshot Capture(SpriteBatch spriteBatch)
        {
            SpriteSortMode sortMode = (SpriteSortMode)spriteBatch.GetType().GetField("sortMode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            BlendState blendState = (BlendState)spriteBatch.GetType().GetField("blendState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            SamplerState samplerState = (SamplerState)spriteBatch.GetType().GetField("samplerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            DepthStencilState depthStencilState = (DepthStencilState)spriteBatch.GetType().GetField("depthStencilState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            RasterizerState rasterizerState = (RasterizerState)spriteBatch.GetType().GetField("rasterizerState", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Effect effect = (Effect)spriteBatch.GetType().GetField("customEffect", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);
            Matrix transformMatrix = (Matrix)spriteBatch.GetType().GetField("transformMatrix", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(spriteBatch);

            return new SpriteBatchSnapshot(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
        }
    }
}
