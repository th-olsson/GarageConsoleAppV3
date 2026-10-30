# Garage 3.0
## Sammanfattning
Applikationen ska simulera ett garage där det går att parkera fordon, hämta ut fordon, se efter vilka fordon som finns där, och vilka egenskaper de har. Allt i en konsol-applikation med huvudmeny och undermenyer.

Applikationen ska som minimum ha:
- En generisk kollektion av fordon; klassen `Garage<T>`
- En fordons-klass, klassen Vehicle
- Ett antal subklasser till fordon
- Ett användargränssnitt som låter oss använda funktionaliteten hos ett garage. Här sker all interaktion med användaren
- En `GarageHandler` som abstraherar ett lager, så att det inte finns någon direkt kontakt mellan användargränssnittet och garage-klassen
- Interfaces i stället för konkreta typer (till exempel `IUI`, `IHandler`)
* * *
## Skillnader mot Garage 2.0
- Garage är nu generiskt: `Garage<T> where T : Vehicle`, och implementerar `IEnumerable<T>`
- Ny klass `GarageHandler` mellan användargränssnittet och garaget
- Vi programmerar mot interfaces (till exempel `IUI`, `IHandler`) och inte mot konkreta typer
- Tillåtet nu: generics, LINQ och enhetstester
- Enhetstester ska skapas i ett eget testprojekt
* * *
## Kravspecifikation
### Garage
- Klassen behöver inte ärva från någon annan klass eller implementera någon annan interface
- [x] Samlingen av fordon ska internt i klassen hanteras som en privat `List`
- [x] Vid instansieringen av ett nytt garage måste kapaciteten anges som argument till konstruktorn.
- [ ] Garage implementeras som en generisk klass: `Garage<T>`
- [ ] Den generiska typen begränsas med en constraint: `where T : Vehicle`
- [ ] Garage implementerar `IEnumerable<T>`, så det går att iterera över en instans med `foreach`
### Vehicle
- [x] Fordon ska implementeras som klassen Vehicle och subklasser till den
- [x] Vehicle innehåller samtliga egenskaper som ska finnas i samtliga fordonstyper. T.ex. registreringsnummer, färg, antal hjul, m.m.
    - [x] Varje registreringsnummer är unikt
- [x] Det måste finnas följande subklasser:
    - [x] Car
    - [x] Motorcycle
    - [x] Bus
    - [x] Boat
    - [x] Airplane
- [x] Varje subklass har minst en egen egenskap
### Arkitektur
- [ ] `GarageHandler` hanterar den funktionalitet som användargränssnittet behöver, så att gränssnittet inte pratar direkt med `Garage<T>`
- [ ] Interface för handlern (`IHandler`)
- [ ] Interface för användargränssnittet (`IUI`)
- [ ] Användargränssnittet använder bara `IHandler`, inte den konkreta klassen
* * *
### Funktionalitet
Det ska gå att
- [x] Lista samtliga parkerade fordon
- [x] Lista fordonstyper och hur många av varje som står i garaget
- [x] Lägga till och ta bort fordon ur garaget
- [x] Sätta en kapacitet (antal parkeringsplatser) vid instansieringen av ett nytt garage
- [x] Möjlighet att populera (sätta in bilar i garaget med ett antal fordon från start)
- [x] Hitta ett specifikt fordon via registreringsnumret. Det ska fungera med både ABC123 samt Abc123 eller AbC123
- [x] Söka efter fordon utifrån en egenskap eller flera (alla möjliga kombinationer från basklassen Vehicle). Exempelvis:
    - Alla svarta fordon med fyra hjul
    - Alla motorcyklar som är rosa och har 3 hjul
    - Alla lastbilar
    - Alla röda fordon
- [x] Användaren ska få feedback på att saker gått bra eller dåligt. Till exempel när vi parkerat ett fordon vill vi få en bekräftelse på att fordonet är parkerat. Om det inte går vill användaren få veta varför.
***
### Gränssnitt
Programmet ska vara en konsolapplikation med ett textbaserat användargränssnitt.
Från gränssnittet skall det gå att:
- [x] Navigera till **samtlig** funktionalitet från garage via gränssnittet
- [x] Skapa ett garage med en användar-specificerad storlek
- [x] Det ska gå att stänga av applikationen från gränssnittet

Applikationen skall fel hantera indata på ett robust sätt, så att den inte kraschar vid felaktig  
inmatning eller användning.
* * *
### Enhetstester
- [ ] Testerna skapas i ett eget testprojekt
- [ ] Vi testar de publika metoderna i klassen `Garage`
- [ ] Testerna är strukturerade som Arrange, Act, Assert
- [ ] Testerna har förklarande namn enligt `MethodName_StateUnderTest_ExpectedBehavior`, t.ex. `Sum_NegativeNumberAs1stParam_ExceptionThrown`
- Gärna TDD: skriv testet före implementationen
- Extra (om tid finns): tester för hela applikationen
* * *
## Förslag på Extra funktionalitet (ej krav):
- Möjlighet att också kunna söka på de fordonsspecifika egenskaperna.
- Hantera flera garage som kan ha olika typer av fordon i sig exempelvis en hangar ett vanligt  
    garage samt ett motorcykelgarage.  
    Detta kommer medföra att man ska kunna manövrera sig mellan dom olika garagen i ui:t för  
    att kunna göra dom tidigare nämnda funktionerna de ska ske på bara det garaget som man  
    har för tillfället valt.
- [x] Det ska tydligt visas vilket garage man för närvarande arbetar med.
- Ett garage består inte längre av fordon utan av parkeringsplatser som i sin tur kan hålla  
    fordon.
- [x] Det går att via C# skriva och läsa till filsystemet från er applikation. Ta reda på hur man gör för  
    att kunna spara ert garage (via meny eller automatiskt vid avstängning) och ladda in ert garage  
    (via meny eller automatiskt vid start av applikationen)
- [x] Olika fordon tar olika stor plats tex en bil tar 1 plats en båt tar 2 platser ett flygplan kräver 3  
    platser osv en motorcykel tar endast 1/3 dels plats.
- När man parkerar ska endast de fordon som garaget har plats för visas som alternativ.
- Läsa in storleken på garaget via konfiguration
- Valfri egen funktionalitet:
    - [x] Skapa nya fordon, med kontroll av registreringsnumrets format
    - [x] Ta bort garaget och börja om