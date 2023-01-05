# World of WarCards

![World Of WarCards Logo](WorldofWarCards.png)

>Proyecto de Programación II. Facultad de Matemática y Computación Universidad de La Habana. Curso 2022. Autores: Daniel Toledo, Osvaldo Moreno, José Antonio Concepción.

## Acerca del juego

World of WarCards es un juego de cartas por turnos. El objetivo es debilitar al rival con los efectos de las cartas escogidas. Cada jugador tiene una energía y una voluntad máxima a utilizar en cada turno. Cada carta consume una cantidad de energía específica y una unidad de voluntad. Cuando una carta se activa no podrá ser reutlizada hasta que ocurra un número espicificado de acciones (Cooldown). El jugador será el encargado de, dadas sus cartas, determinar una estrategia ganadora en cada juego.

## Introducción al uso de la consola

La aplicación de consola esta compuesta por 3 secciones:

- **`Jugar`**
- **`Opciones`**
- **`Créditos`**

Una vez seleccionada la opción **Jugar** desde el menú interactivo, usted deberá introducir los datos de los jugadores y escoger las cartas con las que jugará cada uno. En el menú de batalla, en la sección superior podrá apreciar las estadísticas de los jugadores, y el log del juego (Turno del player y acciones que ocurren). En la sección inferior se encuentran las cartas escogidas y un menú de acciones que se pueden  realizar, utilice las flechas izquierda y derecha para moverse a través de las cartas disponibles, y los números mostrados delante del nombre de cada acción, para realizar la misma. En la sección de **Opciones** podrá ver todas las cartas disponibles en el juego con su descripción. Además, podrá realizar un simulación de un juego entre jugadores virtuales de su elección.

Las clases del manejo de la interfaz virtual se encuentran dentro de la carpeta Console. Se recomienda al usuario usar una consola totalmente expandida para que pueda disfrutar de una mejor experiencia de juego.

## Sobre la creación de cartas

El juego es totalmente configurable, los jugadores pueden acceder a [*code.txt*](Program/code.txt) para crear y modificar las cartas disponibles. Para ello cuentan con un conjunto de piezas que debe usar. Por lo que traemos una breve explicación de cada una de ellas.
Cada pieza esta representada por una palabra clave que el usuario debe introduccir para hacer su uso. Las piezas son:

- `Poderes`: Estas recogen los poderes más simples a partir de los cuales el usario podra componer y crear sus poderes. Podra ver los poderes detalladamente el uso de cada uno de ellos en [Manual de Poderes](ManualPoderes.md).
  
- `Objetivos`: Los objetivos son piezas que se encargan de determinar a quién irá dirigdo un poder. Por ejemplo, si el poder es de daño, el objetivo determina a quien se le aplicará el daño. Los objetivos se pueden ver detalladamente en [Manual de Objetivos](ManualObjetivos.md).

Con estas piezas el usuario es capáz de crear un estructura superior y más compleja, los `Efectos`. Los efectos son piezas que se encargan de combinar los poderes y los objetivos para crear un efecto completo.
Los efectos no usan palabras claves, comienzan con un corchete `[` y terminan con otro `]`. Dentro de los corchetes el usuario introduce los poderes que desee, luego un punto y coma `;` y los objetivos que desee. Luciendo asi `[objetivos;poderes]`. A continuación se muestran algunos ejemplos practicos de efectos:

```c++
[next(); modifyhealth(-1)] //daño al siguiente jugador

[next(), self(); modifyhealth(-1.2), modifyenergy(-1)] //daño y consumo de energia al siguiente jugador y a si mismo

// notese que el grupo de poderes se separa por comas ',' , al igual que los objetivos 
```
