*MonkeySpeak Script Template
*Created by Gerolkae


* Dream member List is assumed to be "My Document\Silver Monkey\Memberlist.txt"
* even if it don't exist. The File is automatically created when these line are used,
* unless Specified by "(5:904) Use file {...} as the dream member list." 
* Using this Line will Use the File Name Specified and create a blank file. The Default
* Location for this file is the same location as MemberList.txt. The BotControler is
* always Assumed to be part of the Dream Member List. This Setting is located in the
* Bot Setup Window

*add Member
(0:17) When someone whispers something with {Add} in it,
  (1:900) and the triggering furre is on my dream Member List,
    (5:101) set variable %mName to %MESSAGE .
    (5:120) use variable %mName and take word 1 and put it into variable %User
    (5:901) add the furre named {%User} to my Dream Member list if they aren't already on it.

*remove member
(0:17) When someone whispers something with {Remove} in it,
  (1:900) and the triggering furre is on my dream Member List,
    (5:101) set variable %mName to %MESSAGE .
    (5:120) use variable %mName and take word 1 and put it into variable %User
    (5:903) remove the furre named {%User} from my Dream Member list if they are on it.

*Command List

(0:16) When someone whispers {commands},
  (1:900) and the triggering furre is on my dream Member List,
    (5:5) whisper {Command Listing} to the triggering furre.

(0:16) When someone whispers {commands},
  (1:900) and the triggering furre is on my dream Member List,
    (5:5) whisper {     Add [Furre]} to the triggering furre.

(0:16) When someone whispers {commands},
  (1:900) and the triggering furre is on my dream Member List,
    (5:5) whisper {     Remove [Furre]} to the triggering furre.
