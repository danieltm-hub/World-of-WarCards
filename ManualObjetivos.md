# WarCards - Manual de Objetivos

Los objetivos, al igual que los poderes pueden recibir parámetros, pero usualmente no son necesarios. Y la sintaxis es la misma que la de los poderes. Por ejemplo:

```c++
PalabraClave() //onjetivo sin parametros
```

## Objetivos

- `NullObjective` : *Palabra Clave:* "nullobj". *Descripción:* No está dirigido a nadie. *Parámetros:* Ninguno.

- `Self` : *Palabra Clave:* "self". *Descripción:* Está dirigido al jugador actual. *Parámetros:* Ninguno.

- `Next` : *Palabra Clave:* "next". *Descripción:* Está dirigido al jugador siguiente al actual. *Parámetros:* Ninguno.

- `LowestHealth` : *Palabra Clave:* "lowesthealth". *Descripción:* Está dirigido al jugador con menos vida. *Parámetros:* Ninguno.

- `HightHealth` : *Palabra Clave:* "highesthealth". *Descripción:* Está dirigido al jugador con más vida. *Parámetros:* Ninguno.

- `EqualHealth` : *Palabra Clave:* "equalhealth". *Descripción:* Está dirigido a los jugadores con la misma cantidad de vida que el jugador actual. *Parámetros:* Ninguno.

- `HightestAvailableCards` : *Palabra Clave:* "highestcards". *Descripción:* Está dirigido al jugador con más cartas disponibles a jugar en su mano. *Parámetros:* Ninguno.

- `LowestAvailableCards` : *Palabra Clave:* "lowestcards". *Descripción:* Está dirigido al jugador con menos cartas disponibles a jugar en su mano. *Parámetros:* Ninguno.
