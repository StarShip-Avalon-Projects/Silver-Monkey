*Enhanced Backup Script for Dreams with Standard Dreams and Limited Phoenix Speak Capacity
* Created by Gerolkae


**SECTION**  Startup
* Define start up variables
(0:0) When the bot starts,
* Define this File other wise it defaults to "My Documents\Silver Monkey\database.db"
*  (5:500) use SQLite database file {...} or create file if it does not exist.

* Lets remember the last Backup we did. If this is the First time we Start as 1
  (5:513) add column {PSBackup} with type {double} to the Furre table.
  (5:551) execute SQLite Database query {CREATE TABLE IF NOT EXISTS DREAM ([Key] TEXT UNIQUE, [Value] Text);}
  (5:551) execute SQLite Database query {INSERT OR IGNORE INTO DREAM([Key], [Value]) VALUES ('PSStage','1');}
  (5:551) execute SQLite Database query {SELECT Value FROM DREAM WHERE Key='PSStage'}.
  (5:552) retrieve field {Value} from SQLite Database query and put it into variable %backupStage.

**SECTION**  PS Backup

*MonkeySpeak Backup/Restore Template
*Created by Gerolkae

(0:299) When the time is {12:00:00 am} hh:mm:ss am/pm FST,
(1:520) and the bot is not in the middle of a PS Backup Process,
   (1:522) and the bot is not in the middle of a PS Restore Process,
(0:16) When someone whispers {Backup},
 (1:15) and the triggering furre is the Bot Controller,
(5:0) say {!prune}.

* Use DS Method to prune the Database This Way Old Records 
* EX:

*[DS]
*(0:31) When a furre says {!prune},
*(1:10) and the triggering furre is the dream owner,
*(5:612) remember the PhoenixSpeak info {PSDays} about this dream, and put it in variable %PSDays.
*    (5:880) forget all the PhoenixSpeak info about furres that haven't been used in %PSDays days 0 hours 0 minutes 0 seconds
*
** Set PSDays to the Number of Days  you want to Keep records in the Live Dream
** Anything Older will be restored when the Furre returns to the dream
*[/DS]


  (5:553) Backup All Character phoenixspeak for the dream
(5:150) take variable %backupStage and add 1 to it.
  (5:551) execute SQLite Database query {UPDATE DREAM Set Value='%backupStage' where Key='PSStage';}

(0:17) When someone whispers something with {restore} in it,
(5:101) set variable %res to 0.
(0:16) When someone whispers {Restore},
(1:520) and the bot is not in the middle of a PS Backup Process,
   (1:522) and the bot is not in the middle of a PS Restore Process,
 (1:15) and the triggering furre is the Bot Controller,
(5:556) restore all Phoenix Speak records for this dream.
(5:101) set variable %res to 1.

(0:17) When someone whispers something with {restore} in it,
(1:103) and variable %res does not equal 1,
(1:500) and the Database info {staff} about the triggering furre is equal to 1,
  (5:100) set variable %Var to {%MESSAGE}.
(5:122) chop off the beginning of variable %Var, removing the first 8 characters of it.
(5:555) restore Phoenix Speak for character {%Var}. (use "[DREAM]" to restore information specific to the dream)

(0:501) When the bot completes backing up the characters Phoenix Speak,
(0:16) When someone whispers {Prune},
 (1:15) and the triggering furre is the Bot Controller,
  (5:557) remove Entries older then 90 days from Phoenix Speak Character backup.


(0:24) When someone enters the dream,
(1:19) and the bot is the dream owner,
(1:520) and the bot is not in the middle of a PS Backup Process,
   (1:522) and the bot is not in the middle of a PS Restore Process,
(1:501) and the Database info {PSBackup} about the triggering furre is not equal to %backupStage,
(5:5) whisper {Restoring your PS Info to the dream.. Please Rejoin the dream for information to take effect} to the triggering furre.
(5:555) restore Phoenix Speak for character {%NAME}. (use "[DREAM]" to restore information specific to the dream)
(5:507) update Database info {PSBackup} about the triggering furre will now be %backupStage.

(0:504) when the bot backsup phoenxix speak for any Furre.
(5:505) add the triggering furre with the default access level "0" to the Furre Table in the database if he, she, or it doesn't exist.
(5:507) update Database info {PSBackup} about the triggering furre will now be %backupStage.



