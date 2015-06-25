* When the bot enters a dream, the dream name is stored to an internal variable, %DREAMNAME.
(0:90) When the bot enters a dream,
* Check if the name of the dream ‘monkeypalace’ is in the dream name.
 (1:175) and message %DREAMNAME does not contain {monkeypalace} anywhere within it,
* To be sure it is the right monkeypalace, we check that the length is exactly that of “monkeypalace” and has no other text in it.
 (1:177) and the length of message %DREAMNAME is not equal to 12
* If it is found that neither of the two conditions are met, we send the dream link command to Furcadia, to join the right dream (and this will check again)
    (5:0) say {`fdl furc://monkeypalace}.