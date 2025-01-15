
DROP DATABASE IF EXISTS M9_BBDDSERVIDOR;
CREATE DATABASE M9_BBDDSERVIDOR;
USE M9_BBDDSERVIDOR;

CREATE TABLE Player ( Name VARCHAR(20) NOT NULL PRIMARY KEY,
					 Password TEXT NOT NULL
					 ) ENGINE=InnoDB;

CREATE TABLE Game (
				   Identifier INTEGER PRIMARY KEY NOT NULL,
				   EndDateTime DATETIME NOT NULL,
				   Duration TIME NOT NULL,
				   Winner TEXT NOT NULL
				   ) ENGINE=InnoDB;

CREATE TABLE Participation (
							Player VARCHAR(20) NOT NULL,
							Game INTEGER NOT NULL,
							Position INTEGER NOT NULL,
							FOREIGN KEY (Player) REFERENCES Player (Name),
							FOREIGN KEY (Game) REFERENCES Game (Identifier)
							) ENGINE=InnoDB;

INSERT INTO Player (Name, Password) VALUES ('Alice', 'pass123');
INSERT INTO Player (Name, Password) VALUES ('Bob', 'secure456');
INSERT INTO Player (Name, Password) VALUES ('Charlie', 'mysecret789');


INSERT INTO Game (Identifier, EndDateTime, Duration, Winner) VALUES (1, '2024-09-29 15:30:00', '01:20:05', 'Alice');
INSERT INTO Game (Identifier, EndDateTime, Duration, Winner) VALUES (2, '2024-09-29 16:30:00', '03:43:01', 'Bob');
INSERT INTO Game (Identifier, EndDateTime, Duration, Winner) VALUES (3, '2024-09-29 17:30:00', '02:11:05', 'Charlie');

INSERT INTO Participation (Player, Game, Position) VALUES ('Alice', 1, 1);
INSERT INTO Participation (Player, Game, Position) VALUES ('Bob', 2, 2);
INSERT INTO Participation (Player, Game, Position) VALUES ('Charlie', 3, 1);
INSERT INTO Participation (Player, Game, Position) VALUES ('Alice', 2, 1);
INSERT INTO Participation (Player, Game, Position) VALUES ('Bob', 1, 2);   
INSERT INTO Participation (Player, Game, Position) VALUES ('Charlie', 2, 1);
INSERT INTO Participation (Player, Game, Position) VALUES ('Alice', 3, 1);  
INSERT INTO Participation (Player, Game, Position) VALUES ('Charlie', 1, 2);

SELECT DISTINCT p2.Name 
FROM Participation p1
JOIN Participation p2 ON p1.Game = p2.Game
WHERE p1.Player = '{Jugador}' AND p2.Player != '{Jugador}';

SELECT g.Identifier, g.EndDateTime, g.Duration, g.Winner, p1.Position AS MiPosicion, p2.Position AS PosicionDelOtroJugador
FROM Game g
JOIN Participation p1 ON g.Identifier = p1.Game
JOIN Participation p2 ON g.Identifier = p2.Game
WHERE p1.Player = '{Jugador}' AND p2.Player = '{OtroJugador}';

SELECT g.Identifier, g.EndDateTime, g.Duration, g.Winner
FROM Game g
WHERE g.EndDateTime BETWEEN '{FechaInicio}' AND '{FechaFin}';
					


SELECT Game.Identifier, Game.EndDateTime
	FROM Player
	JOIN Participation ON Player.Name = Participation.Player
	JOIN Game ON Participation.Game = Game.Identifier
	WHERE Game.Winner = 'Charlie';
