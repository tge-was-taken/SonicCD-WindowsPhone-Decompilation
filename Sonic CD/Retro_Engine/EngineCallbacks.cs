using System;
using System.Threading;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Sonic_CD;

namespace Retro_Engine
{
	// Token: 0x0200000B RID: 11
	public static class EngineCallbacks
	{
		// Token: 0x0600003E RID: 62 RVA: 0x0000A23C File Offset: 0x0000843C
		static EngineCallbacks()
		{
			EngineCallbacks.restartMessage[5] = EngineCallbacks.restartMessage[5].Replace("@", " " + Environment.NewLine);
			EngineCallbacks.exitMessage[5] = EngineCallbacks.exitMessage[5].Replace("@", " " + Environment.NewLine);
			EngineCallbacks.upsellMessage[5] = EngineCallbacks.upsellMessage[5].Replace("@", " " + Environment.NewLine);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000A62C File Offset: 0x0000882C
		public static void PlayVideoFile(char[] fileName)
		{
			int num = FileIO.StringLength(ref fileName);
			string text = new string(fileName);
			string text2 = "Content/Video/" + text.Remove(num) + ".wmv";
			AudioPlayback.StopMusic();
			Thread.Sleep(1000);
			new MediaPlayerLauncher
			{
				Media = new Uri(text2, UriKind.Relative),
				Location = 1,
				Controls = 2
			}.Show();
			EngineCallbacks.waitValue = 0;
			GlobalAppDefinitions.gameMode = 9;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000A6A4 File Offset: 0x000088A4
		public static void OnlineSetAchievement(int achievementID, int achievementDone)
		{
			if (achievementDone > 99 && GlobalAppDefinitions.gameOnlineActive == 1 && GlobalAppDefinitions.gameTrialMode == 0)
			{
				switch (achievementID)
				{
				case 0:
					EngineCallbacks.gameRef.AwardAchievement("88 Miles Per Hour");
					return;
				case 1:
					EngineCallbacks.gameRef.AwardAchievement("Just One Hug is Enough");
					return;
				case 2:
					EngineCallbacks.gameRef.AwardAchievement("Paradise Found");
					return;
				case 3:
					EngineCallbacks.gameRef.AwardAchievement("Take the High Road");
					return;
				case 4:
					EngineCallbacks.gameRef.AwardAchievement("King of the Rings");
					return;
				case 5:
					EngineCallbacks.gameRef.AwardAchievement("Statue Saviour");
					return;
				case 6:
					EngineCallbacks.gameRef.AwardAchievement("Heavy Metal");
					return;
				case 7:
					EngineCallbacks.gameRef.AwardAchievement("All Stages Clear");
					return;
				case 8:
					EngineCallbacks.gameRef.AwardAchievement("Treasure Hunter");
					return;
				case 9:
					EngineCallbacks.gameRef.AwardAchievement("Dr Eggman Got Served");
					return;
				case 10:
					EngineCallbacks.gameRef.AwardAchievement("Just In Time");
					return;
				case 11:
					EngineCallbacks.gameRef.AwardAchievement("Saviour of the Planet");
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000A7C8 File Offset: 0x000089C8
		public static void OnlineSetLeaderboard(int leaderboardID, int result)
		{
			EngineCallbacks.gameRef.SetLeaderboard(leaderboardID, result);
			switch (leaderboardID)
			{
			default:
				return;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000A844 File Offset: 0x00008A44
		public static void OnlineLoadAchievementsMenu()
		{
			int num = 0;
			for (int i = 0; i < 12; i++)
			{
				num += EngineCallbacks.gameRef.achievementEarned[i];
			}
			TextSystem.SetupTextMenu(StageSystem.gameMenu[0], 0);
			TextSystem.SetupTextMenu(StageSystem.gameMenu[1], 0);
			switch (GlobalAppDefinitions.gameLanguage)
			{
			default:
			{
				string text = EngineCallbacks.achievementText[0] + "    (" + num.ToString() + "/12)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				text = EngineCallbacks.gamerscoreText[0] + "    (" + EngineCallbacks.gameRef.earnedGamerScore.ToString() + "/200)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				break;
			}
			case 1:
			{
				string text = EngineCallbacks.achievementText[1] + "    (" + num.ToString() + "/12)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				text = EngineCallbacks.gamerscoreText[1] + "    (" + EngineCallbacks.gameRef.earnedGamerScore.ToString() + "/200)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				break;
			}
			case 2:
			{
				string text = EngineCallbacks.achievementText[2] + "    (" + num.ToString() + "/12)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				text = EngineCallbacks.gamerscoreText[2] + "    (" + EngineCallbacks.gameRef.earnedGamerScore.ToString() + "/200)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				break;
			}
			case 3:
			{
				string text = EngineCallbacks.achievementText[3] + "    (" + num.ToString() + "/12)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				text = EngineCallbacks.gamerscoreText[3] + "    (" + EngineCallbacks.gameRef.earnedGamerScore.ToString() + "/200)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				break;
			}
			case 4:
			{
				string text = EngineCallbacks.achievementText[4] + "    (" + num.ToString() + "/12)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				text = EngineCallbacks.gamerscoreText[4] + "    (" + EngineCallbacks.gameRef.earnedGamerScore.ToString() + "/200)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				break;
			}
			case 5:
			{
				string text = EngineCallbacks.achievementText[5] + "    (" + num.ToString() + "/12)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				text = EngineCallbacks.gamerscoreText[5] + "    (" + EngineCallbacks.gameRef.earnedGamerScore.ToString() + "/200)";
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[0], text.ToCharArray());
				break;
			}
			}
			for (int i = 0; i < 12; i++)
			{
				ObjectSystem.objectEntityList[34 + i].value[1] = EngineCallbacks.gameRef.achievementEarned[i];
				ObjectSystem.objectEntityList[34 + i].frame = (byte)EngineCallbacks.gameRef.achievementID[i];
				string text;
				if (EngineCallbacks.gameRef.achievementName[i] != null)
				{
					text = EngineCallbacks.gameRef.achievementName[i] + "    (" + EngineCallbacks.gameRef.achievementGamerScore[i].ToString() + " G)";
				}
				else
				{
					text = "Achievement Name    (" + EngineCallbacks.gameRef.achievementGamerScore[i].ToString() + " G)";
				}
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[1], text.ToCharArray());
				if (EngineCallbacks.gameRef.achievementDesc[i] != null)
				{
					text = EngineCallbacks.gameRef.achievementDesc[i];
				}
				else
				{
					text = "Achievement Description";
				}
				TextSystem.AddTextMenuEntryMapped(StageSystem.gameMenu[1], text.ToCharArray());
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public static void OnlineLoadLeaderboardsMenu()
		{
			EngineCallbacks.gameRef.LoadLeaderboardEntries();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000AC38 File Offset: 0x00008E38
		public static void RetroEngineCallback(int callbackID)
		{
			switch (callbackID)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 11:
			case 12:
			case 13:
				break;
			case 6:
				if (!Guide.IsVisible)
				{
					Guide.BeginShowMessageBox(EngineCallbacks.restartTitle[(int)GlobalAppDefinitions.gameLanguage], EngineCallbacks.restartMessage[(int)GlobalAppDefinitions.gameLanguage], new string[]
					{
						EngineCallbacks.yesMessage[(int)GlobalAppDefinitions.gameLanguage],
						EngineCallbacks.noMessage[(int)GlobalAppDefinitions.gameLanguage]
					}, 1, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.ConfirmationScreen), null);
					return;
				}
				break;
			case 7:
				if (FileIO.activeStageList == 0)
				{
					if (!Guide.IsVisible)
					{
						Guide.BeginShowMessageBox(EngineCallbacks.exitTitle[(int)GlobalAppDefinitions.gameLanguage], EngineCallbacks.exitMessage[(int)GlobalAppDefinitions.gameLanguage], new string[]
						{
							EngineCallbacks.yesMessage[(int)GlobalAppDefinitions.gameLanguage],
							EngineCallbacks.noMessage[(int)GlobalAppDefinitions.gameLanguage]
						}, 1, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.ExitConfirmation), null);
						return;
					}
				}
				else if (!Guide.IsVisible)
				{
					Guide.BeginShowMessageBox(EngineCallbacks.exitTitle[(int)GlobalAppDefinitions.gameLanguage], EngineCallbacks.exitMessage[(int)GlobalAppDefinitions.gameLanguage], new string[]
					{
						EngineCallbacks.yesMessage[(int)GlobalAppDefinitions.gameLanguage],
						EngineCallbacks.noMessage[(int)GlobalAppDefinitions.gameLanguage]
					}, 1, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.ConfirmationScreen), null);
					return;
				}
				break;
			case 8:
			{
				SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
				if (signedInGamer != null)
				{
					Guide.ShowMarketplace(signedInGamer.PlayerIndex);
					return;
				}
				break;
			}
			case 9:
				new WebBrowserTask
				{
					Uri = new Uri("http://www.sega.com/legal/terms_mobile.php", UriKind.Absolute)
				}.Show();
				return;
			case 10:
				new WebBrowserTask
				{
					Uri = new Uri("http://www.sega.com/legal/privacy_mobile.php", UriKind.Absolute)
				}.Show();
				return;
			case 14:
				if (!Guide.IsVisible)
				{
					Guide.BeginShowMessageBox(EngineCallbacks.upsellTitle[(int)GlobalAppDefinitions.gameLanguage], EngineCallbacks.upsellMessage[(int)GlobalAppDefinitions.gameLanguage], new string[]
					{
						EngineCallbacks.yesMessage[(int)GlobalAppDefinitions.gameLanguage],
						EngineCallbacks.noMessage[(int)GlobalAppDefinitions.gameLanguage]
					}, 1, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.UpsellScreen), null);
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000AE60 File Offset: 0x00009060
		public static void UpsellScreen(IAsyncResult ar)
		{
			int valueOrDefault = Guide.EndShowMessageBox(ar).GetValueOrDefault();
			int? num = 0;
			if (num != null)
			{
				switch (valueOrDefault)
				{
				case 0:
				{
					GlobalAppDefinitions.gameMode = 1;
					GlobalAppDefinitions.gameMessage = 3;
					SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
					if (signedInGamer != null)
					{
						Guide.ShowMarketplace(signedInGamer.PlayerIndex);
					}
					if (GlobalAppDefinitions.gameTrialMode == 1 && !Guide.IsTrialMode)
					{
						GlobalAppDefinitions.gameTrialMode = 0;
						GlobalAppDefinitions.gameMode = 2;
						return;
					}
					break;
				}
				case 1:
					GlobalAppDefinitions.gameMode = 1;
					GlobalAppDefinitions.gameMessage = 4;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000AEE4 File Offset: 0x000090E4
		public static void ConfirmationScreen(IAsyncResult ar)
		{
			int valueOrDefault = Guide.EndShowMessageBox(ar).GetValueOrDefault();
			int? num = 0;
			if (num != null)
			{
				switch (valueOrDefault)
				{
				case 0:
					GlobalAppDefinitions.gameMode = 1;
					GlobalAppDefinitions.gameMessage = 3;
					return;
				case 1:
					GlobalAppDefinitions.gameMode = 1;
					GlobalAppDefinitions.gameMessage = 4;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000AF30 File Offset: 0x00009130
		public static void ExitConfirmation(IAsyncResult ar)
		{
			int valueOrDefault = Guide.EndShowMessageBox(ar).GetValueOrDefault();
			int? num = 0;
			if (num != null)
			{
				switch (valueOrDefault)
				{
				case 0:
					EngineCallbacks.gameRef.Exit();
					return;
				case 1:
					GlobalAppDefinitions.gameMode = 1;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000AF74 File Offset: 0x00009174
		public static void LiveErrorMessage(IAsyncResult ar)
		{
			Guide.EndShowMessageBox(ar);
			GlobalAppDefinitions.gameMode = 1;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000AF84 File Offset: 0x00009184
		public static void ShowLiveUpdateMessage()
		{
			if (!Guide.IsVisible)
			{
				Guide.BeginShowMessageBox(EngineCallbacks.updateTitle[(int)GlobalAppDefinitions.gameLanguage], EngineCallbacks.updateMessage[(int)GlobalAppDefinitions.gameLanguage], new string[]
				{
					EngineCallbacks.yesMessage[(int)GlobalAppDefinitions.gameLanguage],
					EngineCallbacks.noMessage[(int)GlobalAppDefinitions.gameLanguage]
				}, 1, MessageBoxIcon.Alert, new AsyncCallback(EngineCallbacks.UpdateMessage), null);
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000AFE8 File Offset: 0x000091E8
		public static void UpdateMessage(IAsyncResult ar)
		{
			int valueOrDefault = Guide.EndShowMessageBox(ar).GetValueOrDefault();
			int? num = 0;
			if (num != null)
			{
				switch (valueOrDefault)
				{
				case 0:
				{
					GlobalAppDefinitions.gameMode = 1;
					GlobalAppDefinitions.gameMessage = 3;
					EngineCallbacks.gameRef.displayTitleUpdateMessage = false;
					if (!Guide.IsTrialMode)
					{
						new MarketplaceDetailTask
						{
							ContentType = (MarketplaceContentType)1
						}.Show();
						return;
					}
					int num2 = 10;
					while (Guide.IsVisible && num2 > 0)
					{
						num2--;
						Thread.Sleep(100);
					}
					SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
					if (signedInGamer != null)
					{
						Guide.ShowMarketplace(signedInGamer.PlayerIndex);
						return;
					}
					break;
				}
				case 1:
					GlobalAppDefinitions.gameMode = 1;
					GlobalAppDefinitions.gameMessage = 4;
					EngineCallbacks.gameRef.displayTitleUpdateMessage = false;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000B0A0 File Offset: 0x000092A0
		public static void StartupRetroEngine()
		{
			if (!EngineCallbacks.engineInit)
			{
				GlobalAppDefinitions.CalculateTrigAngles();
				GraphicsSystem.GenerateBlendLookupTable();
				if (FileIO.CheckRSDKFile())
				{
					GlobalAppDefinitions.LoadGameConfig("Data/Game/GameConfig.bin".ToCharArray());
				}
				AudioPlayback.InitAudioPlayback();
				StageSystem.InitFirstStage();
				ObjectSystem.ClearScriptData();
				EngineCallbacks.engineInit = true;
				return;
			}
			RenderDevice.UpdateHardwareTextures();
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0000B0F0 File Offset: 0x000092F0
		public static void ProcessMainLoop()
		{
			switch (GlobalAppDefinitions.gameMode)
			{
			case 0:
				GraphicsSystem.gfxIndexSize = 0;
				GraphicsSystem.gfxVertexSize = 0;
				GraphicsSystem.gfxIndexSizeOpaque = 0;
				GraphicsSystem.gfxVertexSizeOpaque = 0;
				StageSystem.ProcessStageSelectMenu();
				return;
			case 1:
				GraphicsSystem.gfxIndexSize = 0;
				GraphicsSystem.gfxVertexSize = 0;
				GraphicsSystem.gfxIndexSizeOpaque = 0;
				GraphicsSystem.gfxVertexSizeOpaque = 0;
				GraphicsSystem.vertexSize3D = 0;
				GraphicsSystem.indexSize3D = 0;
				GraphicsSystem.render3DEnabled = false;
				StageSystem.ProcessStage();
				if (EngineCallbacks.prevMessage == GlobalAppDefinitions.gameMessage)
				{
					GlobalAppDefinitions.gameMessage = 0;
					EngineCallbacks.prevMessage = 0;
					return;
				}
				EngineCallbacks.prevMessage = GlobalAppDefinitions.gameMessage;
				return;
			case 2:
				GlobalAppDefinitions.LoadGameConfig("Data/Game/GameConfig.bin".ToCharArray());
				StageSystem.InitFirstStage();
				FileIO.ResetCurrentStageFolder();
				return;
			case 3:
			case 7:
				break;
			case 4:
				GlobalAppDefinitions.LoadGameConfig("Data/Game/GameConfig.bin".ToCharArray());
				StageSystem.InitErrorMessage();
				FileIO.ResetCurrentStageFolder();
				return;
			case 5:
				GlobalAppDefinitions.gameMode = 1;
				RenderDevice.highResMode = 1;
				return;
			case 6:
				GlobalAppDefinitions.gameMode = 1;
				RenderDevice.highResMode = 0;
				return;
			case 8:
				if (EngineCallbacks.waitValue < 8)
				{
					EngineCallbacks.waitValue++;
					return;
				}
				GlobalAppDefinitions.gameMode = 1;
				return;
			case 9:
				if (EngineCallbacks.waitValue < 60)
				{
					EngineCallbacks.waitValue++;
					return;
				}
				GlobalAppDefinitions.gameMode = 1;
				break;
			default:
				return;
			}
		}

		// Token: 0x040000BF RID: 191
		public const int DISPLAY_LOGOS = 0;

		// Token: 0x040000C0 RID: 192
		public const int TITLE_SCREEN_PRESS_START = 1;

		// Token: 0x040000C1 RID: 193
		public const int ENTER_TIMEATTACK_NOTIFY = 2;

		// Token: 0x040000C2 RID: 194
		public const int EXIT_TIMEATTACK_NOTIFY = 3;

		// Token: 0x040000C3 RID: 195
		public const int FINISH_GAME_NOTIFY = 4;

		// Token: 0x040000C4 RID: 196
		public const int RETURN_TO_ARCADE_SELECTED = 5;

		// Token: 0x040000C5 RID: 197
		public const int RESTART_GAME_SELECTED = 6;

		// Token: 0x040000C6 RID: 198
		public const int EXIT_GAME_SELECTED = 7;

		// Token: 0x040000C7 RID: 199
		public const int UNLOCK_FULL_GAME_SELECTED = 8;

		// Token: 0x040000C8 RID: 200
		public const int TERMS_SELECTED = 9;

		// Token: 0x040000C9 RID: 201
		public const int PRIVACY_SELECTED = 10;

		// Token: 0x040000CA RID: 202
		public const int TRIAL_ENDED = 11;

		// Token: 0x040000CB RID: 203
		public const int SETTINGS_SELECTED = 12;

		// Token: 0x040000CC RID: 204
		public const int FULL_VERSION_ONLY = 14;

		// Token: 0x040000CD RID: 205
		public static Sonic_CD.Game gameRef;

		// Token: 0x040000CE RID: 206
		public static string[] restartTitle = new string[]
		{
			"Restart",
			"Recommencer",
			"Ricominciare",
			"Neustart",
			"Reiniciar",
			"Restart"
		};

		// Token: 0x040000CF RID: 207
		public static string[] exitTitle = new string[]
		{
			"Exit",
			"Quitter",
			"Abbandonare",
			"Verlassen",
			"Abandonar",
			"Exit"
		};

		// Token: 0x040000D0 RID: 208
		public static string[] restartMessage = new string[]
		{
			"Are you sure you want to restart? Any unsaved progress will be lost.",
			"Veux-tu vraiment recommencer la partie? Toute progression non sauvegardée sera perdue.",
			"Sei sicuro di voler ricominciare la partita? I progressi non salvati andranno perduti.",
			"Möchtest du das rennen wirklich verlassen? Ungespeicherter Fortschritt geht verloren.",
			"¿Seguro que quieres reiniciar? Todo progreso no guardado se perderá.",
			"リスタートしますか？@セーブされていないデータが@きえるかのうせいがあります。"
		};

		// Token: 0x040000D1 RID: 209
		public static string[] exitMessage = new string[]
		{
			"Are you sure you want to exit? Any unsaved progress will be lost.",
			"Veux-tu vraiment quitter la partie? Toute progression non sauvegardée sera perdue.",
			"Sei sicuro di voler abbandonare la partita? I progressi non salvati andranno perduti.",
			"Möchtest du das spiel wirklich verlassen? Ungespeicherter Fortschritt geht verloren.",
			"¿Seguro que quieres abandonar? Todo progreso no guardado se perderá.",
			"ゲームを終了しますか？@セーブされていないデータが@きえるかのうせいがあります。"
		};

		// Token: 0x040000D2 RID: 210
		public static string[] upsellTitle = new string[]
		{
			"Buy Full Game",
			"Acheter le jeu complet",
			"Acquista gioco completo",
			"Spiele-Vollversion kaufen",
			"Comprar juego completo",
			"完全版の購入"
		};

		// Token: 0x040000D3 RID: 211
		public static string[] upsellMessage = new string[]
		{
			"This feature is not available in the trial version. Buy Full Game?",
			"Cette option n’est pas disponible avec la version d’essai. Acheter le jeu complet ?",
			"Questa opzione non è disponibile nella versione di prova. Acquistare il gioco completo?",
			"Diese Funktion ist in der Demo-Version nicht verfügbar. Spiele-Vollversion kaufen?",
			"Esta función no está disponible en la prueba. ¿Comprar juego completo?",
			"完全版をアンロック この機能は、@完全版でのみご利用いただけます。@完全版を購入しますか？"
		};

		// Token: 0x040000D4 RID: 212
		public static string[] updateTitle = new string[]
		{
			"Title Update Available",
			"Mise à jour du titre disponible",
			"È disponibile un aggiornamento del gioco",
			"Titel-Aktualisierung verfügbar",
			"Actualización del título disponible",
			"アップデートが可能です"
		};

		// Token: 0x040000D5 RID: 213
		public static string[] updateMessage = new string[]
		{
			"An update is available! This update is required to connect to Xbox LIVE. Update now?",
			"Une mise à jour est disponible ! Elle est nécessaire pour se connecter à Xbox LIVE. Mettre à jour maintenant ?",
			"È disponibile un aggiornamento! Quest'aggiornamento è necessario per connettersi a Xbox LIVE. Aggiornare ora?",
			"Eine Aktualisierung ist verfügbar! Diese Aktualisierung wird benötigt, um eine Verbindung zu Xbox LIVE herzustellen. Jetzt aktualisieren?",
			"¡Hay una actualización disponible! Esta actualización es necesaria para conectar con Xbox LIVE. ¿Actualizar ahora?",
			"アップデートが可能です。このアップデートはXbox LIVEへの接続が必要です。アップデートを行いますか？"
		};

		// Token: 0x040000D6 RID: 214
		public static string[] liveErrorMessage = new string[]
		{
			"Unable to connect to Xbox LIVE at this time. Please check your connection.",
			"Connexion au Xbox LIVE impossible. Vérifiez votre connexion.",
			"Impossibile connettersi a Xbox LIVE. Verifica la connessione.",
			"Es konnte momentan keine Verbindung zu Xbox LIVE hergestellt werden. Bitte überprüfen Sie Ihre Verbindung.",
			"No se puede conectar con Xbox LIVE. Comprueba tu conexión.",
			"Xbox LIVE に接続できませんでした。ネットワーク接続を確認してください。"
		};

		// Token: 0x040000D7 RID: 215
		public static string[] yesMessage = new string[]
		{
			"Yes",
			"Oui",
			"Sí",
			"Ya",
			"Sí",
			"Yes"
		};

		// Token: 0x040000D8 RID: 216
		public static string[] noMessage = new string[]
		{
			"No",
			"Non",
			"No",
			"Nein",
			"No",
			"No"
		};

		// Token: 0x040000D9 RID: 217
		public static string[] achievementText = new string[]
		{
			"Achievements ",
			"Succès ",
			"Obiettivi ",
			"Erfolge ",
			"Logros ",
			"Achievements "
		};

		// Token: 0x040000DA RID: 218
		public static string[] gamerscoreText = new string[]
		{
			"Gamerscore ",
			"Score ",
			"Punteggio ",
			"Punkte ",
			"Puntuación ",
			"Gamerscore "
		};

		// Token: 0x040000DB RID: 219
		private static int prevMessage = 0;

		// Token: 0x040000DC RID: 220
		private static bool engineInit = false;

		// Token: 0x040000DD RID: 221
		private static int waitValue;
	}
}
