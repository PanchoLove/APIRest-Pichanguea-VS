# APIRest-Pichanguea-VS
API Rest pichanguea creado con C#


## Instrucciones
Ejecutar desde visual studio, se abrira una pagina como "localhost:55242", en este ejemplo la ruta de todas las funcionalidades seria: "localhost:55242/api", ademas en "localhost:55242/help" se encuentra una lista con el detalle de todos los metodos del servicio (Generado automaticamente).

## Lista de URLs y sus metodos
Como se menciono el detalle de los metodos se encuentra en /help, aqui solo se mencionan aquellos que pueden considerarse como relevantes para las historias de usuario:

 -Crear Usuario (Jugador)
 -Inicio de sesion
 -ver informacion de un partido
 -ver informacion de un equipo
 -ver informacion de un jugador
 -obtener los partidos de un jugador
 -obtener los equipos de un jugador
 -modificar asistencia de un jugador
 -modificar asistencia de un jugador incluyendo galletas
 -Agregar galletas
 -Modificar cantidad de galletas
 -Obtener los mensajes del chat de un partido
 -Enviar un mensaje a un caht de partido
 -Obtener los mensajes del chat de un equipo
 -Enviar un mensaje a un caht de equipo


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

Se retorna un json con el jugador + un token de prueba (version simplificada respecto de la version anterior)

### ver informacion de un partido

@GET : ```api/Partido/{id}```

ejemplo1: api/Partido?id=1
ejemplo2: api/Partido/1

(Si no se incluye el id retorna una lista con todos los partidos, de aqui en mas aplica lo mismo para todas las Urls como por ejemplo equipo, usuario, etc)


### ver informacion de un equipo

@GET : ```api/Partido/{id}```


### ver informacion de un jugador

@GET: ``` api/Jugador/{id} ```


### obtener los partidos de un jugador

@GET: ``` api/Jugador/{idJugador}/Partidos ```

ejemplo: api/Jugador/1/Partidos

### obtener los equipos de un jugador

@GET: ``` api/Jugador/{idJugador}/Equipos ```

### modificar asistencia de un jugador

@PUT: ``` api/Jugador/{idJugador}/Partidos/{idPartido}/Asistencia/{estado} ```

donde "estado" corresponde a un entero que indica si el jugador posee una asistencia confirmada, cancelada, etc. Falta consultar con el cliente acerca de que valor tiene realmente cada estado, de momento el servicio asume como Confirmado=1 y Cancelado=0

### modificar asistencia de un jugador incluyendo galletas

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

### Enviar un mensaje a un caht de partido
Analoga a la funcion anterior, la diferencia esta en que para cada caso cambia el body del post

@GET ``` api/Partido/{idPartido}/Chat ```

Body: 
	``` 
	{
	 "idPartido": 1.0,
	 "idJugador": 1.0,
	 "contenido": "sample string 1"
	} 
	```


@GET ``` api/Jugador/{idJugador}/Partidos/{idPartido}/Chat ```

Body: 
	``` 
	{
	 "contenido": "sample string 1"
	}  
 	```

### Obtener los mensajes del chat de un equipo
Analogo a obtener los mensajes de un partido: 

@GET ``` api/Equipo/{idPartido}/Chat ```

@GET ``` api/Equipo/{idJugador}/Partidos/{idPartido}/Chat ```

### Enviar un mensaje a un caht de partido
idem

@GET ``` api/Equipo/{idPartido}/Chat ```

Body:
	```
	{
	 "idPartido": 1.0,
	 "idJugador": 1.0,
	 "contenido": "sample string 1"
	}  
	  ```


@GET ``` api/Jugador/{idJugador}/Equipos/{idPartido}/Chat ```

Body: 
	```
	{
	 "contenido": "sample string 1"
	}  
	```

