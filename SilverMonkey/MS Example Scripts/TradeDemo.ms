*MSPK V02.00 Silver Monkey  
*Silver Monkey MonkeySpeak Script File
*Created by Gerolkae
* Based on Furbots Trade Gold DragonScale for contest entry.

(0:0) When the bot starts,
        (5:100) set variable %TradeName to {NO Furre}.
        (5:102) print {Startup: } to the console. 

(0:46) When the bot sees a trade request,
        (5:101) set variable %safety to 2.
        (5:102) print {Global Catch } to the console. 
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console. 

(0:48) When the bot sees a trade request with {asks you to have a trading session} in it,
        (5:120) use variable %MESSAGE  and take word 1 and put it into variable %TradeName 
        (5:6) whisper {You have started a trading session with an automation, as such, the wording has to be exact for the script to follow through and there be a successful trade. This session is for the purpose of trading one SD to the bot for testing purposes.} to {%TradeName}.
        (5:0) say {trade-start}
        (5:101) set variable %safety to 1.
        (5:102) print {Trade-Start Responce: %MESSAGE } to the console.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.

* You and Gerolkae have begun a trading session. Commands
(0:48) When the bot sees a trade request with {have begun a trading session.}  in it,
*    (1:105) and variable %TradeName does not equal {NO Furre},
        (5:101) set variable %safety to 1.
        (5:102) print {Session Began Responce: %MESSAGE } to the console.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.

(0:48) When the bot sees a trade request with {has added One Silver DragonScale to their current offer} in it,
        (5:0) say {trade-reason Testing Silver Monkey}
        (5:0) say {trade-accept}
        (5:6) whisper {Offer is valid, set your trade reason to 'Bot Test'} to {%TradeName}.
        (5:101) set variable %safety to 1.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.


(0:48) When the bot sees a trade request with {You accept the current offer} in it,
    (1:105) and variable %TradeName does not equal {NO Furre},
        (5:101) set variable %safety to 1.

        (5:102) print {Accept Rsponce: %MESSAGE } to the console.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.

(0:48) When the bot sees a trade request with {Bot Test} in it,
        (5:120) use variable %MESSAGE  and take word 1 and put it into variable %Victem
        (5:101) set variable %safety to 1.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.

(0:48) When the bot sees a trade request with {Bot Test} in it,
    (1:102) and variable %TradeName equals %Victem,
        (5:6) whisper {Trade reason has been recognized by this script, please use 'trade-accept' and click YES on the popup to successfully conclude this session.} to {%TradeName}.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.

(0:48) When the bot sees a trade request with {Bot Test} in it,
    (1:105) and variable %TradeName does not equal {NO Furre},
        (5:101) set variable %safety to 1.
        (5:102) print {BOT Test Responce: %MESSAGE } to the console.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.

(0:46) When the bot sees a trade request,
    (1:102) and variable %safety equals 2,
    (1:105) and variable %TradeName does not equal {NO Furre},
        (5:102) print {Failure Responce: %MESSAGE } to the console.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.
        (5:6) whisper {Invalid Action, Trade Terminated} to {%TradeName}.
        (5:0) say {trade-end}
        (5:100) set variable %TradeName to {NO Furre}.
        (5:102) print {safety %safety } to the console. 
        (5:102) print {Tradename %TradeName } to the console.



*Endtriggers* 8888 *Endtriggers*
