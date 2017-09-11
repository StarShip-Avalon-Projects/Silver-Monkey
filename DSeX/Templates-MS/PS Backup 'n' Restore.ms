*MonkeySpeak Backup/Restore Template
*Created by Gerolkae

(0:299) When the time is {12:00:00 am} hh:mm:ss am/pm FST,
 (1:520) and the bot is not in the middle of a PS Backup Process,
 (1:522) and the bot is not in the middle of a PS Restore Process,
(0:16) When someone whispers {Backup},
 (1:520) and the bot is not in the middle of a PS Backup Process,
 (1:522) and the bot is not in the middle of a PS Restore Process,
 (1:15) and the triggering furre is the Bot Controller,
  (5:553) Backup All Character phoenixspeak for the dream

(0:16) When someone whispers {Restore},
 (1:15) and the triggering furre is the Bot Controller,
 (1:520) and the bot is not in the middle of a PS Backup Process,
 (1:522) and the bot is not in the middle of a PS Restore Process,
   (5:556) restore all phoenxi speak characters for this dream.

(0:501) When the bot completes backing up the characters Phoenix Speak,
(0:16) When someone whispers {Prune},
 (1:520) and the bot is not in the middle of a PS Backup Process,
 (1:522) and the bot is not in the middle of a PS Restore Process,

 (1:15) and the triggering furre is the Bot Controller,
  (5:557) remove Entries older then 90 days from Phoenix Speak Character backup.

(0:24) When someone enters the dream,
 (1:520) and the bot is not in the middle of a PS Backup Process,
 (1:522) and the bot is not in the middle of a PS Restore Process,
(0:26) When someone leaves the dream,
 (1:520) and the bot is not in the middle of a PS Backup Process,
 (1:522) and the bot is not in the middle of a PS Restore Process,
(5:554) backup Character named {%NAME} phoenix speak 


(0:0) When the bot starts,
(5:500) use SQLite database file {DataBase.db} or create file if it does not exist with table "Furre".
* this defaults to "My Documents/Silver Monkey/Database.db"




