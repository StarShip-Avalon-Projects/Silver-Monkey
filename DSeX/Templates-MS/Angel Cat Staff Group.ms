


***  Make sure Botcontroller has Rah Power
(0:15) When someone whispers something,
(1:15) and the triggering furre is the Bot Controller,

*** Pull Rah level from StaffLevels Table
(5:551) execute SQLite Database query {SELECT * FROM StaffLevels WHERE Name = 'Rah' LIMIT 1, 0;}.
 (5:552) retrieve field {Level} from SQLite Database query and put it into variable %RahLevel.
(5:507) update Database info {Access Level} about the triggering furre will now be %RahLevel.





**SECTION**  Configuration

(0:1) When the bot logs into furcadia,
*** Staff Levels Translates Level Names to Numbers
*** This table can be adjusted to suuit the dream needs
(5:551) execute SQLite Database query {CREATE TABLE IF NOT EXISTS StaffLevels (id INTEGER PRIMARY KEY, Name TEXT,Level INTEGER);}. 
(5:551) execute SQLite Database query {INSERT OR IGNORE INTO StaffLevels (id, Name, Value)] VALUES (1, 'Rah', 250);}. 
(5:551) execute SQLite Database query {INSERT OR IGNORE INTO StaffLevels (id, Name, Value)] VALUES (2, 'Tanest', 200);}. 
(5:551) execute SQLite Database query {INSERT OR IGNORE INTO StaffLevels (id, Name, Value)] VALUES (3, 'SrMember', 150);}. 
(5:551) execute SQLite Database query {INSERT OR IGNORE INTO StaffLevels (id, Name, Value)] VALUES (4, 'JrMember', 100);}. 
(5:551) execute SQLite Database query {INSERT OR IGNORE INTO StaffLevels (id, Name, Value)] VALUES (5, 'Recruit', 50);}. 


**SECTION**  Bot Controller Commands


*** Make-[StaffGroup] [Furre Name]
(0:17) When someone whispers something with {Make-} in it,
(1:15) and the triggering furre is the Bot Controller,

***Capture our varables
(5:100) set variable %Command to {%MESSAGE}.
(5:100) set variable %Furre to {%MESSAGE}.

*** Set Command Variable  to the command said
(5:120) use variable %Command and take word position 1 and put it into variable %Command.
*** Grab the number of characters in Command for Stripping %Command from %Furre
(5:126) count the number of characters in string variable %Command and put them into variable %Length.
(5:150) take variable %Length and add 1 to it.
*** Filter the Access Level requested
    (5:122) chop off the beginning of variable %Command, removing the first 5 characters of it.

*** Security Sake. Prevent SQL Injection
(5:550) take variable %Command , prepare it for a SQLite Database query, and put it in variable %Command.
***Look up Access Level from table
(5:101) set variable %DataQuery to 0.
*** If no lines are returned Processing Ceses
(5:551) execute SQLite Database query {SELECT * FROM StaffLevels WHERE Name = '%Command' LIMIT 1, 0;}.
(5:101) set variable %DataQuery to 1.
 (5:552) retrieve field {Level} from SQLite Database query and put it into variable %CommandLevel.
*** Clean up %Furre
    (5:122) chop off the beginning of variable %Furre, removing the first %Length characters of it.
(5:100) set variable %FurreShort to {%Furre}.
*** Shortnames are SQL Injection safe
(5:127) take variable %FurreShort and convert it to Furcadia short name. (without special characters or spaces or pipe "|").

*** Set %Furre [Access Level]
(5:506) add furre named {%Furre} with the default access level "%CommandLevel" to the Furre Table in the database if he, she, or it doesn't exist.
(5:508) update Database info {Access Level} about the furre named {%FurreShort} will now be %CommandLevel.
(5:5) whisper {Sucessfully updated Database info for %Furre} to the triggering furre.

*** Tell the Triggering Furre there was an Error processing the command
(0:17) When someone whispers something with {Make-} in it,
(1:15) and the triggering furre is the Bot Controller,
(1:102) and variable %DataQuery equals 0,
(5:5) whisper {Error Processing Command %MESSAGE} to the triggering furre.


(0:16) When someone whispers {Commands},
(1:15) and the triggering furre is the Bot Controller,
    (5:5) whisper {RemoveMember [Furre Name]} to the triggering furre.

(0:17) When someone whispers something with {RemoveMember } in it,
  (1:15) and the triggering furre is the Bot Controller,
    (5:76) set Variable %NewMember to the Message the bot last received.
    (5:122) chop off the beginning of variable %NewMember, removing the first 13 characters of it.
    (5:519) delete all Database info about the furre named {%NewMember}.
    (5:5) whisper {%NewMember removed from Members List} to the triggering furre.

(0:16) When someone whispers {Commands},
(1:15) and the triggering furre is the Bot Controller,
    (5:522) get the total number of records from table {StaffLevels} and put it into variable %CmdCount.
    (5:101) set variable %CmdName to %NAME .
    (5:300) create timer 1 to go off every 0.300 second(s).
    (5:101) set variable %CmdIDX to 0 .
    (5:5) whisper {Listing Make- Commands} to the triggering furre.

(0:300) When timer 5 goes off,
  (1:152) and variable %CmdIDX is less than %CmdCount,
    (5:100) set variable %ListName2 to {NoFurre}.
    (5:551) execute SQLite Database query {SELECT * FROM StaffLevels LIMIT 1 OFFSET %CmdIDX}.
    (5:552) retrieve field {Name} from SQLite Database query and put it into variable %ListName2.

(0:300) When timer 5 goes off,
  (1:105) and variable %ListName2 does not equal {NoFurre},
    (5:6) whisper {Make-%ListName2 [FurreName]} to {%CmdName}.

(0:300) When timer 5 goes off,
    (5:150) take variable %CmdIDX and add 1 to it.

(0:300) When timer 5 goes off,
  (1:102) and variable %CmdIDX equals %CmdCount,
    (5:301) stop timer 5.




**SECTION**  Staff Commands


(0:17) When someone whispers something with {...} in it,

*** Change Name to the Level you want the command to be.
(5:551) execute SQLite Database query {SELECT * FROM StaffLevels WHERE Name = 'Rah' LIMIT 1, 0;}.
 (5:552) retrieve field {Level} from SQLite Database query and put it into variable %CommandLevel.

(0:17) When someone whispers something with {...} in it,
(1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to %CommandLevel,


(0:16) When someone whispers {Commands},
  (1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to 50,
    (5:5) whisper {ListMembers} to the triggering furre.
(0:16) When someone whispers {Listmembers},
  (1:504) and the Database info {Access Level} about the triggering furre is greater than or equal to 50,
    (5:522) get the total number of records from table {FURRE} and put it into variable %FurreCount.
    (5:101) set variable %ListName to %NAME .
    (5:300) create timer 1 to go off every 0.300 second(s).
    (5:101) set variable %ListIDX to 0 .
    (5:5) whisper {Listing Members} to the triggering furre.
    (5:5) whisper {Name [Staff Level]} to the triggering furre.

(0:300) When timer 1 goes off,
  (1:152) and variable %ListIDX is less than %FurreCount,
    (5:100) set variable %ListName2 to {NoFurre}.
    (5:551) execute SQLite Database query {SELECT * FROM FURRE WHERE [Access Level] >0 LIMIT 1 OFFSET %ListIDX}.
    (5:552) retrieve field {Name} from SQLite Database query and put it into variable %ListName2.
    (5:552) retrieve field {Level} from SQLite Database query and put it into variable %Level.
    (5:551) execute SQLite Database query {SELECT * FROM StaffLevels WHERE Level = %Level LIMIT 1, 0;}.
    (5:552) retrieve field {Level} from SQLite Database query and put it into variable %CommandLevel.

(0:300) When timer 1 goes off,
  (1:105) and variable %ListName2 does not equal {NoFurre},
    (5:6) whisper {%ListName2 [%CommandLevel]} to {%ListName}.

(0:300) When timer 1 goes off,
    (5:150) take variable %ListIDX and add 1 to it.

(0:300) When timer 1 goes off,
  (1:102) and variable %ListIDX equals %FurreCount,
    (5:301) stop timer 1.


