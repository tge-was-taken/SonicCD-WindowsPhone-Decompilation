using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Retro_Engine;

namespace Sonic_CD
{
	// Token: 0x0200000E RID: 14
	public class Game : Microsoft.Xna.Framework.Game
	{
		private static Dictionary<string, int> achievementKeyToId;
		private static Dictionary<string, int> languageCodeToId;

		// Token: 0x06000060 RID: 96 RVA: 0x0000BB1C File Offset: 0x00009D1C
		public Game()
		{
			this.graphics = new GraphicsDeviceManager(this);
			this.graphics.IsFullScreen = true;
			this.graphics.PreferredBackBufferWidth = 800;
			this.graphics.PreferredBackBufferHeight = 480;
			this.graphics.PreferredBackBufferFormat = SurfaceFormat.Bgr565;
			this.graphics.PreferredDepthStencilFormat = DepthFormat.None;
			this.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(this.graphics_PreparingDeviceSettings);
			this.Content = new ContentManagerPatch( Content.ServiceProvider );
			base.Content.RootDirectory = "Content";
			base.TargetElapsedTime = TimeSpan.FromSeconds(0.016666666666666666);
			base.InactiveSleepTime = TimeSpan.FromSeconds(1.0);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000BC1A File Offset: 0x00009E1A
		private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
		{
			e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000BC2D File Offset: 0x00009E2D
		protected override void Initialize()
		{
			SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>(this.GamerSignedInCallback);
			// API only available on WP
			//if (Environment.DeviceType != 1)
			//{
			//	this.gamerServiceInstance = new GamerServicesComponent(this);
			//	base.Components.Add(this.gamerServiceInstance);
			//}
			base.Initialize();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000BC6C File Offset: 0x00009E6C
		protected void GamerSignedInCallback(object sender, SignedInEventArgs args)
		{
			SignedInGamer gamer = args.Gamer;
			if (gamer != null)
			{
				GlobalAppDefinitions.gameOnlineActive = 2;
				if (GlobalAppDefinitions.gameOnlineActive == 2)
				{
					GlobalAppDefinitions.gameOnlineActive = 3;
					gamer.BeginGetAchievements(new AsyncCallback(this.GetAchievementsCallback), gamer);
					return;
				}
			}
			else
			{
				GlobalAppDefinitions.gameOnlineActive = 0;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000BCB4 File Offset: 0x00009EB4
		protected void GetAchievementsCallback(IAsyncResult result)
		{
			SignedInGamer signedInGamer = result.AsyncState as SignedInGamer;
			if (signedInGamer == null)
			{
				GlobalAppDefinitions.gameOnlineActive = 0;
				return;
			}
			GlobalAppDefinitions.gameOnlineActive = 1;
			lock (this.achievementsLockObject)
			{
				this.maxGamerScore = 0;
				this.earnedGamerScore = 0;
				this.achievements = signedInGamer.EndGetAchievements(result);
				for (int i = 0; i < this.achievements.Count; i++)
				{
					Achievement achievement = this.achievements[i];
					this.achievementName[i] = achievement.Name;
					this.achievementDesc[i] = achievement.Description;
					this.achievementGamerScore[i] = achievement.GamerScore;
					string key;
					if ((key = achievement.Key) != null)
					{
						if (achievementKeyToId == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(12);
							dictionary.Add("88 Miles Per Hour", 0);
							dictionary.Add("Just One Hug is Enough", 1);
							dictionary.Add("Paradise Found", 2);
							dictionary.Add("Take the High Road", 3);
							dictionary.Add("King of the Rings", 4);
							dictionary.Add("Statue Saviour", 5);
							dictionary.Add("Heavy Metal", 6);
							dictionary.Add("All Stages Clear", 7);
							dictionary.Add("Treasure Hunter", 8);
							dictionary.Add("Dr Eggman Got Served", 9);
							dictionary.Add("Just In Time", 10);
							dictionary.Add("Saviour of the Planet", 11);
							achievementKeyToId = dictionary;
						}
						int num;
						if (achievementKeyToId.TryGetValue(key, out num))
						{
							switch (num)
							{
							case 0:
								this.achievementID[i] = 0;
								break;
							case 1:
								this.achievementID[i] = 1;
								break;
							case 2:
								this.achievementID[i] = 2;
								break;
							case 3:
								this.achievementID[i] = 3;
								break;
							case 4:
								this.achievementID[i] = 4;
								break;
							case 5:
								this.achievementID[i] = 5;
								break;
							case 6:
								this.achievementID[i] = 6;
								break;
							case 7:
								this.achievementID[i] = 7;
								break;
							case 8:
								this.achievementID[i] = 8;
								break;
							case 9:
								this.achievementID[i] = 9;
								break;
							case 10:
								this.achievementID[i] = 10;
								break;
							case 11:
								this.achievementID[i] = 11;
								break;
							}
						}
					}
					this.maxGamerScore += achievement.GamerScore;
					if (achievement.IsEarned)
					{
						this.earnedGamerScore += achievement.GamerScore;
						this.achievementEarned[i] = 1;
					}
					else
					{
						this.achievementEarned[i] = 0;
					}
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000BF5C File Offset: 0x0000A15C
		public void AwardAchievement(string achievementKey)
		{
			SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
			if (signedInGamer == null)
			{
				return;
			}
			lock (this.achievementsLockObject)
			{
				if (this.achievements != null)
				{
					foreach (Achievement achievement in this.achievements)
					{
						if (achievement.Key == achievementKey)
						{
							if (!achievement.IsEarned)
							{
								signedInGamer.BeginAwardAchievement(achievementKey, new AsyncCallback(this.AwardAchievementCallback), signedInGamer);
							}
							break;
						}
					}
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000C018 File Offset: 0x0000A218
		protected void AwardAchievementCallback(IAsyncResult result)
		{
			SignedInGamer signedInGamer = result.AsyncState as SignedInGamer;
			if (signedInGamer != null)
			{
				signedInGamer.EndAwardAchievement(result);
				signedInGamer.BeginGetAchievements(new AsyncCallback(this.GetAchievementsCallback), signedInGamer);
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000C050 File Offset: 0x0000A250
		public void LoadLeaderboardEntries()
		{
			SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
			int num = ObjectSystem.globalVariables[114];
			try
			{
				LeaderboardIdentity leaderboardId;
				if (num == 0)
				{
					leaderboardId = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, num);
				}
				else
				{
					leaderboardId = LeaderboardIdentity.Create(LeaderboardKey.BestTimeLifeTime, num + 1);
				}
				GlobalAppDefinitions.gameMode = 7;
				LeaderboardReader.BeginRead(leaderboardId, signedInGamer, 100, new AsyncCallback(this.LeaderboardReadCallback), signedInGamer);
			}
			catch (GameUpdateRequiredException e)
			{
				this.ReadNullLeaderboardEntries();
				this.HandleGameUpdateRequired(e);
			}
			catch (Exception)
			{
				this.ReadNullLeaderboardEntries();
				Guide.BeginShowMessageBox("Xbox LIVE", EngineCallbacks.liveErrorMessage[(int)GlobalAppDefinitions.gameLanguage], new string[]
				{
					"OK"
				}, 0, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.LiveErrorMessage), null);
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x0000C118 File Offset: 0x0000A318
		protected void LeaderboardReadCallback(IAsyncResult result)
		{
			object asyncState = result.AsyncState;
			try
			{
				this.leaderboardReader = LeaderboardReader.EndRead(result);
				this.ReadLeaderboardEntries();
			}
			catch (GameUpdateRequiredException e)
			{
				this.ReadNullLeaderboardEntries();
				this.HandleGameUpdateRequired(e);
			}
			catch (Exception)
			{
				this.ReadNullLeaderboardEntries();
				Guide.BeginShowMessageBox("Xbox LIVE", EngineCallbacks.liveErrorMessage[(int)GlobalAppDefinitions.gameLanguage], new string[]
				{
					"OK"
				}, 0, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.LiveErrorMessage), null);
			}
			GlobalAppDefinitions.gameMode = 1;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000C1B0 File Offset: 0x0000A3B0
		protected void ReadLeaderboardEntries()
		{
			int num = ObjectSystem.globalVariables[114];
			TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
			LeaderboardReader leaderboardReader = this.leaderboardReader;
			if (num == 0)
			{
				for (int i = 0; i < this.leaderboardReader.Entries.Count; i++)
				{
					LeaderboardEntry leaderboardEntry = leaderboardReader.Entries[i];
					string text = string.Format("{0,4}{1,-15}{2,1}{3,8}", new object[]
					{
						(i + 1).ToString() + ".",
						leaderboardEntry.Gamer.Gamertag,
						" ",
						leaderboardEntry.Columns.GetValueInt32("BestScore").ToString()
					});
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], text.ToCharArray());
				}
			}
			else
			{
				for (int i = 0; i < this.leaderboardReader.Entries.Count; i++)
				{
					LeaderboardEntry leaderboardEntry2 = leaderboardReader.Entries[i];
					int num2 = leaderboardEntry2.Columns.GetValueInt32("BestTime");
					int num3 = num2 / 6000;
					int num4 = num2 / 100 % 60;
					num2 %= 100;
					string text = string.Format("{0,4}{1,-15}{2,2}{3,1}{4,1}{5,2}{6,1}{7,2}", new object[]
					{
						(i + 1).ToString() + ".",
						leaderboardEntry2.Gamer.Gamertag,
						"  ",
						num3.ToString(),
						"'",
						num4.ToString(),
						"\"",
						num2.ToString()
					});
					TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], text.ToCharArray());
				}
			}
			for (int i = this.leaderboardReader.Entries.Count; i < 100; i++)
			{
				string text = string.Format("{0,4}{1,-15}", (i + 1).ToString() + ".", "---------------");
				TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], text.ToCharArray());
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000C3E0 File Offset: 0x0000A5E0
		protected void ReadNullLeaderboardEntries()
		{
			int num = ObjectSystem.globalVariables[114];
			TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
			for (int i = 0; i < 100; i++)
			{
				string text = string.Format("{0,4}{1,-15}", (i + 1).ToString() + ".", "---------------");
				TextSystem.AddTextMenuEntry(StageSystem.gameMenu[0], text.ToCharArray());
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000C448 File Offset: 0x0000A648
		public void SetLeaderboard(int leaderboardID, int result)
		{
			LeaderboardOutcome value = LeaderboardOutcome.Win;
			SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
			try
			{
				if (leaderboardID == 0)
				{
					LeaderboardIdentity leaderboardId = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, leaderboardID);
					LeaderboardEntry leaderboard = signedInGamer.LeaderboardWriter.GetLeaderboard(leaderboardId);
					leaderboard.Rating = (long)result;
					leaderboard.Columns.SetValue("Outcome", value);
					leaderboard.Columns.SetValue("TimeStamp", DateTime.Now);
				}
				else
				{
					LeaderboardIdentity leaderboardId2 = LeaderboardIdentity.Create(LeaderboardKey.BestTimeLifeTime, leaderboardID);
					LeaderboardEntry leaderboard2 = signedInGamer.LeaderboardWriter.GetLeaderboard(leaderboardId2);
					leaderboard2.Rating = (long)result;
					leaderboard2.Columns.SetValue("Outcome", value);
					leaderboard2.Columns.SetValue("TimeStamp", DateTime.Now);
				}
			}
			catch (GameUpdateRequiredException e)
			{
				this.HandleGameUpdateRequired(e);
			}
			catch (Exception)
			{
				this.ReadNullLeaderboardEntries();
				Guide.BeginShowMessageBox("Xbox LIVE", EngineCallbacks.liveErrorMessage[(int)GlobalAppDefinitions.gameLanguage], new string[]
				{
					"OK"
				}, 0, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.LiveErrorMessage), null);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000C560 File Offset: 0x0000A760
		protected override void OnActivated(object sender, EventArgs args)
		{
			if (StageSystem.stageMode == 2)
			{
				if (GlobalAppDefinitions.gameMode == 7)
				{
					GlobalAppDefinitions.gameMode = 1;
					GlobalAppDefinitions.gameMessage = 4;
				}
			}
			else
			{
				if (GlobalAppDefinitions.gameMode == 7)
				{
					GlobalAppDefinitions.gameMode = 1;
				}
				GlobalAppDefinitions.gameMessage = 2;
				AudioPlayback.ResumeSound();
			}
			base.OnActivated(sender, args);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000C5AC File Offset: 0x0000A7AC
		protected override void OnDeactivated(object sender, EventArgs args)
		{
			GlobalAppDefinitions.gameMessage = 2;
			if (StageSystem.stageMode != 2 && GlobalAppDefinitions.gameMode != 7)
			{
				AudioPlayback.PauseSound();
			}
			GlobalAppDefinitions.gameMode = 7;
			base.OnDeactivated(sender, args);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000C5D8 File Offset: 0x0000A7D8
		protected override void LoadContent()
		{
			GlobalAppDefinitions.CalculateTrigAngles();
			RenderDevice.InitRenderDevice(base.GraphicsDevice);
			RenderDevice.SetScreenDimensions(800, 480);
			EngineCallbacks.StartupRetroEngine();
			EngineCallbacks.gameRef = this;
			AudioPlayback.gameRef = this;
			string twoLetterISOLanguageName;
			if ((twoLetterISOLanguageName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName) != null)
			{
				if (languageCodeToId == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(6);
					dictionary.Add("en", 0);
					dictionary.Add("fr", 1);
					dictionary.Add("it", 2);
					dictionary.Add("de", 3);
					dictionary.Add("es", 4);
					dictionary.Add("ja", 5);
					languageCodeToId = dictionary;
				}
				int num;
				if (languageCodeToId.TryGetValue(twoLetterISOLanguageName, out num))
				{
					switch (num)
					{
					case 1:
						GlobalAppDefinitions.gameLanguage = 1;
						goto IL_FC;
					case 2:
						GlobalAppDefinitions.gameLanguage = 2;
						goto IL_FC;
					case 3:
						GlobalAppDefinitions.gameLanguage = 3;
						goto IL_FC;
					case 4:
						GlobalAppDefinitions.gameLanguage = 4;
						goto IL_FC;
					case 5:
						GlobalAppDefinitions.gameLanguage = 5;
						goto IL_FC;
					}
				}
			}
			GlobalAppDefinitions.gameLanguage = 0;
			IL_FC:
			if (Guide.IsTrialMode)
			{
				GlobalAppDefinitions.gameTrialMode = 1;
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000C6EE File Offset: 0x0000A8EE
		protected override void UnloadContent()
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000C6F0 File Offset: 0x0000A8F0
		protected override void Update(GameTime gameTime)
		{
			int num = 0;
			InputSystem.CheckKeyboardInput();
			TouchCollection state = TouchPanel.GetState();
			InputSystem.ClearTouchData();
			foreach (TouchLocation touchLocation in state)
			{
				switch (touchLocation.State)
				{
				case TouchLocationState.Pressed:
					InputSystem.AddTouch(touchLocation.Position.X, touchLocation.Position.Y, num);
					break;
				case TouchLocationState.Moved:
					InputSystem.AddTouch(touchLocation.Position.X, touchLocation.Position.Y, num);
					break;
				}
				num++;
			}
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				if (FileIO.activeStageList == 0)
				{
					switch (StageSystem.stageListPosition)
					{
					case 4:
					case 5:
						InputSystem.touchData.start = 1;
						break;
					default:
						InputSystem.touchData.buttonB = 1;
						break;
					}
				}
				else if (StageSystem.stageMode == 2)
				{
					if (ObjectSystem.objectEntityList[9].state == 3 && GlobalAppDefinitions.gameMode == 1)
					{
						ObjectSystem.objectEntityList[9].state = 4;
						ObjectSystem.objectEntityList[9].value[0] = 0;
						ObjectSystem.objectEntityList[9].value[1] = 0;
						ObjectSystem.objectEntityList[9].alpha = 248;
						AudioPlayback.PlaySfx(27, 0);
					}
				}
				else
				{
					GlobalAppDefinitions.gameMessage = 2;
				}
			}
			if (StageSystem.stageMode != 2)
			{
				EngineCallbacks.ProcessMainLoop();
			}
			try
			{
				base.Update(gameTime);
			}
			catch (GameUpdateRequiredException e)
			{
				this.HandleGameUpdateRequired(e);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
		private void HandleGameUpdateRequired(GameUpdateRequiredException e)
		{
			this.gamerServiceInstance.Enabled = false;
			this.displayTitleUpdateMessage = true;
			this.signinStatus = Game.SigninStatus.UpdateNeeded;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000C8BC File Offset: 0x0000AABC
		protected override void Draw(GameTime gameTime)
		{
			if (this.displayTitleUpdateMessage)
			{
				EngineCallbacks.ShowLiveUpdateMessage();
			}
			if (StageSystem.stageMode == 2)
			{
				EngineCallbacks.ProcessMainLoop();
			}
			if (RenderDevice.highResMode == 0)
			{
				RenderDevice.FlipScreen();
			}
			else
			{
				RenderDevice.FlipScreenHRes();
			}
			base.Draw(gameTime);
		}

		// Token: 0x040000F6 RID: 246
		private const int LeaderboardPageSize = 100;

		// Token: 0x040000F7 RID: 247
		private GraphicsDeviceManager graphics;

		// Token: 0x040000F8 RID: 248
		private object achievementsLockObject = new object();

		// Token: 0x040000F9 RID: 249
		private AchievementCollection achievements;

		// Token: 0x040000FA RID: 250
		public int earnedGamerScore;

		// Token: 0x040000FB RID: 251
		public int maxGamerScore;

		// Token: 0x040000FC RID: 252
		public string[] achievementName = new string[12];

		// Token: 0x040000FD RID: 253
		public string[] achievementDesc = new string[12];

		// Token: 0x040000FE RID: 254
		public int[] achievementEarned = new int[12];

		// Token: 0x040000FF RID: 255
		public int[] achievementGamerScore = new int[12];

		// Token: 0x04000100 RID: 256
		public int[] achievementID = new int[12];

		// Token: 0x04000101 RID: 257
		private LeaderboardReader leaderboardReader;

		// Token: 0x04000102 RID: 258
		private GamerServicesComponent gamerServiceInstance;

		// Token: 0x04000103 RID: 259
		protected Game.SigninStatus signinStatus;

		// Token: 0x04000104 RID: 260
		public bool displayTitleUpdateMessage;

		// Token: 0x0200000F RID: 15
		protected enum SigninStatus
		{
			// Token: 0x04000106 RID: 262
			None,
			// Token: 0x04000107 RID: 263
			SigningIn,
			// Token: 0x04000108 RID: 264
			Local,
			// Token: 0x04000109 RID: 265
			LIVE,
			// Token: 0x0400010A RID: 266
			Error,
			// Token: 0x0400010B RID: 267
			UpdateNeeded
		}
	}

	public class ContentManagerPatch : Microsoft.Xna.Framework.Content.ContentManager
	{
		public ContentManagerPatch( IServiceProvider serviceProvider ) : base( serviceProvider )
		{
		}

		protected override Stream OpenStream( string assetName )
		{
			var path = Path.Combine(RootDirectory, assetName);
			var xnbPath = path + ".xnb";
			if ( File.Exists( xnbPath ) )
			{
				// Patch platform code
				var stream = new FileStream( xnbPath, FileMode.Open, FileAccess.ReadWrite );
				stream.Seek( 0x3, SeekOrigin.Current );
				stream.WriteByte( ( byte )'w' );
				stream.Seek( 0, SeekOrigin.Begin );
				return stream;
			}
			else
			{
				return File.OpenRead( path );
			}
		}
	}
}
