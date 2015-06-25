*MSPK V03.00 Silver Monkey
* Demo Help Bot System
*Created by Gerolkae

* Define start up variables
(0:0) When the bot starts,
        (5:100) set variable %ListName  to {NoFurre}.
        (5:500) use SQLite database file {HelpDesk.db} or create file if it does not exist with table "Furre".

* Capture Dream Emit for help
(0:23) When someone emits something with {#HELP} in it,
        (5:551) execute SQLite Database query {CREATE TABLE IF NOT EXISTS HELPQUE (ID INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,Name TEXT UNIQUE,Message TEXT, STATUS TEXT, Time TEXT);}

(0:23) When someone emits something with {#HELP} in it,
        (5:101) set variable %HelpMessage to %MESSAGE
        (5:120) use variable %HelpMessage and take word 2 and put it into variable %Furre 
        (5:126) count the number of characters in string variable %Furre and put them into variable %NameCount .
        (5:150) take variable %NameCount and add 7  to it.
        (5:122) chop off the beginning of variable %HelpMessage, removing the first %NameCount  characters of it.
        (5:127) take variable %Furre and Convert it to Furcadia short name. (with out special Characters or spaces or pipe "|")
        (5:550) take variable %Furre  , prepare it for a SQLite Database query, and put it in variable %Furre .
        (5:550) take variable %HelpMessage , prepare it for a SQLite Database query, and put it in variable %HelpMessage .
        (5:30) set variable %time to the current local time.
        (5:551) execute SQLite Database query { INSERT OR IGNORE INTO HELPQUE (Name,Message,STATUS, Time) VALUES('%Furre', '%HelpMessage', 'NEW','%time' );}

*Check if the HelpQue exists
(0:16) When someone whispers {Que},
    (1:502) and the Database info {Access Level} about the triggering furre is greater than 5
        (5:551) execute SQLite Database query { SELECT * FROM sqlite_master WHERE name ='HELPQUE' and type='table' ;}
        (5:552) retrieve field {Name} from SQLite Database query and put it into variable %TableName .
        (5:101) set variable %TableExist to 0
(0:16) When someone whispers {Que},
    (1:502) and the Database info {Access Level} about the triggering furre is greater than 5
    (1:104) and variable %TableName  equals {HELPQUE},
        (5:5) whisper {Listing HelpQue} to the triggering furre.
        (5:522) get the total number of records from table {HELPQUE} and put it into variable %QueMax .
        (5:300) create timer 1 to go off every 0.3 second(s).
        (5:101) set variable %ListName to %NAME .
        (5:101) set variable %Counter to 0 
        (5:101) set variable %TableExist to 1

(0:16) When someone whispers {Que},
    (1:502) and the Database info {Access Level} about the triggering furre is greater than 5
    (1:102) and variable %TableExist equals 0,
        (5:5) whisper {The HelpQue is empty} to the triggering furre.

(0:300) when timer 1 goes off,
        (5:150) take variable %Counter and add 1  to it.
 (0:300) when timer 1 goes off,
    (1:105) and variable %ListName  does not equal {NoFurre},
       (5:551) execute SQLite Database query { SELECT * FROM HELPQUE WHERE ID=%Counter ;}
        (5:552) retrieve field {Name} from SQLite Database query and put it into variable %QName .
        (5:552) retrieve field {Message} from SQLite Database query and put it into variable %QMsg .
        (5:552) retrieve field {STATUS} from SQLite Database query and put it into variable %QStat .
        (5:552) retrieve field {Time} from SQLite Database query and put it into variable %QTime .
        (5:30) set variable %time to the current local time.
        (5:35) use variable %QTime as a DateTime string and subtract Date Time variable %time and put it into variable %WaitTime
        (5:6) whisper {ID %Counter Name %QName Status %QStat Waiting: %WaitTime Message %QMsg  } to {%ListName}.
(0:300) when timer 1 goes off,
    (1:102) and variable %Counter equals %QueMax
        (5:301) stop timer 1
        (5:100) set variable %ListName  to {NoFurre}.

(0:16) When someone whispers {ResetQue},
    (1:502) and the Database info {Access Level} about the triggering furre is greater than 5
        (5:551) execute SQLite Database query { DROP TABLE IF EXISTS HELPQUE ;}
        (5:5) whisper {HelpQue cleared} to the triggering furre.


(0:17) When someone whispers something with {Take} in it,
    (1:502) and the Database info {Access Level} about the triggering furre is greater than 5
        (5:101) set variable %tMsg to %MESSAGE .
        (5:120) use variable %tMsg and take word 1 and put it into variable %tID
        (5:551) execute SQLite Database query { SELECT * FROM HELPQUE WHERE ID=%tID  ;}
        (5:552) retrieve field {Name} from SQLite Database query and put it into variable %tName .

(0:17) When someone whispers something with {Take} in it,
    (1:502) and the Database info {Access Level} about the triggering furre is greater than 5
        (5:551) execute SQLite Database query { UPDATE  HELPQUE SET STATUS='COMPLETE'  WHERE ID=%tID ;}.
        (5:6) whisper {%NAME is now your helper. Feel free to whisper him or her} to {%tName}.
        (5:0) say {!chat %NAME is now the helper for %tName }

(0:17) When someone whispers something with {Promote} in it,
(1:15) and the triggering furre is the Bot Controller,
        (5:120) use variable %MESSAGE and take word 2 and put it into variable %pLvl 
        (5:120) use variable %MESSAGE and take word 1 and put it into variable %pName
        (5:506) Add furre named {%pName} with the default access level "0" to the Furre Table in the database if he, she or it don't exist.
        (5:508) update Database info {Access Level} about the furre named {%pName} will now be %pLvl 
        (5:127) take variable %pName and Convert it to Furcadia short name. (with out special Characters or spaces or pipe "|")
        (5:0) say {ps 1 set character.%pName.staff=%pLvl}
        (5:6) whisper {Staff level %pLvl granted} to {%pName}.
        (5:5) whisper {Staff Access Granted} to the triggering furre.


*Endtriggers* 8888 *Endtriggers*

