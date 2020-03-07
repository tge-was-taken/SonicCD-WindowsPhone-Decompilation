using System;
using Microsoft.Xna.Framework.Input;

namespace Retro_Engine
{
	// Token: 0x02000030 RID: 48
	public static class InputSystem
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00031BF4 File Offset: 0x0002FDF4
		public static void AddTouch(float touchX, float touchY, int pointerID)
		{
			for (int i = 0; i < 4; i++)
			{
				if (InputSystem.touchData.touchDown[i] == 0)
				{
					InputSystem.touchData.touchDown[i] = 1;
					InputSystem.touchData.touchY[i] = (int)touchY;
					InputSystem.touchData.touchX[i] = (int)touchX;
					InputSystem.touchData.touchID[i] = pointerID;
					i = 4;
				}
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00031C54 File Offset: 0x0002FE54
		public static void SetTouch(float touchX, float touchY, int pointerID)
		{
			for (int i = 0; i < 4; i++)
			{
				if (InputSystem.touchData.touchID[i] == pointerID && InputSystem.touchData.touchDown[i] == 1)
				{
					InputSystem.touchData.touchY[i] = (int)touchY;
					InputSystem.touchData.touchX[i] = (int)touchX;
					i = 4;
				}
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00031CAC File Offset: 0x0002FEAC
		public static void RemoveTouch(int pointerID)
		{
			for (int i = 0; i < 4; i++)
			{
				if (InputSystem.touchData.touchID[i] == pointerID)
				{
					InputSystem.touchData.touchDown[i] = 0;
				}
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00031CE4 File Offset: 0x0002FEE4
		public static void ClearTouchData()
		{
			InputSystem.touchData.touches = 0;
			InputSystem.touchData.touchDown[0] = 0;
			InputSystem.touchData.touchDown[1] = 0;
			InputSystem.touchData.touchDown[2] = 0;
			InputSystem.touchData.touchDown[3] = 0;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00031D30 File Offset: 0x0002FF30
		public static void CheckKeyboardInput()
		{
			try
			{
				KeyboardState state = Keyboard.GetState();
				if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
				{
					InputSystem.touchData.up = 1;
				}
				else
				{
					InputSystem.touchData.up = 0;
				}
				if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
				{
					InputSystem.touchData.down = 1;
				}
				else
				{
					InputSystem.touchData.down = 0;
				}
				if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
				{
					InputSystem.touchData.left = 1;
				}
				else
				{
					InputSystem.touchData.left = 0;
				}
				if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
				{
					InputSystem.touchData.right = 1;
				}
				else
				{
					InputSystem.touchData.right = 0;
				}
				if (state.IsKeyDown(Keys.NumPad1) || state.IsKeyDown(Keys.J))
				{
					InputSystem.touchData.buttonA = 1;
				}
				else
				{
					InputSystem.touchData.buttonA = 0;
				}
				if (state.IsKeyDown(Keys.NumPad2) || state.IsKeyDown(Keys.K))
				{
					InputSystem.touchData.buttonB = 1;
				}
				else
				{
					InputSystem.touchData.buttonB = 0;
				}
				if (state.IsKeyDown(Keys.NumPad3) || state.IsKeyDown(Keys.L))
				{
					InputSystem.touchData.buttonC = 1;
				}
				else
				{
					InputSystem.touchData.buttonC = 0;
				}
				if (state.IsKeyDown(Keys.Enter) || state.IsKeyDown(Keys.V))
				{
					InputSystem.touchData.start = 1;
				}
				else
				{
					InputSystem.touchData.start = 0;
				}
				if (state.IsKeyDown(Keys.Space))
				{
					InputSystem.touchControls = true;
				}
				else
				{
					if (InputSystem.touchControls)
					{
						ObjectSystem.globalVariables[110] = (ObjectSystem.globalVariables[110] + 1 & 1);
					}
					InputSystem.touchControls = false;
				}
			}
			catch
			{
				InputSystem.touchData.up = 0;
				InputSystem.touchData.down = 0;
				InputSystem.touchData.left = 0;
				InputSystem.touchData.right = 0;
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00031F34 File Offset: 0x00030134
		public static void CheckKeyDown(InputResult gameInput, byte keyFlags)
		{
			gameInput.touches = 0;
			for (int i = 0; i < 4; i++)
			{
				if (InputSystem.touchData.touchDown[i] == 1)
				{
					gameInput.touchDown[gameInput.touches] = InputSystem.touchData.touchDown[i];
					gameInput.touchX[gameInput.touches] = InputSystem.touchData.touchX[i] * GlobalAppDefinitions.SCREEN_XSIZE / InputSystem.touchWidth;
					gameInput.touchY[gameInput.touches] = InputSystem.touchData.touchY[i] * 240 / InputSystem.touchHeight;
					gameInput.touches++;
				}
			}
			if ((1 & keyFlags) == 1)
			{
				gameInput.up = InputSystem.touchData.up;
			}
			if ((2 & keyFlags) == 2)
			{
				gameInput.down = InputSystem.touchData.down;
			}
			if ((4 & keyFlags) == 4)
			{
				gameInput.left = InputSystem.touchData.left;
			}
			if ((8 & keyFlags) == 8)
			{
				gameInput.right = InputSystem.touchData.right;
			}
			if ((16 & keyFlags) == 16)
			{
				gameInput.buttonA = InputSystem.touchData.buttonA;
			}
			if ((32 & keyFlags) == 32)
			{
				gameInput.buttonB = InputSystem.touchData.buttonB;
			}
			if ((64 & keyFlags) == 64)
			{
				gameInput.buttonC = InputSystem.touchData.buttonC;
			}
			if ((128 & keyFlags) == 128)
			{
				gameInput.start = InputSystem.touchData.start;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00032098 File Offset: 0x00030298
		public static void MenuKeyDown(InputResult gameInput, byte keyFlags)
		{
			gameInput.touches = 0;
			for (int i = 0; i < 4; i++)
			{
				if (InputSystem.touchData.touchDown[i] == 1)
				{
					gameInput.touchDown[gameInput.touches] = InputSystem.touchData.touchDown[i];
					gameInput.touchX[gameInput.touches] = InputSystem.touchData.touchX[i] * GlobalAppDefinitions.SCREEN_XSIZE / InputSystem.touchWidth;
					gameInput.touchY[gameInput.touches] = InputSystem.touchData.touchY[i] * 240 / InputSystem.touchHeight;
					gameInput.touches++;
				}
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00032140 File Offset: 0x00030340
		public static void CheckKeyPress(InputResult gameInput, byte keyFlags)
		{
			if ((1 & keyFlags) == 1)
			{
				if (InputSystem.touchData.up == 1)
				{
					if (InputSystem.inputPress.up == 0)
					{
						InputSystem.inputPress.up = 1;
						gameInput.up = 1;
					}
					else
					{
						gameInput.up = 0;
					}
				}
				else
				{
					gameInput.up = 0;
					InputSystem.inputPress.up = 0;
				}
			}
			if ((2 & keyFlags) == 2)
			{
				if (InputSystem.touchData.down == 1)
				{
					if (InputSystem.inputPress.down == 0)
					{
						InputSystem.inputPress.down = 1;
						gameInput.down = 1;
					}
					else
					{
						gameInput.down = 0;
					}
				}
				else
				{
					gameInput.down = 0;
					InputSystem.inputPress.down = 0;
				}
			}
			if ((4 & keyFlags) == 4)
			{
				if (InputSystem.touchData.left == 1)
				{
					if (InputSystem.inputPress.left == 0)
					{
						InputSystem.inputPress.left = 1;
						gameInput.left = 1;
					}
					else
					{
						gameInput.left = 0;
					}
				}
				else
				{
					gameInput.left = 0;
					InputSystem.inputPress.left = 0;
				}
			}
			if ((8 & keyFlags) == 8)
			{
				if (InputSystem.touchData.right == 1)
				{
					if (InputSystem.inputPress.right == 0)
					{
						InputSystem.inputPress.right = 1;
						gameInput.right = 1;
					}
					else
					{
						gameInput.right = 0;
					}
				}
				else
				{
					gameInput.right = 0;
					InputSystem.inputPress.right = 0;
				}
			}
			if ((16 & keyFlags) == 16)
			{
				if (InputSystem.touchData.buttonA == 1)
				{
					if (InputSystem.inputPress.buttonA == 0)
					{
						InputSystem.inputPress.buttonA = 1;
						gameInput.buttonA = 1;
					}
					else
					{
						gameInput.buttonA = 0;
					}
				}
				else
				{
					gameInput.buttonA = 0;
					InputSystem.inputPress.buttonA = 0;
				}
			}
			if ((32 & keyFlags) == 32)
			{
				if (InputSystem.touchData.buttonB == 1)
				{
					if (InputSystem.inputPress.buttonB == 0)
					{
						InputSystem.inputPress.buttonB = 1;
						gameInput.buttonB = 1;
					}
					else
					{
						gameInput.buttonB = 0;
					}
				}
				else
				{
					gameInput.buttonB = 0;
					InputSystem.inputPress.buttonB = 0;
				}
			}
			if ((64 & keyFlags) == 64)
			{
				if (InputSystem.touchData.buttonC == 1)
				{
					if (InputSystem.inputPress.buttonC == 0)
					{
						InputSystem.inputPress.buttonC = 1;
						gameInput.buttonC = 1;
					}
					else
					{
						gameInput.buttonC = 0;
					}
				}
				else
				{
					gameInput.buttonC = 0;
					InputSystem.inputPress.buttonC = 0;
				}
			}
			if ((128 & keyFlags) == 128)
			{
				if (InputSystem.touchData.start == 1)
				{
					if (InputSystem.inputPress.start == 0)
					{
						InputSystem.inputPress.start = 1;
						gameInput.start = 1;
						return;
					}
					gameInput.start = 0;
					return;
				}
				else
				{
					gameInput.start = 0;
					InputSystem.inputPress.start = 0;
				}
			}
		}

		// Token: 0x0400025A RID: 602
		public const int BUTTON_UP = 1;

		// Token: 0x0400025B RID: 603
		public const int BUTTON_DOWN = 2;

		// Token: 0x0400025C RID: 604
		public const int BUTTON_LEFT = 4;

		// Token: 0x0400025D RID: 605
		public const int BUTTON_RIGHT = 8;

		// Token: 0x0400025E RID: 606
		public const int BUTTON_A = 16;

		// Token: 0x0400025F RID: 607
		public const int BUTTON_B = 32;

		// Token: 0x04000260 RID: 608
		public const int BUTTON_C = 64;

		// Token: 0x04000261 RID: 609
		public const int BUTTON_START = 128;

		// Token: 0x04000262 RID: 610
		public const int ALL_BUTTONS = 255;

		// Token: 0x04000263 RID: 611
		public const int NO_BUTTONS = 0;

		// Token: 0x04000264 RID: 612
		public static int touchWidth;

		// Token: 0x04000265 RID: 613
		public static int touchHeight;

		// Token: 0x04000266 RID: 614
		public static bool touchControls = false;

		// Token: 0x04000267 RID: 615
		public static InputResult inputPress = new InputResult();

		// Token: 0x04000268 RID: 616
		public static InputResult touchData = new InputResult();
	}
}
