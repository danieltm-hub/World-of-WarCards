# WarCards - Manual de Poderes

Los poderes, como toda pieza del juego puede recibir parámetros. Los parámetros son valores que necesita el poder para realizar su función. Los parametros son específicos de cada poder, por lo que se recomienda leer la descripción de cada uno para saber cuáles son los parámetros que recibe. Los parámetros se intruducen luego del poder, encerrados en llaves () y separados por comas. Por ejemplo:

```c++
PalabraClave() //poder sin parametros

PalabraClave(parametro) //poder con un parametro

PalabraClave(parametro1, parametro2, parametro3) // poder con 3 parametros
```

## Poderes

- `NullPower` : *Palabra Clave:* "nullpow". *Descripción:* No hace nada. *Parámetros:* Ninguno.

- `ModifyHealth` : *Palabra Clave:* "modifyhealth". *Descripción:* Modifica la cantidad dada de vida del jugador al que poder está dirigido, es decir, si la cantidad es positiva  recuperará vida la cantidad indicada, y si la cantidad es negativa la perderá. *Parámetros:* Recibe una expresión numérica.

- `ModifyEnergy` : *Palabra Clave:* "modifyenergy". *Descripción:* Modifica la cantidad dada de enegía del jugador al que poder está dirigido, es decir, si la cantidad es positiva  recuperará energía, y si la cantidad es negativa la perderá. *Parámetros:* Recibe una expresión numérica.
  