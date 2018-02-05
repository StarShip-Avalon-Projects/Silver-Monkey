*MSPK V07.00 Silver Monkey
*MonkeySpeak Script File
*Created by Gerolkae   

(0:34) When the bot enters a Dream,
		(5:102) print {Beginning Turret Tests} to the log.
		(5:102) print {Arrived in Dream %DREAMNAME} to the log.

(0:34) When the bot enters a Dream,
    (1:35) and the Dream Name is not {The Shoddy Ribbon: Murder Mystery Hotel(WIP)},
    (1:35) and the Dream Name is not {Furrabian Nights},
	   (5:16) whisper {DREAMNAME is Not %DREAMNAME} to furre named {%BOTCONTROLLER}.
         (5:16) whisper {Downloading Dream furc://theshoddyribbon} to furre named {%BOTCONTROLLER}.
	  (5:10) say {`fdl furc://theshoddyribbon}.
	(1:34) and the Dream Name is {The Shoddy Ribbon: Murder Mystery Hotel(WIP)},
		(5:16) whisper {DREAMNAME is %DREAMNAME} to furre named {%BOTCONTROLLER}.
	(1:34) and the Dream Name is {Furrabian Nights},
		(5:16) whisper {DREAMNAME is %DREAMNAME} to furre named {%BOTCONTROLLER}.

*Endtriggers* 8888 *Endtriggers*