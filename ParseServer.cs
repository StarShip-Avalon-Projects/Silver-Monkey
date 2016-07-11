using System;

public class Class1
{
	public Class1()
	{
        

private void ParseServerData(string data, bool Handled)
{
	// page = engine.LoadFromString(cBot.MS_Script)
	if (data == "Dragonroar") {
		BotConnecting();
		//  Login Sucessful
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
	} else if (data == "&&&&&&&&&&&&&") {
		loggingIn = 2;
		TS_Status_Client.Image = My.Resources.images3;
		ReLogCounter = 0;
		//(0:1) When the bot logs into furcadia,
		MainMSEngine.PageExecute(1);
		if ((ReconnectTimeOutTimer != null))
			ReconnectTimeOutTimer.Dispose();
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		// Species Tags
	} else if (data.StartsWith("]-")) {
		if (data.StartsWith("]-#A")) {
			SpeciesTag.Enqueue(data.Substring(4));
		} else if (data.StartsWith("]-#B")) {
			BadgeTag.Enqueue(data.Substring(2));
		}

		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//DS Variables

		//Popup Dialogs!
	} else if (data.StartsWith("]#")) {
		//]#<idstring> <style 0-17> <message that might have spaces in>
		Regex repqq = new Regex("^\\]#(.*?) (\\d+) (.*?)$");
		System.Text.RegularExpressions.Match m = repqq.Match(data);
		Rep r = default(Rep);
		r.ID = m.Groups(1).Value;
		r.type = m.Groups(2).Value.ToInteger;
		Repq.Enqueue(r);
		MainMSEngine.PageSetVariable("MESSAGE", m.Groups(3).Value, true);
		Player.Message = m.Groups(3).Value;
		MainMSEngine.PageExecute(95, 96);
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
	} else if (data.StartsWith("0")) {
		InDream = true;
		//Phoenix Speak event
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
	} else if (data.StartsWith("3")) {
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//self Induced Dragon Speak Event
	} else if (data.StartsWith("6")) {
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Dragon Speak event
	} else if (data.StartsWith("7")) {
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Dragon Speak Addon (Follows Instructions 6 and 7
	} else if (data.StartsWith("8")) {
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//]s(.+)1 (.*?) (.*?) 0
	} else if (data.StartsWith("]s")) {
		Regex t = new Regex("\\]s(.+)1 (.*?) (.*?) 0", RegexOptions.IgnoreCase);
		System.Text.RegularExpressions.Match m = t.Match(data);

		if (BotName.ToFurcShortName == m.Groups(2).Value.ToFurcShortName) {
			MainMSEngine.PageExecute();
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Look response
	} else if (data.StartsWith("]f") & bConnected() & InDream == true) {
		short length = 14;
		if (Look) {
			LookQue.Enqueue(data.Substring(2));
		} else {
			if (data.Substring(2, 1) != "t") {
				length = 30;
			} else {
				length = 14;
			}
			try {
				Player = NametoFurre(data.Remove(0, length + 2), true);
				// If Player.ID = 0 Then Exit Sub
				Player.Color = data.Substring(2, length);
				if (IsBot(Player))
					Look = false;
				if (DREAM.List.ContainsKey(Player.ID))
					DREAM.List.Item(Player.ID) = Player;
			} catch (Exception eX) {
				ErrorLogging logError = new ErrorLogging(eX, this);
			}

		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Spawn Avatar
	} else if (data.StartsWith("<") & bConnected()) {
		try {
			if (data.Length < 29)
				return;
			// Debug.Print(data)
			Player.ID = ConvertFromBase220(data.Substring(1, 4));

			if (DREAM.List.ContainsKey(Player.ID)) {
				Player = DREAM.List.Item(Player.ID);
			}


			Player.X = Convert.ToUInt32(ConvertFromBase220(data.Substring(5, 2)) * 2);
			Player.Y = ConvertFromBase220(data.Substring(7, 2));
			Player.Shape = ConvertFromBase220(data.Substring(9, 2));



			uint NameLength = ConvertFromBase220(data.Substring(11, 1));
			Player.Name = data.Substring(12, Convert.ToInt32(NameLength)).Replace("|", " ");

			uint ColTypePos = Convert.ToUInt32(12 + NameLength);
			Player.ColorType = Convert.ToChar(data.Substring(Convert.ToInt32(ColTypePos), 1));
			uint ColorSize = 10;
			if (Player.ColorType != "t") {
				ColorSize = 30;
			}
			int sColorPos = Convert.ToInt32(ColTypePos + 1);

			Player.Color = data.Substring(sColorPos, Convert.ToInt32(ColorSize));

			int FlagPos = data.Length - 6;
			Player.Flag = Convert.ToInt32(ConvertFromBase220(data.Substring(FlagPos, 1)));
			int AFK_Pos = data.Length - 5;
			string AFKStr = data.Substring(AFK_Pos, 4);
			Player.AFK = ConvertFromBase220(data.Substring(AFK_Pos, 4));
			int FlagCheck = Convert.ToInt32(Flags.CHAR_FLAG_NEW_AVATAR) - Player.Flag;

			// Add New Arrivals to Dream List
			// One or the other will trigger it
			IsBot(Player);
			MainMSEngine.PageSetVariable(MS_Name, Player.ShortName);

			if (Player.Flag == 4 | !DREAM.List.ContainsKey(Player.ID)) {
				DREAM.List.Add(Player.ID, Player);
				if (InDream)
					UpDateDreamList(Player.Name);
				if (Player.Flag == 2) {
					FURRE Bot = fIDtoFurre(Convert.ToUInt32(BotUID));
					ViewArea VisableRectangle = getTargetRectFromCenterCoord(Convert.ToInt32(Bot.X), Convert.ToInt32(Bot.Y));
					if (VisableRectangle.X <= Player.X & VisableRectangle.Y <= Player.Y & VisableRectangle.height >= Player.Y & VisableRectangle.length >= Player.X) {
						Player.Visible = true;
					} else {
						Player.Visible = false;
					}
					MainMSEngine.PageExecute(28, 29, 24, 25);
				} else {
					MainMSEngine.PageExecute(24, 25);
				}
			} else if (Player.Flag == 2) {
				FURRE Bot = fIDtoFurre(Convert.ToUInt32(BotUID));
				ViewArea VisableRectangle = getTargetRectFromCenterCoord(Convert.ToInt32(Bot.X), Convert.ToInt32(Bot.Y));
				if (VisableRectangle.X <= Player.X & VisableRectangle.Y <= Player.Y & VisableRectangle.height >= Player.Y & VisableRectangle.length >= Player.X) {
					Player.Visible = true;
				} else {
					Player.Visible = false;
				}
				MainMSEngine.PageExecute(28, 29);


			} else if (Player.Flag == 1) {

			} else if (Player.Flag == 0) {
			}
			if (DREAM.List.ContainsKey(Player.ID)) {
				DREAM.List.Item(Player.ID) = Player;
			}

		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
			return;
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Remove Furre
	//And loggingIn = False
	} else if (data.StartsWith(")") & bConnected()) {
		try {
			uint remID = ConvertFromBase220(data.Substring(1, 4));
			// remove departure from List
			if (DREAM.List.ContainsKey(remID) == true) {
				Player = DREAM.List.Item(remID);
				MainMSEngine.PageSetVariable(MS_Name, Player.Name);
				MainMSEngine.PageExecute(26, 27, 30, 31);
				DREAM.List.Remove(remID);
				UpDateDreamList("");
			}
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Animated Move
	//And loggingIn = False
	} else if (data.StartsWith("/") & bConnected()) {
		try {
			Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)));
			Player.X = Convert.ToUInt32(ConvertFromBase220(data.Substring(5, 2)) * 2);
			Player.Y = ConvertFromBase220(data.Substring(7, 2));
			Player.Shape = ConvertFromBase220(data.Substring(9, 2));
			FURRE Bot = fIDtoFurre(Convert.ToUInt32(BotUID));
			ViewArea VisableRectangle = getTargetRectFromCenterCoord(Convert.ToInt32(Bot.X), Convert.ToInt32(Bot.Y));
			if (VisableRectangle.X <= Player.X & VisableRectangle.Y <= Player.Y & VisableRectangle.height >= Player.Y & VisableRectangle.length >= Player.X) {
				Player.Visible = true;
			} else {
				Player.Visible = false;
			}
			if (DREAM.List.ContainsKey(Player.ID))
				DREAM.List.Item(Player.ID) = Player;
			IsBot(Player);
			MainMSEngine.PageSetVariable(MS_Name, Player.ShortName);
			MainMSEngine.PageExecute(28, 29, 30, 31, 601, 602);
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		// Move Avatar
	//And loggingIn = False
	} else if (data.StartsWith("A") & bConnected()) {
		try {
			Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)));
			Player.X = Convert.ToUInt32(ConvertFromBase220(data.Substring(5, 2)) * 2);
			Player.Y = ConvertFromBase220(data.Substring(7, 2));
			Player.Shape = ConvertFromBase220(data.Substring(9, 2));

			FURRE Bot = fIDtoFurre(Convert.ToUInt32(BotUID));
			ViewArea VisableRectangle = getTargetRectFromCenterCoord(Convert.ToInt32(Bot.X), Convert.ToInt32(Bot.Y));

			if (VisableRectangle.X <= Player.X & VisableRectangle.Y <= Player.Y & VisableRectangle.height >= Player.Y & VisableRectangle.length >= Player.X) {
				Player.Visible = true;
			} else {
				Player.Visible = false;
			}
			if (DREAM.List.ContainsKey(Player.ID))
				DREAM.List.Item(Player.ID) = Player;

			IsBot(Player);
			MainMSEngine.PageSetVariable(MS_Name, Player.ShortName);
			MainMSEngine.PageExecute(28, 29, 30, 31, 601, 602);
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		// Update Color Code
	//And loggingIn = False
	} else if (data.StartsWith("B") != false & bConnected() & InDream) {
		try {
			Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)));
			Player.Shape = ConvertFromBase220(data.Substring(5, 2));
			uint ColTypePos = 7;
			Player.ColorType = Convert.ToChar(data.Substring(Convert.ToInt32(ColTypePos), 1));
			uint ColorSize = 10;
			if (Player.ColorType != "t") {
				ColorSize = 30;
			}
			uint sColorPos = Convert.ToUInt32(ColTypePos + 1);
			Player.Color = data.Substring(Convert.ToInt32(sColorPos), Convert.ToInt32(ColorSize));
			if (DREAM.List.ContainsKey(Player.ID))
				DREAM.List.Item(Player.ID) = Player;
			IsBot(Player);
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Hide Avatar
	//And loggingIn = False
	} else if (data.StartsWith("C") != false & bConnected()) {
		try {
			Player = fIDtoFurre(ConvertFromBase220(data.Substring(1, 4)));
			Player.X = Convert.ToUInt32(ConvertFromBase220(data.Substring(5, 2)) * 2);
			Player.Y = ConvertFromBase220(data.Substring(7, 2));
			Player.Visible = false;
			if (DREAM.List.ContainsKey(Player.ID))
				DREAM.List.Item(Player.ID) = Player;
			IsBot(Player);
			MainMSEngine.PageSetVariable(MS_Name, Player.Name);
			MainMSEngine.PageExecute(30, 31);
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Display Disconnection Dialog
	#if DEBUG
	} else if (data.StartsWith("[")) {
		Console.WriteLine("Disconnection Dialog:" + data);
		#endif
		InDream = false;
		DREAM.List.Clear();
		UpDateDreamList("");

		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		Interaction.MsgBox(data, MsgBoxStyle.Critical, "Disconnection Error");

		return;


		//;{mapfile}	Load a local map (one in the furcadia folder)
		//]q {name} {id}	Request to download a specific patch
	} else if (data.StartsWith(";") || data.StartsWith("]q") || data.StartsWith("]r")) {
		#if DEBUG
		try {
			Debug.Print("Entering new Dream" + data);
			#endif
			MainMSEngine.PageSetVariable("DREAMOWNER", "");
			MainMSEngine.PageSetVariable("DREAMNAME", "");
			HasShare = false;
			NoEndurance = false;

			DREAM.List.Clear();
			UpDateDreamList("");
			InDream = false;
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
	} else if (data.StartsWith("]z")) {
		BotUID = uint.Parse(data.Remove(0, 2));
		//Snag out UID
	} else if (data.StartsWith("]B")) {
		try {
			BotUID = uint.Parse(data.Remove(0, 2));
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
	} else if (data.StartsWith("~")) {
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
	} else if (data.StartsWith("=")) {
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
	#if DEBUG
	} else if (data.StartsWith("]c")) {
		Console.WriteLine(data);
		#endif
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
	} else if (data.StartsWith("]C")) {
		if (data.StartsWith("]C0")) {
			string dname = data.Substring(10);
			if (dname.Contains(":")) {
				string NameStr = dname.Substring(0, dname.IndexOf(":"));
				if (NameStr.ToFurcShortName == BotName.ToFurcShortName) {
					HasShare = true;
				}
				MainMSEngine.PageSetVariable(Main.VarPrefix + "DREAMOWNER", NameStr);
			} else if (dname.EndsWith("/") && !dname.Contains(":")) {
				string NameStr = dname.Substring(0, dname.IndexOf("/"));
				if (NameStr.ToFurcShortName == BotName.ToFurcShortName) {
					HasShare = true;
				}
				MainMSEngine.PageSetVariable("DREAMOWNER", NameStr);
			}

			MainMSEngine.PageSetVariable("DREAMNAME", dname);
			MainMSEngine.PageExecute(90, 91);
		}
		#if DEBUG
		Console.WriteLine(data);
		#endif
		if (smProxy.IsClientConnected)
			smProxy.SendClient(data + Constants.vbLf);
		return;
		//Process Channels Seperatly
	} else if (data.StartsWith("(")) {
		if (ThroatTired == false & data.StartsWith("(<font color='warning'>Your throat is tired. Try again in a few seconds.</font>")) {
			ThroatTired = true;

			//Throat Tired Syndrome, Halt all out going data for a few seconds
			TimeSpan Ts = TimeSpan.FromSeconds(cMain.TT_TimeOut);
			TroatTiredDelay = new Threading.Timer(TroatTiredDelayTick, null, Ts, Ts);
			//(0:92) When the bot detects the "Your throat is tired. Please wait a few seconds" message,
			MainMSEngine.PageExecute(92);
			//Exit Sub
			if (smProxy.IsClientConnected)
				smProxy.SendClient(data + Constants.vbLf);
			return;
		}

		ChannelProcess(ref data, Handled);
		return;
	}

	if (smProxy.IsClientConnected)
		smProxy.SendClient(data + Constants.vbLf);

}

int pslen = 0;
char NextLetter = null;
object ChannelLock = new object();
public string Channel;
/// <summary>
/// Channel Parser
/// RegEx Style Processing here
/// </summary>
/// <param name="data"></param>
/// <remarks></remarks>
public void ChannelProcess(ref string data, bool Handled)
{
	//Strip the trigger Character
	// page = engine.LoadFromString(cBot.MS_Script)
	data = data.Remove(0, 1);
	bool psCheck = false;
	string SpecTag = "";
	Channel = Regex.Match(data, ChannelNameFilter).Groups(1).Value;
	string Color = Regex.Match(data, EntryFilter).Groups(1).Value;
	string User = "";
	string Desc = "";
	string Text = "";
	if (!Handled) {
		Text = Regex.Match(data, EntryFilter).Groups(2).Value;
		User = Regex.Match(data, NameFilter).Groups(3).Value;
		if (!string.IsNullOrEmpty(User))
			Player = NametoFurre(User, true);
		Player.Message = "";
		Desc = Regex.Match(data, DescFilter).Groups(2).Value;
		Regex mm = new Regex(Iconfilter);
		System.Text.RegularExpressions.Match ds = mm.Match(Text);
		Text = mm.Replace(Text, "[" + ds.Groups(1).Value + "] ");
		Regex s = new Regex(ChannelNameFilter);
		Text = s.Replace(Text, "");
	} else {
		User = Player.Name;
		Text = Player.Message;
	}
	DiceSides = 0;
	DiceCount = 0;
	DiceCompnentMatch = "";
	DiceModifyer = 0;
	DiceResult = 0;

	lock (Lock) {
		ErrorMsg = "";
		ErrorNum = 0;
	}
	if (Channel == "@news" | Channel == "@spice") {
		try {
			sndDisplay(Text);
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (Color == "success") {
		
			return;


	} else if (Channel == "@roll") {
		Regex DiceREGEX = new Regex(DiceFilter, RegexOptions.IgnoreCase);
		System.Text.RegularExpressions.Match DiceMatch = DiceREGEX.Match(data);

		//Matches, in order:
		//1:      shortname()
		//2:      full(name)
		//3:      dice(count)
		//4:      sides()
		//5: +/-#
		//6: +/-  (component match)
		//7:      additional(Message)
		//8:      Final(result)

		Player = NametoFurre(DiceMatch.Groups(3).Value, true);
		Player.Message = DiceMatch.Groups(7).Value;
		MainMSEngine.PageSetVariable(Main.VarPrefix + "MESSAGE", DiceMatch.Groups(7).Value);
		double.TryParse(DiceMatch.Groups(4).Value, DiceSides);
		double.TryParse(DiceMatch.Groups(3).Value, DiceCount);
		DiceCompnentMatch = DiceMatch.Groups(6).Value;
		DiceModifyer = 0.0;
		double.TryParse(DiceMatch.Groups(5).Value, DiceModifyer);
		double.TryParse(DiceMatch.Groups(8).Value, DiceResult);

		if (IsBot(Player)) {
			MainMSEngine.PageExecute(130, 131, 132, 136);
		} else {
			MainMSEngine.PageExecute(133, 134, 135, 136);
		}

		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (Channel == "@dragonspeak" || Channel == "@emit" || Color == "emit") {
		try {
			//(<font color='dragonspeak'><img src='fsh://system.fsh:91' alt='@emit' /><channel name='@emit' /> Furcadian Academy</font>
			sndDisplay(Text, fColorEnum.Emit);

			MainMSEngine.PageSetVariable(Main.VarPrefix + "MESSAGE", Text.Substring(5));
			// Execute (0:21) When someone emits something
			MainMSEngine.PageExecute(21, 22, 23);
			// Execute (0:22) When someone emits {...}
			//' Execute (0:23) When someone emits something with {...} in it

		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
		//'BCast (Advertisments, Announcments)
	} else if (Color == "bcast") {
		string AdRegEx = "<channel name='(.*)' />";

		string chan = Regex.Match(data, AdRegEx).Groups(1).Value;

		try {
			switch (chan) {
				case "@advertisements":
					if (cMain.Advertisment)
						return;

					AdRegEx = "\\[(.*?)\\] (.*?)</font>";
					string adMessage = Regex.Match(data, AdRegEx).Groups(2).Value;
					sndDisplay(Text);
					break;
				case "@bcast":
					if (cMain.Broadcast)
						return;

					string u = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value;
					sndDisplay(Text);
					break;
				case "@announcements":
					if (cMain.Announcement)
						return;

					string u = Regex.Match(data, "<channel name='@(.*?)' />(.*?)</font>").Groups(2).Value;
					sndDisplay(Text);
					break;
				default:
					#if DEBUG
					Console.WriteLine("Unknown ");
					Console.WriteLine("BCAST:" + data);
					#endif
					break;
			}


		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
		//'SAY
	} else if (Color == "myspeech") {
		try {
			Regex t = new Regex(YouSayFilter);
			string u = t.Match(data).Groups(1).ToString;
			Text = t.Match(data).Groups(2).ToString;
			if (SpeciesTag.Count() > 0) {
				SpecTag = SpeciesTag.Peek;
				SpeciesTag.Dequeue();
				Player.Color = SpecTag;
				if (DREAM.List.ContainsKey(Player.ID))
					DREAM.List.Item(Player.ID) = Player;
			}

			sndDisplay("You " + u + ", \"" + Text + "\"", fColorEnum.Say);
			Player.Message = Text;
			MainMSEngine.PageSetVariable(Main.VarPrefix + "MESSAGE", Text);
			// Execute (0:5) When some one says something
			//MainMSEngine.PageExecute(5, 6, 7, 18, 19, 20)
			//' Execute (0:6) When some one says {...}
			//' Execute (0:7) When some one says something with {...} in it
			//' Execute (0:18) When someone says or emotes something
			//' Execute (0:19) When someone says or emotes {...}
			//' Execute (0:20) When someone says or emotes something with {...} in it
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (!string.IsNullOrEmpty(User) & string.IsNullOrEmpty(Channel) & string.IsNullOrEmpty(Color) & Regex.Match(data, NameFilter).Groups(2).Value != "forced") {
		System.Text.RegularExpressions.Match tt = Regex.Match(data, "\\(you see(.*?)\\)", RegexOptions.IgnoreCase);
		Regex t = new Regex(NameFilter);

		if (!tt.Success) {
			try {
				Text = t.Replace(data, "");
				Text = Text.Remove(0, 2);

				if (SpeciesTag.Count() > 0) {
					SpecTag = SpeciesTag.Peek;
					SpeciesTag.Clear();
					Player.Color = SpecTag;
					if (DREAM.List.ContainsKey(Player.ID))
						DREAM.List.Item(Player.ID) = Player;
				}
				Channel = "say";
				sndDisplay(User + " says, \"" + Text + "\"", fColorEnum.Say);
				MainMSEngine.PageSetVariable(MS_Name, User);
				MainMSEngine.PageSetVariable("MESSAGE", Text);
				Player.Message = Text;
				// Execute (0:5) When some one says something
				MainMSEngine.PageExecute(5, 6, 7, 18, 19, 20);
				// Execute (0:6) When some one says {...}
				// Execute (0:7) When some one says something with {...} in it
				// Execute (0:18) When someone says or emotes something
				// Execute (0:19) When someone says or emotes {...}
				// Execute (0:20) When someone says or emotes something with {...} in it

			} catch (Exception eX) {
				ErrorLogging logError = new ErrorLogging(eX, this);

			}

			if (smProxy.IsClientConnected)
				smProxy.SendClient("(" + data + Constants.vbLf);
			return;
		} else {
			try {
				//sndDisplay("You See '" & User & "'")
				Look = true;
			} catch (Exception eX) {
				ErrorLogging logError = new ErrorLogging(eX, this);
			}
		}

	} else if (!string.IsNullOrEmpty(Desc)) {
		try {
			string DescName = Regex.Match(data, DescFilter).Groups(1).ToString();

			Player = NametoFurre(DescName, true);
			if (LookQue.Count > 0) {
				string colorcode = LookQue.Peek;
				if (colorcode.StartsWith("t")) {
					colorcode = colorcode.Substring(0, 14);

				} else if (colorcode.StartsWith("u")) {
				} else if (colorcode.StartsWith("v")) {
					//RGB Values
				}
				Player.Color = colorcode;
				LookQue.Dequeue();
			}
			if (BadgeTag.Count() > 0) {
				SpecTag = BadgeTag.Peek;
				BadgeTag.Clear();
				Player.Badge = SpecTag;
			} else if (!string.IsNullOrEmpty(Player.Badge)) {
				Player.Badge = "";
			}
			Player.Desc = Desc.Substring(6);
			if (DREAM.List.ContainsKey(Player.ID))
				DREAM.List.Item(Player.ID) = Player;
			MainMSEngine.PageSetVariable(MS_Name, DescName);
			MainMSEngine.PageExecute(600);
			//sndDisplay)
			if (string.IsNullOrEmpty(Player.Tag)) {
				sndDisplay("You See '" + Player.Name + "'\\par" + Desc);
			} else {
				sndDisplay("You See '" + Player.Name + "'\\par" + Player.Tag + " " + Desc);
			}
			Look = false;
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (Color == "shout") {
		//'SHOUT
		try {
			Regex t = new Regex(YouSayFilter);
			string u = t.Match(data).Groups(1).ToString;
			Text = t.Match(data).Groups(2).ToString;
			if (string.IsNullOrEmpty(User)) {
				sndDisplay("You " + u + ", \"" + Text + "\"", fColorEnum.Shout);
			} else {
				Text = Regex.Match(data, "shouts: (.*)</font>").Groups(1).ToString();
				sndDisplay(User + " shouts, \"" + Text + "\"", fColorEnum.Shout);
			}
			if (!IsBot(Player)) {
				MainMSEngine.PageSetVariable(Main.VarPrefix + "MESSAGE", Text);
				Player.Message = Text;
				// Execute (0:8) When some one shouts something
				MainMSEngine.PageExecute(8, 9, 10);
				// Execute (0:9) When some one shouts {...}
				// Execute (0:10) When some one shouts something with {...} in it


			}
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (Color == "query") {
		string QCMD = Regex.Match(data, "<a.*?href='command://(.*?)'>").Groups(1).ToString;
		//Player = NametoFurre(User, True)
		switch (QCMD) {
			case "summon":
				//'JOIN
				try {
					sndDisplay(User + " requests to join you.");
					//If Not IsBot(Player) Then
					MainMSEngine.PageExecute(34, 35);
					//End If
				} catch (Exception eX) {
					ErrorLogging logError = new ErrorLogging(eX, this);
				}
				break;
			case "join":
				//'SUMMON
				try {
					sndDisplay(User + " requests to summon you.");
					//If Not IsBot(Player) Then
					MainMSEngine.PageExecute(32, 33);
					//End If
				} catch (Exception eX) {
					ErrorLogging logError = new ErrorLogging(eX, this);
				}
				break;
			case "follow":
				//'LEAD
				try {
					sndDisplay(User + " requests to lead.");
					//If Not IsBot(Player) Then
					MainMSEngine.PageExecute(36, 37);
					//End If
				} catch (Exception eX) {
					ErrorLogging logError = new ErrorLogging(eX, this);
				}
				break;
			case "lead":
				//'FOLLOW
				try {
					sndDisplay(User + " requests the bot to follow.");
					//If Not IsBot(Player) Then
					MainMSEngine.PageExecute(38, 39);
					//End If
				} catch (Exception eX) {
					ErrorLogging logError = new ErrorLogging(eX, this);
				}
				break;
			case "cuddle":
				try {
					sndDisplay(User + " requests the bot to cuddle.");
					//If Not IsBot(Player) Then
					MainMSEngine.PageExecute(40, 41);
					//End If
				} catch (Exception eX) {
					ErrorLogging logError = new ErrorLogging(eX, this);
				}
				break;
			default:
				sndDisplay("## Unknown " + Channel + "## " + data);
				break;
		}

		//NameFilter

		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (Color == "whisper") {
		//'WHISPER
		try {
			string WhisperFrom = Regex.Match(data, "whispers, \"(.*?)\" to you").Groups(1).Value;
			string WhisperTo = Regex.Match(data, "You whisper \"(.*?)\" to").Groups(1).Value;
			string WhisperDir = Regex.Match(data, string.Format("<name shortname='(.*?)' src='whisper-(.*?)'>")).Groups(2).Value;
			if (WhisperDir == "from") {
				//Player = NametoFurre(User, True)
				Player.Message = WhisperFrom;
				if (BadgeTag.Count() > 0) {
					SpecTag = BadgeTag.Peek;
					BadgeTag.Clear();
					Player.Badge = SpecTag;
				} else {
					Player.Badge = "";
				}

				if (DREAM.List.ContainsKey(Player.ID))
					DREAM.List.Item(Player.ID) = Player;


				sndDisplay(User + " whispers\"" + WhisperFrom + "\" to you.", fColorEnum.Whisper);
				if (!IsBot(Player)) {
					MainMSEngine.PageSetVariable(Main.VarPrefix + "MESSAGE", Player.Message);
					// Execute (0:15) When some one whispers something
					MainMSEngine.PageExecute(15, 16, 17);
					// Execute (0:16) When some one whispers {...}
					// Execute (0:17) When some one whispers something with {...} in it
				}


			} else {
				WhisperTo = WhisperTo.Replace("<wnd>", "");
				sndDisplay("You whisper\"" + WhisperTo + "\" to " + User + ".", fColorEnum.Whisper);
			}
		} catch (Exception eX) {
			ErrorLogging logError = new ErrorLogging(eX, this);
		}
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (Color == "warning") {

		return;
	} else if (Color == "trade") {

		return;
	} else if (Color == "emote") {


		return;
	} else if (Color == "channel") {

return;
	} else if (Color == "notify") {
		string NameStr = "";
		if (Text.StartsWith("Players banished from your dreams: ")) {
			//Banish-List
			//[notify> Players banished from your dreams:  
			//`(0:54) When the bot sees the banish list
			BanishString.Clear();
			string[] tmp = Text.Substring(35).Split(',');
			foreach (string t in tmp) {
				BanishString.Add(t);
			}
			MainMSEngine.PageSetVariable(VarPrefix + "BANISHLIST", string.Join(" ", BanishString.ToArray));
			MainMSEngine.PageExecute(54);

		} else if (Text.StartsWith("The banishment of player ")) {
			//banish-off <name> (on list)
			//[notify> The banishment of player (.*?) has ended.  

			//(0:56) When the bot successfully removes a furre from the banish list,
			//(0:58) When the bot successfully removes the furre named {...} from the banish list,
			Regex t = new Regex("The banishment of player (.*?) has ended.");
			NameStr = t.Match(data).Groups(1).Value;
			MainMSEngine.PageSetVariable("BANISHNAME", NameStr);
			MainMSEngine.PageExecute(56, 56);
			for (int I = 0; I <= BanishString.Count - 1; I++) {
				if (BanishString.Item(I).ToString.ToFurcShortName == NameStr.ToFurcShortName) {
					BanishString.RemoveAt(I);
					break; // TODO: might not be correct. Was : Exit For
				}
			}
			MainMSEngine.PageSetVariable("BANISHLIST", string.Join(" ", BanishString.ToArray));
		}

		sndDisplay("[notify> " + Text, fColorEnum.DefaultColor);
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (Color == "error") {
		lock (Lock) {
			ErrorMsg = Text;
			ErrorNum = 2;
		}
		MainMSEngine.PageExecute(800);
		string NameStr = "";
		if (Text.Contains("There are no furres around right now with a name starting with ")) {
			//Banish <name> (Not online)
			//Error:>>  There are no furres around right now with a name starting with (.*?) . 

			//(0:50) When the Bot fails to banish a furre,
			//(0:51) When the bot fails to banish the furre named {...},
			Regex t = new Regex("There are no furres around right now with a name starting with (.*?) .");
			NameStr = t.Match(data).Groups(1).Value;
			MainMSEngine.PageSetVariable("BANISHNAME", NameStr);
			MainMSEngine.PageExecute(50, 51);
			MainMSEngine.PageSetVariable("BANISHLIST", string.Join(" ", BanishString.ToArray));
		} else if (Text == "Sorry, this player has not been banished from your dreams.") {
			//banish-off <name> (not on list)
			//Error:>> Sorry, this player has not been banished from your dreams.

			//(0:55) When the Bot fails to remove a furre from the banish list,
			//(0:56) When the bot fails to remove the furre named {...} from the banish list,
			NameStr = BanishName;
			MainMSEngine.PageSetVariable("BANISHNAME", NameStr);
			MainMSEngine.PageSetVariable("BANISHLIST", string.Join(" ", BanishString.ToArray));
			MainMSEngine.PageExecute(50, 51);
		} else if (Text == "You have not banished anyone.") {
			//banish-off-all (empty List)
			//Error:>> You have not banished anyone. 

			//(0:59) When the bot fails to see the banish list,
			BanishString.Clear();
			MainMSEngine.PageExecute(59);
			MainMSEngine.PageSetVariable(VarPrefix + "BANISHLIST", "");
		} else if (Text == "You do not have any cookies to give away right now!") {
			MainMSEngine.PageExecute(95);
		}

		sndDisplay("Error:>> " + Text);
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (data.StartsWith("Communication")) {
		sndDisplay("Error: Communication Error.  Aborting connection.");
		ProcExit = false;
		DisconnectBot();
		//LogSaveTmr.Enabled = False

	} else if (Channel == "@cookie") {
		// <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Cookie <a href='http://www.furcadia.com/cookies/Cookie%20Economy.html'>bank</a> has currently collected: 0</font>
		// <font color='emit'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> All-time Cookie total: 0</font>
		// <font color='success'><img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> Your cookies are ready.  http://furcadia.com/cookies/ for more info!</font>
		//<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.

		Regex CookieToMe = new Regex(string.Format("{0}", CookieToMeREGEX));
		if (CookieToMe.Match(data).Success) {
			MainMSEngine.PageSetVariable(MS_Name, CookieToMe.Match(data).Groups(2).Value);
			MainMSEngine.PageExecute(42, 43);
		}
		Regex CookieToAnyone = new Regex(string.Format("<name shortname='(.*?)'>(.*?)</name> just gave <name shortname='(.*?)'>(.*?)</name> a (.*?)"));
		if (CookieToAnyone.Match(data).Success) {
			//MainMSEngine.PageSetVariable(VarPrefix & MS_Name, CookieToAnyone.Match(data).Groups(3).Value)
			if (callbk.IsBot(NametoFurre(CookieToAnyone.Match(data).Groups(3).Value, true))) {
				MainMSEngine.PageExecute(42, 43);
			} else {
				MainMSEngine.PageExecute(44);
			}


		}
		Regex CookieFail = new Regex(string.Format("You do not have any (.*?) left!"));
		if (CookieFail.Match(data).Success) {
			MainMSEngine.PageExecute(45);
		}
		Regex EatCookie = new Regex(Regex.Escape("<img src='fsh://system.fsh:90' alt='@cookie' /><channel name='@cookie' /> You eat a cookie.") + "(.*?)");
		if (EatCookie.Match(data).Success) {
			MainMSEngine.PageSetVariable(Main.VarPrefix + "MESSAGE", "You eat a cookie." + EatCookie.Replace(data, ""));
			Player.Message = "You eat a cookie." + EatCookie.Replace(data, "");
			MainMSEngine.PageExecute(49);

		}
		sndDisplay(Text, fColorEnum.DefaultColor);
		if (smProxy.IsClientConnected)
			smProxy.SendClient("(" + data + Constants.vbLf);
		return;
	} else if (data.StartsWith("PS")) {
		Int16 PS_Stat = 0;
		//(PS Ok: get: result: bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340
		MainMSEngine.PageSetVariable(Main.VarPrefix + "MESSAGE", data);
		Player.Message = data;
		Regex psResult = new Regex(string.Format("^PS (\\d+)? ?Ok: get: result: (.*)$"));
		//Regex: ^\(PS Ok: get: result: (.*)$
		System.Text.RegularExpressions.Match psMatch = psResult.Match(string.Format("{0}", data));
		if (psMatch.Success) {
			Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat);
			Regex psResult1 = new Regex("^<empty>$");
			System.Text.RegularExpressions.Match psMatch2 = psResult1.Match(psMatch.Groups(2).Value);
			if (psMatch2.Success & CurrentPS_Stage == PS_BackupStage.GetAlphaNumericList) {
				if (NextLetter != '{') {
					ServerStack.Enqueue("ps get character." + incrementLetter(NextLetter) + "*");
				} else {
					psCheck = ProcessPSData(1, PSinfo, data);
				}

			} else {

				//Add "," to the end of match #1.
				//Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
				string input = psMatch.Groups(2).Value.ToString + ",";
				//Regex: ( ?([^=]+)=('?)(.+?)('?)),
				lock (ChannelLock) {
					if (CurrentPS_Stage != PS_BackupStage.GetAlphaNumericList)
						PSinfo.Clear();
					MatchCollection mc = Regex.Matches(input, "\\s?(.*?)=('?)(\\d+|.*?)(\\2),?");
					int i = 0;
					for (i = 0; i <= mc.Count - 1; i++) {
						System.Text.RegularExpressions.Match m = mc.Item(i);
						if (!PSinfo.ContainsKey(m.Groups(1).Value))
							PSinfo.Add(m.Groups(1).Value.ToString, m.Groups(3).Value);
						//Match(1) : Value(Name)
						//Match 2: Empty if number, ' if string
						//Match(3) : Value()
					}
					//Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat)
					if (CurrentPS_Stage != PS_BackupStage.GetAlphaNumericList) {
						try {
							psCheck = ProcessPSData(PS_Stat, PSinfo, data);
						} catch (Exception ex) {
							ErrorLogging e = new ErrorLogging(ex, this);
						}
					} else if (CurrentPS_Stage == PS_BackupStage.GetAlphaNumericList & NextLetter != '{') {
						System.Text.RegularExpressions.Match m = mc.Item(mc.Count - 1);
						NextLetter = incrementLetter(m.Groups(1).Value.ToString);
						if (NextLetter != '{') {
							ServerStack.Enqueue("ps get character." + NextLetter + "*");
						} else {
							psCheck = ProcessPSData(1, PSinfo, data);
						}
					} else if (CurrentPS_Stage == PS_BackupStage.GetAlphaNumericList & NextLetter == '{') {
						//CurrentPS_Stage = PS_BackupStage.GetList
						try {
							psCheck = ProcessPSData(PS_Stat, PSinfo, data);
						} catch (Exception ex) {
							ErrorLogging e = new ErrorLogging(ex, this);
						}
					}
				}
			}
		}


		psResult = new Regex(string.Format("^PS (\\d+)? ?Ok: get: multi_result (\\d+)/(\\d+): (.+)$"));
		//Regex: ^\(PS Ok: get: result: (.*)$
		psMatch = psResult.Match(string.Format("{0}", data));

		if (psMatch.Success) {
			Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat);
			if (psMatch.Groups(2).Value.ToString == "1" & CurrentPS_Stage == PS_BackupStage.GetList) {
				pslen = 0;
				PSinfo.Clear();
				PS_Page = "";
			} else if (CurrentPS_Stage == PS_BackupStage.GetAlphaNumericList) {
				pslen = 0;
			}

			//Add "," to the end of match #1.
			//Input: "bank=200, clearance=10, member=1, message='test', stafflv=2, sys_lastused_date=1340046340,"
			//Dim input As String = psMatch.Groups(4).Value.ToString
			PS_Page += psMatch.Groups(4).Value.ToString;
			pslen += data.Length + 1;
			//Regex: ( ?([^=]+)=('?)(.+?)('?)),

			if (psMatch.Groups(2).Value == psMatch.Groups(3).Value) {
				//PS_Page += ","

				lock (ChannelLock) {
					MatchCollection mc = Regex.Matches(string.Format(PS_Page), string.Format("\\s?(.*?)=('?)(\\d+|.*?)(\\2),?"), RegexOptions.IgnorePatternWhitespace);
					if (CurrentPS_Stage != PS_BackupStage.GetAlphaNumericList)
						PSinfo.Clear();
					for (int i = 0; i <= mc.Count - 1; i++) {
						System.Text.RegularExpressions.Match m = mc.Item(i);
						if (!PSinfo.ContainsKey(m.Groups(1).Value))
							PSinfo.Add(m.Groups(1).Value, m.Groups(3).Value);
						//Match(1) : Value(Name)
						//Match 2: Empty if number, ' if string
						//Match(3) : Value()
					}
					int test = 1000 * psMatch.Groups(3).Value.ToInteger;
					if (pslen > 1000 * psMatch.Groups(3).Value.ToInteger & CurrentPS_Stage == PS_BackupStage.GetList) {
						CurrentPS_Stage = PS_BackupStage.GetAlphaNumericList;
						System.Text.RegularExpressions.Match m = mc.Item(mc.Count - 1);
						ServerStack.Enqueue("ps get character." + m.Groups(1).Value.Substring(0, 1) + "*");

					} else if (CurrentPS_Stage != PS_BackupStage.GetAlphaNumericList) {
						try {
							psCheck = ProcessPSData(PS_Stat, PSinfo, data);
						} catch (Exception ex) {
							ErrorLogging e = new ErrorLogging(ex, this);
						}
					} else if (CurrentPS_Stage == PS_BackupStage.GetAlphaNumericList & NextLetter != '{') {
						System.Text.RegularExpressions.Match m = mc.Item(mc.Count - 1);
						NextLetter = incrementLetter(m.Groups(1).Value.ToString);
						if (NextLetter != '{') {
							ServerStack.Enqueue("ps get character." + NextLetter + "*");
						} else {
							psCheck = ProcessPSData(1, PSinfo, data);
						}
					} else if (CurrentPS_Stage == PS_BackupStage.GetAlphaNumericList & NextLetter == '{') {
						//CurrentPS_Stage = PS_BackupStage.GetList
						try {
							psCheck = ProcessPSData(PS_Stat, PSinfo, data);
						} catch (Exception ex) {
							ErrorLogging e = new ErrorLogging(ex, this);
						}
					}
				}
			}
			//(PS 5 Error: get: Query error: Field 'Bob' does not exist

		}

		psResult = new Regex("^PS (\\d+)? ?Ok: set: Ok$");
		//^PS (\d+)? ?Ok: set: Ok
		psMatch = psResult.Match(data);
		if (psMatch.Success) {
			PSinfo.Clear();
			Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat);
			try {
				ProcessPSData(PS_Stat, PSinfo, data);
			} catch (Exception ex) {
				ErrorLogging e = new ErrorLogging(ex, this);
			}
		}
		//PS (\d+) Error: Sorry, PhoenixSpeak commands are currently not available in this dream.
		psResult = new Regex("^PS (\\d+)? ?Error: (.*?)");
		psMatch = psResult.Match(data);
		if (psMatch.Success) {
			psResult = new Regex("^PS (\\d+)? ?Error: Sorry, PhoenixSpeak commands are currently not available in this dream.$");
			//Regex: ^\(PS Ok: get: result: (.*)$
			//PS (\d+)? ?Error: get: Query error: (.+) Unexpected character '(.+)' at column (\d+)
			System.Text.RegularExpressions.Match psMatch2 = psResult.Match(data);
			Regex psResult2 = new Regex("^PS (\\d+)? ?Error: set");
			System.Text.RegularExpressions.Match psmatch3 = psResult2.Match(data);
			Regex psResult3 = new Regex("PS (\\d+)? ?Error: set: Query error: Only (\\d+) rows allowed.");
			System.Text.RegularExpressions.Match psmatch4 = psResult3.Match(data);
			if (psMatch2.Success | psmatch3.Success | psmatch4.Success) {
				PS_Abort();
				if (psmatch4.Success) {
					MainEngine.MSpage.Execute(503);
				}
			} else {
				Int16.TryParse(psMatch.Groups(1).Value.ToString, PS_Stat);

				if (CurrentPS_Stage == PS_BackupStage.off) {
					MainMSEngine.PageExecute(80, 81, 82);

				} else if (CurrentPS_Stage == PS_BackupStage.GetList) {
					if (PS_Stat != CharacterList.Count) {
						string str = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*";
						ServerStack.Enqueue(str);
						psSendCounter = Convert.ToInt16(PS_Stat + 1);

						psReceiveCounter = PS_Stat;

					} else if (PS_Stat == CharacterList.Count) {
						CurrentPS_Stage = PS_BackupStage.off;


					}
				} else if (CurrentPS_Stage == PS_BackupStage.GetTargets & psSendCounter == psReceiveCounter + 1) {
					if (PS_Stat != CharacterList.Count) {
						string str = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*";
						ServerStack.Enqueue(str);
						psSendCounter = Convert.ToInt16(PS_Stat + 1);
						psReceiveCounter = PS_Stat;
					} else if (PS_Stat == CharacterList.Count) {
						CurrentPS_Stage = PS_BackupStage.off;
						PSBackupRunning = false;
						CharacterList.Clear();
						psReceiveCounter = 0;
						psSendCounter = 1;
						//(0:501) When the bot completes backing up the characters Phoenix Speak,
						SendClientMessage("SYSTEM:", "Completed Backing up Dream Characters set.");
						MainEngine.MSpage.Execute(501);
					}

				} else if (CurrentPS_Stage == PS_BackupStage.RestoreAllCharacterPS & PS_Stat <= PSS_Stack.Count - 1 & psSendCounter == psReceiveCounter + 1) {
					if (PS_Stat != PSS_Stack.Count - 1) {
						LastSentPS = 0;
						PSS_Struct ss = new PSS_Struct();
						ss = PSS_Stack(PS_Stat);
						ServerStack.Enqueue(ss.Cmd);
						psSendCounter = Convert.ToInt16(PS_Stat + 1);
						psReceiveCounter = PS_Stat;

					} else if (PS_Stat == PSS_Stack.Count - 1) {
						PSRestoreRunning = false;
						SendClientMessage("SYSTEM:", "Completed Character restoration to the dream");
						//(0:501) When the bot completes backing up the characters Phoenix Speak,
						MainEngine.MSpage.Execute(503);
						CurrentPS_Stage = PS_BackupStage.off;
					}
				} else if (CurrentPS_Stage == PS_BackupStage.GetSingle & PS_Stat <= CharacterList.Count & psSendCounter == psReceiveCounter + 1) {
					if (PS_Stat != CharacterList.Count) {
						string str = "ps " + (PS_Stat + 1).ToString + " get character." + CharacterList(PS_Stat).name + ".*";
						ServerStack.Enqueue(str);
						psSendCounter = Convert.ToInt16(PS_Stat + 1);
						psReceiveCounter = PS_Stat;
					} else if (PS_Stat == CharacterList.Count) {
						CurrentPS_Stage = PS_BackupStage.off;
						CharacterList.Clear();
						psReceiveCounter = 0;
						psSendCounter = 1;
						PSBackupRunning = false;
					}
				}
			}
		}

		return;

	} else if (data.StartsWith("(You enter the dream of")) {

		return;


	} else {
//default
		return;
	}
	// If smProxy.IsClientConnected Then smProxy.SendClient("(" + data + vbLf)
	// Exit Sub
}
	}
}
