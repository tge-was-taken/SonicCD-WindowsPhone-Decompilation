using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Sonic_CD;

namespace Retro_Engine
{
	// Token: 0x0200000C RID: 12
	public static class AudioPlayback
	{
		// Token: 0x0600004D RID: 77 RVA: 0x0000B22C File Offset: 0x0000942C
		static AudioPlayback()
		{
			for (int i = 0; i < AudioPlayback.musicTracks.Length; i++)
			{
				AudioPlayback.musicTracks[i] = new MusicTrackInfo();
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000B2CA File Offset: 0x000094CA
		private static void HeadphonesPauseMusicCheck(object sender, EventArgs eventArgs)
		{
			if (MediaPlayer.State == MediaState.Paused && AudioPlayback.musicStatus == 1 && AudioPlayback.musicTracks[AudioPlayback.currentMusicTrack].loop && StageSystem.stageMode != 2 && GlobalAppDefinitions.gameMode != 7)
			{
				MediaPlayer.Resume();
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000B304 File Offset: 0x00009504
		public static void InitAudioPlayback()
		{
			FileData fileData = new FileData();
			char[] array = new char[32];
			MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(AudioPlayback.HeadphonesPauseMusicCheck);
			for (int i = 0; i < 8; i++)
			{
				AudioPlayback.channelSfxNum[i] = -1;
			}
			if (FileIO.LoadFile("Data/Game/GameConfig.bin".ToCharArray(), fileData))
			{
				byte b = FileIO.ReadByte();
				for (int i = 0; i < (int)b; i++)
				{
					byte b2 = FileIO.ReadByte();
				}
				b = FileIO.ReadByte();
				for (int i = 0; i < (int)b; i++)
				{
					byte b2 = FileIO.ReadByte();
				}
				b = FileIO.ReadByte();
				for (int i = 0; i < (int)b; i++)
				{
					byte b2 = FileIO.ReadByte();
				}
				byte b3 = FileIO.ReadByte();
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					for (int i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
				}
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					for (int i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
					}
				}
				b3 = FileIO.ReadByte();
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					int i;
					byte b2;
					for (i = 0; i < (int)b; i++)
					{
						b2 = FileIO.ReadByte();
						array[i] = (char)b2;
					}
					array[i] = '\0';
					b2 = FileIO.ReadByte();
					b2 = FileIO.ReadByte();
					b2 = FileIO.ReadByte();
					b2 = FileIO.ReadByte();
				}
				b3 = FileIO.ReadByte();
				AudioPlayback.numGlobalSFX = (int)b3;
				for (int j = 0; j < (int)b3; j++)
				{
					b = FileIO.ReadByte();
					int i;
					for (i = 0; i < (int)b; i++)
					{
						byte b2 = FileIO.ReadByte();
						array[i] = (char)b2;
					}
					array[i] = '\0';
					FileIO.GetFileInfo(fileData);
					FileIO.CloseFile();
					AudioPlayback.LoadSfx(array, j);
					FileIO.SetFileInfo(fileData);
				}
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000B4AC File Offset: 0x000096AC
		public static void ReleaseAudioPlayback()
		{
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000B4AE File Offset: 0x000096AE
		public static void ReleaseGlobalSFX()
		{
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000B4B0 File Offset: 0x000096B0
		public static void ReleaseStageSFX()
		{
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000B4B2 File Offset: 0x000096B2
		public static void SetGameVolumes(int bgmVolume, int sfxVolume)
		{
			AudioPlayback.musicVolumeSetting = (float)bgmVolume * 0.01f;
			AudioPlayback.SetMusicVolume(AudioPlayback.musicVolume);
			AudioPlayback.sfxVolumeSetting = (float)sfxVolume * 0.01f;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000B4D8 File Offset: 0x000096D8
		public static void StopAllSFX()
		{
			for (int i = 0; i < 8; i++)
			{
				if (AudioPlayback.channelSfxNum[i] > -1 && !AudioPlayback.channelInstance[i].IsDisposed)
				{
					AudioPlayback.channelInstance[i].Stop();
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000B515 File Offset: 0x00009715
		public static void PauseSound()
		{
			if (MediaPlayer.GameHasControl)
			{
				MediaPlayer.Pause();
				AudioPlayback.musicStatus = 2;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000B529 File Offset: 0x00009729
		public static void ResumeSound()
		{
			if (MediaPlayer.GameHasControl)
			{
				MediaPlayer.Resume();
				AudioPlayback.musicStatus = 1;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000B540 File Offset: 0x00009740
		public static void SetMusicTrack(char[] fileName, int trackNo, byte loopTrack, uint loopPoint)
		{
			char[] array = "Music/".ToCharArray();
			int num = FileIO.StringLength(ref fileName);
			for (int i = 0; i < num; i++)
			{
				if (fileName[i] == '/')
				{
					fileName[i] = '-';
				}
			}
			num -= 4;
			if (num < 0)
			{
				num = 0;
			}
			if (fileName.Length > 0)
			{
				fileName[num] = '\0';
			}
			FileIO.StrCopy(ref AudioPlayback.musicTracks[trackNo].trackName, ref array);
			FileIO.StrAdd(ref AudioPlayback.musicTracks[trackNo].trackName, ref fileName);
			if (loopTrack == 1)
			{
				AudioPlayback.musicTracks[trackNo].loop = true;
			}
			else
			{
				AudioPlayback.musicTracks[trackNo].loop = false;
			}
			AudioPlayback.musicTracks[trackNo].loopPoint = loopPoint;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000B5DF File Offset: 0x000097DF
		public static void SetMusicVolume(int volume)
		{
			if (volume < 0)
			{
				volume = 0;
			}
			if (volume > 100)
			{
				volume = 100;
			}
			AudioPlayback.musicVolume = volume;
			if (MediaPlayer.GameHasControl)
			{
				MediaPlayer.Volume = (float)volume * 0.01f * AudioPlayback.musicVolumeSetting;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000B614 File Offset: 0x00009814
		public static void PlayMusic(int trackNo)
		{
			if (MediaPlayer.GameHasControl && AudioPlayback.musicTracks[trackNo].trackName[0] != '\0')
			{
				string text = new string(AudioPlayback.musicTracks[trackNo].trackName);
				text = text.Remove(FileIO.StringLength(ref AudioPlayback.musicTracks[trackNo].trackName));
				Song song = AudioPlayback.gameRef.Content.Load<Song>(text);
				AudioPlayback.currentMusicTrack = trackNo;
				MediaPlayer.Play(song);
				MediaPlayer.IsRepeating = AudioPlayback.musicTracks[trackNo].loop;
				MediaPlayer.IsMuted = false;
				MediaPlayer.Volume = AudioPlayback.musicVolumeSetting;
				AudioPlayback.musicVolume = 100;
				AudioPlayback.musicStatus = 1;
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000B6B0 File Offset: 0x000098B0
		public static void StopMusic()
		{
			if (MediaPlayer.GameHasControl)
			{
				MediaPlayer.Stop();
				MediaPlayer.IsRepeating = false;
				MediaPlayer.IsMuted = true;
				AudioPlayback.musicStatus = 0;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000B6D0 File Offset: 0x000098D0
		public static void LoadSfx(char[] fileName, int sfxNum)
		{
			FileData fData = new FileData();
			char[] filePath = new char[64];
			char[] array = "Data/SoundFX/".ToCharArray();
			if (sfxNum > -1 && sfxNum < 256)
			{
				FileIO.StrCopy(ref filePath, ref array);
				FileIO.StrAdd(ref filePath, ref fileName);
				if (FileIO.LoadFile(filePath, fData))
				{
					byte b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					b = FileIO.ReadByte();
					uint num = 1U;
					uint num2 = 0U;
					while (num > 0U && num2 < 400U)
					{
						switch (num)
						{
						case 1U:
							b = FileIO.ReadByte();
							if (b == 100)
							{
								num += 1U;
							}
							else
							{
								num = 1U;
							}
							break;
						case 2U:
							b = FileIO.ReadByte();
							if (b == 97)
							{
								num += 1U;
							}
							else
							{
								num = 1U;
							}
							break;
						case 3U:
							b = FileIO.ReadByte();
							if (b == 116)
							{
								num += 1U;
							}
							else
							{
								num = 1U;
							}
							break;
						case 4U:
							b = FileIO.ReadByte();
							if (b == 97)
							{
								num = 0U;
							}
							else
							{
								num = 1U;
							}
							break;
						}
						num2 += 1U;
					}
					b = FileIO.ReadByte();
					num = (uint)b;
					b = FileIO.ReadByte();
					num += (uint)((uint)b << 8);
					b = FileIO.ReadByte();
					num += (uint)((uint)b << 16);
					b = FileIO.ReadByte();
					num += (uint)((uint)b << 24);
					num <<= 1;
					byte[] array2 = new byte[num + (num / 32U << 1)];
					if (!BitConverter.IsLittleEndian)
					{
						for (num2 = 0U; num2 < num; num2 += 2U)
						{
							array2[(int)((UIntPtr)num2)] = (byte)(FileIO.ReadByte() - 128);
							array2[(int)((UIntPtr)(num2 + 1U))] = 0;
						}
					}
					else
					{
						for (num2 = 0U; num2 < num; num2 += 2U)
						{
							array2[(int)((UIntPtr)num2)] = 0;
							array2[(int)((UIntPtr)(num2 + 1U))] = (byte)(FileIO.ReadByte() - 128);
						}
					}
					if (AudioPlayback.sfxLoaded[sfxNum])
					{
						AudioPlayback.sfxSamples[sfxNum].Dispose();
					}
					AudioPlayback.sfxSamples[sfxNum] = new SoundEffect(array2, 44100, AudioChannels.Mono);
					AudioPlayback.sfxLoaded[sfxNum] = true;
					FileIO.CloseFile();
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000B908 File Offset: 0x00009B08
		public static void PlaySfx(int sfxNum, byte sLoop)
		{
			for (int i = 0; i < 8; i++)
			{
				if (AudioPlayback.channelSfxNum[i] == sfxNum)
				{
					if (!AudioPlayback.channelInstance[i].IsDisposed)
					{
						AudioPlayback.channelInstance[i].Stop();
					}
					AudioPlayback.nextChannelPos = i;
					i = 8;
				}
			}
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos] = AudioPlayback.sfxSamples[sfxNum].CreateInstance();
			if (sLoop == 1)
			{
				AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].IsLooped = true;
			}
			else
			{
				AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].IsLooped = false;
			}
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].Pan = 0f;
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].Volume = AudioPlayback.sfxVolumeSetting;
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].Play();
			AudioPlayback.channelSfxNum[AudioPlayback.nextChannelPos] = sfxNum;
			AudioPlayback.nextChannelPos++;
			if (AudioPlayback.nextChannelPos == 8)
			{
				AudioPlayback.nextChannelPos = 0;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000B9EC File Offset: 0x00009BEC
		public static void StopSfx(int sfxNum)
		{
			for (int i = 0; i < 8; i++)
			{
				if (AudioPlayback.channelSfxNum[i] == sfxNum)
				{
					AudioPlayback.channelSfxNum[i] = -1;
					if (!AudioPlayback.channelInstance[i].IsDisposed)
					{
						AudioPlayback.channelInstance[i].Stop();
					}
				}
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000BA34 File Offset: 0x00009C34
		public static void SetSfxAttributes(int sfxNum, int volume, int pan)
		{
			for (int i = 0; i < 8; i++)
			{
				if (AudioPlayback.channelSfxNum[i] == sfxNum)
				{
					if (!AudioPlayback.channelInstance[i].IsDisposed)
					{
						AudioPlayback.channelInstance[i].Stop();
					}
					AudioPlayback.nextChannelPos = i;
					i = 8;
				}
			}
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos] = AudioPlayback.sfxSamples[sfxNum].CreateInstance();
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].IsLooped = false;
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].Pan = (float)pan * 0.01f;
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].Volume = AudioPlayback.sfxVolumeSetting;
			AudioPlayback.channelInstance[AudioPlayback.nextChannelPos].Play();
			AudioPlayback.channelSfxNum[AudioPlayback.nextChannelPos] = sfxNum;
			AudioPlayback.nextChannelPos++;
			if (AudioPlayback.nextChannelPos == 8)
			{
				AudioPlayback.nextChannelPos = 0;
			}
		}

		// Token: 0x040000DE RID: 222
		public const int MUSIC_STOPPED = 0;

		// Token: 0x040000DF RID: 223
		public const int MUSIC_PLAYING = 1;

		// Token: 0x040000E0 RID: 224
		public const int MUSIC_PAUSED = 2;

		// Token: 0x040000E1 RID: 225
		public const int MUSIC_LOADING = 3;

		// Token: 0x040000E2 RID: 226
		public const int MUSIC_READY = 4;

		// Token: 0x040000E3 RID: 227
		public const int NUM_CHANNELS = 8;

		// Token: 0x040000E4 RID: 228
		public static Game gameRef;

		// Token: 0x040000E5 RID: 229
		public static int numGlobalSFX = 0;

		// Token: 0x040000E6 RID: 230
		public static int numStageSFX = 0;

		// Token: 0x040000E7 RID: 231
		public static SoundEffect[] sfxSamples = new SoundEffect[256];

		// Token: 0x040000E8 RID: 232
		public static bool[] sfxLoaded = new bool[256];

		// Token: 0x040000E9 RID: 233
		public static SoundEffectInstance[] channelInstance = new SoundEffectInstance[8];

		// Token: 0x040000EA RID: 234
		public static int[] channelSfxNum = new int[8];

		// Token: 0x040000EB RID: 235
		public static int nextChannelPos;

		// Token: 0x040000EC RID: 236
		public static bool musicEnabled = true;

		// Token: 0x040000ED RID: 237
		public static int musicVolume = 100;

		// Token: 0x040000EE RID: 238
		public static float musicVolumeSetting = 1f;

		// Token: 0x040000EF RID: 239
		public static float sfxVolumeSetting = 1f;

		// Token: 0x040000F0 RID: 240
		public static int musicStatus = 0;

		// Token: 0x040000F1 RID: 241
		public static int currentMusicTrack;

		// Token: 0x040000F2 RID: 242
		public static MusicTrackInfo[] musicTracks = new MusicTrackInfo[16];
	}
}
