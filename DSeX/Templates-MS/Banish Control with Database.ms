*MonkeySpeak Script Template
*Created by Gerolkae 

**SECTION**  Member List
*Membership

*Furre Table Default Columns
* ID, Name, Access Level,date added, date modified
* Other Collumns can be added is nessary

(0:17) When someone whispers something with {AddMember } in it,
  (1:15) and the triggering furre is the Bot Controller,
    (5:76) set Variable %NewMember to the Message the bot last received.
    (5:122) chop off the beginning of variable %NewMember, removing the first 10 characters of it.
    (5:506) add furre named {%NewMember} with the default access level "1" to the Furre Table in the database if he, she, or it doesn't exist.
    (5:5) whisper {%NewMember Added to Members List} to the triggering furre.


(0:17) When someone whispers something with {RemoveMember } in it,
  (1:15) and the triggering furre is the Bot Controller,
    (5:76) set Variable %NewMember to the Message the bot last received.
    (5:122) chop off the beginning of variable %NewMember, removing the first 13 characters of it.
    (5:519) delete all Database info about the furre named {%NewMember}.
    (5:5) whisper {%NewMember removed from Members List} to the triggering furre.


*share Control
(0:24) When someone enters the dream,
  (1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to 1,
    (5:0) say {share %NAME}.

(0:17) When someone whispers something with {UnshareMember } in it,
  (1:15) and the triggering furre is the Bot Controller,
    (5:76) set Variable %NewMember to the Message the bot last received.
    (5:122) chop off the beginning of variable %NewMember, removing the first 14 characters of it.
    (5:0) say {unshare %NewMember}.
    (5:5) whisper {%NewMember no longer has share} to the triggering furre.

(0:17) When someone whispers something with {shareMember } in it,
  (1:15) and the triggering furre is the Bot Controller,
    (5:76) set Variable %NewMember to the Message the bot last received.
    (5:122) chop off the beginning of variable %NewMember, removing the first 12 characters of it.
    (5:0) say {share %NewMember}.
    (5:5) whisper {%NewMember now has share} to the triggering furre.


**SECTION**  Banish Control

* retrieve the banish list from the server and keep it updated for Temp Banishes
(0:91) When the bot enters a dream named {dream:name/},
    (5:50) ask the server for the banish-list.

* The default file is created automatically for this it is 
* "My Document/Silver Monkey/SilverMonkey.db"
*(5:500) use SQLite database file {database.db} or create file if it does not exist.


* List the banish list to the whispering furre
(0:16) When someone whispers {banish-list},
  (1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to 1,
    (5:5) whisper {Players banished: %BANISHLIST} to the triggering furre.

* Banish the furre Named {...}
(0:17) When someone whispers something with {banish } in it,
  (1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to 1,
    (5:76) set Variable %banned to the Message the bot last received.
    (5:0) say {%banned}.

* Unbanish a Furre Named {...}
(0:17) When someone whispers something with {banish-off } in it,
  (1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to 1,
    (5:76) set Variable %unbanned to the Message the bot last received.
    (5:0) say {%unbanned}.

*temporarily banish a furre named {...}
(0:17) When someone whispers something with {tempbanish } in it,
  (1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to 1,
    (5:76) set Variable %banned to the Message the bot last received.
    (5:0) say {%banned}.

**SECTION**  ListMembers

(0:16) When someone whispers {Listmembers},
  (1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to 1,
    (5:522) get the total number of records from table {FURRE} and put it into variable %FurreCount.
    (5:101) set variable %ListName to %NAME .
    (5:300) create timer 1 to go off every 0.300 second(s).
    (5:101) set variable %ListIDX to 0 .
    (5:5) whisper {Listing Members} to the triggering furre.

(0:300) When timer 1 goes off,
  (1:152) and variable %ListIDX is less than %FurreCount,
    (5:100) set variable %ListName2 to {NoFurre}.
    (5:551) execute SQLite Database query {SELECT * FROM FURRE WHERE [Access Level] != 0 LIMIT 1 OFFSET %ListIDX}.
    (5:552) retrieve field {Name} from SQLite Database query and put it into variable %ListName2.

(0:300) When timer 1 goes off,
  (1:105) and variable %ListName2 does not equal {NoFurre},
    (5:6) whisper {%ListName2} to {%ListName}.

(0:300) When timer 1 goes off,
    (5:150) take variable %ListIDX and add 1 to it.

(0:300) When timer 1 goes off,
  (1:102) and variable %ListIDX equals %FurreCount,
    (5:301) stop timer 1.
