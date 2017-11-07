# APIRest-Pichanguea-VS
API Rest pichanguea creado con C#


## Instrucciones
Ejecutar desde visual studio, se abrira una pagina como "localhost:55242", en este ejemplo la ruta de todas las funcionalidades seria: "localhost:55242/api", ademas en "localhost:55242/help" se encuentra una lista con el detalle de todos los metodos del servicio (Generado automaticamente).

## Lista de URLs y sus metodos
Como se menciono el detalle de los metodos se encuentra en /help, aqui solo se mencionan aquellos que pueden considerarse como relevantes para las historias de usuario

resumen:

    -crear Usuario (Jugador)
    -inicio de sesion
    -ver informacion de un partido
    -ver informacion de un equipo
    -ver informacion de un jugador
    -obtener todos los partidos de un jugador desde la fecha actual (confirmados o no confirmados)
    -obtener los partidos confirmados del jugador desde la fecha actual
    -obtener los partidos NO confirmados del jugador desde la fecha actual
    -obtener los equipos de un jugador
    -modificar asistencia de un jugador
    -modificar asistencia de un jugador incluyendo galletas
    -agregar galletas
    -modificar cantidad de galletas
    -obtener los mensajes del chat de un partido
    -enviar un mensaje a un chat de partido
    -obtener los mensajes del chat de un equipo
    -enviar un mensaje a un chat de equipo
    -solicitar ingreso a un equipo


### Crear Usuario (Jugador)

@POST: ```api/Jugador```

body: 
```
	{
	  "jugUsername": "sample string 2",
	  "jugPassword": "sample string 3",
 	  "jugRut": 1.0,
          "jugRutDv": "sample string 4",
          "jugNombre": "sample string 5",
          "jugPaterno": "sample string 6",
	  "jugMaterno": "sample string 7",
	  "jugFono": "sample string 8",
	  "jugCelular": "sample string 9",
          "jugEmail": "sample string 10",
	  "jugFoto": "sample string 11",
          "jugCreacion": "2017-10-22T06:50:20.7831785-03:00",
          "jugApodo": "sample string 12"
	 }
```


(El servicio permite que se omitan parametros)

### Inicio de sesion

@GET : ```api/Sesion?usuario={usuario}&pass={pass}```

Donde "usuario" puede ser el username o el correo
Se retorna un json con el jugador + un token de prueba (version simplificada respecto de la version anterior)

### Ver informacion de un partido

@GET : ```api/Partido/{id}```

ejemplo1: api/Partido?id=1

ejemplo2: api/Partido/1

(Si no se incluye el id retorna una lista con todos los partidos, de aqui en mas aplica lo mismo para todas las Urls como por ejemplo equipo, usuario, etc)


### Ver informacion de un equipo

@GET : ```api/Equipo/{id}```


### Ver informacion de un jugador

@GET: ``` api/Jugador/{id} ```


### Obtener todos los partidos de un jugador desde la fecha actual hacia adelante

retorna todos los partidos del jugador, sean confirmados o no confirmados

@GET: ``` api/Jugador/{idJugador}/Partidos ```

ejemplo: api/Jugador/10003/Partidos

### Obtener los partidos confirmados de un jugador desde la fecha actual hacia adelante

retorna todos aquellos partidos del jugador en donde se ha realizado una acción (se ha confirmado o negado la asistencia) 

@GET ``` api/Jugador/{idJugador}/Partidos/Confirmados ```

ejemplo: api/Jugador/10003/Partidos/Confirmados

### Obtener los partidos NO confirmados de un jugador desde la fecha actual hacia adelante

retorna todos aquellos partidos del jugador en donde NO se ha realizado una acción (NO se ha confirmado ni negado la asistencia) 

@GET ``` api/Jugador/{idJugador}/Partidos/NoConfirmados ```

ejemplo: api/Jugador/10003/Partidos/Confirmados

###---------------------------------------------
### OJO, PARA LOS ULTIMOS 3 MÉTODOS SOLO HACE ALGO CON LA ID "10003", YA QUE CON ELLA SE CREARON PARTIDOS FICTICIOSOS, PARA LOS OTROS JUGADORES NO EXISTEN PARTIDOS FUTUROS.
###---------------------------------------------


### Obtener los equipos de un jugador

@GET: ``` api/Jugador/{idJugador}/Equipos ```

### Modificar asistencia de un jugador

@PUT: ``` api/Jugador/{idJugador}/Partidos/{idPartido}/Asistencia/{estado} ```



### Modificar asistencia de un jugador incluyendo galletas

similar al metodo anterior pero ademas se puede indicar una cantidad de galletas agregar a la asistencia

@PUT ``` api/Jugador/{idJugador}/Partidos/{idPartido}/Asistencia?estado={estado}&galletas={galletas} ```

(si el estado es de cancelado se ignora la cantidad de galletas y no se utiliza)

### Agregar galletas

@POST ``` api/Jugador/{idJugador}/Partidos/{idPartido}/Asistencia/Galletas/{cantidad} ```

### Modificar cantidad de galletas

@PUT ``` api/Jugador/{idJugador}/Partidos/{idPartido}/Asistencia/Galletas/{cantidad} ```

### Obtener los mensajes del chat de un partido
Existen 2 maneras, una es desde la URL de api/jugador/{idJugador}/Partidos/{idPartido}/Chat y la otra desde la URL de "api/Partido/{idPartido}/Chat" :

@GET ``` api/Partido/{idPartido}/Chat ```

@GET ``` api/Jugador/{idJugador}/Partidos/{idPartido}/Chat ```

### Enviar un mensaje a un chat de partido
Analoga a la funcion anterior, la diferencia esta en que para cada caso cambia el body del post

@GET ``` api/Partido/{idPartido}/Chat ```

Body: 
``` 
	{
	  "idPartido": 1.0,
	  "idJugador": 1.0,
	  "contenidoMensaje": "sample string 1"
	} 
```


@GET ``` api/Jugador/{idJugador}/Partidos/{idPartido}/Chat ```

Body: 
``` 
	{
	  "contenidoMensaje": "sample string 1"
	}  
```

### Obtener los mensajes del chat de un equipo
Analogo a obtener los mensajes de un partido: 

@GET ``` api/Equipo/{idPartido}/Chat ```

@GET ``` api/Equipo/{idJugador}/Partidos/{idPartido}/Chat ```

### Enviar un mensaje a un chat de Equipo
idem

@GET ``` api/Equipo/{idPartido}/Chat ```

Body:
```
	{
	  "idPartido": 1.0,
	  "idJugador": 1.0,
	  "contenidoMensaje": "sample string 1"
	}  
```


@GET ``` api/Jugador/{idJugador}/Equipos/{idPartido}/Chat ```

Body: 
```
	{
	  "contenidoMensaje": "sample string 1"
	}  
```

### Solicitar ingreso a un equipo

@POST ``` api/Equipo/{idEquipo}/SolicitarIngreso/{idJugador} ```

@POST ``` api/Equipo/{idEquipo}/SolicitarIngreso?idJugador={idJugador} ```


