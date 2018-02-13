*MSPK V07.00 Silver Monkey
*MonkeySpeak Script File
*Created by <name> 

(0:11) When someone says {ping},
(5:10) say {pong}.

says
(0:10) When someone says something,
(5:10)  say {%NAME said %MESSAGE}.

*THIS ONE DID*
(0:12) When someone says something with {asdf} in it,
(5:10)  say {%NAME triggered specific string 'asdf'}.

*THIS ONE DIDNT
(0:9) When someone shouts {ping},
(5:12) shout {pong}.

*THIS ONE DIDNT
(0:11) When someone says {test},
(5:10)  say {tested}.

*THIS ONE TRIGGERED*
(0:8) When someone shouts something,
(5:12) shout {%NAME shouted something!}.

*feedback loop, needs (1:6)
(0:10) When someone shouts something with {asdf} in it,
(5:12) shout {%NAME shouted string 'asdf'}.

*This is too specific I guess, since it doesn't process. The raw server data might need some work?* Since 0:13 works.
(0:17) When someone emotes {boops},
(5:11) emote {baps}.
anyone emote
*Doesn't seem to work, (1:6) needs some work.
(0:16) When anyone emotes something,
(1:11) and the triggering furre's name is not {El|Jacob}, <---- This line seems to be ignored? Or processed incorrectly?
(5:11) emote {was triggered.}.

*THIS WORKS
(0:18) When someone emotes something with {touches} in it,
(5:11) emote {scrmz}.
whis

*Broke
(0:20) When someone whispers {Hi},
(5:5) whisper {Hello %NAME} to the triggering furre.

(0:1) When the bot logs into Furcadia,
(5:16)  whisper {Logged on} to furre named {Gerolkae}.


(0:34) When the bot enters a Dream,
(5:300) create timer 13 to go off every 900 second(s) with a start delay of # second(s).

(0:36) When the bot leaves a Dream,
(5:301) stop timer 13.

(0:34) When the bot enters a Dream,
(5:102) print {Enter Dream %DREAMNAME} to the log.

(0:26) When someone emits {test},
(5:6)  whisper {Seen emit: %MESSAGE} to furre named {%BOTCONTROLLER}.


(0:34) When the bot enters a Dream,
  (1:35) and the Dream Name is not {Furrabian Nights},
  (1:35) and the Dream Name is not {The Shoddy Ribbon},
(5:102) print {Check Dream Object } to the log.

*Endtriggers* 8888 *Endtriggers*
