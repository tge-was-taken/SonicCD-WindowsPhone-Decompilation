using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Retro_Engine
{
	// Token: 0x0200000A RID: 10
	public static class RenderDevice
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00009A1C File Offset: 0x00007C1C
		public static void InitRenderDevice(GraphicsDevice graphicsRef)
		{
			RenderDevice.gDevice = graphicsRef;
			RenderDevice.effect = new BasicEffect(RenderDevice.gDevice);
			RenderDevice.effect.TextureEnabled = true;
			GraphicsSystem.SetupPolygonLists();
			for (int i = 0; i < 6; i++)
			{
				RenderDevice.gfxTexture[i] = new Texture2D(RenderDevice.gDevice, 1024, 1024, false, SurfaceFormat.Bgra5551);
			}
			RenderDevice.renderTarget = new RenderTarget2D(RenderDevice.gDevice, 400, 240, false, SurfaceFormat.Bgr565, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			RenderDevice.rasterState.CullMode = CullMode.None;
			RenderDevice.gDevice.RasterizerState = RenderDevice.rasterState;
			RenderDevice.gDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
			RenderDevice.screenSprite = new SpriteBatch(RenderDevice.gDevice);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00009AD0 File Offset: 0x00007CD0
		public static void UpdateHardwareTextures()
		{
			RenderDevice.gDevice.Textures[0] = null;
			GraphicsSystem.SetActivePalette(0, 0, 240);
			GraphicsSystem.UpdateTextureBufferWithTiles();
			GraphicsSystem.UpdateTextureBufferWithSortedSprites();
			RenderDevice.gfxTexture[0].SetData<ushort>(GraphicsSystem.texBuffer);
			for (byte b = 1; b < 6; b += 1)
			{
				GraphicsSystem.SetActivePalette(b, 0, 240);
				GraphicsSystem.UpdateTextureBufferWithTiles();
				GraphicsSystem.UpdateTextureBufferWithSprites();
				RenderDevice.gfxTexture[(int)b].SetData<ushort>(GraphicsSystem.texBuffer);
			}
			GraphicsSystem.SetActivePalette(0, 0, 240);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00009B58 File Offset: 0x00007D58
		public static void SetScreenDimensions(int width, int height)
		{
			InputSystem.touchWidth = width;
			InputSystem.touchHeight = height;
			RenderDevice.viewWidth = InputSystem.touchWidth;
			RenderDevice.viewHeight = InputSystem.touchHeight;
			float num = (float)RenderDevice.viewWidth / (float)RenderDevice.viewHeight;
			num *= 240f;
			RenderDevice.bufferWidth = (int)num;
			RenderDevice.bufferWidth += 8;
			RenderDevice.bufferWidth = RenderDevice.bufferWidth >> 4 << 4;
			if (RenderDevice.bufferWidth > 400)
			{
				RenderDevice.bufferWidth = 400;
			}
			RenderDevice.viewAspect = 0.75f;
			if (RenderDevice.viewHeight >= 480)
			{
				GlobalAppDefinitions.HQ3DFloorEnabled = true;
			}
			else
			{
				GlobalAppDefinitions.HQ3DFloorEnabled = false;
			}
			if (RenderDevice.viewHeight >= 480)
			{
				GraphicsSystem.SetScreenRenderSize(RenderDevice.bufferWidth, RenderDevice.bufferWidth);
				RenderDevice.bufferWidth *= 2;
				RenderDevice.bufferHeight = 480;
			}
			else
			{
				RenderDevice.bufferHeight = 240;
				GraphicsSystem.SetScreenRenderSize(RenderDevice.bufferWidth, RenderDevice.bufferWidth);
			}
			RenderDevice.orthWidth = GlobalAppDefinitions.SCREEN_XSIZE * 16;
			RenderDevice.projection2D = Matrix.CreateOrthographicOffCenter(4f, (float)(RenderDevice.orthWidth + 4), 3844f, 4f, 0f, 100f);
			RenderDevice.projection3D = Matrix.CreatePerspectiveFieldOfView(1.8325957f, RenderDevice.viewAspect, 0.1f, 2000f) * Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, -0.045f, 0f);
			RenderDevice.screenRect = new Rectangle(0, 0, RenderDevice.viewWidth, RenderDevice.viewHeight);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00009CE0 File Offset: 0x00007EE0
		public static void FlipScreen()
		{
			RenderDevice.gDevice.SetRenderTarget(RenderDevice.renderTarget);
			RenderDevice.effect.Texture = RenderDevice.gfxTexture[GraphicsSystem.texPaletteNum];
			RenderDevice.effect.World = Matrix.Identity;
			RenderDevice.effect.View = Matrix.Identity;
			RenderDevice.effect.Projection = RenderDevice.projection2D;
			RenderDevice.effect.LightingEnabled = false;
			RenderDevice.effect.VertexColorEnabled = true;
			RenderDevice.gDevice.RasterizerState = RenderDevice.rasterState;
			if (GraphicsSystem.render3DEnabled)
			{
				foreach (EffectPass effectPass in RenderDevice.effect.CurrentTechnique.Passes)
				{
					effectPass.Apply();
					RenderDevice.gDevice.BlendState = BlendState.Opaque;
					RenderDevice.gDevice.SamplerStates[0] = SamplerState.PointClamp;
					if (GraphicsSystem.gfxIndexSizeOpaque > 0)
					{
						RenderDevice.effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, 0, (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, (int)GraphicsSystem.gfxIndexSizeOpaque);
					}
				}
				RenderDevice.gDevice.BlendState = BlendState.NonPremultiplied;
				RenderDevice.effect.World = Matrix.CreateTranslation(GraphicsSystem.floor3DPos) * Matrix.CreateRotationY(3.1415927f * (180f + GraphicsSystem.floor3DAngle) / 180f);
				RenderDevice.effect.Projection = RenderDevice.projection3D;
				foreach (EffectPass effectPass2 in RenderDevice.effect.CurrentTechnique.Passes)
				{
					effectPass2.Apply();
					if (GraphicsSystem.indexSize3D > 0)
					{
						RenderDevice.effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex3D>(PrimitiveType.TriangleList, GraphicsSystem.polyList3D, 0, (int)GraphicsSystem.vertexSize3D, GraphicsSystem.gfxPolyListIndex, 0, (int)GraphicsSystem.indexSize3D);
					}
				}
				RenderDevice.effect.World = Matrix.Identity;
				RenderDevice.effect.Projection = RenderDevice.projection2D;
				using (List<EffectPass>.Enumerator enumerator3 = RenderDevice.effect.CurrentTechnique.Passes.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						EffectPass effectPass3 = enumerator3.Current;
						effectPass3.Apply();
						int num = (int)(GraphicsSystem.gfxIndexSize - GraphicsSystem.gfxIndexSizeOpaque);
						if (num > 0)
						{
							RenderDevice.effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, (int)GraphicsSystem.gfxVertexSizeOpaque, (int)(GraphicsSystem.gfxVertexSize - GraphicsSystem.gfxVertexSizeOpaque), GraphicsSystem.gfxPolyListIndex, 0, num);
						}
					}
					goto IL_351;
				}
			}
			foreach (EffectPass effectPass4 in RenderDevice.effect.CurrentTechnique.Passes)
			{
				effectPass4.Apply();
				RenderDevice.gDevice.BlendState = BlendState.Opaque;
				RenderDevice.gDevice.SamplerStates[0] = SamplerState.PointClamp;
				if (GraphicsSystem.gfxIndexSizeOpaque > 0)
				{
					RenderDevice.effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, 0, (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, (int)GraphicsSystem.gfxIndexSizeOpaque);
				}
				RenderDevice.gDevice.BlendState = BlendState.NonPremultiplied;
				int num = (int)(GraphicsSystem.gfxIndexSize - GraphicsSystem.gfxIndexSizeOpaque);
				if (num > 0)
				{
					RenderDevice.effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, (int)GraphicsSystem.gfxVertexSizeOpaque, (int)(GraphicsSystem.gfxVertexSize - GraphicsSystem.gfxVertexSizeOpaque), GraphicsSystem.gfxPolyListIndex, 0, num);
				}
			}
			IL_351:
			RenderDevice.effect.Texture = null;
			RenderDevice.gDevice.SetRenderTarget(null);
			RenderDevice.screenSprite.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
			RenderDevice.screenSprite.Draw(RenderDevice.renderTarget, RenderDevice.screenRect, Color.White);
			RenderDevice.screenSprite.End();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000A0CC File Offset: 0x000082CC
		public static void FlipScreenHRes()
		{
			RenderDevice.effect.Texture = RenderDevice.gfxTexture[GraphicsSystem.texPaletteNum];
			RenderDevice.effect.World = Matrix.Identity;
			RenderDevice.effect.View = Matrix.Identity;
			RenderDevice.effect.Projection = RenderDevice.projection2D;
			RenderDevice.effect.LightingEnabled = false;
			RenderDevice.effect.VertexColorEnabled = true;
			RenderDevice.gDevice.RasterizerState = RenderDevice.rasterState;
			foreach (EffectPass effectPass in RenderDevice.effect.CurrentTechnique.Passes)
			{
				effectPass.Apply();
				RenderDevice.gDevice.BlendState = BlendState.Opaque;
				RenderDevice.gDevice.SamplerStates[0] = SamplerState.LinearClamp;
				if (GraphicsSystem.gfxIndexSizeOpaque > 0)
				{
					RenderDevice.effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, 0, (int)GraphicsSystem.gfxVertexSizeOpaque, GraphicsSystem.gfxPolyListIndex, 0, (int)GraphicsSystem.gfxIndexSizeOpaque);
				}
				RenderDevice.gDevice.BlendState = BlendState.NonPremultiplied;
				int num = (int)(GraphicsSystem.gfxIndexSize - GraphicsSystem.gfxIndexSizeOpaque);
				if (num > 0)
				{
					RenderDevice.effect.GraphicsDevice.DrawUserIndexedPrimitives<DrawVertex>(PrimitiveType.TriangleList, GraphicsSystem.gfxPolyList, (int)GraphicsSystem.gfxVertexSizeOpaque, (int)(GraphicsSystem.gfxVertexSize - GraphicsSystem.gfxVertexSizeOpaque), GraphicsSystem.gfxPolyListIndex, 0, num);
				}
			}
			RenderDevice.effect.Texture = null;
		}

		// Token: 0x040000AD RID: 173
		public const int NUM_TEXTURES = 6;

		// Token: 0x040000AE RID: 174
		private static GraphicsDevice gDevice;

		// Token: 0x040000AF RID: 175
		private static BasicEffect effect;

		// Token: 0x040000B0 RID: 176
		private static Matrix projection2D;

		// Token: 0x040000B1 RID: 177
		private static Matrix projection3D;

		// Token: 0x040000B2 RID: 178
		private static RenderTarget2D renderTarget;

		// Token: 0x040000B3 RID: 179
		private static SpriteBatch screenSprite;

		// Token: 0x040000B4 RID: 180
		private static Rectangle screenRect;

		// Token: 0x040000B5 RID: 181
		private static RasterizerState rasterState = new RasterizerState();

		// Token: 0x040000B6 RID: 182
		public static Texture2D[] gfxTexture = new Texture2D[6];

		// Token: 0x040000B7 RID: 183
		public static int orthWidth;

		// Token: 0x040000B8 RID: 184
		public static int viewWidth;

		// Token: 0x040000B9 RID: 185
		public static int viewHeight;

		// Token: 0x040000BA RID: 186
		public static float viewAspect;

		// Token: 0x040000BB RID: 187
		public static int bufferWidth;

		// Token: 0x040000BC RID: 188
		public static int bufferHeight;

		// Token: 0x040000BD RID: 189
		public static int highResMode = 0;

		// Token: 0x040000BE RID: 190
		public static bool useFBTexture = true;
	}
}
