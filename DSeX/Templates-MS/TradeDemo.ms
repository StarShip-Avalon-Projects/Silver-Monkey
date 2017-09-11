*MSPK V02.00 Silver Monkey  
*Silver Monkey MonkeySpeak Script File
*Created by Gerolkae
* Based on Furbots Trade Gold DragonScale for contest entry.

*Block 0
* When the Bot starts up we want to preset %TradeName to {No Furre}
(0:0) When the bot starts,
        (5:100) set variable %TradeName to {NO Furre}.

* Block 1 
* Each time the bot sees a Trade responce from the server We Safety lock the commands
* This way if some one does and invalid trade we can catch it at the end.

(0:46) When the bot sees a trade request,
        (5:101) set variable %safety to 2.

* Block 2
* This is how we start  trading session with a Furre. 
* We Set the Safety 1 because its a Valid responce from the server.
* Also we want to message the Furre with some information what we're doining here.
* Of Course we want to start the trade by saying {trade-start}

(0:48) When the bot sees a trade request with {asks you to have a trading session} in it,
        (5:120) use variable %MESSAGE  and take word 1 and put it into variable %TradeName 
        (5:6) whisper {You have started a trading session with an automation, as such, the wording has to be exact for the script to follow through and there be a successful trade. This session is for the purpose of trading one SD to the bot for testing purposes.} to {%TradeName}.
        (5:0) say {trade-start}
        (5:101) set variable %safety to 1.


* Block 3
* Here we detect what the furre wants to offer. In this case The Furre Offers 1 Silver Dragon Scale.
* We Set our Trade Reason and accept the trade offer and notify the furre it's good for us

(0:48) When the bot sees a trade request with {has added One Silver DragonScale to their current offer} in it,
        (5:0) say {trade-reason Testing Silver Monkey}
        (5:0) say {trade-accept}
        (5:6) whisper {Offer is valid, set your trade reason to 'Bot Test'} to {%TradeName}.
        (5:101) set variable %safety to 1.

* Block 4
* catch Valid Server response so we don't error and Drop the trade
(0:48) When the bot sees a trade request with {You accept the current offer} in it,
    (1:105) and variable %TradeName does not equal {NO Furre},
        (5:101) set variable %safety to 1.

* Block 5
* catch Valid Server response so we don't error and Drop the trade
* We instructed the Furre to set the Reason to 'Bot Test' earlier so we Store thier name
* to Compare with in the next block
(0:48) When the bot sees a trade request with {Bot Test} in it,
        (5:120) use variable %MESSAGE  and take word 1 and put it into variable %Victem

* Block 6
* %TradeName and %Vicem are the same and we recognize the trade reason let the furre know its good. 
(0:48) When the bot sees a trade request with {Bot Test} in it,
    (1:102) and variable %TradeName equals %Victem,
        (5:6) whisper {Trade reason has been recognized by this script, please use 'trade-accept' and click YES on the popup to successfully conclude this session.} to {%TradeName}.

* Block 7
*trap the good responce so we don't drop the trade
(0:48) When the bot sees a trade request with {Bot Test} in it,
    (1:105) and variable %TradeName does not equal {NO Furre},
(0:48) When the bot sees a trade request with {have begun a trading session.}  in it,
    (1:105) and variable %TradeName does not equal {NO Furre},
(0:48) When the bot sees a trade request with {You set your trade reason to } in it,
    (1:105) and variable %TradeName does not equal {NO Furre},
(0:48) When the bot sees a trade request with {Your items have been traded}  in it,
    (1:105) and variable %TradeName does not equal {NO Furre},
        (5:101) set variable %safety to 1.

* Block 10
*  Lets Complete the Trade and capture the Valid responce. Here we can Update a database if we need too.
*  After all the Good Stuff is Done.. reset %TradeName to {No Furre} for the next Furre to do buisness with.

(0:47) When the bot sees the trade request {Your items have been traded and the trade session is now complete. Log off and back on to see the results. }
    (1:105) and variable %TradeName does not equal {NO Furre},
        (5:6) whisper {Thank you for your trade, Please have a nice day} to {%TradeName}.
        (5:100) set variable %TradeName to {NO Furre}.

*Block 11
* OOPS No Valid responces seen.. Lets drop the trade and make them start over
(0:46) When the bot sees a trade request,
    (1:102) and variable %safety equals 2,
    (1:105) and variable %TradeName does not equal {NO Furre},
        (5:102) print {Failure Responce: %MESSAGE } to the console.
        (5:6) whisper {Invalid Action, Trade Terminated} to {%TradeName}.
        (5:0) say {trade-end}
        (5:100) set variable %TradeName to {NO Furre}.

*Endtriggers* 8888 *Endtriggers*
