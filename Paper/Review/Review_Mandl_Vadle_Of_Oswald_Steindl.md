# Review von Histogram Multiprocessing (MPI)
### von Mandl Matthias und Vadle Peter an Hannes Oswald und Steindl Maximilian

* ## + Die Definition eines Wortes würden mit großer Wahrscheinlichkeit viele verachlässigen. Dass über dieses sehr wichtige Detail sich so viele Gedanken gemacht worden ist, zeigt von Genauigkeit und minimiert den womöglich auftretenden Interpretationsspielraum.


* ## + Die Tabelle für die möglichen Ascii Characters ist eine gute Beschränkung, die einem Benutzer der Applikation direkt unterstützt und wodurch mögliche Fehler aufgrund von nicht beinhalteten Zeichen schnell gefunden werden können, anstatt ewig debuggen zu müssen.


* ## - "Figure 1: ASCII table: The marked characters and all characters not in this table were defined as valid parts of a word."  Hier ist etwas unklar welche characters jetzt noch genau zu den gelben dazu kommen. Womöglich wäre es zu aufwendig alle characters zu dokumentieren, aber zum Beispiel ein Verweis wäre hier villeicht hilfreich gewesen.


* ## + Die Berücksichtigung bei dem Trennen des Textes für die Parallele Abarbeitung, dass Wörter nicht in der Mitte zerteilt werden dürfen ist in unseren Augen ein wichtiges Detail, auf was hier eingegangen wird.


* ## + Das Sequence Diagram inklusive der Loops gibt einen guten Überblick über die Arbeitsweise des Programs. 


* ## + Der Unterschied zwischen Figur 6 und Figur 8 ist unserer Meinung sehr gut dargestellt. Dass Die bessere Performance erst wirklich auftritt wenn der Text 10x genommen wird ist sehr interesant. Genauso, dass das Erstellen von 16 und 32 Prozessen eine so enorm hohe Zeit benötigt.


* ## + Der Aufbau des gesamten Dokuments ist vorallem mit der Beschreibung der Problemstellung und dem Conclusio unserer Meinung nach sehr gut gelungen. 


* ## - Eine Beispielausgabe wäre als Veranschaulichung und für das Verständnis ein guter Zusatz gewesen.

</br>
</br>

* ## + Es ist sehr gut dass die verwendete Zeit für das Verteilen sowie das Sammeln der Daten veranschaulicht wurde. Dadurch sieht man wirklich wie viel Zeit für dies verwendet wurde.


* ## + Weiters ist es ein guter Gedanke das Buch 10mal einzulesen damit man sieht wie das Program mit wirklich große Dateien umgeht.


* ## - Beim lesen wird nicht sofort klar ob alle Wörter gezählt werden, oder wie oft ein wort im text vorkommt.
