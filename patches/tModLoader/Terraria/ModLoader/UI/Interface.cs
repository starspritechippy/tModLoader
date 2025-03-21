using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.DownloadManager;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.GameContent.UI.States;
using Terraria.Social.Steam;

namespace Terraria.ModLoader.UI
{
	internal static class Interface
	{
		internal const int modsMenuID = 10000;
		internal const int modSourcesID = 10001;
		//set initial Main.menuMode to loadModsID
		internal const int loadModsID = 10002;
		internal const int buildModID = 10003;
		internal const int errorMessageID = 10005;
		internal const int reloadModsID = 10006;
		internal const int modBrowserID = 10007;
		internal const int modInfoID = 10008;
		//internal const int downloadModID = 10009;
		//internal const int modControlsID = 10010;
		//internal const int managePublishedID = 10011;
		internal const int updateMessageID = 10012;
		internal const int infoMessageID = 10013;
		//internal const int enterPassphraseMenuID = 10015;
		internal const int modPacksMenuID = 10016;
		internal const int tModLoaderSettingsID = 10017;
		//internal const int enterSteamIDMenuID = 10018;
		internal const int extractModID = 10019;
		internal const int downloadProgressID = 10020;
		internal const int progressID = 10023;
		internal const int modConfigID = 10024;
		internal const int createModID = 10025;
		internal const int exitID = 10026;
		internal static UIMods modsMenu = new UIMods();
		internal static UILoadMods loadMods = new UILoadMods();
		internal static UIModSources modSources = new UIModSources();
		internal static UIBuildMod buildMod = new UIBuildMod();
		internal static UIErrorMessage errorMessage = new UIErrorMessage();
		internal static UIModBrowser modBrowser = new UIModBrowser();
		internal static UIModInfo modInfo = new UIModInfo();
		//internal static UIManagePublished managePublished = new UIManagePublished();
		internal static UIUpdateMessage updateMessage = new UIUpdateMessage();
		internal static UIInfoMessage infoMessage = new UIInfoMessage();
		//internal static UIEnterPassphraseMenu enterPassphraseMenu = new UIEnterPassphraseMenu();
		internal static UIModPacks modPacksMenu = new UIModPacks();
		//internal static UIEnterSteamIDMenu enterSteamIDMenu = new UIEnterSteamIDMenu();
		internal static UIExtractMod extractMod = new UIExtractMod();
		internal static UIModConfig modConfig = new UIModConfig();
		internal static UIModConfigList modConfigList = new UIModConfigList();
		internal static UICreateMod createMod = new UICreateMod();
		internal static UIProgress progress = new UIProgress();
		internal static UIDownloadProgress downloadProgress = new UIDownloadProgress();

		// adds to Terraria.Main.DrawMenu in Main.menuMode == 0, after achievements
		//Interface.AddMenuButtons(this, this.selectedMenu, array9, array7, ref num, ref num3, ref num10, ref num5);
		internal static void AddMenuButtons(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons) {
			/* 
			 * string legacyInfoButton = Language.GetTextValue("tModLoader.13InfoButton");
			buttonNames[buttonIndex] = legacyInfoButton;
			if (selectedMenu == buttonIndex) {
				SoundEngine.PlaySound(SoundID.MenuOpen);
			}
			buttonIndex++;
			numButtons++;
			*/
		}

		internal static void ResetData() {
			modBrowser.Reset();
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
		}

		//internal static void AddSettingsMenuButtons(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, int[] virticalSpacing, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons)
		//{
		//	buttonIndex++;
		//	numButtons++;
		//	buttonNames[buttonIndex] = "Mod " + Lang.menu[66];
		//	if (selectedMenu == buttonIndex)
		//	{
		//		SoundEngine.PlaySound(10, -1, -1, 1);
		//		Main.menuMode = modControlsID;
		//	}
		//	for (int k = 0; k < numButtons; k++)
		//	{
		//		buttonScales[k] = 0.73f;
		//		virticalSpacing[k] = 0;
		//	}
		//	virticalSpacing[numButtons - 1] = 8;
		//}

		//add to end of if else chain of Main.menuMode in Terraria.Main.DrawMenu
		//Interface.ModLoaderMenus(this, this.selectedMenu, array9, array7, array4, ref num2, ref num4, ref num5, ref flag5);
		internal static void ModLoaderMenus(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, int[] buttonVerticalSpacing, ref int offY, ref int spacing, ref int numButtons, ref bool backButtonDown) {
			if (Main.menuMode == loadModsID) {
				if (ModLoader.ShowFirstLaunchWelcomeMessage) {
					ModLoader.ShowFirstLaunchWelcomeMessage = false;
					infoMessage.Show(Language.GetTextValue("tModLoader.FirstLaunchWelcomeMessage"), Main.menuMode);
				}

				/*
				else if (!ModLoader.AlphaWelcomed) {
					ModLoader.AlphaWelcomed = true;
					infoMessage.Show(Language.GetTextValue("tModLoader.WelcomeMessageBeta"), Main.menuMode);
					Main.SaveSettings();
				}
				*/
				else if (ModLoader.ShowWhatsNew) {
					ModLoader.ShowWhatsNew = false;
					if (File.Exists("RecentGitHubCommits.txt")) {
						bool LastLaunchedShaInRecentGitHubCommits = false;
						var messages = new System.Text.StringBuilder();
						var recentcommitsfilecontents = File.ReadLines("RecentGitHubCommits.txt");
						foreach (var commitEntry in recentcommitsfilecontents) {
							string[] parts = commitEntry.Split(' ', 2);
							if (parts.Length == 2) {
								string sha = parts[0];
								string message = parts[1];
								if (sha != ModLoader.LastLaunchedTModLoaderAlphaSha)
									messages.Append("\n  " + message);
								if (sha == ModLoader.LastLaunchedTModLoaderAlphaSha) {
									LastLaunchedShaInRecentGitHubCommits = true;
									break;
								}
							}
						}
						if (LastLaunchedShaInRecentGitHubCommits)
							infoMessage.Show(Language.GetTextValue("tModLoader.WhatsNewMessage") + messages.ToString(), Main.menuMode, null, Language.GetTextValue("tModLoader.ViewOnGitHub"),
								() => {
									SoundEngine.PlaySound(SoundID.MenuOpen);
									Utils.OpenToURL($"https://github.com/tModLoader/tModLoader/compare/{ModLoader.LastLaunchedTModLoaderAlphaSha}...1.4");
								});
					}
				}
				else if (ModLoader.PreviewFreezeNotification) {
					ModLoader.PreviewFreezeNotification = false;
					ModLoader.LastPreviewFreezeNotificationSeen = new Version(BuildInfo.tMLVersion.Major, BuildInfo.tMLVersion.Minor);
					infoMessage.Show(Language.GetTextValue("tModLoader.MonthlyFreezeNotification"), Main.menuMode, null, Language.GetTextValue("tModLoader.ModsMoreInfo"),
						() => {
							SoundEngine.PlaySound(SoundID.MenuOpen);
							Utils.OpenToURL($"https://github.com/tModLoader/tModLoader/wiki/tModLoader-Release-Cycle#14");
						});
					Main.SaveSettings();
				}
				else if (!ModLoader.DetectedModChangesForInfoMessage) { // Keep this at the end of the if/else chain since it doesn't necessarily change Main.menuMode
					ModLoader.DetectedModChangesForInfoMessage = true;

					string info = ModOrganizer.DetectModChangesForInfoMessage();
					if (info != null)
						infoMessage.Show(Language.GetTextValue("tModLoader.ShowNewUpdatedModsInfoMessage") + info, Main.menuMode);
				}
			}
			if (Main.MenuUI.CurrentState == modSources) {
				if (!ModLoader.SeenFirstLaunchModderWelcomeMessage) {
					ModLoader.SeenFirstLaunchModderWelcomeMessage = true;
					infoMessage.Show(Language.GetTextValue("tModLoader.MSFirstLaunchModderWelcomeMessage"), modSourcesID, null, Language.GetTextValue("tModLoader.ViewOnGitHub"),
						() => {
							SoundEngine.PlaySound(SoundID.MenuOpen);
							Utils.OpenToURL($"https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide");
						});
					Main.SaveSettings();
				}
			}
			if (Main.menuMode == modsMenuID) {
				Main.MenuUI.SetState(modsMenu);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == modSourcesID) {
				Main.MenuUI.SetState(modSources);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == createModID) {
				Main.MenuUI.SetState(createMod);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == loadModsID) {
				Main.menuMode = 888;
				Main.MenuUI.SetState(loadMods);
			}
			else if (Main.menuMode == buildModID) {
				Main.MenuUI.SetState(buildMod);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == errorMessageID) {
				Main.MenuUI.SetState(errorMessage);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == reloadModsID) {
				ModLoader.Reload();
			}
			else if (Main.menuMode == modBrowserID) {
				Main.MenuUI.SetState(modBrowser);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == modInfoID) {
				Main.MenuUI.SetState(modInfo);
				Main.menuMode = 888;
			}
			//else if (Main.menuMode == modControlsID)
			//{
			//	UIModControls.ModLoaderMenus(main, selectedMenu, buttonNames, buttonScales, buttonVerticalSpacing, ref offY, ref spacing, ref numButtons);
			//}
			else if (Main.menuMode == updateMessageID) {
				Main.MenuUI.SetState(updateMessage);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == infoMessageID) {
				Main.MenuUI.SetState(infoMessage);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == modPacksMenuID) {
				Main.MenuUI.SetState(modPacksMenu);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == extractModID) {
				Main.MenuUI.SetState(extractMod);
				Main.menuMode = 888;
			}
			else if(Main.menuMode == progressID) {
				Main.MenuUI.SetState(progress);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == downloadProgressID) {
				Main.MenuUI.SetState(downloadProgress);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == tModLoaderSettingsID) {
				offY = 210;
				spacing = 42;
				numButtons = 10;
				buttonVerticalSpacing[numButtons - 1] = 18;
				for (int i = 0; i < numButtons; i++) {
					buttonScales[i] = 0.75f;
				}
				int buttonIndex = 0;
				buttonNames[buttonIndex] = (ModNet.downloadModsFromServers ? Language.GetTextValue("tModLoader.DownloadFromServersYes") : Language.GetTextValue("tModLoader.DownloadFromServersNo"));
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModNet.downloadModsFromServers = !ModNet.downloadModsFromServers;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = (ModNet.onlyDownloadSignedMods ? Language.GetTextValue("tModLoader.DownloadSignedYes") : Language.GetTextValue("tModLoader.DownloadSignedNo"));
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModNet.onlyDownloadSignedMods = !ModNet.onlyDownloadSignedMods;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = (ModLoader.autoReloadAndEnableModsLeavingModBrowser ? Language.GetTextValue("tModLoader.AutomaticallyReloadAndEnableModsLeavingModBrowserYes") : Language.GetTextValue("tModLoader.AutomaticallyReloadAndEnableModsLeavingModBrowserNo"));
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModLoader.autoReloadAndEnableModsLeavingModBrowser = !ModLoader.autoReloadAndEnableModsLeavingModBrowser;
				}

				
				buttonIndex++;
				buttonNames[buttonIndex] = (ModLoader.autoReloadRequiredModsLeavingModsScreen ? Language.GetTextValue("tModLoader.AutomaticallyReloadRequiredModsLeavingModsScreenYes") : Language.GetTextValue("tModLoader.AutomaticallyReloadRequiredModsLeavingModsScreenNo"));
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModLoader.autoReloadRequiredModsLeavingModsScreen = !ModLoader.autoReloadRequiredModsLeavingModsScreen;
				}

				/*
				buttonIndex++;
				buttonNames[buttonIndex] = (Main.UseExperimentalFeatures ? Language.GetTextValue("tModLoader.ExperimentalFeaturesYes") : Language.GetTextValue("tModLoader.ExperimentalFeaturesNo"));
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					Main.UseExperimentalFeatures = !Main.UseExperimentalFeatures;
				}
				*/

				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue($"tModLoader.RemoveForcedMinimumZoom{(ModLoader.removeForcedMinimumZoom ? "Yes" : "No")}");
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModLoader.removeForcedMinimumZoom = !ModLoader.removeForcedMinimumZoom;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue($"tModLoader.AttackSpeedScalingTooltipVisibility{ModLoader.attackSpeedScalingTooltipVisibility}");
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModLoader.attackSpeedScalingTooltipVisibility = (ModLoader.attackSpeedScalingTooltipVisibility + 1) % 3;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue($"tModLoader.ShowMemoryEstimates{(ModLoader.showMemoryEstimates ? "Yes" : "No")}");
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModLoader.showMemoryEstimates = !ModLoader.showMemoryEstimates;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue($"tModLoader.ShowModMenuNotifications{(ModLoader.notifyNewMainMenuThemes ? "Yes" : "No")}");
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModLoader.notifyNewMainMenuThemes = !ModLoader.notifyNewMainMenuThemes;
				}

				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue($"tModLoader.ShowNewUpdatedModsInfo{(ModLoader.showNewUpdatedModsInfo ? "Yes" : "No")}");
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModLoader.showNewUpdatedModsInfo = !ModLoader.showNewUpdatedModsInfo;
				}

				/*
				buttonIndex++;
				buttonNames[buttonIndex] = Language.GetTextValue("tModLoader.ClearMBCredentials");
				if (selectedMenu == buttonIndex) {
					SoundEngine.PlaySound(SoundID.MenuTick);
					ModLoader.modBrowserPassphrase = "";
					ModLoader.SteamID64 = "";
				}
				*/

				buttonIndex++;
				buttonNames[buttonIndex] = Lang.menu[5].Value;
				if (selectedMenu == buttonIndex || backButtonDown) {
					backButtonDown = false;
					Main.menuMode = 11;
					SoundEngine.PlaySound(11, -1, -1, 1);
				}
			}
			else if (Main.menuMode == modConfigID)
			{
				Main.MenuUI.SetState(modConfig);
				Main.menuMode = 888;
			}
			else if (Main.menuMode == exitID) {
				Environment.Exit(0);
			}
		}

		internal static void ServerModMenu(out bool reloadMods) {
			bool exit = false;

			reloadMods = false;

			while (!exit) {
				Console.WriteLine("Terraria Server " + Main.versionNumber2 + " - " + ModLoader.versionedName);
				Console.WriteLine();
				var mods = ModOrganizer.FindMods(logDuplicates: true);
				for (int k = 0; k < mods.Length; k++) {
					Console.Write((k + 1) + "\t\t" + mods[k].DisplayName);
					Console.WriteLine(" (" + (mods[k].Enabled ? "enabled" : "disabled") + ")");
				}
				if (mods.Length == 0) {
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine(Language.GetTextValue("tModLoader.ModsNotFoundServer", ModLoader.ModPath));
					Console.ResetColor();
				}
				Console.WriteLine("e\t\t" + Language.GetTextValue("tModLoader.ModsEnableAll"));
				Console.WriteLine("d\t\t" + Language.GetTextValue("tModLoader.ModsDisableAll"));
				Console.WriteLine("r\t\t" + Language.GetTextValue("tModLoader.ModsReloadAndReturn"));
				Console.WriteLine(Language.GetTextValue("tModLoader.AskForModIndex"));
				Console.WriteLine();
				Console.WriteLine(Language.GetTextValue("tModLoader.AskForCommand"));
				string command = Console.ReadLine();
				if (command == null) {
					command = "";
				}
				command = command.ToLower();
				Console.Clear();
				if (command == "e") {
					foreach (var mod in mods) {
						mod.Enabled = true;
					}
				}
				else if (command == "d") {
					foreach (var mod in mods) {
						mod.Enabled = false;
					}
				}
				else if (command == "r") {
					//Do not reload mods here, just to ensure that Main.DedServ_PostModLoad isn't in the call stack during mod reload, to allow hooking into it.
					reloadMods = true;
					exit = true;
				}
				else if (int.TryParse(command, out int value) && value > 0 && value <= mods.Length) {
					mods[value - 1].Enabled ^= true;
				}
			}
		}

		internal static void ServerModBrowserMenu() {
			bool exit = false;
			Console.Clear();
			while (!exit) {
				Console.WriteLine();
				Console.WriteLine("b\t\t" + Language.GetTextValue("tModLoader.MBServerReturnToMenu"));
				Console.WriteLine();
				Console.WriteLine(Language.GetTextValue("tModLoader.MBServerAskForModName"));
				string command = Console.ReadLine();
				if (command == null) {
					command = "";
				}
				if (command == "b" || command == "B") {
					exit = true;
				}
				else {
					string modname = command;
					try {
						if (!WorkshopHelper.QueryHelper.CheckWorkshopConnection()) {
							Console.WriteLine(Language.GetTextValue("NoWorkshopAccess"));
							break;
						}

						var info = WorkshopHelper.QueryHelper.FindModDownloadItem(modname);
						if(info == null)
							Console.WriteLine($"No mod with the name {modname} found on the workshop.");
						else
							info.InnerDownloadWithDeps();
					}
					catch (Exception e) {
						Console.WriteLine(Language.GetTextValue("tModLoader.MBServerDownloadError", modname, e.ToString()));
					}
				}
			}
			//Console.Clear();
		}

		internal static void MessageBoxShow(string text, string caption = null) {
			// MessageBox.Show fails on Mac, this method will open a text file to show a message.
			caption = caption ?? "Terraria: Error" + $" ({ModLoader.versionedName})";
			string logsLoc = Path.Combine(Directory.GetCurrentDirectory(), "tModLoader-Logs");

			string message = Language.GetTextValue("tModLoader.ClientLogHint", text, logsLoc);
			if(Language.ActiveCulture == null) // Simple backup approach in case error happens before localization is loaded
				message = string.Format("{0}\n\nA client.log file containing error information has been generated in\n{1}\n(You will need to share this file if asking for help)", text, logsLoc);
#if !NETCORE
#if !MAC
			System.Windows.Forms.MessageBox.Show(message, caption);
#else
			File.WriteAllText("fake-messagebox.txt", $"{caption}\n\n{text}");
			Process.Start("fake-messagebox.txt");
#endif
#else
			SDL2.SDL.SDL_ShowSimpleMessageBox(SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, caption, message, IntPtr.Zero);
#endif
		}

		internal static void MessageBoxShow(Exception e, string caption = null, bool generateTip = false) {
			string tip = "";

			if (generateTip) {
				if (e is OutOfMemoryException)
					tip = Language.GetTextValue("tModLoader.OutOfMemoryHint");
				else if (e is InvalidOperationException || e is NullReferenceException || e is IndexOutOfRangeException || e is ArgumentNullException)
					tip = Language.GetTextValue("tModLoader.ModExceptionHint");
				else if (e is IOException && e.Message.Contains("cloud file provider"))
					tip = Language.GetTextValue("tModLoader.OneDriveHint");
				else if (e is System.Threading.SynchronizationLockException)
					tip = Language.GetTextValue("tModLoader.AntivirusHint");
				else if (e is TypeInitializationException)
					tip = Language.GetTextValue("tModLoader.TypeInitializationHint");
			}

			string message = e.ToString() + tip;

			MessageBoxShow(message, caption);
		}
	}
}
