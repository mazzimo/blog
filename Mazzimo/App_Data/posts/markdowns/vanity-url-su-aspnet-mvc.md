In questi giorni sto notando tramite il contatore che le visite di questo blog stanno quasi raggiungendo la straordinaria cifra di ben 1000 visitatori, rendendo il blog stesso il piu' letto tra le persone che lo visitano.

Eh gia, precedentemente era il secondo.

Pavoneggiamenti a parte, per festeggiare questo traguardo sarei potuto uscire ad ubriacarmi o andare in thailandia a fare del turismo sessuale, ma ho personalmente deciso di premiare la vostra fiducia scrivendo un post pieno di tecnicismi, come solo il buon mazzimo sa fare.

Oggi parlero' di come implementare i vanity URL nelle nostre web application su ASP.NET MVC.


![titolo](http://1.bp.blogspot.com/-g2xLwQjt9Sw/UVL6-cQJcJI/AAAAAAAAAI8/DuHGkzWH0Kg/s1600/clean_urls.png \"title\")

il tipo di vanityURL che verra' implementato sara', del tipo utilizzato per esempio su twitter e piu recentemente su facebook, roba del tipo:

    www.tuosito.com/mazzimo

visualizzera' il profilo dell'utente *mazzimo*

    www.tuosito.com/mazzimo/messagges

visualizzera' i messaggi dell'utente *mazzimo*

e cosi' via. I vanity URL's per quanto fighi, sono comunque delle belle gatte da pelare da gestire, perche' bisogna fare in modo che gli stessi non sovrascrivano gli url gia' occupati da url che non vogliamo sovrascrivere.

partiamo dal nostro esempio: abbiamo il controller *UsersController* che ha al suo interno tutte le pagine relative all utente:

    public class UsersController : Controller
    {
        public ActionResult Index(string name)
        {
    
        }
 
        public ActionResult Messages(string name)
        {
    
        }
 
    }
    
andate nel file *RouteConfig.cs* piazzato nella cartella *App_Start* e nella funzione *RegisterRoutes* vi troverete piu' o meno una situazione del genere:

    public static void RegisterRoutes(RouteCollection routes)
    {
        //Default route
        routes.MapRoute(
            name: \"Default\",
            url: \"{controller}/{action}/{id}\",
            defaults: new { controller = \"Home\", action = \"Index\", id = UrlParameter.Optional }
        );
    }
    
questa funzione effettua l'inizializzazione di tutte le regole di routing che permetteranno di chiamare il controller giusto in base all'url in input. Nel caso originale ovvero e' *{classe_controller}/{nome_metodo}/{parametro_id}*.

lasciando la regola cosi' com'e', per raggiungere le nostre due pagine per l'utente *mazzimo* dovremo utilizzare rispettivamente gli url *www.tuosito.com/Users?name=mazzimo* e *www.tuosito.com/Users/Messages/?name=mazzimo*, il che fa abbastanza cagare.

Per ottenere quello che vogliamo (intendo i vanity Url, non il dominio del mondo) dovremo modificare questa funzione.

Abbiamo fondamentalmente 3 obiettivi principali:

- Modificare la regola attuale in modo tale da essere ignorata se non c'e' nessun controller che corrisponde;
- Aggiungere l'ulteriore regola (subordinata alla prima) che effettuera' la chiamata al controller degli utenti (verra' posizionato successivamente alla regola di default, in modo tale che le pagine attualmente esistenti non verranno coperte dai nostri vanity URL);
- Fare in modo che questa regola venga ignorata se quello specifico url non corrisponda ad un utente (in realta' questo passo non e' obbligatorio, ma e' consigliato in caso di aggiunte successive di nuove regole);

## Passo numero 1: saltare la prima regola se non corrisponde a nessun controller

Innanzitutto, bisognera' utilizzare un diverso overload della funzione *MapRoute* che contempla i **costraints**, oggetti che ci permetteranno di effettuare ulteriori controlli. Utilizzeremo i costraint per verificare se al nostro url corrisponde effettivamente un controller. Solitamente possono essere aggiunti come costraint espressioni regolari oppure classi che implementano l'interfaccia *IRouteConstraint*. Il nostro e' appunto quest'ultimo caso:

    public class ControllerConstraint : IRouteConstraint
    {
        static List<string> ControllerNames = GetControllerNames();
 
        private static List<string> GetControllerNames()
        {
            List<string> result = new List<string>();
            foreach(Type t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                if( typeof(IController).IsAssignableFrom(t) && t.Name.EndsWith(\"Controller\"))
                {
                    result.Add(t.Name.Substring(0, t.Name.Length - 10).ToLower());
                }
            }
            return result;
        }
 
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return ControllerNames.Contains(values[parameterName].ToString().ToLower());
        }
    }

Questa classe deve essere indicata come costraint alla chiamata della funzione *MapRoute* all'interno del file *RouteConfig.cs* come segue:

    //Default route
    routes.MapRoute(
        name: \"Default\",
        url: \"{controller}/{action}/{id}\",
        defaults: new { controller = \"Home\", action = \"Index\", id = UrlParameter.Optional },
        //aggiungere questa linea
        constraints: new { controller = new ControllerConstraint() }
    );

## Passo numero 2: aggiungere la chiamata che gestira' il vanity URL

Bisogna modificare il file *RouteConfig.cs* aggiungendo un ulteriore regola di ruouting successiva alla prima (in modo tale che sia subordinata alla regola precedente):

    public static void RegisterRoutes(RouteCollection routes)
    {
        //Default route
        routes.MapRoute(
            name: \"Default\",
            url: \"{controller}/{action}/{id}\",
            defaults: new { controller = \"Home\", action = \"Index\", id = UrlParameter.Optional },
            constraints: new { controller = new ControllerConstraint() }
        );
 
        //Additional route
        routes.MapRoute(
            name: \"VanityUrl\",
            url: \"{name}/{action}/{id}\",
            defaults: new { controller = typeof(UsersController).Name.Replace(\"Controller\", \"\"), action = \"Index\", id = UrlParameter.Optional }
        );
    }

Il tutto, cosi', gia funziona alla grande: le richieste verranno redirette nel nostro controller *UserController* solo se la prima regola viene ignorata.

Utilizzando lo stesso esempio di prima per l'utente mazzimo, raggiungeremo le stesse pagine indicate con gli url :

*www.tuosito.com/mazzimo* e *www.tuosito.com/mazzimo/Messages/*

## Passo numero 3: controllo degli username

Vogliamo essere ancora piu' precisi? Possiamo aggiungere un altra classe che useremo come costraint.

Il tutto e' analogo a quanto visto prima, solo che invece di riprendere i nomi dei controller tramite reflection, effettueremo una query per controllare se l'username indicato rappresenta effettivamente un username esistente.

    public class UserNameConstraint : IRouteConstraint
    {
 
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            //Effettua la query
            //se esiste un username uguale a values[parameterName].ToString().ToLower()
                //ritorna True
            //altrimenti
                //ritorna False
        }
    }
    
Successivamente, cambiare la regola appena aggiunta in questo modo:

    //Additional route
    routes.MapRoute(
        name: \"VanityUrl\",
        url: \"{name}/{action}/{id}\",
        defaults: new { controller = typeof(UsersController).Name.Replace(\"Controller\", \"\"), action = \"Index\", id = UrlParameter.Optional }
        constraints: new { name = new UserNameConstraint() }
    );