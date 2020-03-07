using System;

namespace Retro_Engine
{
	// Token: 0x02000002 RID: 2
	public static class PlayerSystem
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		static PlayerSystem()
		{
			for (int i = 0; i < PlayerSystem.playerList.Length; i++)
			{
				PlayerSystem.playerList[i] = new PlayerObject();
			}
			for (int i = 0; i < PlayerSystem.cSensor.Length; i++)
			{
				PlayerSystem.cSensor[i] = new CollisionSensor();
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020BC File Offset: 0x000002BC
		public static void SetPlayerScreenPosition(PlayerObject playerO)
		{
			int num = playerO.xPos >> 16;
			int num2 = playerO.yPos >> 16;
			if (StageSystem.newYBoundary1 > StageSystem.yBoundary1)
			{
				if (StageSystem.yScrollOffset > StageSystem.newYBoundary1)
				{
					StageSystem.yBoundary1 = StageSystem.newYBoundary1;
				}
				else
				{
					StageSystem.yBoundary1 = StageSystem.yScrollOffset;
				}
			}
			if (StageSystem.newYBoundary1 < StageSystem.yBoundary1)
			{
				if (StageSystem.yScrollOffset > StageSystem.yBoundary1)
				{
					StageSystem.yBoundary1 = StageSystem.newYBoundary1;
				}
				else
				{
					StageSystem.yBoundary1--;
				}
			}
			if (StageSystem.newYBoundary2 < StageSystem.yBoundary2)
			{
				if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2 && StageSystem.yScrollOffset + 240 > StageSystem.newYBoundary2)
				{
					StageSystem.yBoundary2 = StageSystem.yScrollOffset + 240;
				}
				else
				{
					StageSystem.yBoundary2--;
				}
			}
			if (StageSystem.newYBoundary2 > StageSystem.yBoundary2)
			{
				if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2)
				{
					StageSystem.yBoundary2 = StageSystem.newYBoundary2;
				}
				else
				{
					StageSystem.yBoundary2++;
				}
			}
			if (StageSystem.newXBoundary1 > StageSystem.xBoundary1)
			{
				if (StageSystem.xScrollOffset > StageSystem.newXBoundary1)
				{
					StageSystem.xBoundary1 = StageSystem.newXBoundary1;
				}
				else
				{
					StageSystem.xBoundary1 = StageSystem.xScrollOffset;
				}
			}
			if (StageSystem.newXBoundary1 < StageSystem.xBoundary1)
			{
				if (StageSystem.xScrollOffset > StageSystem.xBoundary1)
				{
					StageSystem.xBoundary1 = StageSystem.newXBoundary1;
				}
				else
				{
					StageSystem.xBoundary1--;
					if (playerO.xVelocity < 0)
					{
						StageSystem.xBoundary1 += playerO.xVelocity >> 16;
						if (StageSystem.xBoundary1 < StageSystem.newXBoundary1)
						{
							StageSystem.xBoundary1 = StageSystem.newXBoundary1;
						}
					}
				}
			}
			if (StageSystem.newXBoundary2 < StageSystem.xBoundary2)
			{
				if (StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE < StageSystem.xBoundary2)
				{
					StageSystem.xBoundary2 = StageSystem.newXBoundary2;
				}
				else
				{
					StageSystem.xBoundary2 = StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE;
				}
			}
			if (StageSystem.newXBoundary2 > StageSystem.xBoundary2)
			{
				if (StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE < StageSystem.xBoundary2)
				{
					StageSystem.xBoundary2 = StageSystem.newXBoundary2;
				}
				else
				{
					StageSystem.xBoundary2++;
					if (playerO.xVelocity > 0)
					{
						StageSystem.xBoundary2 += playerO.xVelocity >> 16;
						if (StageSystem.xBoundary2 > StageSystem.newXBoundary2)
						{
							StageSystem.xBoundary2 = StageSystem.newXBoundary2;
						}
					}
				}
			}
			int num3 = StageSystem.xScrollA;
			int num4 = StageSystem.xScrollB;
			int num5 = num - (num3 + GlobalAppDefinitions.SCREEN_CENTER);
			if (Math.Abs(num5) < 25)
			{
				if (num > num3 + GlobalAppDefinitions.SCREEN_SCROLL_RIGHT)
				{
					num3 += num - (num3 + GlobalAppDefinitions.SCREEN_SCROLL_RIGHT);
					num4 = num3 + GlobalAppDefinitions.SCREEN_XSIZE;
				}
				if (num < num3 + GlobalAppDefinitions.SCREEN_SCROLL_LEFT)
				{
					num3 -= num3 + GlobalAppDefinitions.SCREEN_SCROLL_LEFT - num;
					num4 = num3 + GlobalAppDefinitions.SCREEN_XSIZE;
				}
			}
			else
			{
				if (num5 > 0)
				{
					num3 += 16;
				}
				else
				{
					num3 -= 16;
				}
				num4 = num3 + GlobalAppDefinitions.SCREEN_XSIZE;
			}
			if (num3 < StageSystem.xBoundary1)
			{
				num3 = StageSystem.xBoundary1;
				num4 = StageSystem.xBoundary1 + GlobalAppDefinitions.SCREEN_XSIZE;
			}
			if (num4 > StageSystem.xBoundary2)
			{
				num4 = StageSystem.xBoundary2;
				num3 = StageSystem.xBoundary2 - GlobalAppDefinitions.SCREEN_XSIZE;
			}
			StageSystem.xScrollA = num3;
			StageSystem.xScrollB = num4;
			if (num > num3 + GlobalAppDefinitions.SCREEN_CENTER)
			{
				StageSystem.xScrollOffset = num - GlobalAppDefinitions.SCREEN_CENTER + StageSystem.screenShakeX;
				playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER - StageSystem.screenShakeX;
				if (num > num4 - GlobalAppDefinitions.SCREEN_CENTER)
				{
					playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER + (num - (num4 - GlobalAppDefinitions.SCREEN_CENTER)) + StageSystem.screenShakeX;
					StageSystem.xScrollOffset = num4 - GlobalAppDefinitions.SCREEN_XSIZE - StageSystem.screenShakeX;
				}
			}
			else
			{
				playerO.screenXPos = num - num3 + StageSystem.screenShakeX;
				StageSystem.xScrollOffset = num3 - StageSystem.screenShakeX;
			}
			num3 = StageSystem.yScrollA;
			num4 = StageSystem.yScrollB;
			num2 += StageSystem.cameraAdjustY;
			num5 = num2 + playerO.lookPos - (num3 + 104);
			if (playerO.trackScroll == 1)
			{
				StageSystem.yScrollMove = 32;
			}
			else
			{
				if (StageSystem.yScrollMove == 32)
				{
					StageSystem.yScrollMove = 104 - playerO.screenYPos - playerO.lookPos >> 1 << 1;
					if (StageSystem.yScrollMove > 32)
					{
						StageSystem.yScrollMove = 32;
					}
					if (StageSystem.yScrollMove < -32)
					{
						StageSystem.yScrollMove = -32;
					}
				}
				if (StageSystem.yScrollMove > 0)
				{
					StageSystem.yScrollMove -= 6;
					if (StageSystem.yScrollMove < 0)
					{
						StageSystem.yScrollMove = StageSystem.yScrollMove;
					}
				}
				if (StageSystem.yScrollMove < 0)
				{
					StageSystem.yScrollMove += 6;
					if (StageSystem.yScrollMove > 0)
					{
						StageSystem.yScrollMove = StageSystem.yScrollMove;
					}
				}
			}
			if (Math.Abs(num5) < Math.Abs(StageSystem.yScrollMove) + 17)
			{
				if (StageSystem.yScrollMove == 32)
				{
					if (num2 + playerO.lookPos > num3 + 104 + StageSystem.yScrollMove)
					{
						num3 += num2 + playerO.lookPos - (num3 + 104 + StageSystem.yScrollMove);
						num4 = num3 + 240;
					}
					if (num2 + playerO.lookPos < num3 + 104 - StageSystem.yScrollMove)
					{
						num3 -= num3 + 104 - StageSystem.yScrollMove - (num2 + playerO.lookPos);
						num4 = num3 + 240;
					}
				}
				else
				{
					num3 = num2 - 104 + StageSystem.yScrollMove + playerO.lookPos;
					num4 = num3 + 240;
				}
			}
			else
			{
				if (num5 > 0)
				{
					num3 += 16;
				}
				else
				{
					num3 -= 16;
				}
				num4 = num3 + 240;
			}
			if (num3 < StageSystem.yBoundary1)
			{
				num3 = StageSystem.yBoundary1;
				num4 = StageSystem.yBoundary1 + 240;
			}
			if (num4 > StageSystem.yBoundary2)
			{
				num4 = StageSystem.yBoundary2;
				num3 = StageSystem.yBoundary2 - 240;
			}
			StageSystem.yScrollA = num3;
			StageSystem.yScrollB = num4;
			if (num2 + playerO.lookPos > num3 + 104)
			{
				StageSystem.yScrollOffset = num2 - 104 + playerO.lookPos + StageSystem.screenShakeY;
				playerO.screenYPos = 104 - playerO.lookPos - StageSystem.screenShakeY;
				if (num2 + playerO.lookPos > num4 - 136)
				{
					playerO.screenYPos = 104 + (num2 - (num4 - 136)) + StageSystem.screenShakeY;
					StageSystem.yScrollOffset = num4 - 240 - StageSystem.screenShakeY;
				}
			}
			else
			{
				playerO.screenYPos = num2 - num3 - StageSystem.screenShakeY;
				StageSystem.yScrollOffset = num3 + StageSystem.screenShakeY;
			}
			playerO.screenYPos -= StageSystem.cameraAdjustY;
			if (StageSystem.screenShakeX != 0)
			{
				if (StageSystem.screenShakeX > 0)
				{
					StageSystem.screenShakeX = -StageSystem.screenShakeX;
				}
				else
				{
					StageSystem.screenShakeX = -StageSystem.screenShakeX;
					StageSystem.screenShakeX--;
				}
			}
			if (StageSystem.screenShakeY != 0)
			{
				if (StageSystem.screenShakeY > 0)
				{
					StageSystem.screenShakeY = -StageSystem.screenShakeY;
					return;
				}
				StageSystem.screenShakeY = -StageSystem.screenShakeY;
				StageSystem.screenShakeY--;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000026F8 File Offset: 0x000008F8
		public static void SetPlayerScreenPositionCDStyle(PlayerObject playerO)
		{
			int num = playerO.xPos >> 16;
			int num2 = playerO.yPos >> 16;
			if (StageSystem.newYBoundary1 > StageSystem.yBoundary1)
			{
				if (StageSystem.yScrollOffset > StageSystem.newYBoundary1)
				{
					StageSystem.yBoundary1 = StageSystem.newYBoundary1;
				}
				else
				{
					StageSystem.yBoundary1 = StageSystem.yScrollOffset;
				}
			}
			if (StageSystem.newYBoundary1 < StageSystem.yBoundary1)
			{
				if (StageSystem.yScrollOffset > StageSystem.yBoundary1)
				{
					StageSystem.yBoundary1 = StageSystem.newYBoundary1;
				}
				else
				{
					StageSystem.yBoundary1--;
				}
			}
			if (StageSystem.newYBoundary2 < StageSystem.yBoundary2)
			{
				if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2 && StageSystem.yScrollOffset + 240 > StageSystem.newYBoundary2)
				{
					StageSystem.yBoundary2 = StageSystem.yScrollOffset + 240;
				}
				else
				{
					StageSystem.yBoundary2--;
				}
			}
			if (StageSystem.newYBoundary2 > StageSystem.yBoundary2)
			{
				if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2)
				{
					StageSystem.yBoundary2 = StageSystem.newYBoundary2;
				}
				else
				{
					StageSystem.yBoundary2++;
				}
			}
			if (StageSystem.newXBoundary1 > StageSystem.xBoundary1)
			{
				if (StageSystem.xScrollOffset > StageSystem.newXBoundary1)
				{
					StageSystem.xBoundary1 = StageSystem.newXBoundary1;
				}
				else
				{
					StageSystem.xBoundary1 = StageSystem.xScrollOffset;
				}
			}
			if (StageSystem.newXBoundary1 < StageSystem.xBoundary1)
			{
				if (StageSystem.xScrollOffset > StageSystem.xBoundary1)
				{
					StageSystem.xBoundary1 = StageSystem.newXBoundary1;
				}
				else
				{
					StageSystem.xBoundary1--;
					if (playerO.xVelocity < 0)
					{
						StageSystem.xBoundary1 += playerO.xVelocity >> 16;
						if (StageSystem.xBoundary1 < StageSystem.newXBoundary1)
						{
							StageSystem.xBoundary1 = StageSystem.newXBoundary1;
						}
					}
				}
			}
			if (StageSystem.newXBoundary2 < StageSystem.xBoundary2)
			{
				if (StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE < StageSystem.xBoundary2)
				{
					StageSystem.xBoundary2 = StageSystem.newXBoundary2;
				}
				else
				{
					StageSystem.xBoundary2 = StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE;
				}
			}
			if (StageSystem.newXBoundary2 > StageSystem.xBoundary2)
			{
				if (StageSystem.xScrollOffset + GlobalAppDefinitions.SCREEN_XSIZE < StageSystem.xBoundary2)
				{
					StageSystem.xBoundary2 = StageSystem.newXBoundary2;
				}
				else
				{
					StageSystem.xBoundary2++;
					if (playerO.xVelocity > 0)
					{
						StageSystem.xBoundary2 += playerO.xVelocity >> 16;
						if (StageSystem.xBoundary2 > StageSystem.newXBoundary2)
						{
							StageSystem.xBoundary2 = StageSystem.newXBoundary2;
						}
					}
				}
			}
			if (playerO.gravity == 0)
			{
				if (playerO.objectPtr.direction == 0)
				{
					if (playerO.speed > 390594 | StageSystem.cameraStyle == 2)
					{
						StageSystem.cameraShift = 1;
					}
					else
					{
						StageSystem.cameraShift = 0;
					}
				}
				else if (playerO.speed < -390594 | StageSystem.cameraStyle == 3)
				{
					StageSystem.cameraShift = 2;
				}
				else
				{
					StageSystem.cameraShift = 0;
				}
			}
			switch (StageSystem.cameraShift)
			{
			case 0:
				if (StageSystem.xScrollMove < 0)
				{
					StageSystem.xScrollMove += 2;
				}
				if (StageSystem.xScrollMove > 0)
				{
					StageSystem.xScrollMove -= 2;
				}
				break;
			case 1:
				if (StageSystem.xScrollMove > -64)
				{
					StageSystem.xScrollMove -= 2;
				}
				break;
			case 2:
				if (StageSystem.xScrollMove < 64)
				{
					StageSystem.xScrollMove += 2;
				}
				break;
			}
			if (num > StageSystem.xBoundary1 + GlobalAppDefinitions.SCREEN_CENTER + StageSystem.xScrollMove)
			{
				StageSystem.xScrollOffset = num - GlobalAppDefinitions.SCREEN_CENTER + StageSystem.screenShakeX - StageSystem.xScrollMove;
				playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER - StageSystem.screenShakeX + StageSystem.xScrollMove;
				if (num - StageSystem.xScrollMove > StageSystem.xBoundary2 - GlobalAppDefinitions.SCREEN_CENTER)
				{
					playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER + (num - (StageSystem.xBoundary2 - GlobalAppDefinitions.SCREEN_CENTER)) + StageSystem.screenShakeX;
					StageSystem.xScrollOffset = StageSystem.xBoundary2 - GlobalAppDefinitions.SCREEN_XSIZE - StageSystem.screenShakeX;
				}
			}
			else
			{
				playerO.screenXPos = num - StageSystem.xBoundary1 + StageSystem.screenShakeX;
				StageSystem.xScrollOffset = StageSystem.xBoundary1 - StageSystem.screenShakeX;
			}
			StageSystem.xScrollA = StageSystem.xScrollOffset;
			StageSystem.xScrollB = StageSystem.xScrollA + GlobalAppDefinitions.SCREEN_XSIZE;
			int num3 = StageSystem.yScrollA;
			int num4 = StageSystem.yScrollB;
			num2 += StageSystem.cameraAdjustY;
			int num5 = num2 + playerO.lookPos - (num3 + 104);
			if (playerO.trackScroll == 1)
			{
				StageSystem.yScrollMove = 32;
			}
			else
			{
				if (StageSystem.yScrollMove == 32)
				{
					StageSystem.yScrollMove = 104 - playerO.screenYPos - playerO.lookPos >> 1 << 1;
					if (StageSystem.yScrollMove > 32)
					{
						StageSystem.yScrollMove = 32;
					}
					if (StageSystem.yScrollMove < -32)
					{
						StageSystem.yScrollMove = -32;
					}
				}
				if (StageSystem.yScrollMove > 0)
				{
					StageSystem.yScrollMove -= 6;
					if (StageSystem.yScrollMove < 0)
					{
						StageSystem.yScrollMove = StageSystem.yScrollMove;
					}
				}
				if (StageSystem.yScrollMove < 0)
				{
					StageSystem.yScrollMove += 6;
					if (StageSystem.yScrollMove > 0)
					{
						StageSystem.yScrollMove = StageSystem.yScrollMove;
					}
				}
			}
			if (Math.Abs(num5) < Math.Abs(StageSystem.yScrollMove) + 17)
			{
				if (StageSystem.yScrollMove == 32)
				{
					if (num2 + playerO.lookPos > num3 + 104 + StageSystem.yScrollMove)
					{
						num3 += num2 + playerO.lookPos - (num3 + 104 + StageSystem.yScrollMove);
						num4 = num3 + 240;
					}
					if (num2 + playerO.lookPos < num3 + 104 - StageSystem.yScrollMove)
					{
						num3 -= num3 + 104 - StageSystem.yScrollMove - (num2 + playerO.lookPos);
						num4 = num3 + 240;
					}
				}
				else
				{
					num3 = num2 - 104 + StageSystem.yScrollMove + playerO.lookPos;
					num4 = num3 + 240;
				}
			}
			else
			{
				if (num5 > 0)
				{
					num3 += 16;
				}
				else
				{
					num3 -= 16;
				}
				num4 = num3 + 240;
			}
			if (num3 < StageSystem.yBoundary1)
			{
				num3 = StageSystem.yBoundary1;
				num4 = StageSystem.yBoundary1 + 240;
			}
			if (num4 > StageSystem.yBoundary2)
			{
				num4 = StageSystem.yBoundary2;
				num3 = StageSystem.yBoundary2 - 240;
			}
			StageSystem.yScrollA = num3;
			StageSystem.yScrollB = num4;
			if (num2 + playerO.lookPos > num3 + 104)
			{
				StageSystem.yScrollOffset = num2 - 104 + playerO.lookPos + StageSystem.screenShakeY;
				playerO.screenYPos = 104 - playerO.lookPos - StageSystem.screenShakeY;
				if (num2 + playerO.lookPos > num4 - 136)
				{
					playerO.screenYPos = 104 + (num2 - (num4 - 136)) + StageSystem.screenShakeY;
					StageSystem.yScrollOffset = num4 - 240 - StageSystem.screenShakeY;
				}
			}
			else
			{
				playerO.screenYPos = num2 - num3 - StageSystem.screenShakeY;
				StageSystem.yScrollOffset = num3 + StageSystem.screenShakeY;
			}
			playerO.screenYPos -= StageSystem.cameraAdjustY;
			if (StageSystem.screenShakeX != 0)
			{
				if (StageSystem.screenShakeX > 0)
				{
					StageSystem.screenShakeX = -StageSystem.screenShakeX;
				}
				else
				{
					StageSystem.screenShakeX = -StageSystem.screenShakeX;
					StageSystem.screenShakeX--;
				}
			}
			if (StageSystem.screenShakeY != 0)
			{
				if (StageSystem.screenShakeY > 0)
				{
					StageSystem.screenShakeY = -StageSystem.screenShakeY;
					return;
				}
				StageSystem.screenShakeY = -StageSystem.screenShakeY;
				StageSystem.screenShakeY--;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002D9C File Offset: 0x00000F9C
		public static void SetPlayerLockedScreenPosition(PlayerObject playerO)
		{
			int num = playerO.xPos >> 16;
			int num2 = playerO.yPos >> 16;
			int num3 = StageSystem.xScrollA;
			int num4 = StageSystem.xScrollB;
			if (num > num3 + GlobalAppDefinitions.SCREEN_CENTER)
			{
				StageSystem.xScrollOffset = num - GlobalAppDefinitions.SCREEN_CENTER + StageSystem.screenShakeX;
				playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER - StageSystem.screenShakeX;
				if (num > num4 - GlobalAppDefinitions.SCREEN_CENTER)
				{
					playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER + (num - (num4 - GlobalAppDefinitions.SCREEN_CENTER)) + StageSystem.screenShakeX;
					StageSystem.xScrollOffset = num4 - GlobalAppDefinitions.SCREEN_XSIZE - StageSystem.screenShakeX;
				}
			}
			else
			{
				playerO.screenXPos = num - num3 + StageSystem.screenShakeX;
				StageSystem.xScrollOffset = num3 - StageSystem.screenShakeX;
			}
			num3 = StageSystem.yScrollA;
			num4 = StageSystem.yScrollB;
			num2 += StageSystem.cameraAdjustY;
			int lookPos = playerO.lookPos;
			StageSystem.yScrollA = num3;
			StageSystem.yScrollB = num4;
			if (num2 + playerO.lookPos > num3 + 104)
			{
				StageSystem.yScrollOffset = num2 - 104 + playerO.lookPos + StageSystem.screenShakeY;
				playerO.screenYPos = 104 - playerO.lookPos - StageSystem.screenShakeY;
				if (num2 + playerO.lookPos > num4 - 136)
				{
					playerO.screenYPos = 104 + (num2 - (num4 - 136)) + StageSystem.screenShakeY;
					StageSystem.yScrollOffset = num4 - 240 - StageSystem.screenShakeY;
				}
			}
			else
			{
				playerO.screenYPos = num2 - num3 - StageSystem.screenShakeY;
				StageSystem.yScrollOffset = num3 + StageSystem.screenShakeY;
			}
			playerO.screenYPos -= StageSystem.cameraAdjustY;
			if (StageSystem.screenShakeX != 0)
			{
				if (StageSystem.screenShakeX > 0)
				{
					StageSystem.screenShakeX = -StageSystem.screenShakeX;
				}
				else
				{
					StageSystem.screenShakeX = -StageSystem.screenShakeX;
					StageSystem.screenShakeX--;
				}
			}
			if (StageSystem.screenShakeY != 0)
			{
				if (StageSystem.screenShakeY > 0)
				{
					StageSystem.screenShakeY = -StageSystem.screenShakeY;
					return;
				}
				StageSystem.screenShakeY = -StageSystem.screenShakeY;
				StageSystem.screenShakeY--;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002F7C File Offset: 0x0000117C
		public static void SetPlayerHLockedScreenPosition(PlayerObject playerO)
		{
			int num = playerO.xPos >> 16;
			int num2 = playerO.yPos >> 16;
			if (StageSystem.newYBoundary1 > StageSystem.yBoundary1)
			{
				if (StageSystem.yScrollOffset > StageSystem.newYBoundary1)
				{
					StageSystem.yBoundary1 = StageSystem.newYBoundary1;
				}
				else
				{
					StageSystem.yBoundary1 = StageSystem.yScrollOffset;
				}
			}
			if (StageSystem.newYBoundary1 < StageSystem.yBoundary1)
			{
				if (StageSystem.yScrollOffset > StageSystem.yBoundary1)
				{
					StageSystem.yBoundary1 = StageSystem.newYBoundary1;
				}
				else
				{
					StageSystem.yBoundary1--;
				}
			}
			if (StageSystem.newYBoundary2 < StageSystem.yBoundary2)
			{
				if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2 && StageSystem.yScrollOffset + 240 > StageSystem.newYBoundary2)
				{
					StageSystem.yBoundary2 = StageSystem.yScrollOffset + 240;
				}
				else
				{
					StageSystem.yBoundary2--;
				}
			}
			if (StageSystem.newYBoundary2 > StageSystem.yBoundary2)
			{
				if (StageSystem.yScrollOffset + 240 < StageSystem.yBoundary2)
				{
					StageSystem.yBoundary2 = StageSystem.newYBoundary2;
				}
				else
				{
					StageSystem.yBoundary2++;
				}
			}
			int num3 = StageSystem.xScrollA;
			int num4 = StageSystem.xScrollB;
			if (num > num3 + GlobalAppDefinitions.SCREEN_CENTER)
			{
				StageSystem.xScrollOffset = num - GlobalAppDefinitions.SCREEN_CENTER + StageSystem.screenShakeX;
				playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER - StageSystem.screenShakeX;
				if (num > num4 - GlobalAppDefinitions.SCREEN_CENTER)
				{
					playerO.screenXPos = GlobalAppDefinitions.SCREEN_CENTER + (num - (num4 - GlobalAppDefinitions.SCREEN_CENTER)) + StageSystem.screenShakeX;
					StageSystem.xScrollOffset = num4 - GlobalAppDefinitions.SCREEN_XSIZE - StageSystem.screenShakeX;
				}
			}
			else
			{
				playerO.screenXPos = num - num3 + StageSystem.screenShakeX;
				StageSystem.xScrollOffset = num3 - StageSystem.screenShakeX;
			}
			num3 = StageSystem.yScrollA;
			num4 = StageSystem.yScrollB;
			num2 += StageSystem.cameraAdjustY;
			int num5 = num2 + playerO.lookPos - (num3 + 104);
			if (playerO.trackScroll == 1)
			{
				StageSystem.yScrollMove = 32;
			}
			else
			{
				if (StageSystem.yScrollMove == 32)
				{
					StageSystem.yScrollMove = 104 - playerO.screenYPos - playerO.lookPos >> 1 << 1;
					if (StageSystem.yScrollMove > 32)
					{
						StageSystem.yScrollMove = 32;
					}
					if (StageSystem.yScrollMove < -32)
					{
						StageSystem.yScrollMove = -32;
					}
				}
				if (StageSystem.yScrollMove > 0)
				{
					StageSystem.yScrollMove -= 6;
					if (StageSystem.yScrollMove < 0)
					{
						StageSystem.yScrollMove = StageSystem.yScrollMove;
					}
				}
				if (StageSystem.yScrollMove < 0)
				{
					StageSystem.yScrollMove += 6;
					if (StageSystem.yScrollMove > 0)
					{
						StageSystem.yScrollMove = StageSystem.yScrollMove;
					}
				}
			}
			if (Math.Abs(num5) < Math.Abs(StageSystem.yScrollMove) + 17)
			{
				if (StageSystem.yScrollMove == 32)
				{
					if (num2 + playerO.lookPos > num3 + 104 + StageSystem.yScrollMove)
					{
						num3 += num2 + playerO.lookPos - (num3 + 104 + StageSystem.yScrollMove);
						num4 = num3 + 240;
					}
					if (num2 + playerO.lookPos < num3 + 104 - StageSystem.yScrollMove)
					{
						num3 -= num3 + 104 - StageSystem.yScrollMove - (num2 + playerO.lookPos);
						num4 = num3 + 240;
					}
				}
				else
				{
					num3 = num2 - 104 + StageSystem.yScrollMove + playerO.lookPos;
					num4 = num3 + 240;
				}
			}
			else
			{
				if (num5 > 0)
				{
					num3 += 16;
				}
				else
				{
					num3 -= 16;
				}
				num4 = num3 + 240;
			}
			if (num3 < StageSystem.yBoundary1)
			{
				num3 = StageSystem.yBoundary1;
				num4 = StageSystem.yBoundary1 + 240;
			}
			if (num4 > StageSystem.yBoundary2)
			{
				num4 = StageSystem.yBoundary2;
				num3 = StageSystem.yBoundary2 - 240;
			}
			StageSystem.yScrollA = num3;
			StageSystem.yScrollB = num4;
			if (num2 + playerO.lookPos > num3 + 104)
			{
				StageSystem.yScrollOffset = num2 - 104 + playerO.lookPos + StageSystem.screenShakeY;
				playerO.screenYPos = 104 - playerO.lookPos - StageSystem.screenShakeY;
				if (num2 + playerO.lookPos > num4 - 136)
				{
					playerO.screenYPos = 104 + (num2 - (num4 - 136)) + StageSystem.screenShakeY;
					StageSystem.yScrollOffset = num4 - 240 - StageSystem.screenShakeY;
				}
			}
			else
			{
				playerO.screenYPos = num2 - num3 - StageSystem.screenShakeY;
				StageSystem.yScrollOffset = num3 + StageSystem.screenShakeY;
			}
			playerO.screenYPos -= StageSystem.cameraAdjustY;
			if (StageSystem.screenShakeX != 0)
			{
				if (StageSystem.screenShakeX > 0)
				{
					StageSystem.screenShakeX = -StageSystem.screenShakeX;
				}
				else
				{
					StageSystem.screenShakeX = -StageSystem.screenShakeX;
					StageSystem.screenShakeX--;
				}
			}
			if (StageSystem.screenShakeY != 0)
			{
				if (StageSystem.screenShakeY > 0)
				{
					StageSystem.screenShakeY = -StageSystem.screenShakeY;
					return;
				}
				StageSystem.screenShakeY = -StageSystem.screenShakeY;
				StageSystem.screenShakeY--;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000033D8 File Offset: 0x000015D8
		public static void ProcessPlayerControl(PlayerObject playerO)
		{
			switch (playerO.controlMode)
			{
			case -1:
				PlayerSystem.delayUp = (ushort)(PlayerSystem.delayUp << 1);
				PlayerSystem.delayUp |= (ushort)playerO.up;
				PlayerSystem.delayDown = (ushort)(PlayerSystem.delayDown << 1);
				PlayerSystem.delayDown |= (ushort)playerO.down;
				PlayerSystem.delayLeft = (ushort)(PlayerSystem.delayLeft << 1);
				PlayerSystem.delayLeft |= (ushort)playerO.left;
				PlayerSystem.delayRight = (ushort)(PlayerSystem.delayRight << 1);
				PlayerSystem.delayRight |= (ushort)playerO.right;
				PlayerSystem.delayJumpPress = (ushort)(PlayerSystem.delayJumpPress << 1);
				PlayerSystem.delayJumpPress |= (ushort)playerO.jumpPress;
				PlayerSystem.delayJumpHold = (ushort)(PlayerSystem.delayJumpHold << 1);
				PlayerSystem.delayJumpHold |= (ushort)playerO.jumpHold;
				return;
			case 0:
				playerO.up = StageSystem.gKeyDown.up;
				playerO.down = StageSystem.gKeyDown.down;
				if (StageSystem.gKeyDown.left == 1 && StageSystem.gKeyDown.right == 1)
				{
					playerO.left = 0;
					playerO.right = 0;
				}
				else
				{
					playerO.left = StageSystem.gKeyDown.left;
					playerO.right = StageSystem.gKeyDown.right;
				}
				playerO.jumpHold = (byte)(StageSystem.gKeyDown.buttonA | StageSystem.gKeyDown.buttonB | StageSystem.gKeyDown.buttonC);
				playerO.jumpPress = (byte)(StageSystem.gKeyPress.buttonA | StageSystem.gKeyPress.buttonB | StageSystem.gKeyPress.buttonC);
				PlayerSystem.delayUp = (ushort)(PlayerSystem.delayUp << 1);
				PlayerSystem.delayUp |= (ushort)playerO.up;
				PlayerSystem.delayDown = (ushort)(PlayerSystem.delayDown << 1);
				PlayerSystem.delayDown |= (ushort)playerO.down;
				PlayerSystem.delayLeft = (ushort)(PlayerSystem.delayLeft << 1);
				PlayerSystem.delayLeft |= (ushort)playerO.left;
				PlayerSystem.delayRight = (ushort)(PlayerSystem.delayRight << 1);
				PlayerSystem.delayRight |= (ushort)playerO.right;
				PlayerSystem.delayJumpPress = (ushort)(PlayerSystem.delayJumpPress << 1);
				PlayerSystem.delayJumpPress |= (ushort)playerO.jumpPress;
				PlayerSystem.delayJumpHold = (ushort)(PlayerSystem.delayJumpHold << 1);
				PlayerSystem.delayJumpHold |= (ushort)playerO.jumpHold;
				return;
			case 1:
				playerO.up = (byte)(PlayerSystem.delayUp >> 15);
				playerO.down = (byte)(PlayerSystem.delayDown >> 15);
				playerO.left = (byte)(PlayerSystem.delayLeft >> 15);
				playerO.right = (byte)(PlayerSystem.delayRight >> 15);
				playerO.jumpPress = (byte)(PlayerSystem.delayJumpPress >> 15);
				playerO.jumpHold = (byte)(PlayerSystem.delayJumpHold >> 15);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00003689 File Offset: 0x00001889
		public static void ProcessPlayerTileCollisions(PlayerObject playerO)
		{
			playerO.flailing[0] = 0;
			playerO.flailing[1] = 0;
			playerO.flailing[2] = 0;
			ObjectSystem.scriptEng.checkResult = 0;
			if (playerO.gravity == 1)
			{
				PlayerSystem.ProcessAirCollision(playerO);
				return;
			}
			PlayerSystem.ProcessPathGrip(playerO);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000036C8 File Offset: 0x000018C8
		public static void ProcessAirCollision(PlayerObject playerO)
		{
			CollisionBox collisionBox = AnimationSystem.collisionBoxList[playerO.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[playerO.animationFile.aniListOffset + (int)playerO.objectPtr.animation].frameListOffset + (int)playerO.objectPtr.frame].collisionBox];
			PlayerSystem.collisionLeft = (int)collisionBox.left[0];
			PlayerSystem.collisionTop = (int)collisionBox.top[0];
			PlayerSystem.collisionRight = (int)collisionBox.right[0];
			PlayerSystem.collisionBottom = (int)collisionBox.bottom[0];
			byte b;
			if (playerO.xVelocity >= 0)
			{
				b = 1;
				PlayerSystem.cSensor[0].yPos = playerO.yPos + 131072;
				PlayerSystem.cSensor[0].collided = 0;
				PlayerSystem.cSensor[0].xPos = playerO.xPos + (PlayerSystem.collisionRight << 16);
			}
			else
			{
				b = 0;
			}
			byte b2;
			if (playerO.xVelocity <= 0)
			{
				b2 = 1;
				PlayerSystem.cSensor[1].yPos = playerO.yPos + 131072;
				PlayerSystem.cSensor[1].collided = 0;
				PlayerSystem.cSensor[1].xPos = playerO.xPos + (PlayerSystem.collisionLeft - 1 << 16);
			}
			else
			{
				b2 = 0;
			}
			PlayerSystem.cSensor[2].xPos = playerO.xPos + ((int)collisionBox.left[1] << 16);
			PlayerSystem.cSensor[3].xPos = playerO.xPos + ((int)collisionBox.right[1] << 16);
			PlayerSystem.cSensor[2].collided = 0;
			PlayerSystem.cSensor[3].collided = 0;
			PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[2].xPos;
			PlayerSystem.cSensor[5].xPos = PlayerSystem.cSensor[3].xPos;
			PlayerSystem.cSensor[4].collided = 0;
			PlayerSystem.cSensor[5].collided = 0;
			byte b3;
			if (playerO.yVelocity >= 0)
			{
				b3 = 1;
				PlayerSystem.cSensor[2].yPos = playerO.yPos + (PlayerSystem.collisionBottom << 16);
				PlayerSystem.cSensor[3].yPos = playerO.yPos + (PlayerSystem.collisionBottom << 16);
			}
			else
			{
				b3 = 0;
			}
			byte b4 = 1;
			PlayerSystem.cSensor[4].yPos = playerO.yPos + (PlayerSystem.collisionTop - 1 << 16);
			PlayerSystem.cSensor[5].yPos = playerO.yPos + (PlayerSystem.collisionTop - 1 << 16);
			int i;
			if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity))
			{
				i = (Math.Abs(playerO.xVelocity) >> 19) + 1;
			}
			else
			{
				i = (Math.Abs(playerO.yVelocity) >> 19) + 1;
			}
			int num = playerO.xVelocity / i;
			int num2 = playerO.yVelocity / i;
			int num3 = playerO.xVelocity - num * (i - 1);
			int num4 = playerO.yVelocity - num2 * (i - 1);
			while (i > 0)
			{
				if (i < 2)
				{
					num = num3;
					num2 = num4;
				}
				i--;
				if (b == 1)
				{
					PlayerSystem.cSensor[0].xPos += num + 65536;
					PlayerSystem.cSensor[0].yPos += num2;
					PlayerSystem.LWallCollision(playerO, PlayerSystem.cSensor[0]);
					if (PlayerSystem.cSensor[0].collided == 1)
					{
						b = 2;
					}
				}
				if (b2 == 1)
				{
					PlayerSystem.cSensor[1].xPos += num - 65536;
					PlayerSystem.cSensor[1].yPos += num2;
					PlayerSystem.RWallCollision(playerO, PlayerSystem.cSensor[1]);
					if (PlayerSystem.cSensor[1].collided == 1)
					{
						b2 = 2;
					}
				}
				if (b == 2)
				{
					playerO.xVelocity = 0;
					playerO.speed = 0;
					playerO.xPos = PlayerSystem.cSensor[0].xPos - PlayerSystem.collisionRight << 16;
					PlayerSystem.cSensor[2].xPos = playerO.xPos + (PlayerSystem.collisionLeft + 1 << 16);
					PlayerSystem.cSensor[3].xPos = playerO.xPos + (PlayerSystem.collisionRight - 2 << 16);
					PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[2].xPos;
					PlayerSystem.cSensor[5].xPos = PlayerSystem.cSensor[3].xPos;
					num = 0;
					num3 = 0;
					b = 3;
				}
				if (b2 == 2)
				{
					playerO.xVelocity = 0;
					playerO.speed = 0;
					playerO.xPos = PlayerSystem.cSensor[1].xPos - PlayerSystem.collisionLeft + 1 << 16;
					PlayerSystem.cSensor[2].xPos = playerO.xPos + (PlayerSystem.collisionLeft + 1 << 16);
					PlayerSystem.cSensor[3].xPos = playerO.xPos + (PlayerSystem.collisionRight - 2 << 16);
					PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[2].xPos;
					PlayerSystem.cSensor[5].xPos = PlayerSystem.cSensor[3].xPos;
					num = 0;
					num3 = 0;
					b2 = 3;
				}
				if (b3 == 1)
				{
					for (int j = 2; j < 4; j++)
					{
						if (PlayerSystem.cSensor[j].collided == 0)
						{
							PlayerSystem.cSensor[j].xPos += num;
							PlayerSystem.cSensor[j].yPos += num2;
							PlayerSystem.FloorCollision(playerO, PlayerSystem.cSensor[j]);
						}
					}
					if (PlayerSystem.cSensor[2].collided == 1 | PlayerSystem.cSensor[3].collided == 1)
					{
						b3 = 2;
						i = 0;
					}
				}
				if (b4 == 1)
				{
					for (int j = 4; j < 6; j++)
					{
						if (PlayerSystem.cSensor[j].collided == 0)
						{
							PlayerSystem.cSensor[j].xPos += num;
							PlayerSystem.cSensor[j].yPos += num2;
							PlayerSystem.RoofCollision(playerO, PlayerSystem.cSensor[j]);
						}
					}
					if (PlayerSystem.cSensor[4].collided == 1 | PlayerSystem.cSensor[5].collided == 1)
					{
						b4 = 2;
						i = 0;
					}
				}
			}
			if (b < 2 && b2 < 2)
			{
				playerO.xPos += playerO.xVelocity;
			}
			if (b4 < 2 && b3 < 2)
			{
				playerO.yPos += playerO.yVelocity;
				return;
			}
			if (b3 == 2)
			{
				playerO.gravity = 0;
				if (PlayerSystem.cSensor[2].collided == 1 && PlayerSystem.cSensor[3].collided == 1)
				{
					if (PlayerSystem.cSensor[2].yPos < PlayerSystem.cSensor[3].yPos)
					{
						playerO.yPos = PlayerSystem.cSensor[2].yPos - PlayerSystem.collisionBottom << 16;
						playerO.angle = PlayerSystem.cSensor[2].angle;
					}
					else
					{
						playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionBottom << 16;
						playerO.angle = PlayerSystem.cSensor[3].angle;
					}
				}
				else if (PlayerSystem.cSensor[2].collided == 1)
				{
					playerO.yPos = PlayerSystem.cSensor[2].yPos - PlayerSystem.collisionBottom << 16;
					playerO.angle = PlayerSystem.cSensor[2].angle;
				}
				else if (PlayerSystem.cSensor[3].collided == 1)
				{
					playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionBottom << 16;
					playerO.angle = PlayerSystem.cSensor[3].angle;
				}
				if (playerO.angle > 160 && playerO.angle < 224 && playerO.collisionMode != 1)
				{
					playerO.collisionMode = 1;
					playerO.xPos -= 262144;
				}
				if (playerO.angle > 32 && playerO.angle < 96 && playerO.collisionMode != 3)
				{
					playerO.collisionMode = 3;
					playerO.xPos += 262144;
				}
				if (playerO.angle < 32 | playerO.angle > 224)
				{
					playerO.controlLock = 0;
				}
				playerO.objectPtr.rotation = playerO.angle << 1;
				int j;
				if (playerO.down == 1)
				{
					if (playerO.angle < 128)
					{
						if (playerO.angle < 16)
						{
							j = playerO.xVelocity;
						}
						else if (playerO.angle < 32)
						{
							if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity >> 1))
							{
								j = playerO.xVelocity;
							}
							else
							{
								j = playerO.yVelocity + playerO.yVelocity / 12 >> 1;
							}
						}
						else if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity))
						{
							j = playerO.xVelocity;
						}
						else
						{
							j = playerO.yVelocity + playerO.yVelocity / 12;
						}
					}
					else if (playerO.angle > 240)
					{
						j = playerO.xVelocity;
					}
					else if (playerO.angle > 224)
					{
						if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity >> 1))
						{
							j = playerO.xVelocity;
						}
						else
						{
							j = -(playerO.yVelocity + playerO.yVelocity / 12 >> 1);
						}
					}
					else if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity))
					{
						j = playerO.xVelocity;
					}
					else
					{
						j = -(playerO.yVelocity + playerO.yVelocity / 12);
					}
				}
				else if (playerO.angle < 128)
				{
					if (playerO.angle < 16)
					{
						j = playerO.xVelocity;
					}
					else if (playerO.angle < 32)
					{
						if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity >> 1))
						{
							j = playerO.xVelocity;
						}
						else
						{
							j = playerO.yVelocity >> 1;
						}
					}
					else if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity))
					{
						j = playerO.xVelocity;
					}
					else
					{
						j = playerO.yVelocity;
					}
				}
				else if (playerO.angle > 240)
				{
					j = playerO.xVelocity;
				}
				else if (playerO.angle > 224)
				{
					if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity >> 1))
					{
						j = playerO.xVelocity;
					}
					else
					{
						j = -(playerO.yVelocity >> 1);
					}
				}
				else if (Math.Abs(playerO.xVelocity) > Math.Abs(playerO.yVelocity))
				{
					j = playerO.xVelocity;
				}
				else
				{
					j = -playerO.yVelocity;
				}
				if (j < -1572864)
				{
					j = -1572864;
				}
				if (j > 1572864)
				{
					j = 1572864;
				}
				playerO.speed = j;
				playerO.yVelocity = 0;
				ObjectSystem.scriptEng.checkResult = 1;
			}
			if (b4 == 2)
			{
				if (PlayerSystem.cSensor[4].collided == 1 && PlayerSystem.cSensor[5].collided == 1)
				{
					if (PlayerSystem.cSensor[4].yPos > PlayerSystem.cSensor[5].yPos)
					{
						playerO.yPos = PlayerSystem.cSensor[4].yPos - PlayerSystem.collisionTop + 1 << 16;
						i = PlayerSystem.cSensor[4].angle;
					}
					else
					{
						playerO.yPos = PlayerSystem.cSensor[5].yPos - PlayerSystem.collisionTop + 1 << 16;
						i = PlayerSystem.cSensor[5].angle;
					}
				}
				else if (PlayerSystem.cSensor[4].collided == 1)
				{
					playerO.yPos = PlayerSystem.cSensor[4].yPos - PlayerSystem.collisionTop + 1 << 16;
					i = PlayerSystem.cSensor[4].angle;
				}
				else if (PlayerSystem.cSensor[5].collided == 1)
				{
					playerO.yPos = PlayerSystem.cSensor[5].yPos - PlayerSystem.collisionTop + 1 << 16;
					i = PlayerSystem.cSensor[5].angle;
				}
				i &= 255;
				int j = (int)GlobalAppDefinitions.ArcTanLookup(playerO.xVelocity, playerO.yVelocity);
				if (i > 64 && i < 98 && j > 160 && j < 194)
				{
					playerO.gravity = 0;
					playerO.angle = i;
					playerO.objectPtr.rotation = playerO.angle << 1;
					playerO.collisionMode = 3;
					playerO.xPos += 262144;
					playerO.yPos -= 131072;
					if (playerO.angle > 96)
					{
						playerO.speed = playerO.yVelocity >> 1;
					}
					else
					{
						playerO.speed = playerO.yVelocity;
					}
				}
				if (i > 158 && i < 192 && j > 190 && j < 224)
				{
					playerO.gravity = 0;
					playerO.angle = i;
					playerO.objectPtr.rotation = playerO.angle << 1;
					playerO.collisionMode = 1;
					playerO.xPos -= 262144;
					playerO.yPos -= 131072;
					if (playerO.angle < 160)
					{
						playerO.speed = -playerO.yVelocity >> 1;
					}
					else
					{
						playerO.speed = -playerO.yVelocity;
					}
				}
				if (playerO.yVelocity < 0)
				{
					playerO.yVelocity = 0;
				}
				ObjectSystem.scriptEng.checkResult = 2;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000043CC File Offset: 0x000025CC
		public static void SetPathGripSensors(PlayerObject playerO)
		{
			CollisionBox collisionBox = AnimationSystem.collisionBoxList[playerO.animationFile.cbListOffset + (int)AnimationSystem.animationFrames[AnimationSystem.animationList[playerO.animationFile.aniListOffset + (int)playerO.objectPtr.animation].frameListOffset + (int)playerO.objectPtr.frame].collisionBox];
			switch (playerO.collisionMode)
			{
			case 0:
				PlayerSystem.collisionLeft = (int)collisionBox.left[0];
				PlayerSystem.collisionTop = (int)collisionBox.top[0];
				PlayerSystem.collisionRight = (int)collisionBox.right[0];
				PlayerSystem.collisionBottom = (int)collisionBox.bottom[0];
				PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionBottom << 16);
				PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[0].yPos;
				PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[0].yPos;
				PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + 262144;
				PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[4].xPos + ((int)(collisionBox.left[1] - 1) << 16);
				PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[4].xPos;
				PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[4].xPos + ((int)collisionBox.right[1] << 16);
				if (playerO.speed > 0)
				{
					PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionRight + 1 << 16);
					return;
				}
				PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionLeft - 1 << 16);
				return;
			case 1:
				PlayerSystem.collisionLeft = (int)collisionBox.left[2];
				PlayerSystem.collisionTop = (int)collisionBox.top[2];
				PlayerSystem.collisionRight = (int)collisionBox.right[2];
				PlayerSystem.collisionBottom = (int)collisionBox.bottom[2];
				PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionRight << 16);
				PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[0].xPos;
				PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[0].xPos;
				PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + 262144;
				PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[4].yPos + ((int)(collisionBox.top[3] - 1) << 16);
				PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[4].yPos;
				PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[4].yPos + ((int)collisionBox.bottom[3] << 16);
				if (playerO.speed > 0)
				{
					PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionTop << 16);
					return;
				}
				PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionBottom - 1 << 16);
				return;
			case 2:
				PlayerSystem.collisionLeft = (int)collisionBox.left[4];
				PlayerSystem.collisionTop = (int)collisionBox.top[4];
				PlayerSystem.collisionRight = (int)collisionBox.right[4];
				PlayerSystem.collisionBottom = (int)collisionBox.bottom[4];
				PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionTop - 1 << 16);
				PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[0].yPos;
				PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[0].yPos;
				PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos - 262144;
				PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[4].xPos + ((int)(collisionBox.left[5] - 1) << 16);
				PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[4].xPos;
				PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[4].xPos + ((int)collisionBox.right[5] << 16);
				if (playerO.speed < 0)
				{
					PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionRight + 1 << 16);
					return;
				}
				PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionLeft - 1 << 16);
				return;
			case 3:
				PlayerSystem.collisionLeft = (int)collisionBox.left[6];
				PlayerSystem.collisionTop = (int)collisionBox.top[6];
				PlayerSystem.collisionRight = (int)collisionBox.right[6];
				PlayerSystem.collisionBottom = (int)collisionBox.bottom[6];
				PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[4].xPos + (PlayerSystem.collisionLeft - 1 << 16);
				PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[0].xPos;
				PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[0].xPos;
				PlayerSystem.cSensor[3].xPos = PlayerSystem.cSensor[4].xPos - 262144;
				PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[4].yPos + ((int)(collisionBox.top[7] - 1) << 16);
				PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[4].yPos;
				PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[4].yPos + ((int)collisionBox.bottom[7] << 16);
				if (playerO.speed > 0)
				{
					PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionBottom << 16);
					return;
				}
				PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[4].yPos + (PlayerSystem.collisionTop - 1 << 16);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000499C File Offset: 0x00002B9C
		public static void ProcessPathGrip(PlayerObject playerO)
		{
			int num = -1;
			PlayerSystem.cSensor[4].xPos = playerO.xPos;
			PlayerSystem.cSensor[4].yPos = playerO.yPos;
			PlayerSystem.cSensor[3].collided = 0;
			PlayerSystem.cSensor[0].angle = playerO.angle;
			PlayerSystem.cSensor[1].angle = playerO.angle;
			PlayerSystem.cSensor[2].angle = playerO.angle;
			PlayerSystem.SetPathGripSensors(playerO);
			int num2 = Math.Abs(playerO.speed);
			int i = num2 >> 18;
			num2 &= 262143;
			while (i > -1)
			{
				int num3;
				int num4;
				if (i < 1)
				{
					num3 = GlobalAppDefinitions.CosValue256[playerO.angle] * num2 >> 8;
					num4 = GlobalAppDefinitions.SinValue256[playerO.angle] * num2 >> 8;
					i = -1;
				}
				else
				{
					num3 = GlobalAppDefinitions.CosValue256[playerO.angle] << 10;
					num4 = GlobalAppDefinitions.SinValue256[playerO.angle] << 10;
					i--;
				}
				if (playerO.speed < 0)
				{
					num3 = -num3;
					num4 = -num4;
				}
				PlayerSystem.cSensor[0].collided = 0;
				PlayerSystem.cSensor[1].collided = 0;
				PlayerSystem.cSensor[2].collided = 0;
				PlayerSystem.cSensor[4].xPos += num3;
				PlayerSystem.cSensor[4].yPos += num4;
				switch (playerO.collisionMode)
				{
				case 0:
					PlayerSystem.cSensor[3].xPos += num3;
					PlayerSystem.cSensor[3].yPos += num4;
					if (playerO.speed > 0)
					{
						PlayerSystem.LWallCollision(playerO, PlayerSystem.cSensor[3]);
					}
					if (playerO.speed < 0)
					{
						PlayerSystem.RWallCollision(playerO, PlayerSystem.cSensor[3]);
					}
					if (PlayerSystem.cSensor[3].collided == 1)
					{
						num3 = 0;
						i = -1;
					}
					for (int j = 0; j < 3; j++)
					{
						PlayerSystem.cSensor[j].xPos += num3;
						PlayerSystem.cSensor[j].yPos += num4;
						PlayerSystem.FindFloorPosition(playerO, PlayerSystem.cSensor[j], PlayerSystem.cSensor[j].yPos >> 16);
					}
					num = -1;
					for (int j = 0; j < 3; j++)
					{
						if (num > -1)
						{
							if (PlayerSystem.cSensor[j].collided == 1)
							{
								if (PlayerSystem.cSensor[j].yPos < PlayerSystem.cSensor[num].yPos)
								{
									num = j;
								}
								if (PlayerSystem.cSensor[j].yPos == PlayerSystem.cSensor[num].yPos && (PlayerSystem.cSensor[j].angle < 8 | PlayerSystem.cSensor[j].angle > 248))
								{
									num = j;
								}
							}
						}
						else if (PlayerSystem.cSensor[j].collided == 1)
						{
							num = j;
						}
					}
					if (num > -1)
					{
						PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[num].yPos << 16;
						PlayerSystem.cSensor[0].angle = PlayerSystem.cSensor[num].angle;
						PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[0].yPos;
						PlayerSystem.cSensor[1].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[0].yPos;
						PlayerSystem.cSensor[2].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[0].yPos - 262144;
						PlayerSystem.cSensor[3].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[1].xPos;
						PlayerSystem.cSensor[4].yPos = PlayerSystem.cSensor[1].yPos - (PlayerSystem.collisionBottom << 16);
					}
					else
					{
						i = -1;
					}
					if (PlayerSystem.cSensor[0].angle < 222 && PlayerSystem.cSensor[0].angle > 128)
					{
						playerO.collisionMode = 1;
					}
					if (PlayerSystem.cSensor[0].angle > 34 && PlayerSystem.cSensor[0].angle < 128)
					{
						playerO.collisionMode = 3;
					}
					break;
				case 1:
					PlayerSystem.cSensor[3].xPos += num3;
					PlayerSystem.cSensor[3].yPos += num4;
					if (playerO.speed > 0)
					{
						PlayerSystem.RoofCollision(playerO, PlayerSystem.cSensor[3]);
					}
					if (playerO.speed < 0)
					{
						PlayerSystem.FloorCollision(playerO, PlayerSystem.cSensor[3]);
					}
					if (PlayerSystem.cSensor[3].collided == 1)
					{
						num4 = 0;
						i = -1;
					}
					for (int j = 0; j < 3; j++)
					{
						PlayerSystem.cSensor[j].xPos += num3;
						PlayerSystem.cSensor[j].yPos += num4;
						PlayerSystem.FindLWallPosition(playerO, PlayerSystem.cSensor[j], PlayerSystem.cSensor[j].xPos >> 16);
					}
					num = -1;
					for (int j = 0; j < 3; j++)
					{
						if (num > -1)
						{
							if (PlayerSystem.cSensor[j].xPos < PlayerSystem.cSensor[num].xPos && PlayerSystem.cSensor[j].collided == 1)
							{
								num = j;
							}
						}
						else if (PlayerSystem.cSensor[j].collided == 1)
						{
							num = j;
						}
					}
					if (num > -1)
					{
						PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[num].xPos << 16;
						PlayerSystem.cSensor[0].angle = PlayerSystem.cSensor[num].angle;
						PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[0].xPos;
						PlayerSystem.cSensor[1].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[0].xPos;
						PlayerSystem.cSensor[2].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[4].yPos = PlayerSystem.cSensor[1].yPos;
						PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[1].xPos - (PlayerSystem.collisionRight << 16);
					}
					else
					{
						i = -1;
					}
					if (PlayerSystem.cSensor[0].angle > 226)
					{
						playerO.collisionMode = 0;
					}
					if (PlayerSystem.cSensor[0].angle < 158)
					{
						playerO.collisionMode = 2;
					}
					break;
				case 2:
					PlayerSystem.cSensor[3].xPos += num3;
					PlayerSystem.cSensor[3].yPos += num4;
					if (playerO.speed > 0)
					{
						PlayerSystem.RWallCollision(playerO, PlayerSystem.cSensor[3]);
					}
					if (playerO.speed < 0)
					{
						PlayerSystem.LWallCollision(playerO, PlayerSystem.cSensor[3]);
					}
					if (PlayerSystem.cSensor[3].collided == 1)
					{
						num3 = 0;
						i = -1;
					}
					for (int j = 0; j < 3; j++)
					{
						PlayerSystem.cSensor[j].xPos += num3;
						PlayerSystem.cSensor[j].yPos += num4;
						PlayerSystem.FindRoofPosition(playerO, PlayerSystem.cSensor[j], PlayerSystem.cSensor[j].yPos >> 16);
					}
					num = -1;
					for (int j = 0; j < 3; j++)
					{
						if (num > -1)
						{
							if (PlayerSystem.cSensor[j].yPos > PlayerSystem.cSensor[num].yPos && PlayerSystem.cSensor[j].collided == 1)
							{
								num = j;
							}
						}
						else if (PlayerSystem.cSensor[j].collided == 1)
						{
							num = j;
						}
					}
					if (num > -1)
					{
						PlayerSystem.cSensor[0].yPos = PlayerSystem.cSensor[num].yPos << 16;
						PlayerSystem.cSensor[0].angle = PlayerSystem.cSensor[num].angle;
						PlayerSystem.cSensor[1].yPos = PlayerSystem.cSensor[0].yPos;
						PlayerSystem.cSensor[1].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[2].yPos = PlayerSystem.cSensor[0].yPos;
						PlayerSystem.cSensor[2].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[3].yPos = PlayerSystem.cSensor[0].yPos + 262144;
						PlayerSystem.cSensor[3].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[1].xPos;
						PlayerSystem.cSensor[4].yPos = PlayerSystem.cSensor[1].yPos - (PlayerSystem.collisionTop - 1 << 16);
					}
					else
					{
						i = -1;
					}
					if (PlayerSystem.cSensor[0].angle > 162)
					{
						playerO.collisionMode = 1;
					}
					if (PlayerSystem.cSensor[0].angle < 94)
					{
						playerO.collisionMode = 3;
					}
					break;
				case 3:
					PlayerSystem.cSensor[3].xPos += num3;
					PlayerSystem.cSensor[3].yPos += num4;
					if (playerO.speed > 0)
					{
						PlayerSystem.FloorCollision(playerO, PlayerSystem.cSensor[3]);
					}
					if (playerO.speed < 0)
					{
						PlayerSystem.RoofCollision(playerO, PlayerSystem.cSensor[3]);
					}
					if (PlayerSystem.cSensor[3].collided == 1)
					{
						num4 = 0;
						i = -1;
					}
					for (int j = 0; j < 3; j++)
					{
						PlayerSystem.cSensor[j].xPos += num3;
						PlayerSystem.cSensor[j].yPos += num4;
						PlayerSystem.FindRWallPosition(playerO, PlayerSystem.cSensor[j], PlayerSystem.cSensor[j].xPos >> 16);
					}
					num = -1;
					for (int j = 0; j < 3; j++)
					{
						if (num > -1)
						{
							if (PlayerSystem.cSensor[j].xPos > PlayerSystem.cSensor[num].xPos && PlayerSystem.cSensor[j].collided == 1)
							{
								num = j;
							}
						}
						else if (PlayerSystem.cSensor[j].collided == 1)
						{
							num = j;
						}
					}
					if (num > -1)
					{
						PlayerSystem.cSensor[0].xPos = PlayerSystem.cSensor[num].xPos << 16;
						PlayerSystem.cSensor[0].angle = PlayerSystem.cSensor[num].angle;
						PlayerSystem.cSensor[1].xPos = PlayerSystem.cSensor[0].xPos;
						PlayerSystem.cSensor[1].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[2].xPos = PlayerSystem.cSensor[0].xPos;
						PlayerSystem.cSensor[2].angle = PlayerSystem.cSensor[0].angle;
						PlayerSystem.cSensor[4].yPos = PlayerSystem.cSensor[1].yPos;
						PlayerSystem.cSensor[4].xPos = PlayerSystem.cSensor[1].xPos - (PlayerSystem.collisionLeft - 1 << 16);
					}
					else
					{
						i = -1;
					}
					if (PlayerSystem.cSensor[0].angle < 30)
					{
						playerO.collisionMode = 0;
					}
					if (PlayerSystem.cSensor[0].angle > 98)
					{
						playerO.collisionMode = 2;
					}
					break;
				}
				if (num > -1)
				{
					playerO.angle = PlayerSystem.cSensor[0].angle;
				}
				if (PlayerSystem.cSensor[3].collided == 1)
				{
					i = -2;
				}
				else
				{
					PlayerSystem.SetPathGripSensors(playerO);
				}
			}
			switch (playerO.collisionMode)
			{
			case 0:
				if (PlayerSystem.cSensor[0].collided == 0 && PlayerSystem.cSensor[1].collided == 0 && PlayerSystem.cSensor[2].collided == 0)
				{
					playerO.gravity = 1;
					playerO.collisionMode = 0;
					playerO.xVelocity = GlobalAppDefinitions.CosValue256[playerO.angle] * playerO.speed >> 8;
					playerO.yVelocity = GlobalAppDefinitions.SinValue256[playerO.angle] * playerO.speed >> 8;
					if (playerO.yVelocity < -1048576)
					{
						playerO.yVelocity = -1048576;
					}
					if (playerO.yVelocity > 1048576)
					{
						playerO.yVelocity = 1048576;
					}
					playerO.speed = playerO.xVelocity;
					playerO.angle = 0;
					if (PlayerSystem.cSensor[3].collided == 1)
					{
						if (playerO.speed > 0)
						{
							playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
						}
						if (playerO.speed < 0)
						{
							playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
						}
						playerO.speed = 0;
						if ((playerO.left == 1 | playerO.right == 1) && playerO.pushing < 2)
						{
							playerO.pushing += 1;
						}
					}
					else
					{
						playerO.pushing = 0;
						playerO.xPos += playerO.xVelocity;
					}
					playerO.yPos += playerO.yVelocity;
					return;
				}
				playerO.angle = PlayerSystem.cSensor[0].angle;
				playerO.objectPtr.rotation = playerO.angle << 1;
				playerO.flailing[0] = PlayerSystem.cSensor[0].collided;
				playerO.flailing[1] = PlayerSystem.cSensor[1].collided;
				playerO.flailing[2] = PlayerSystem.cSensor[2].collided;
				if (PlayerSystem.cSensor[3].collided == 1)
				{
					if (playerO.speed > 0)
					{
						playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
					}
					if (playerO.speed < 0)
					{
						playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
					}
					playerO.speed = 0;
					if ((playerO.left == 1 | playerO.right == 1) && playerO.pushing < 2)
					{
						playerO.pushing += 1;
					}
				}
				else
				{
					playerO.pushing = 0;
					playerO.xPos = PlayerSystem.cSensor[4].xPos;
				}
				playerO.yPos = PlayerSystem.cSensor[4].yPos;
				return;
			case 1:
				if (PlayerSystem.cSensor[0].collided == 0 && PlayerSystem.cSensor[1].collided == 0 && PlayerSystem.cSensor[2].collided == 0)
				{
					playerO.gravity = 1;
					playerO.collisionMode = 0;
					playerO.xVelocity = GlobalAppDefinitions.CosValue256[playerO.angle] * playerO.speed >> 8;
					playerO.yVelocity = GlobalAppDefinitions.SinValue256[playerO.angle] * playerO.speed >> 8;
					if (playerO.yVelocity < -1048576)
					{
						playerO.yVelocity = -1048576;
					}
					if (playerO.yVelocity > 1048576)
					{
						playerO.yVelocity = 1048576;
					}
					playerO.speed = playerO.xVelocity;
					playerO.angle = 0;
				}
				else if (playerO.speed < 163840 && playerO.speed > -163840 && playerO.controlLock == 0)
				{
					playerO.gravity = 1;
					playerO.angle = 0;
					playerO.collisionMode = 0;
					playerO.speed = playerO.xVelocity;
					playerO.controlLock = 30;
				}
				else
				{
					playerO.angle = PlayerSystem.cSensor[0].angle;
					playerO.objectPtr.rotation = playerO.angle << 1;
				}
				if (PlayerSystem.cSensor[3].collided == 1)
				{
					if (playerO.speed > 0)
					{
						playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionTop << 16;
					}
					if (playerO.speed < 0)
					{
						playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionBottom << 16;
					}
					playerO.speed = 0;
				}
				else
				{
					playerO.yPos = PlayerSystem.cSensor[4].yPos;
				}
				playerO.xPos = PlayerSystem.cSensor[4].xPos;
				return;
			case 2:
				if (PlayerSystem.cSensor[0].collided == 0 && PlayerSystem.cSensor[1].collided == 0 && PlayerSystem.cSensor[2].collided == 0)
				{
					playerO.gravity = 1;
					playerO.collisionMode = 0;
					playerO.xVelocity = GlobalAppDefinitions.CosValue256[playerO.angle] * playerO.speed >> 8;
					playerO.yVelocity = GlobalAppDefinitions.SinValue256[playerO.angle] * playerO.speed >> 8;
					playerO.flailing[0] = 0;
					playerO.flailing[1] = 0;
					playerO.flailing[2] = 0;
					if (playerO.yVelocity < -1048576)
					{
						playerO.yVelocity = -1048576;
					}
					if (playerO.yVelocity > 1048576)
					{
						playerO.yVelocity = 1048576;
					}
					playerO.angle = 0;
					playerO.speed = playerO.xVelocity;
					if (PlayerSystem.cSensor[3].collided == 1)
					{
						if (playerO.speed > 0)
						{
							playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
						}
						if (playerO.speed < 0)
						{
							playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
						}
						playerO.speed = 0;
					}
					else
					{
						playerO.xPos += playerO.xVelocity;
					}
				}
				else if (playerO.speed > -163840 && playerO.speed < 163840)
				{
					playerO.gravity = 1;
					playerO.angle = 0;
					playerO.collisionMode = 0;
					playerO.speed = playerO.xVelocity;
					playerO.flailing[0] = 0;
					playerO.flailing[1] = 0;
					playerO.flailing[2] = 0;
					if (PlayerSystem.cSensor[3].collided == 1)
					{
						if (playerO.speed > 0)
						{
							playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
						}
						if (playerO.speed < 0)
						{
							playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
						}
						playerO.speed = 0;
					}
					else
					{
						playerO.xPos += playerO.xVelocity;
					}
				}
				else
				{
					playerO.angle = PlayerSystem.cSensor[0].angle;
					playerO.objectPtr.rotation = playerO.angle << 1;
					if (PlayerSystem.cSensor[3].collided == 1)
					{
						if (playerO.speed < 0)
						{
							playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionRight << 16;
						}
						if (playerO.speed > 0)
						{
							playerO.xPos = PlayerSystem.cSensor[3].xPos - PlayerSystem.collisionLeft + 1 << 16;
						}
						playerO.speed = 0;
					}
					else
					{
						playerO.xPos = PlayerSystem.cSensor[4].xPos;
					}
				}
				playerO.yPos = PlayerSystem.cSensor[4].yPos;
				return;
			case 3:
				if (PlayerSystem.cSensor[0].collided == 0 && PlayerSystem.cSensor[1].collided == 0 && PlayerSystem.cSensor[2].collided == 0)
				{
					playerO.gravity = 1;
					playerO.collisionMode = 0;
					playerO.xVelocity = GlobalAppDefinitions.CosValue256[playerO.angle] * playerO.speed >> 8;
					playerO.yVelocity = GlobalAppDefinitions.SinValue256[playerO.angle] * playerO.speed >> 8;
					if (playerO.yVelocity < -1048576)
					{
						playerO.yVelocity = -1048576;
					}
					if (playerO.yVelocity > 1048576)
					{
						playerO.yVelocity = 1048576;
					}
					playerO.speed = playerO.xVelocity;
					playerO.angle = 0;
				}
				else if (playerO.speed > -163840 && playerO.speed < 163840 && playerO.controlLock == 0)
				{
					playerO.gravity = 1;
					playerO.angle = 0;
					playerO.collisionMode = 0;
					playerO.speed = playerO.xVelocity;
					playerO.controlLock = 30;
				}
				else
				{
					playerO.angle = PlayerSystem.cSensor[0].angle;
					playerO.objectPtr.rotation = playerO.angle << 1;
				}
				if (PlayerSystem.cSensor[3].collided == 1)
				{
					if (playerO.speed > 0)
					{
						playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionBottom << 16;
					}
					if (playerO.speed < 0)
					{
						playerO.yPos = PlayerSystem.cSensor[3].yPos - PlayerSystem.collisionTop + 1 << 16;
					}
					playerO.speed = 0;
				}
				else
				{
					playerO.yPos = PlayerSystem.cSensor[4].yPos;
				}
				playerO.xPos = PlayerSystem.cSensor[4].xPos;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00005DD4 File Offset: 0x00003FD4
		public static void FloorCollision(PlayerObject playerO, CollisionSensor cSensorRef)
		{
			int num = cSensorRef.yPos >> 16;
			for (int i = 0; i < 48; i += 16)
			{
				if (cSensorRef.collided == 0)
				{
					int num2 = cSensorRef.xPos >> 16;
					int num3 = num2 >> 7;
					int num4 = (num2 & 127) >> 4;
					int num5 = (cSensorRef.yPos >> 16) - 16 + i;
					int num6 = num5 >> 7;
					int num7 = (num5 & 127) >> 4;
					if (num2 > -1 && num5 > -1)
					{
						int num8 = (int)StageSystem.stageLayouts[0].tileMap[num3 + (num6 << 8)] << 6;
						num8 += num4 + (num7 << 3);
						int num9 = (int)StageSystem.tile128x128.tile16x16[num8];
						if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num8] != 2 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num8] != 3)
						{
							switch (StageSystem.tile128x128.direction[num8])
							{
							case 0:
							{
								int num10 = (num2 & 15) + (num9 << 4);
								if ((num5 & 15) > (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10] - 16) + i && StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10] < 15)
								{
									cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10] + (num6 << 7) + (num7 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num9] & 255U);
								}
								break;
							}
							case 1:
							{
								int num10 = 15 - (num2 & 15) + (num9 << 4);
								if ((num5 & 15) > (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10] - 16) + i && StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10] < 15)
								{
									cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10] + (num6 << 7) + (num7 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - (StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num9] & 255U));
								}
								break;
							}
							case 2:
							{
								int num10 = (num2 & 15) + (num9 << 4);
								if ((num5 & 15) > (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num10] - 16) + i)
								{
									cSensorRef.yPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num10]) + (num6 << 7) + (num7 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(384U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num9] & 4278190080U) >> 24) & 255U);
								}
								break;
							}
							case 3:
							{
								int num10 = 15 - (num2 & 15) + (num9 << 4);
								if ((num5 & 15) > (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num10] - 16) + i)
								{
									cSensorRef.yPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num10]) + (num6 << 7) + (num7 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - (384U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num9] & 4278190080U) >> 24) & 255U));
								}
								break;
							}
							}
						}
						if (cSensorRef.collided == 1)
						{
							if (cSensorRef.angle < 0)
							{
								cSensorRef.angle += 256;
							}
							if (cSensorRef.angle > 255)
							{
								cSensorRef.angle -= 256;
							}
							if (cSensorRef.yPos - num > 14)
							{
								cSensorRef.yPos = num << 16;
								cSensorRef.collided = 0;
							}
							else if (cSensorRef.yPos - num < -17)
							{
								cSensorRef.yPos = num << 16;
								cSensorRef.collided = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000061CC File Offset: 0x000043CC
		public static void FindFloorPosition(PlayerObject playerO, CollisionSensor cSensorRef, int prevCollisionPos)
		{
			int angle = cSensorRef.angle;
			for (int i = 0; i < 48; i += 16)
			{
				if (cSensorRef.collided == 0)
				{
					int num = cSensorRef.xPos >> 16;
					int num2 = num >> 7;
					int num3 = (num & 127) >> 4;
					int num4 = (cSensorRef.yPos >> 16) - 16 + i;
					int num5 = num4 >> 7;
					int num6 = (num4 & 127) >> 4;
					if (num > -1 && num4 > -1)
					{
						int num7 = (int)StageSystem.stageLayouts[0].tileMap[num2 + (num5 << 8)] << 6;
						num7 += num3 + (num6 << 3);
						int num8 = (int)StageSystem.tile128x128.tile16x16[num7];
						if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num7] != 2 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num7] != 3)
						{
							switch (StageSystem.tile128x128.direction[num7])
							{
							case 0:
							{
								int num9 = (num & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num9] < 64)
								{
									cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num9] + (num5 << 7) + (num6 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 255U);
								}
								break;
							}
							case 1:
							{
								int num9 = 15 - (num & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num9] < 64)
								{
									cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num9] + (num5 << 7) + (num6 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - (StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 255U));
								}
								break;
							}
							case 2:
							{
								int num9 = (num & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num9] > -64)
								{
									cSensorRef.yPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num9]) + (num5 << 7) + (num6 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(384U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 4278190080U) >> 24) & 255U);
								}
								break;
							}
							case 3:
							{
								int num9 = 15 - (num & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num9] > -64)
								{
									cSensorRef.yPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num9]) + (num5 << 7) + (num6 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - (384U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 4278190080U) >> 24) & 255U));
								}
								break;
							}
							}
						}
						if (cSensorRef.collided == 1)
						{
							if (cSensorRef.angle < 0)
							{
								cSensorRef.angle += 256;
							}
							if (cSensorRef.angle > 255)
							{
								cSensorRef.angle -= 256;
							}
							if (Math.Abs(cSensorRef.angle - angle) > 32 && Math.Abs(cSensorRef.angle - 256 - angle) > 32 && Math.Abs(cSensorRef.angle + 256 - angle) > 32)
							{
								cSensorRef.yPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
								cSensorRef.angle = angle;
								i = 48;
							}
							else if (cSensorRef.yPos - prevCollisionPos > 14)
							{
								cSensorRef.yPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
							}
							else if (cSensorRef.yPos - prevCollisionPos < -14)
							{
								cSensorRef.yPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000065C4 File Offset: 0x000047C4
		public static void LWallCollision(PlayerObject playerO, CollisionSensor cSensorRef)
		{
			int num = cSensorRef.xPos >> 16;
			for (int i = 0; i < 48; i += 16)
			{
				if (cSensorRef.collided == 0)
				{
					int num2 = (cSensorRef.xPos >> 16) - 16 + i;
					int num3 = num2 >> 7;
					int num4 = (num2 & 127) >> 4;
					int num5 = cSensorRef.yPos >> 16;
					int num6 = num5 >> 7;
					int num7 = (num5 & 127) >> 4;
					if (num2 > -1 && num5 > -1)
					{
						int num8 = (int)StageSystem.stageLayouts[0].tileMap[num3 + (num6 << 8)] << 6;
						num8 += num4 + (num7 << 3);
						int num9 = (int)StageSystem.tile128x128.tile16x16[num8];
						if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num8] != 1 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num8] < 3)
						{
							switch (StageSystem.tile128x128.direction[num8])
							{
							case 0:
							{
								int num10 = (num5 & 15) + (num9 << 4);
								if ((num2 & 15) > (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num10] - 16) + i)
								{
									cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num10] + (num3 << 7) + (num4 << 4);
									cSensorRef.collided = 1;
								}
								break;
							}
							case 1:
							{
								int num10 = (num5 & 15) + (num9 << 4);
								if ((num2 & 15) > (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num10] - 16) + i)
								{
									cSensorRef.xPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num10]) + (num3 << 7) + (num4 << 4);
									cSensorRef.collided = 1;
								}
								break;
							}
							case 2:
							{
								int num10 = 15 - (num5 & 15) + (num9 << 4);
								if ((num2 & 15) > (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num10] - 16) + i)
								{
									cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num10] + (num3 << 7) + (num4 << 4);
									cSensorRef.collided = 1;
								}
								break;
							}
							case 3:
							{
								int num10 = 15 - (num5 & 15) + (num9 << 4);
								if ((num2 & 15) > (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num10] - 16) + i)
								{
									cSensorRef.xPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num10]) + (num3 << 7) + (num4 << 4);
									cSensorRef.collided = 1;
								}
								break;
							}
							}
						}
						if (cSensorRef.collided == 1)
						{
							if (cSensorRef.xPos - num > 15)
							{
								cSensorRef.xPos = num << 16;
								cSensorRef.collided = 0;
							}
							else if (cSensorRef.xPos - num < -15)
							{
								cSensorRef.xPos = num << 16;
								cSensorRef.collided = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000689C File Offset: 0x00004A9C
		public static void FindLWallPosition(PlayerObject playerO, CollisionSensor cSensorRef, int prevCollisionPos)
		{
			int angle = cSensorRef.angle;
			for (int i = 0; i < 48; i += 16)
			{
				if (cSensorRef.collided == 0)
				{
					int num = (cSensorRef.xPos >> 16) - 16 + i;
					int num2 = num >> 7;
					int num3 = (num & 127) >> 4;
					int num4 = cSensorRef.yPos >> 16;
					int num5 = num4 >> 7;
					int num6 = (num4 & 127) >> 4;
					if (num > -1 && num4 > -1)
					{
						int num7 = (int)StageSystem.stageLayouts[0].tileMap[num2 + (num5 << 8)] << 6;
						num7 += num3 + (num6 << 3);
						int num8 = (int)StageSystem.tile128x128.tile16x16[num7];
						if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num7] < 3)
						{
							switch (StageSystem.tile128x128.direction[num7])
							{
							case 0:
							{
								int num9 = (num4 & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num9] < 64)
								{
									cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num9] + (num2 << 7) + (num3 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 65280U) >> 8);
								}
								break;
							}
							case 1:
							{
								int num9 = (num4 & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num9] > -64)
								{
									cSensorRef.xPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num9]) + (num2 << 7) + (num3 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 16711680U) >> 16));
								}
								break;
							}
							case 2:
							{
								int num9 = 15 - (num4 & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num9] < 64)
								{
									cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num9] + (num2 << 7) + (num3 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(384U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 65280U) >> 8) & 255U);
								}
								break;
							}
							case 3:
							{
								int num9 = 15 - (num4 & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num9] > -64)
								{
									cSensorRef.xPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num9]) + (num2 << 7) + (num3 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - (384U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 16711680U) >> 16) & 255U));
								}
								break;
							}
							}
						}
						if (cSensorRef.collided == 1)
						{
							if (cSensorRef.angle < 0)
							{
								cSensorRef.angle += 256;
							}
							if (cSensorRef.angle > 255)
							{
								cSensorRef.angle -= 256;
							}
							if (Math.Abs(angle - cSensorRef.angle) > 32)
							{
								cSensorRef.xPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
								cSensorRef.angle = angle;
								i = 48;
							}
							else if (cSensorRef.xPos - prevCollisionPos > 14)
							{
								cSensorRef.xPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
							}
							else if (cSensorRef.xPos - prevCollisionPos < -14)
							{
								cSensorRef.xPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00006C48 File Offset: 0x00004E48
		public static void RWallCollision(PlayerObject playerO, CollisionSensor cSensorRef)
		{
			int num = cSensorRef.xPos >> 16;
			for (int i = 0; i < 48; i += 16)
			{
				if (cSensorRef.collided == 0)
				{
					int num2 = (cSensorRef.xPos >> 16) + 16 - i;
					int num3 = num2 >> 7;
					int num4 = (num2 & 127) >> 4;
					int num5 = cSensorRef.yPos >> 16;
					int num6 = num5 >> 7;
					int num7 = (num5 & 127) >> 4;
					if (num2 > -1 && num5 > -1)
					{
						int num8 = (int)StageSystem.stageLayouts[0].tileMap[num3 + (num6 << 8)] << 6;
						num8 += num4 + (num7 << 3);
						int num9 = (int)StageSystem.tile128x128.tile16x16[num8];
						if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num8] != 1 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num8] < 3)
						{
							switch (StageSystem.tile128x128.direction[num8])
							{
							case 0:
							{
								int num10 = (num5 & 15) + (num9 << 4);
								if ((num2 & 15) < (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num10] + 16) - i)
								{
									cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num10] + (num3 << 7) + (num4 << 4);
									cSensorRef.collided = 1;
								}
								break;
							}
							case 1:
							{
								int num10 = (num5 & 15) + (num9 << 4);
								if ((num2 & 15) < (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num10] + 16) - i)
								{
									cSensorRef.xPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num10]) + (num3 << 7) + (num4 << 4);
									cSensorRef.collided = 1;
								}
								break;
							}
							case 2:
							{
								int num10 = 15 - (num5 & 15) + (num9 << 4);
								if ((num2 & 15) < (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num10] + 16) - i)
								{
									cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num10] + (num3 << 7) + (num4 << 4);
									cSensorRef.collided = 1;
								}
								break;
							}
							case 3:
							{
								int num10 = 15 - (num5 & 15) + (num9 << 4);
								if ((num2 & 15) < (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num10] + 16) - i)
								{
									cSensorRef.xPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num10]) + (num3 << 7) + (num4 << 4);
									cSensorRef.collided = 1;
								}
								break;
							}
							}
						}
						if (cSensorRef.collided == 1)
						{
							if (cSensorRef.xPos - num > 15)
							{
								cSensorRef.xPos = num << 16;
								cSensorRef.collided = 0;
							}
							else if (cSensorRef.xPos - num < -15)
							{
								cSensorRef.xPos = num << 16;
								cSensorRef.collided = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00006F20 File Offset: 0x00005120
		public static void FindRWallPosition(PlayerObject playerO, CollisionSensor cSensorRef, int prevCollisionPos)
		{
			int angle = cSensorRef.angle;
			for (int i = 0; i < 48; i += 16)
			{
				if (cSensorRef.collided == 0)
				{
					int num = (cSensorRef.xPos >> 16) + 16 - i;
					int num2 = num >> 7;
					int num3 = (num & 127) >> 4;
					int num4 = cSensorRef.yPos >> 16;
					int num5 = num4 >> 7;
					int num6 = (num4 & 127) >> 4;
					if (num > -1 && num4 > -1)
					{
						int num7 = (int)StageSystem.stageLayouts[0].tileMap[num2 + (num5 << 8)] << 6;
						num7 += num3 + (num6 << 3);
						int num8 = (int)StageSystem.tile128x128.tile16x16[num7];
						if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num7] < 3)
						{
							switch (StageSystem.tile128x128.direction[num7])
							{
							case 0:
							{
								int num9 = (num4 & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num9] > -64)
								{
									cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num9] + (num2 << 7) + (num3 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 16711680U) >> 16);
								}
								break;
							}
							case 1:
							{
								int num9 = (num4 & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num9] < 64)
								{
									cSensorRef.xPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num9]) + (num2 << 7) + (num3 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 65280U) >> 8));
								}
								break;
							}
							case 2:
							{
								int num9 = 15 - (num4 & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num9] > -64)
								{
									cSensorRef.xPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].rightWallMask[num9] + (num2 << 7) + (num3 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(384U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 16711680U) >> 16) & 255U);
								}
								break;
							}
							case 3:
							{
								int num9 = 15 - (num4 & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num9] < 64)
								{
									cSensorRef.xPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].leftWallMask[num9]) + (num2 << 7) + (num3 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - (384U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 65280U) >> 8) & 255U));
								}
								break;
							}
							}
						}
						if (cSensorRef.collided == 1)
						{
							if (cSensorRef.angle < 0)
							{
								cSensorRef.angle += 256;
							}
							if (cSensorRef.angle > 255)
							{
								cSensorRef.angle -= 256;
							}
							if (Math.Abs(cSensorRef.angle - angle) > 32)
							{
								cSensorRef.xPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
								cSensorRef.angle = angle;
								i = 48;
							}
							else if (cSensorRef.xPos - prevCollisionPos > 14)
							{
								cSensorRef.xPos = prevCollisionPos >> 16;
								cSensorRef.collided = 0;
							}
							else if (cSensorRef.xPos - prevCollisionPos < -14)
							{
								cSensorRef.xPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000072CC File Offset: 0x000054CC
		public static void RoofCollision(PlayerObject playerO, CollisionSensor cSensorRef)
		{
			int num = cSensorRef.yPos >> 16;
			for (int i = 0; i < 48; i += 16)
			{
				if (cSensorRef.collided == 0)
				{
					int num2 = cSensorRef.xPos >> 16;
					int num3 = num2 >> 7;
					int num4 = (num2 & 127) >> 4;
					int num5 = (cSensorRef.yPos >> 16) + 16 - i;
					int num6 = num5 >> 7;
					int num7 = (num5 & 127) >> 4;
					if (num2 > -1 && num5 > -1)
					{
						int num8 = (int)StageSystem.stageLayouts[0].tileMap[num3 + (num6 << 8)] << 6;
						num8 += num4 + (num7 << 3);
						int num9 = (int)StageSystem.tile128x128.tile16x16[num8];
						if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num8] != 1 && StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num8] < 3)
						{
							switch (StageSystem.tile128x128.direction[num8])
							{
							case 0:
							{
								int num10 = (num2 & 15) + (num9 << 4);
								if ((num5 & 15) < (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num10] + 16) - i)
								{
									cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num10] + (num6 << 7) + (num7 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num9] & 4278190080U) >> 24);
								}
								break;
							}
							case 1:
							{
								int num10 = 15 - (num2 & 15) + (num9 << 4);
								if ((num5 & 15) < (int)(StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num10] + 16) - i)
								{
									cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num10] + (num6 << 7) + (num7 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num9] & 4278190080U) >> 24));
								}
								break;
							}
							case 2:
							{
								int num10 = (num2 & 15) + (num9 << 4);
								if ((num5 & 15) < (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10] + 16) - i)
								{
									cSensorRef.yPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10]) + (num6 << 7) + (num7 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(384U - (StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num9] & 255U) & 255U);
								}
								break;
							}
							case 3:
							{
								int num10 = 15 - (num2 & 15) + (num9 << 4);
								if ((num5 & 15) < (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10] + 16) - i)
								{
									cSensorRef.yPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num10]) + (num6 << 7) + (num7 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - (384U - (StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num9] & 255U) & 255U));
								}
								break;
							}
							}
						}
						if (cSensorRef.collided == 1)
						{
							if (cSensorRef.angle < 0)
							{
								cSensorRef.angle += 256;
							}
							if (cSensorRef.angle > 255)
							{
								cSensorRef.angle -= 256;
							}
							if (cSensorRef.yPos - num > 14)
							{
								cSensorRef.yPos = num << 16;
								cSensorRef.collided = 0;
							}
							else if (cSensorRef.yPos - num < -14)
							{
								cSensorRef.yPos = num << 16;
								cSensorRef.collided = 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00007690 File Offset: 0x00005890
		public static void FindRoofPosition(PlayerObject playerO, CollisionSensor cSensorRef, int prevCollisionPos)
		{
			int angle = cSensorRef.angle;
			for (int i = 0; i < 48; i += 16)
			{
				if (cSensorRef.collided == 0)
				{
					int num = cSensorRef.xPos >> 16;
					int num2 = num >> 7;
					int num3 = (num & 127) >> 4;
					int num4 = (cSensorRef.yPos >> 16) + 16 - i;
					int num5 = num4 >> 7;
					int num6 = (num4 & 127) >> 4;
					if (num > -1 && num4 > -1)
					{
						int num7 = (int)StageSystem.stageLayouts[0].tileMap[num2 + (num5 << 8)] << 6;
						num7 += num3 + (num6 << 3);
						int num8 = (int)StageSystem.tile128x128.tile16x16[num7];
						if (StageSystem.tile128x128.collisionFlag[(int)playerO.collisionPlane, num7] < 3)
						{
							switch (StageSystem.tile128x128.direction[num7])
							{
							case 0:
							{
								int num9 = (num & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num9] > -64)
								{
									cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num9] + (num5 << 7) + (num6 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 4278190080U) >> 24);
								}
								break;
							}
							case 1:
							{
								int num9 = 15 - (num & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num9] > -64)
								{
									cSensorRef.yPos = (int)StageSystem.tileCollisions[(int)playerO.collisionPlane].roofMask[num9] + (num5 << 7) + (num6 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - ((StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 4278190080U) >> 24));
								}
								break;
							}
							case 2:
							{
								int num9 = (num & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num9] < 64)
								{
									cSensorRef.yPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num9]) + (num5 << 7) + (num6 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(384U - (StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 255U) & 255U);
								}
								break;
							}
							case 3:
							{
								int num9 = 15 - (num & 15) + (num8 << 4);
								if (StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num9] < 64)
								{
									cSensorRef.yPos = (int)(15 - StageSystem.tileCollisions[(int)playerO.collisionPlane].floorMask[num9]) + (num5 << 7) + (num6 << 4);
									cSensorRef.collided = 1;
									cSensorRef.angle = (int)(256U - (384U - (StageSystem.tileCollisions[(int)playerO.collisionPlane].angle[num8] & 255U) & 255U));
								}
								break;
							}
							}
						}
						if (cSensorRef.collided == 1)
						{
							if (cSensorRef.angle < 0)
							{
								cSensorRef.angle += 256;
							}
							if (cSensorRef.angle > 255)
							{
								cSensorRef.angle -= 256;
							}
							if (Math.Abs(cSensorRef.angle - angle) > 32)
							{
								cSensorRef.yPos = prevCollisionPos << 16;
								cSensorRef.collided = 0;
								cSensorRef.angle = angle;
								i = 48;
							}
							else
							{
								if (cSensorRef.yPos - prevCollisionPos > 15)
								{
									cSensorRef.yPos = prevCollisionPos << 16;
									cSensorRef.collided = 0;
								}
								if (cSensorRef.yPos - prevCollisionPos < -15)
								{
									cSensorRef.yPos = prevCollisionPos << 16;
									cSensorRef.collided = 0;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x04000001 RID: 1
		public const int MAX_PLAYERS = 2;

		// Token: 0x04000002 RID: 2
		public static ushort delayLeft;

		// Token: 0x04000003 RID: 3
		public static ushort delayRight;

		// Token: 0x04000004 RID: 4
		public static ushort delayUp;

		// Token: 0x04000005 RID: 5
		public static ushort delayDown;

		// Token: 0x04000006 RID: 6
		public static ushort delayJumpPress;

		// Token: 0x04000007 RID: 7
		public static ushort delayJumpHold;

		// Token: 0x04000008 RID: 8
		public static byte jumpWait;

		// Token: 0x04000009 RID: 9
		public static byte numActivePlayers = 1;

		// Token: 0x0400000A RID: 10
		public static byte playerMenuNum = 0;

		// Token: 0x0400000B RID: 11
		public static PlayerObject[] playerList = new PlayerObject[2];

		// Token: 0x0400000C RID: 12
		public static CollisionSensor[] cSensor = new CollisionSensor[6];

		// Token: 0x0400000D RID: 13
		public static int collisionLeft;

		// Token: 0x0400000E RID: 14
		public static int collisionTop;

		// Token: 0x0400000F RID: 15
		public static int collisionRight;

		// Token: 0x04000010 RID: 16
		public static int collisionBottom;
	}
}
