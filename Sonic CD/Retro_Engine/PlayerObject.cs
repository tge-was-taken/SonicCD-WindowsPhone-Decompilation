using System;

namespace Retro_Engine
{
	// Token: 0x02000003 RID: 3
	public class PlayerObject
	{
		// Token: 0x04000011 RID: 17
		public int objectNum;

		// Token: 0x04000012 RID: 18
		public int xPos;

		// Token: 0x04000013 RID: 19
		public int yPos;

		// Token: 0x04000014 RID: 20
		public int xVelocity;

		// Token: 0x04000015 RID: 21
		public int yVelocity;

		// Token: 0x04000016 RID: 22
		public int speed;

		// Token: 0x04000017 RID: 23
		public int screenXPos;

		// Token: 0x04000018 RID: 24
		public int screenYPos;

		// Token: 0x04000019 RID: 25
		public int angle;

		// Token: 0x0400001A RID: 26
		public int timer;

		// Token: 0x0400001B RID: 27
		public int lookPos;

		// Token: 0x0400001C RID: 28
		public int[] value = new int[8];

		// Token: 0x0400001D RID: 29
		public byte collisionMode;

		// Token: 0x0400001E RID: 30
		public byte skidding;

		// Token: 0x0400001F RID: 31
		public byte pushing;

		// Token: 0x04000020 RID: 32
		public byte collisionPlane;

		// Token: 0x04000021 RID: 33
		public sbyte controlMode;

		// Token: 0x04000022 RID: 34
		public byte controlLock;

		// Token: 0x04000023 RID: 35
		public PlayerStatistics movementStats = new PlayerStatistics();

		// Token: 0x04000024 RID: 36
		public byte visible;

		// Token: 0x04000025 RID: 37
		public byte tileCollisions;

		// Token: 0x04000026 RID: 38
		public byte objectInteraction;

		// Token: 0x04000027 RID: 39
		public byte left;

		// Token: 0x04000028 RID: 40
		public byte right;

		// Token: 0x04000029 RID: 41
		public byte up;

		// Token: 0x0400002A RID: 42
		public byte down;

		// Token: 0x0400002B RID: 43
		public byte jumpPress;

		// Token: 0x0400002C RID: 44
		public byte jumpHold;

		// Token: 0x0400002D RID: 45
		public byte followPlayer1;

		// Token: 0x0400002E RID: 46
		public byte trackScroll;

		// Token: 0x0400002F RID: 47
		public byte gravity;

		// Token: 0x04000030 RID: 48
		public byte water;

		// Token: 0x04000031 RID: 49
		public byte[] flailing = new byte[3];

		// Token: 0x04000032 RID: 50
		public AnimationFileList animationFile;

		// Token: 0x04000033 RID: 51
		public ObjectEntity objectPtr;
	}
}
