Contexto / Propósito
Este videojuego es una práctica de clase del curso GECEGS en Desarrollo de Videojuegos y Realidad Virtual en el centro CPIFP Alan Turing.
El objetivo principal es aplicar conceptos de desarrollo en Unity 2D, incluyendo mecánicas de plataformas, puntuación, enemigos y recolección de objetos.

Historia
Hongui, una pequeña seta curiosa, deja su hogar seguro para explorar el Bosque Brillante y recolectar diamantes mágicos.
En su camino deberá esquivar águilas, ranas y caracoles mientras demuestra que, aunque seas pequeño, la valentía puede llevarte a grandes aventuras.

Mecánicas
Sistema de daño: Los enemigos causan daño al entrar en contacto con nuestro personaje.
Saltos: Doble salto desde el suelo y salto único si se inicia desde el aire.
Dash: Desplazamiento en linea recta hacia la posición a la cual está mirando el personaje.
Abrir cofres: Al acercarse a un cofre saltará una exclamación indicando que ese cofre podra ser abierto.
Recolección de cofres: Necesitaras abrir cofres normales que contengan tesoros para completar el nivel.
Apertura de mimos: Algunos cofres no contendrán tesoros sino un enemigo que atacará al personaje.

Enemigos: 
Slime Rojo: Camina de lado a lado sin caerse de las plataformas.
Slime Verde: Sube y baja por el escenario mientras gira.
Gallina: Se desplaza horizontalmente y haciendo parabolas.

Código fuente
El proyecto está desarrollado en Unity 6000.2.6 con C#. Entre los scripts podemos ver:

AudioManager: Controla gran parte de los sonidos.
Cartel: Se encarga de la aparición de los carteles a modo tutorial del nivel 1.
Chest: Gestiona el comportamiento de los cofres, tanto normales como mimicos.
EnemyController: Gestiona el comportamiento de los enemigos en escena (slimes y gallinas)
GameConstanst: Almacena constantes del juego.
GameManager: Gestiona la lógica global del juego, estados de niveles.
HUDController: Controla los iconos, botones y toda la parte del HUD del juego.
LevelManager: Gestiona el cambio entre escenas, pantallas de win y game over.
LevelSelected: Permite cambiar entre escenas.
MenuManager: Es el controlador del propio menú.
ParallaxEffect: Gestiona el efeccto parallax del fondo.
PlayerHealth: Controlador de la vida del personaje.
PlayerMovement: Controlador del movimiento del personaje.
PlayerPoint: Controlador de los puntos del juego.
WaitChest: Gestiona el tiempo de espera desde la aparición del tesoro hasta que pueda ser recogido.

Los scripts contienen comentarios que indican que esta pasando en cada parte.

Sistema de puntuación
Cuanto menos tiempo se tarde en terminar el nivel mayor será la puntuación final obtenida.

Créditos
Desarrollo y Producción
Ismael Parra Cerezo

Tilesets

Escenario: Pixel Woods
https://cubic-tree.itch.io/pixel-woods-free-platformer-tileset

HUD: Pixel Fantasy
https://franuka.itch.io/rpg-ui-pack-demo

Personajes / Sprites

Dullahan
Modelo: Ismael Parra Cerezo
Animaciones: Ismael Parra Cerezo

Slimes
Modelo: Ismael Parra Cerezo
Animaciones: Ismael Parra Cerezo

Gallina
Modelo: Ismael Parra Cerezo
Animaciones: Ismael Parra Cerezo

Cofre/Mimico
Modelo: Ismael Parra Cerezo
Animaciones: Ismael Parra Cerezo

Sonidos
Echoes of the Hollow Crown (AI Suno)
