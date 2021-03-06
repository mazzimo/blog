Ho da poco scoperto una feature molto interessante di ASP.NET MVC 4 che aiutera' tantissimo chi e' intento a sviluppare web application che si vedano in modo decente su dispositivi mobili e non.

![titolo](http://1.bp.blogspot.com/-6NfXQXQ0umk/UWkjIhxmKII/AAAAAAAAAJQ/ibGHZw3tJJ0/s1600/meme.jpg \"title\")

Lo scopo e' fornire diverse versioni della stessa View, creandone una aggiuntiva che sara' adattata per smartphone e tablet. e' possibile addirittura fare in modo di fornire un ulteriore view per un particolare User-Agent.

Ma andiamo per gradi. consideriamo l'esempio piu' banale: modificare il layout della nostra web application in base al dispositivo utilizzato.

Tutto quello che basta fare e' creare un ulteriore copia del nostro attuale layout (nel caso standard si trova nel path **Views\\Shared\\_Layout.cshtml**), rinominarlo **_Layout.Mobile.cshtml** e effettuare le nostre modifiche.

in questo modo MVC 4 in modo TOTALMENTE AUTOMATICO utilizzera' il file **_Layout.Mobile.cshtml** per renderizzare la pagina solo in caso di dispositivi mobili ,altrimenti utilizzera' il classico **_Layout.cshtml**

Questo vale non solo per i Layout ma anche per tutte le Views, comprese le Partial.

**.Mobile** e' il solo suffisso che viene fornito di default, ma possiamo crearne di nuovi in modo tale da avere una particolare renderizzazione su un particolare Browser o dispositivo.

per aggiungere un suffisso, bisogna andare a modificare il file **Global.asax** aggiungendo la seguente istruzione alla funzione *Application_Start*:

    DisplayModeProvider.Instance.Modes.Insert(0, 
        //indicare qui il nome del nuovo suffisso
        new DefaultDisplayMode(\"iPhone\")
        {
            //indicare quale condizione dev'essere soddisfatta
            //per utilizzare la view con il nuovo suffisso
            //(solitamente controllando l'user agent)
            ContextCondition = (context => context.GetOverriddenUserAgent().Contains(\"iPhone\"))
        });
        
Nell'esempio banale fornito, avremo la possibilita' di utilizzare un ulteriore copia del layout nominata \"_Layout.iPhone.cshtml\" per la visualizzazione su iPhone.