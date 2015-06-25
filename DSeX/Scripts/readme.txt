Your probably wondering what the heck this folder is for and whether or not you should delete it..
(hopefully thats not the case) DO NOT DELETE THIS FOLDER.  Wizzerd depends on the contents in this 
folder to complete its main function,  

Generate Code based on user inputted values.

First off I would like to let everyone know that even though this program has the familiarty of 
Draconic Magician it was written entirely differently.  It is not a replacement for Draconic Magician.  I only used Draconic Magician as a base in what to go off of because Lothus Marque/Syphor Knight had a great idea but his program hasn't been updated in over a year.  Credits go to Lothus Marque/Syphor Knight for the base look and operation of this application.  Thank him.  

(This is exempt because it is now Open Source)
It is only 1.0.0.0  Much to be done and many more features to be added.
(End exemption)

Wizzerd provides the user with Draconic Magician familiarity while introducing the ability to 
Share and Recieve ini files.  (Exempt)As the popularity grows I'll probably work on more user 
friendly tools for making life easier when making the code.ini files.  A wizard most likely.
(/Exempt)

(Exempt)TO GET MORE SCRIPTS SIMPLY GO TO Tools>Get Base Scripts
Only did this because some users won't want to be forced to download scripts they won't use.(/Exempt)

The structure of a script is as follows:
[Main]
Name=
Author=
Description=
V#=


[Code]
<script goes here>

(Familiar?)

Description is the (you guessed it!) description. (The description may be as big as you desire.
 FOUND OUT: this isn't the case with the IniFile.dll I was using, it only accepts up to 255 chars 
 and won't display more :(  Yes with .Net Microsoft forget to put in Ini functionality cause their 
 "vision" includes only xml)  Although if you share the ds file don't expect everyone to read it.
V is the value as displayed in the list. (v1, V2, v3 and so on) Case insensitive.

Under [Code] section. You put your DS, editing the values you wish to have change. They must take
 the form of ^#^ where # is the same as the V entry you want to be used in this place. You can put 
 anything between ^#^ any symbol, number, word, etc.
(Exempt)(Might work on making a check for whether or not the value should be a variable,string or 
object #)(/Exempt)

Example: (0:3) When somebody moves into object type ^1^,


All Scripts must be saved as INI files and stored in the "Scripts" subfolder.

(Pointless stuff)
Version History:
>V1.0.0.0-
>Initial Release
(End Pointless Stuff, It actually got taken up to v1.2 with the fade options and updating capability)
