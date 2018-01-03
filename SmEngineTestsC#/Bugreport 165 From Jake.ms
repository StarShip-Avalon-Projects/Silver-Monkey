*MSPK V04.00 Silver Monkey
*MonkeySpeak Script File
*Created by <name> 


*WHAT WORKS;

*THIS ONE DIDNT
(0:6) When someone says {ping},
(5:0) say {pong}.

*THIS ONE DID
(0:5) When someone says something,
(5:0) say {%NAME said %MESSAGE}.

*THIS ONE DID*
(0:7) When someone says something with {asdf} in it,
(5:0) say {%NAME triggered specific string 'asdf'}.

*THIS ONE DIDNT
(0:9) When someone shouts {ping},
(5:2) shout {pong}.

*THIS ONE DIDNT
(0:6) When someone says {test},
(5:0) say {tested}.

*THIS ONE TRIGGERED*
(0:8) When someone shouts something,
(5:2) shout {%NAME shouted something!}.

*feedback loop, needs (1:6)
(0:10) When someone shouts something with {asdf} in it,
(5:2) shout {%NAME shouted string 'asdf'}.

*This is too specific I guess, since it doesn't process. The raw server data might need some work?* Since 0:13 works.
(0:12) When someone emotes {boops},
(5:1) emote {baps}.

*Doesn't seem to work, (1:6) needs some work.
(0:11) When someone emotes something,
(1:6) and the triggering furre's name is not {El|Jacob}, <---- This line seems to be ignored? Or processed incorrectly?
(5:1) emote {was triggered.}.

*THIS WORKS
(0:13) When someone emotes something with {touches} in it,
(5:1) emote {scrmz}.


*Broke
(0:16) When someone whispers {Hi},
(5:5) whisper {Hello %NAME} to the triggering furre.


(0:1) When the bot logs into furcadia,
(5:6)  whisper {Logged on} to furre named {Gerolkae}.

*(0:1) When the bot logs into furcadia,
*    (5:250) create a table as %mytable
*    (5:251) with table %mytable put 123 in it at key {123}.
*    (6:250) for each entry in table %mytable put it into %entry,
*        (5:102) print {%entry} to the console.
*        (5:102) print {%entry} to the console.
*        (5:102) print {%entry} to the console.
*        (5:102) print {%entry} to the console.
*(0:1) When the bot logs into furcadia,
*        (5:102) print {123} to the console.
*        (5:102) print {123} to the console.
*        (5:102) print {123} to the console.
*        (5:102) print {123} to the console.

(0:90) When the bot enters a Dream,
(5:300) create timer 13 to go off every 900 second(s) with a start delay of # second(s).

(0:97)  When the bot leaves a Dream,
(5:301) stop timer 13.

*(0:0) when the script is started,
*    (5:250) create a table as %myTable.
*    (5:100) set %hello to {hi}
*    (5:101) set %i to 0
*    (5:252) with table %myTable put {%hello} in it at key {myKey1}.
*    (5:252) with table %myTable put {%hello} in it at key {myKey2}.
*    (5:252) with table %myTable put {%hello} in it at key {myKey3}.
*    (5:252) with table %myTable put {%hello} in it at key {myKey4}.
*    (5:252) with table %myTable put {%hello} in it at key {myKey5}.
*    (5:252) with table %myTable put {%hello} in it at key {myKey6}.
*    (5:252) with table %myTable put {%hello} in it at key {myKey7}.
*    (6:250) for each entry in table %myTable put it into %entry,
*        (5:102) print {%entry} to the console.
*        (5:150) take variable %i and add 1 to it.
*        (5:102) print {%i} to the console.
*    (6:454) after the loop is done,
*        (5:102) print {I'm done!} to the console.
*        (1:250) and variable %myTable is a table,
*            (5:101) set %myTable[myKey1] to 123
*            (5:102) print {%myTable[myKey1]} to the console.

*(0:1) When the bot logs into furcadia,
*    (5:250) create a table as %mytable
*    (5:251) with table %mytable put 123 in it at key {123}.
*    (6:250) for each entry in table %mytable put it into %entry,
*        (5:102) print {%entry} to the console.
*        (5:102) print {%entry} to the console.
*        (5:102) print {%entry} to the console.
*        (5:102) print {%entry} to the console.
*    (6:450) while variable %myTable[123] is not 1234,
*        (5:150) take variable %myTable[123] and add 1 to it.
*    (6:454) after the loop is done,
*        (5:102) print {%mytable[123]} to the console.

(0:90)  When the bot enters a Dream,
(5:102) print {Enter Dream %DREAMNAME} to the log.

(0:21)  When someone emits something,
(0:22)  When someone emits {test}
(5:6)  whisper {Seen emit: %MESSAGE} to furre named {%BOTCONTROLLER}.


`(0:90) When the bot enters a dream,
  (1:24) and the Dream Name is not {Furrabian Nights},
  (1:24) and the Dream Name is not {The Shoddy Ribbon},
(5:102) print {Check Dream Object } to the log.

*Endtriggers* 8888 *Endtriggers*
