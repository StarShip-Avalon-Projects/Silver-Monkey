* When the bot starts, create an empty variable for boolean checking, if the bot should perform actions, or not.
(0:0) When the bot starts,
* This will be used to stop and start the bot if its throat gets tired.
(5:101) set variable %Act to 1

* When a furre enters the dream,
(0:24) When someone enters the dream,
* Check if bot should perform the actions
 (1:102) and variable %Act equals 1
* Read their description into string variable "%Description"
(5:600) set variable %Description to the Triggering furre's description.
* Emitloud their name and description to the dream 
* %NAME is a special variable that gets filled with the triggering furre's name, by SM.
(5:4) Emitloud {%NAME has entered! (%Description)}.

* If the bot's throat gets tired, turn %Act to 'false' and start a wait timer
(0:92) When the bot detects the (this should be something)
(5:101) set variable %Act to 0
(5:300) create timer 1 to go off every 90 second(s).

* When that timer goes off, stop it and reset the %Act variable to true and the bot continues.
(0:300) When timer 1 goes off,
(5:301) stop timer 1
(5:101) set variable %Act to 1

* This does not stop the bot, only tells it not to do anything, if its throat is tired and waits a period to resolve that issue.
* You could still have it perform tasks that do not send data to the server, while it waits for the flood protection to reset.
