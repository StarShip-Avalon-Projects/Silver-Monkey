*MSPK V07.00 Silver Monkey
*MonkeySpeak Script File
*Created by Gerolkae 

*Set the Furre Table Fields
(0:0) When the Monkey Speak Engine Starts,
*		(5:312) set the time zone to {...}
	(5:300) create timer 1 to go off every 1 second(s) with a start delay of 0 second(s).
    (5:500)  use SQLite database file {EltaDatabase.db} or create file if it does not exist.
	(5:513) add column {ArrivalTime} with type {TEXT} to the Furre table.
    (5:513)  add column {ArrivalDate} with type {TEXT} to the Furre table.
    (5:513)  add column {DepartureTime} with type {TEXT} to the Furre table.
    (5:513) add column {DepartureDate} with type {TEXT} to the Furre table.


*Capture Arrival/Departure Date and Times
(0:0) When the Monkey Speak Engine Starts,
(0:300) when timer 1 goes off,
    (5:310) get the current year and put it into variable %Year.
    (5:309) get the current month and put it into variable %Month.
    (5:308) get the current day of the month and put it into variable %Day.
    (5:305) get the current hour and put it into variable %Hour.
    (5:306) get the current minutes and put it into variable %Minutes.
    (5:307) get the current seconds and put it into variable %Seconds.

*Make sure the Furre is on the Furre Table
(0:30) When anyone enters the Dream,
    (5:501)  add the triggering furre with the default access level "0" to the Furre Table in the database if he, she, or it doesn't exist.
*Time stamp Dream Arrivals
    (5:505)  update Database info {ArrivalDate} about the triggering furre will now be {%Year-%Month-%Day}.
    (5:505)  update Database info {ArrivalTime} about the triggering furre will now be {%Hour:%Minutes:%Seconds}.
* Clear Departure times as the Furre is in the dream
		(5:505) update Database Record {DepartureDate} about the triggering furre will now be { }.
		(5:505) update Database Record {DepartureTime} about the triggering furre will now be { }.

*Timestamp Dream Departures
(0:32) When anyone leaves the Dream,
		(5:505) update Database Record {DepartureDate} about the triggering furre will now be {%Year-%Month-%Day}.
		(5:505) update Database Record {DepartureTime} about the triggering furre will now be {%Hour:%Minutes:%Seconds}.

(0:21) When anyone whispers something with {!LastSeen } in it,
	(1:1) and the triggering furre is the Bot Controller,

* Could potentially use the following line or any other Restriction
*	(1:500) and the Database Record {Access level} about the triggering furre is equal to #,
    (5:402) with {%MESSAGE} get words starting at 9 to and set it to variable %LastFurre.
		(5:125) take variable %LastFurre and convert it to Furcadia short name. (without special characters or spaces or pipe "|").
		(5:508) select Database Record {ArrivalTime} about the furre named {%LastFurre}, and put it in variable %ArrivalTime.
		(5:508) select Database Record {ArrivalDate} about the furre named {%LastFurre}, and put it in variable %ArrivalDate.
        (5:15) whisper {Last seen %LastFurre on %ArrivalDate at %ArrivalTime} to the triggering furre.

(0:34) When the bot enters a Dream,
    (5:16) whisper {%DREAMNAME} to furre named {%BOTCONTROLLER}.

(0:42) When anyone requests to join the bot,
    (5:40) summon the triggering furre
    (5:102) print {%NAME requested to join} to the log.

*Endtriggers* 8888 *Endtriggers*
