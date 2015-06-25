* Create to variables, when the bots start. These are blank containers.
(0:0) When the bot starts,
	(5:30) set variable %Time to the current local time.
	(5:101) set variable %SecondTimer to 0

* %Time is a string. When the bot enters a dream, it will emitloud when it was started.
(0:90) When the bot enters a dream,
	(5:4) Emitloud {I started at, %Time}.

* When someone talks to the bot, it resets the counter.
(0:5) When someone says something,
* You must always stop timers when they are no longer needed.
	(5:301) stop timer 1
	(5:3) Emit {%SecondsPast seconds have passed since someone last talked to me!}.
	(5:101) set variable %SecondsPast to 0* And start it again
	(5:300) create timer SecondTimer to go off every 1 second(s).

* When ever the timer goes off (Once a second) it will increment the value, %SecondsPast
* %SecondsPast is an integer (whole number)
(0:300) When timer 1 goes off,
	(5:150) take variable %SecondsPast and add 1 to it.

* UNLIKE DragonSpeak, MonkeySpeak’s variables are case sensitive (%a is not the same as %A).
* MonkeySpeak stores its numerical variables as double precision and can thus do real numbers (like 4.88993).