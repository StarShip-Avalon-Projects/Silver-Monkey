*MonkeySpeak Script Template
*Created by Gerolkae 

**SECTION**  Member List
*Membership

* The default file is created automatically for this it is 
* "My Document/Silvermonkey/MembersList.txt"

(0:17) When someone whispers something with {AddMember } in it,
(1:15) and the triggering furre is the Bot Controller,
(5:76) set Variable %NewMember to the Message the bot last received.
(5:122) chop off the beginning of variable %NewMember, removing the first 10 characters of it.
(5:901) add the furre named {%NewMember} to my Dream Member list if they aren't already on it.
(5:5) whisper {%NewMember Added to Members List} to the triggering furre.


(0:17) When someone whispers something with {RemoveMember } in it,
(1:15) and the triggering furre is the Bot Controller,
(5:76) set Variable %NewMember to the Message the bot last received.
(5:122) chop off the beginning of variable %NewMember, removing the first 13 characters of it.
(5:903) remove the furre named {%NewMember} from my Dream Member list if they are on it.
(5:5) whisper {%NewMember removed from Members List} to the triggering furre.

(0:16) When someone whispers {MemberList},
(1:900) and the triggering furre is on my dream Member List,
(5:905) store member list to variable %MemberList.
(5:5) whisper {Dream Members: %MemberList} to the triggering furre.
**SECTION**  Banish Control

* retrieve the banish list from the server and keep it updated for Temp Banishes
(0:91) When the bot enters a dream named {dream:name},
(0:299) When the time is {12:00:00 am} hh:mm:ss am/pm FST,
  (5:50) ask the server for the banish-list.

* List the banish list to the whispering furre
(0:16) When someone whispers {banish-list},
(1:900) and the triggering furre is on my dream Member List,
  (5:5) whisper {Players banished: %BANISHLIST} to the triggering furre.

* Banish the furre Named {...}
(0:17) When someone whispers something with {banish } in it,
(1:900) and the triggering furre is on my dream Member List,
  (5:76) set Variable %banned to the Message the bot last received.
  (5:0) say {%banned}.

* Unbanish a Furre Named {...}
(0:17) When someone whispers something with {banish-off } in it,
(1:900) and the triggering furre is on my dream Member List,
  (5:76) set Variable %unbanned to the Message the bot last received.
  (5:0) say {%unbanned}.

*temporarily banish a furre named {...}
(0:17) When someone whispers something with {tempbanish } in it,
(1:900) and the triggering furre is on my dream Member List,
  (5:76) set Variable %banned to the Message the bot last received.
  (5:0) say {%banned}.











