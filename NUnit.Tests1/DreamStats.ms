*MSPK V04.00 Silver Monkey
*MonkeySpeak Script File
*Created by <name> 



(0:90) When the bot enters a Dream,
  (1:21) and the furre named {theshoddyribbon} is the Dream owner,
    (5:300) create timer 13 to go off every 900 second(s). DreamStats Update

*** Send the Data to the Dream Statistic api
(0:90) When the bot enters a Dream,
  (1:21) and the furre named {theshoddyribbon} is the Dream owner,
(0:300) When timer 13 goes off,
  (1:21) and the furre named {theshoddyribbon} is the Dream owner,
    (5:19) clear the Web-Stack.
    (5:10)  Set the web URL to {https://silvermonkey.tsprojects.org/charts/smAPI.php},
*(5:10)  Set the web URL to {http://silvermonkey.tsprojects.org/postecho.php},


    (5:100) set variable %user to {Silver Monkey}.
    (5:100) set variable %password to {T8fok13dx}.
    (5:100) set variable %action to {PostData}.
    (5:702) count the number of active furres in the dream and put it in the variable %ActiveFurreCount.
    (5:703) count the number of A.F.K furres in the drean and put it in the variable %AFKFurreCount.
    (5:701) save the dream list count to variable %FurreCount.

    (5:17) store variable %user to the Web-Stack.
    (5:17) store variable %password to the Web-Stack.
    (5:17) store variable %action to the Web-Stack.

    (5:17) store variable %DREAMOWNER to the Web-Stack.

    (5:17) store variable %ActiveFurreCount to the Web-Stack.
    (5:17) store variable %AFKFurreCount to the Web-Stack.
    (5:17) store variable %FurreCount to the Web-Stack.

    (5:18) send POST request to send the Web-Stack to URL.

*(0:70) When the bot receives a variable list by sending the Web-Stack.


(0:16) When someone whispers {Hi},
(5:5) whisper {Hello %NAME} to the triggering furre.

*Endtriggers* 8888 *Endtriggers*
